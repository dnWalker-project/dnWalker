using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class RefEqualsParameterExpression : BinaryParameterExpression
    {
        public RefEqualsParameterExpression(ParameterRef leftOperand, ParameterRef rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override ParameterExpressionType ExpressionType
        {
            get
            {
                return ParameterExpressionType.RefEquals;
            }
        }

        public override void ApplyTo(IParameterContext ctx, object value)
        {
            throw new NotImplementedException();
        }
    }
}
