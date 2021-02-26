using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public static class ExpressionExtensions
    {
        public static Expression Simplify(this Expression expression)
        {
            try
            {
                return ExpressionOptimizer.visit(expression);
            }
            catch
            {
                return expression;
            }
        }
    }
}
