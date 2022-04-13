using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public static class SystemLinqExpressionExtensions
    {
        private static readonly Dictionary<Type, object> _zeros = new Dictionary<Type, object>();
        public static Expression AsBoolean(this Expression expression)
        {
            Type t = expression.Type;
            if (t == typeof(bool)) return expression;

            if (expression is ConstantExpression constExpr)
            {
                int value = (int) Convert.ChangeType(constExpr.Value, typeof(int));
                return Expression.Constant(value > 0 ? true : false);
            }

            return Expression.NotEqual(expression, Expression.Constant(GetZero(t)));

            static object GetZero(Type t)
            {
                if (!_zeros.TryGetValue(t, out object zero))
                {
                    zero = Convert.ChangeType(0, t);
                    _zeros.Add(t, zero);
                }
                return zero;
            }
        }

        public static Expression Optimize(this Expression expression)
        {
            return SystemLinqExpressionOptimizer.Instance.Visit(expression);
        }
    }
}
