using MMC.Data;
using MMC.State;
using System;

using dnWalker.Symbolic.Expressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Symbolic
{
    public static class DataElementExtensions
    {
        private const string ExpressionAttribute = "expression";
        public static Expression AsExpression(this IDataElement dataElement, ExplicitActiveState cur)
        {
            if (!dataElement.TryGetExpression(cur, out Expression expression))
            {
                ExpressionFactory factory = cur.GetExpressionFactory();
                switch (dataElement)
                {
                    case Int4 i4: return factory.MakeIntegerConstant(i4.Value);
                    case UnsignedInt4 u4: return factory.MakeIntegerConstant(u4.Value);
                    case Int8 i8: return factory.MakeIntegerConstant(i8.Value);
                    case UnsignedInt8 u8: return factory.MakeIntegerConstant(((long)u8.Value)); //! how to handle overflow?
                    case Float4 f4: return factory.MakeRealConstant(f4.Value);
                    case Float8 f8: return factory.MakeRealConstant(f8.Value);
                    case ConstantString constStr: return constStr.Value == null ? factory.StringNullExpression : factory.MakeStringConstant(constStr.Value);
                    case ObjectReference objRef:
                        return objRef.IsNull() ? factory.NullExpression :
                       // not null => generate a new variable based on the type name & process the concrete value
                       ProcessNewObject(objRef);
                }
            }
            return expression;

            Expression ProcessNewObject(ObjectReference objRef)
            {
                cur.TryGetSymbolicContext(out SymbolicContext context);
                Debug.Assert(context != null);
                return context.ProcessExistingObject(objRef);
            }
        }
        public static void SetExpression(this IDataElement dataElement, ExplicitActiveState cur, Expression expression)
        {
            cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, ExpressionAttribute, expression);
        }

        public static bool TryGetExpression(this IDataElement dataElement, ExplicitActiveState cur, [NotNullWhen(true)] out Expression expression)
        {
            return cur.PathStore.CurrentPath.TryGetObjectAttribute(dataElement, ExpressionAttribute, out expression);
        }

    }
}
