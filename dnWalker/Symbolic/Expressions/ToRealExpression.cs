using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class ToRealExpression : Expression
    {
        public ToRealExpression(Expression inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public override ExpressionType Type => ExpressionType.Real;

        public Expression Inner { get; }

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitToReal(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitToReal(this, state);
    }
}
