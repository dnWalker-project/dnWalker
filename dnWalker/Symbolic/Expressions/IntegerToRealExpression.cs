using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class IntegerToRealExpression : Expression
    {
        public IntegerToRealExpression(Expression inner)
        {
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
            if (inner.Type != ExpressionType.Integer) throw new ArgumentException("The inner expression must be of type integer.", nameof(inner));
        }

        public override ExpressionType Type => ExpressionType.Real;

        public Expression Inner { get; }

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitIntegerToReal(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitIntegerToReal(this, state);
    }
}
