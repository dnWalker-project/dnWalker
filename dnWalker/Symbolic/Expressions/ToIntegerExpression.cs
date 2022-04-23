using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class ToIntegerExpression : Expression
    {
        public ToIntegerExpression(Expression inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override ExpressionType Type => ExpressionType.Integer;

        public Expression Inner { get; }

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitToInteger(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitToInteger(this, state);
    }
}
