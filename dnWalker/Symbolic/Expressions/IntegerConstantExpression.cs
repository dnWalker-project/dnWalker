using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class IntegerConstantExpression : ConstantExpression<long>
    {
        internal IntegerConstantExpression(long value) : base(value)
        {
        }

        public override ExpressionType Type => ExpressionType.Integer;

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitIntegerConstant(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitIntegerConstant(this, state);
    }
}
