namespace dnWalker.Parameters.Expressions
{
    public class IsNullParameterExpression : UnaryParameterExpression
    {
        public IsNullParameterExpression(ParameterReference operand) : base(operand)
        {
        }

        internal static readonly char IdChar = 'N';

        protected override char Identifier
        {
            get { return IdChar; }
        }

        public override bool TryApplyTo(ParameterStore store, object value)
        {
            if (Operand.TryResolve(store, out IParameter? resolved) &&
                resolved is IReferenceTypeParameter referenceTypeParameter &&
                value is bool boolValue)
            {
                referenceTypeParameter.IsNull = boolValue;
                return true;
            }
            return false;
        }
    }
}

