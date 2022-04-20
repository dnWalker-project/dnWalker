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

        public IVariable Variable { get; }

        public LocationExpression(IVariable variable)
        {
            Variable = variable ?? throw new ArgumentNullException(nameof(variable));
            if (!Variable.VariableType.IsReference())
                throw new ArgumentException("The variable must be of type Array or String.", nameof(variable));
        }
    }
}
