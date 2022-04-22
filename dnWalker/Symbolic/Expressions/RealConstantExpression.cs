using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class RealConstantExpression : ConstantExpression<double>
    {
        public RealConstantExpression(double value) : base(value)
        {
        }

        public override ExpressionType Type => ExpressionType.Real;

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitRealConstant(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitRealConstant(this, state);

        public override Expression AsBoolean()
        {
            return Value != 0 ? True : False;
        }
    }
}
