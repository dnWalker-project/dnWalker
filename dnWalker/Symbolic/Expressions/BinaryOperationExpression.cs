using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class BinaryOperationExpression : Expression
    {
        private ExpressionType GetExpressionType(Operator op, Expression left, Expression right)
        {
            if (op.IsComparison())
            {
                return ExpressionType.Boolean;
            }
            else if (op.IsLogical())
            {
                return ExpressionType.Boolean;
            }
            else if (op.IsArithmetic())
            {
                // we enforce that both left and right has the same expression type inside the constructor
                return left.Type;
            }

            throw new NotSupportedException("Unexpected operator.");
        }

        public override ExpressionType Type => GetExpressionType(Operator, Left, Right);


        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitBinaryOperation(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitBinaryOperation(this, state);

        internal BinaryOperationExpression(Operator op, Expression left, Expression right)
        {
            Operator = op;
            Left = left ?? throw new ArgumentNullException(nameof(left));
            Right = right ?? throw new ArgumentNullException(nameof(right));

            if (!op.IsBinary()) throw new ArgumentException("Must be a binary operator.", nameof(op));
        }

        public Operator Operator { get; }
        public Expression Left { get; }
        public Expression Right { get; }

    }
}
