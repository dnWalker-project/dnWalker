using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class ValueOfParameterExpression : UnaryParameterExpression
    {
        public ValueOfParameterExpression(ParameterRef operand) : base(operand)
        {
        }

        public override ParameterExpressionType ExpressionType
        {
            get
            {
                return ParameterExpressionType.ValueOf;
            }
        }

        public override void ApplyTo(IParameterSet ctx, object value)
        {
            IPrimitiveValueParameter parameter = Operand.Resolve<IPrimitiveValueParameter>(ctx) ?? throw new Exception("Cannot resolve the operand.");

            parameter.Value = value;
        }
    }
}
