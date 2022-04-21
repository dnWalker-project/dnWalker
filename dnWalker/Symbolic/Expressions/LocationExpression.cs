using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    /// <summary>
    /// Represents location of an array, object or string (ref types).
    /// </summary>
    public class LocationExpression : Expression
    {
        public override ExpressionType Type => ExpressionType.Location;

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitLocation(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitLocation(this, state);

        public Expression Expression { get; }

        public LocationExpression(Expression expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            ExpressionType type = expression.Type;

            if (type != ExpressionType.String &&
                type != ExpressionType.Location)
                throw new ArgumentException("The expression must be of type Location.", nameof(expression));
        }
    }
}
