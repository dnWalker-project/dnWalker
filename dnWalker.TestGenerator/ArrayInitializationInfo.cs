using dnWalker.Parameters;

using System;

namespace dnWalker.TestGenerator
{
    internal class ArrayInitializationInfo : ParameterInitializationInfo
    {
        public Type ExpectedElementType
        {
            get
            {
                return ExpectedType.GetElementType() ?? throw new Exception("The ExpectedType was not an array type!");
            }
        }

        public ArrayInitializationInfo(ParameterRef reference, Type expectedType) : base(reference, expectedType)
        {
        }



        public override void AddContext(IParameterContext context)
        {
            base.AddContext(context);
        }
    }
}