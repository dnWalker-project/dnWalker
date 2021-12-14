namespace dnWalker.Parameters.Expressions
{
    public abstract class UnaryParameterExpression : ParameterExpression
    {
        protected UnaryParameterExpression(ParameterReference operand)
        {
            Operand = operand;
        }

        public ParameterReference Operand { get; }

        public override string ToString()
        {
            return $"{Identifier}{Operand}";
        }
    }
}

