using dnWalker.Symbolic.Expressions;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static class AllocationExtensions
    {
        public static IExpression GetExpression(this Allocation allocation, ExplicitActiveState cur)
        {
            if (!cur.TryGetObjectAttribute<IExpression>(allocation, "symbolic_expression", out var expression))
            {

            }

            return expression;
        }
    }
}
