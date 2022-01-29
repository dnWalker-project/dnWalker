using dnWalker.Parameters;
using dnWalker.TestGenerator.Reflection;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace dnWalker.TestGenerator
{
    internal class ObjectInitializationInfo : ParameterInitializationInfo
    {

        public ObjectInitializationInfo(ParameterRef reference, Type expectedType) : base(reference, expectedType)
        {
        }

        private HashSet<FieldInfo> _usedFields = new HashSet<FieldInfo>();
        private Dictionary<MethodInfo, int> _invokedMethods = new Dictionary<MethodInfo, int>();

        public IReadOnlyCollection<FieldInfo> UsedFields
        {
            get
            {
                return _usedFields;
            }
        }

        public IReadOnlyDictionary<MethodInfo, int> InvokedMethods
        {
            get
            {
                return _invokedMethods;
            }
        }

        public override void AddContext(IParameterSet context)
        {
            base.AddContext(context);

            if (Reference.TryResolve(context, out IObjectParameter? op))
            {
                Type t = ExpectedType;

                foreach (var f in op.GetFields())
                {
                    FieldInfo fi = t.GetField(f.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new Exception($"Could not find field '{f.Key}' in type '{t}'");
                    _usedFields.Add(fi);
                }

                foreach (var m in op.GetMethodResults())
                {
                    MethodInfo method = t.GetMethodFromSignature(m.Key) ?? throw new Exception($"Could not find the '{m.Key}' method");
                    if (!_invokedMethods.ContainsKey(method))
                    {
                        _invokedMethods[method] = 1;
                    }
                    else
                    {
                        _invokedMethods[method]++;
                    }
                }

                // TODO: when implemented...
                // foreach (var implementedType t in op.GetImplementedTypes())
                //{
                //}
            }
        }
    }
}