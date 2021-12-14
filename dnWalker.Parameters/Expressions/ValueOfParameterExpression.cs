using System;

namespace dnWalker.Parameters.Expressions
{
    public class ValueOfParameterExpression : UnaryParameterExpression
    {
        public ValueOfParameterExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'V';

        protected override char Identifier
        {
            get { return IdChar; }
        }

        public override bool TryApplyTo(ParameterStore store, object value)
        {
            if (Operand.TryResolve(store, out IParameter? resolvedParameter) &&
                resolvedParameter is IPrimitiveValueParameter primitiveValueParameter &&
                value.GetType() == Type.GetType(primitiveValueParameter.TypeName))
            {
                primitiveValueParameter.Value = value;
                return true;
            }
            return false;
        }
    }
}

