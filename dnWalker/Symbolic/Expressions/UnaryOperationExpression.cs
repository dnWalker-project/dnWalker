using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class UnaryOperationExpression : Expression
    {
        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitUnaryOperation(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitUnaryOperation(this, state);


        internal UnaryOperationExpression(Operator op, Expression operand)
        {
            Operator = op;
            Operand = operand ?? throw new ArgumentNullException(nameof(operand));

            if (!op.IsUnary()) throw new ArgumentException("Must be a unary operator.", nameof(op));
        }

        public Operator Operator { get; }
        public Expression Operand { get; }

        public override ExpressionType Type => Operand.Type;
    }
}
