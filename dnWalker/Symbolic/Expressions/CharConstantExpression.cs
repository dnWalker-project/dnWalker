using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class CharConstantExpression : ConstantExpression<char>
    {
        public CharConstantExpression(char value) : base(value)
        {
        }

        public override ExpressionType Type => ExpressionType.Char;

        public override Expression Accept(ExpressionVisitor visitor) => visitor.VisitCharConstant(this);
        public override Expression Accept<TState>(ExpressionVisitor<TState> visitor, TState state) => visitor.VisitCharConstant(this, state);

        public override Expression AsBoolean()
        {
            return Value != '\0' ? True : False;
        }
    }
}
