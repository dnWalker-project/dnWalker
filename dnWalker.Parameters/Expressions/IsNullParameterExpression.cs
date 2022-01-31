using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Expressions
{
    public class IsNullParameterExpression : UnaryParameterExpression
    {
        public IsNullParameterExpression(ParameterRef operand) : base(operand)
        {
        }

        public override ParameterExpressionType ExpressionType
        {
            get
            {
                return ParameterExpressionType.IsNull;
            }
        }

        public override void ApplyTo(IParameterSet ctx, object value)
        {
            bool? isNull = value as bool?;

            IReferenceTypeParameter parameter = Operand.Resolve<IReferenceTypeParameter>(ctx) ?? throw new Exception("Cannot resolve the operand.");

            parameter.IsNull = isNull;
        }
    }
}
