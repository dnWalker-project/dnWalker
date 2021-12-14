namespace dnWalker.Parameters.Expressions
{
    public abstract class BinaryParameterExpression : ParameterExpression
    {
        protected BinaryParameterExpression(ParameterReference lhs, ParameterReference rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        public ParameterReference Lhs { get; }
        public ParameterReference Rhs { get; }

        public override string ToString()
        {
            return $"{Identifier}{Lhs}{Rhs}";
        }
    }
}

