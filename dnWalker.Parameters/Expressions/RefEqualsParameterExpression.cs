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
            bool refEquals = LeftOperand == RightOperand;

            if ((bValue && refEquals) ||
                (!bValue && !refEquals))
            {
                // we want them to be equal AND they are equal
                // or we do not wan them to be equal AND they are not equal
                // => do nothing
                return;
            }
            else if (bValue && !refEquals)
            {
                // we want them to be equal and, at the moment, they are not
                
                IParameter lhs = LeftOperand.Resolve(ctx)!;
                IParameter rhs = RightOperand.Resolve(ctx)!;

                // move accessors from RHS to LHS
                foreach (var a in rhs.Accessors)
                {
                    a.ChangeTarget(LeftOperand, ctx);
                }

                rhs.Accessors.Clear();

                // merge traits, right now just copy from RHS to LHS, with overwrite
                if (rhs is IFieldOwner rhsFieldOwner && lhs is IFieldOwner lhsFieldOwner)
                {
                    rhsFieldOwner.MoveTo(lhsFieldOwner);
                }
                if (rhs is IMethodResolver rhsMethodResolver && lhs is IMethodResolver lhsMethodResolver)
                {
                    rhsMethodResolver.MoveTo(lhsMethodResolver);
                }
                if (rhs is IItemOwner rhsItemOwner && lhs is IItemOwner lhsItemOwner)
                {
                    rhsItemOwner.MoveTo(lhsItemOwner);
                }

                // remove RHS from the context
                ctx.Remove(RightOperand);
            }
            else if (!bValue && refEquals)
            {
                // we want them not to be equal AND they are equal
                // => for each accessor construct a new parameter
                // if there is only one accessor = there is nothing to be done, the equality cannot be broken
                IParameter original = LeftOperand.Resolve(ctx)!;

                IList<ParameterAccessor> accessors = original.Accessors;
                while (accessors.Count > 1) // we wan to keep 1 accessor 
                {
                    IParameter newParameter = original.CloneData(ctx);
                    ParameterAccessor accessor = accessors[accessors.Count - 1];
                    accessors.RemoveAt(accessors.Count - 1);

                    // this should break the connection between the owner ant original parameter and
                    // create connection between the owner and new parameter
                    accessor.ChangeTarget(newParameter.Reference, ctx);
                }
            }
        }
    }
}
