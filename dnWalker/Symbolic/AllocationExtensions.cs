using MMC.State;
using System.Linq.Expressions;

namespace dnWalker.Symbolic
{
    public static class AllocationExtensions
    {
        public static Expression GetExpression(this Allocation allocation, ExplicitActiveState cur)
        {
            if (!cur.TryGetObjectAttribute<Expression>(allocation, "symbolic_expression", out var expression))
            {

            }

            return expression;
        }
    }
}
