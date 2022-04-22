using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    /// <summary>
    /// Represents length of a string or array.
    /// </summary>
    /// <remarks>
    /// <see cref="LengthExpression"/> is produced by LDLEN and CALLVIRT System.String.get_Length instructions.
    /// </remarks>
    public class LengthExpression : Expression
    {
        public override ExpressionType Type => ExpressionType.Integer;

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitLength(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitLength(this, state);

        public Expression Expression { get; }

        public LengthExpression(Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));

            if (expression.Type != ExpressionType.Location &&
                expression.Type != ExpressionType.String)
            {
                throw new ArgumentException("The expression must be of Array or String.", nameof(expression));
            }
        }

    }
}
