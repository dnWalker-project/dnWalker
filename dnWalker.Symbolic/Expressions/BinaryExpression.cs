using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class BinaryExpression : Expression
    {
        internal BinaryExpression(Operator op, Expression left, Expression right)
        {
            Operator = op;
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));

            Type = op.GetResultType(left, right);
        }

        public Operator Operator { get; }
        public Expression Left { get; }
        public Expression Right { get; }

        public override TypeSig Type { get; }

        protected internal override Expression Accept(ExpressionVisitor visitor) => visitor.VisitBinary(this);
        protected internal override Expression Accept<T>(ExpressionVisitor<T> visitor, T context) => visitor.VisitBinary(this, context);
    }
}
