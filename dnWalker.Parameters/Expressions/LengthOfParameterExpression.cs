using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class LengthOfParameterExpression : UnaryParameterExpression
    {
        public LengthOfParameterExpression(ParameterRef operand) : base(operand)
        {
        }

        public override ParameterExpressionType ExpressionType
        {
            get
            {
                return ParameterExpressionType.LengthOf; 
            }
        }

        public override void ApplyTo(IParameterContext ctx, object value)
        {
            int? length = value as int?;

            IArrayParameter parameter = Operand.Resolve<IArrayParameter>(ctx) ?? throw new Exception("Cannot resolve the operand.");

            parameter.Length = length;
        }
    }
}
