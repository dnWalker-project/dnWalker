using dnWalker.Parameters;

using System;

namespace dnWalker.TestGenerator
{
    internal class PrimitiveValueInitializationInfo : ParameterInitializationInfo
    {
        public PrimitiveValueInitializationInfo(ParameterRef reference, Type expectedType) : base(reference, expectedType)
        {
        }

        public override void AddContext(IParameterContext context)
        {
            base.AddContext(context);

        }
    }
}