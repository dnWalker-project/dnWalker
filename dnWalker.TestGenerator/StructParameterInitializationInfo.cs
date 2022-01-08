using dnWalker.Parameters;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace dnWalker.TestGenerator
{
    internal class StructParameterInitializationInfo : ParameterInitializationInfo
    {
        public HashSet<FieldInfo> _usedFields = new HashSet<FieldInfo>();

        public StructParameterInitializationInfo(ParameterRef reference, Type expectedType) : base(reference, expectedType)
        {
        }

        public override void AddContext(IParameterContext context)
        {
            base.AddContext(context);

            if (Reference.TryResolve(context, out IStructParameter? sp))
            {
                Type t = ExpectedType;

                foreach (var f in sp.GetFields())
                {
                    FieldInfo fi = t.GetField(f.Key) ?? throw new Exception($"Could not find field '{f.Key}' in type '{t}'");
                    _usedFields.Add(fi);
                }
            }
        }
    }
}