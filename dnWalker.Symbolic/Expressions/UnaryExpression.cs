using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class UnaryExpression : Expression
    {
        internal UnaryExpression(Operator op, Expression inner)
        {
            Operator = op;
            Inner = inner ?? throw new ArgumentNullException(nameof(inner));
            Type = op.GetResultType(inner);
        }

        public Operator Operator { get; }
        public Expression Inner { get; }
        public override TypeSig Type { get; }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitUnary(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitUnary(this, context);
    }
}
