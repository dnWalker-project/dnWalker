using System;
namespace dnWalker.Symbolic.Expressions
{
    public class StringConstantExpression : Expression
    {
        public override ExpressionType Type => ExpressionType.String;
        public override Expression AsBoolean() => Expression.True; // string constant is never null
        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitStringConstant(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitStringConstant(this, state);

        public StringConstantExpression(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }
    }
}

