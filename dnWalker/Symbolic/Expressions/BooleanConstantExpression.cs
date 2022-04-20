namespace dnWalker.Symbolic.Expressions
{
    public class BooleanConstantExpression : ConstantExpression<bool>
    {
        public BooleanConstantExpression(bool value) : base(value)
        {
        }


        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitBooleanConstant(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitBooleanConstant(this, state);

        public override Expression AsBoolean() => this;
        public override ExpressionType Type => ExpressionType.Boolean;
    }
}