using MMC.Data;
using MMC.State;
using System;

using dnWalker.Symbolic.Expressions;

namespace dnWalker.Symbolic
{
    public static class DataElementExtensions
    {
        public static Expression AsExpression(this IDataElement dataElement)
        {
            switch (dataElement)
            {
                case Float8 float8: return Expression.Constant(float8.Value);
                case Float4 float4: return Expression.Constant(float4.Value);
                case Int4 int4: return Expression.Constant(int4.Value);
                case UnsignedInt4 uint4: return Expression.Constant(uint4.Value);
                case Int8 int8: return Expression.Constant(int8.Value);
                case UnsignedInt8 uint8: return Expression.Constant(uint8.Value);
                case ConstantString s: return Expression.Constant(s.Value);
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
