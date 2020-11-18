namespace dnWalker.Symbolic.Expressions
{
    public class Constant<T> : IExpression
    {
        public Constant(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
