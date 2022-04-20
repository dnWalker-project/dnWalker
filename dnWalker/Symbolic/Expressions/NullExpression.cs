using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class NullExpression : Expression
    {
        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitNull(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitNull(this, state);

        public override ExpressionType Type => ExpressionType.Location;

        public override Expression AsBoolean() => Expression.False;

        internal NullExpression() { }
    }
}
