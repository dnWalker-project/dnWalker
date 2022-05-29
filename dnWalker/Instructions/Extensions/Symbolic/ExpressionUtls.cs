using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public static class ExpressionUtils
    {
        public static bool GetExpressions(ExplicitActiveState cur, IDataElement dataElement, out Expression expression)
        {
            return dataElement.TryGetExpression(cur, out expression);
        }

        public static bool GetExpressions(ExplicitActiveState cur, IDataElement de1, IDataElement de2, out Expression expr1, out Expression expr2)
        {
            de1.TryGetExpression(cur, out expr1);
            de2.TryGetExpression(cur, out expr2);

            if (expr1 == null && expr2 != null)
            {
                expr1 = de1.AsExpression(cur);
            }
            else if (expr1 != null && expr2 == null)
            {
                expr2 = de2.AsExpression(cur);
            }

            return expr1 != null;
        }

        public static bool GetExpressions(ExplicitActiveState cur, IDataElement de1, IDataElement de2, IDataElement de3, out Expression expr1, out Expression expr2, out Expression expr3)
        {
            de1.TryGetExpression(cur, out expr1);
            de2.TryGetExpression(cur, out expr2);
            de3.TryGetExpression(cur, out expr3);

            if (expr1 != null || expr2 != null || expr3 != null)
            {
                expr1 ??= de1.AsExpression(cur);
                expr2 ??= de2.AsExpression(cur);
                expr3 ??= de3.AsExpression(cur);
            }


            return expr1 != null;
        }
    }
}
