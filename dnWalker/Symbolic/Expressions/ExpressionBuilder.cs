using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class ExpressionBuilder
    {
        public static IExpression CompareExpression(IExpression left, IExpression right, string op)
        {
            return null;
        }

        internal static IExpression CreateConstant<T>(string name)
        {
            return null;// new Constant<T>(name);
        }
    }
}
