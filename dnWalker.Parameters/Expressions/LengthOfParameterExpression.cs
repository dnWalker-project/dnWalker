namespace dnWalker.Parameters.Expressions
{
    public class LengthOfParameterExpression : UnaryParameterExpression
    {
        public LengthOfParameterExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'L';

        protected override char Identifier
        {
            get { return IdChar; }
        }

        public override bool TryApplyTo(ParameterStore store, object value)
        {
            if (Operand.TryResolve(store, out IParameter? operand) &&
                operand is IItemOwnerParameter itemOwnerParameter &&
                value is int intValue)
            {
                itemOwnerParameter.Length = intValue;
                return true;
            }
            return false;
        }
    }
}

