using dnWalker.Parameters;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {
        private static readonly ConditionalWeakTable<PrimitiveValueParameter, ParameterExpression> _expresions = new ConditionalWeakTable<PrimitiveValueParameter, ParameterExpression>();

        private static ParameterExpression CreateExpression(PrimitiveValueParameter primitiveValueParameter)
        {
            switch (primitiveValueParameter)
            {
                case BooleanParameter p: return Expression.Parameter(typeof(Boolean), p.FullName.ToString());
                case CharParameter p: return Expression.Parameter(typeof(Char), p.FullName.ToString());
                case ByteParameter p: return Expression.Parameter(typeof(Byte), p.FullName.ToString());
                case SByteParameter p: return Expression.Parameter(typeof(SByte), p.FullName.ToString());
                case Int16Parameter p: return Expression.Parameter(typeof(Int16), p.FullName.ToString());
                case Int32Parameter p: return Expression.Parameter(typeof(Int32), p.FullName.ToString());
                case Int64Parameter p: return Expression.Parameter(typeof(Int64), p.FullName.ToString());
                case UInt16Parameter p: return Expression.Parameter(typeof(UInt16), p.FullName.ToString());
                case UInt32Parameter p: return Expression.Parameter(typeof(UInt32), p.FullName.ToString());
                case UInt64Parameter p: return Expression.Parameter(typeof(UInt64), p.FullName.ToString());
                case SingleParameter p: return Expression.Parameter(typeof(Single), p.FullName.ToString());
                case DoubleParameter p: return Expression.Parameter(typeof(Double), p.FullName.ToString());
                default:
                    throw new NotSupportedException();
            }
        }

        public static ParameterExpression AsExpression(this PrimitiveValueParameter primitiveValueParameter)
        {
            if (_expresions.TryGetValue(primitiveValueParameter, out ParameterExpression expression))
            {
                return expression;
            }

            expression = CreateExpression(primitiveValueParameter);
            _expresions.AddOrUpdate(primitiveValueParameter, expression);
            return expression;
        }
    }
}
