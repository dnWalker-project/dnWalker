using MMC.Data;
using System;
using System.Linq.Expressions;

namespace dnWalker.Symbolic
{
    public static class DataElementExtensions
    {
        public static Expression AsExpression(this IDataElement dataElement)
        {
            if (dataElement is IIntegerElement integerElement)
            {
                return Expression.Constant(Convert.ChangeType(integerElement, typeof(int)), typeof(int));
            }

            throw new NotSupportedException();
        }
    }
}
