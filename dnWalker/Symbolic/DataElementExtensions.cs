using MMC.Data;
using MMC.State;
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

            switch (dataElement)
            {
                case Float8 float8: return Expression.Constant(float8.Value, typeof(Double));
                case Float4 float4: return Expression.Constant(float4.Value, typeof(Single));
                case Int4 int4: return Expression.Constant(int4.Value, typeof(Int32));
                case UnsignedInt4 uint4: return Expression.Constant(uint4.Value, typeof(UInt32));
                case Int8 int8: return Expression.Constant(int8.Value, typeof(Int64));
                case UnsignedInt8 uint8: return Expression.Constant(uint8.Value, typeof(UInt64));
                case ConstantString s: return Expression.Constant(s, typeof(String));
            }

            if (dataElement is INumericElement numericElement)
            {
                return Expression.Constant(Convert.ChangeType(numericElement, typeof(int)), typeof(int));
            }

            throw new NotSupportedException();
        }

        public static void SetExpression(this IDataElement dataElement, Expression expression, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, nameof(expression), expression);
        }

        public static bool TryGetExpression(this IDataElement dataElement, ExplicitActiveState cur, out Expression expression)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(dataElement, nameof(expression), out expression);
        }
    }
}
