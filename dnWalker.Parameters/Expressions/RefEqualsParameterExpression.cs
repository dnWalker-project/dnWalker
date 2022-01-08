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
            // value  = TRUE               | FALSE
            // refEq 
            // TRUE     noop               | clone the one which is not alias or lhs
            // FALSE    merge lhs with rhs | noop

            bool bValue = (bool)value;
            bool refEquals = ctx.RefEquals(LeftOperand, RightOperand);

            if ((bValue && refEquals) ||
                (!bValue && !refEquals))
            {
                return;
            }
            if (bValue && !refEquals)
            {
                // 1 both are IReferenceTypeParameter => copy traits from LHS to RHS & remove LHS & make alias of RHS with LHS reference
                // 2 one is IAliasParameter and other is IReferenceParameter => change 
            }
        }
    }
}
