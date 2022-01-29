using dnWalker.Parameters;
using dnWalker.TestGenerator.Reflection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace dnWalker.TestGenerator
{
    public class TestGeneratorContext
    {
        //public TestGeneratorContext(Assembly assembly, ExplorationData explorationData, ExplorationIterationData iterationData)
        //{
        //    SUTAssembly = assembly;
        //    ExplorationData = explorationData;
        //    IterationData = iterationData;
        //}

        public TestGeneratorContext(Assembly assembly, ExplorationData explorationData)
        {
            SUTAssembly = assembly;
            ExplorationData = explorationData;

            _sutType = AppDomain.CurrentDomain.GetType(explorationData.MethodSignature.DeclaringTypeFullName) ?? throw new Exception($"Could not find type '{explorationData.MethodSignature.DeclaringTypeFullName}'");
            _sutMethod = _sutType.GetMethodFromSignature(explorationData.MethodSignature) ?? throw new Exception($"Could not find method '{explorationData.MethodSignature}'");

            if (explorationData.Iterations != null && explorationData.Iterations.Length > 0)
            {
                foreach (var iterationData in explorationData.Iterations)
                {
                    IParameterSet ctx = iterationData.StartingParameterContext;
                    foreach (var keyValue in ctx.Parameters)
                    {
                        if (!_initializations.TryGetValue(keyValue.Key, out ParameterInitializationInfo? initInfo))
                        {
                            initInfo = ParameterInitializationInfo.Create(keyValue.Value);
                            _initializations.Add(keyValue.Key, initInfo);
                        }

                        initInfo.AddContext(ctx);
                    }


                    ctx = iterationData.EndingParameterContext;
                    foreach (var keyValue in ctx.Parameters.Where(kv => !_initializations.ContainsKey(kv.Key)))
                    {
                        if (!_initializations.TryGetValue(keyValue.Key, out ParameterInitializationInfo? initInfo))
                        {
                            initInfo = ParameterInitializationInfo.Create(keyValue.Value);
                            _initializations.Add(keyValue.Key, initInfo);
                        }

                        initInfo.AddContext(ctx);
                    }
                }
            }

        }

        private readonly MethodInfo _sutMethod;
        private readonly Type _sutType;

        private Dictionary<ParameterRef, ParameterInitializationInfo> _initializations = new Dictionary<ParameterRef, ParameterInitializationInfo>();

        public Assembly SUTAssembly 
        {
            get; 
        }

        public Type SUTType
        {
            get
            {
                return _sutType;
            }
        }

        public MethodInfo SUTMethod
        {
            get
            {
                return _sutMethod;
            }
        }

        public ExplorationData ExplorationData
        {
            get;
        }

        public string TestNamespaceName
        {
            get
            {
                return SUTType.Namespace + ".Tests";
            }
        }

        public string TestClassName
        {
            get
            {
                return SUTType.Name + "_Tests_" + SUTMethod.Name;
            }
        }

        public IReadOnlyDictionary<ParameterRef, ParameterInitializationInfo> Initializations
        {
            get
            {
                return _initializations;
            }
        }
    }
}
