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

            _sutMethodSignature = MethodSignature.FromString(explorationData.MethodSignature);
        }

        private readonly MethodSignature _sutMethodSignature;

        public Assembly SUTAssembly 
        {
            get; 
        }

        public Type SUTType
        {
            get
            {
                return _sutMethodSignature.DeclaringType;
            }
        }

        public MethodInfo SUTMethod
        {
            get
            {
                return _sutMethodSignature.MethodInfo;
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
    }
}
