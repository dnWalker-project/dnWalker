using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class RealToIntegerExpression : Expression
    {
        public RealToIntegerExpression(Expression inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
            if (inner.Type != ExpressionType.Real) throw new ArgumentException("The inner expression must be of type real.", nameof(inner));
        }

        public override ExpressionType Type => ExpressionType.Integer;

        public Expression Inner { get; }

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitRealToInteger(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitRealToInteger(this, state);
    }
}
