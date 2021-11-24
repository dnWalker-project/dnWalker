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
        private static readonly ConditionalWeakTable<Parameter, ParameterExpression> _expresions = new ConditionalWeakTable<Parameter,ParameterExpression>();

        private static void CacheExpression(Parameter parameter, ParameterExpression expression)
        {
            _expresions.AddOrUpdate(parameter, expression);
        }

        private static bool TryGetExpression(Parameter parameter, [NotNullWhen(true)] out ParameterExpression? expression)
        {
            return _expresions.TryGetValue(parameter, out expression);
        }

        private static ParameterExpression CreateExpression<TValue>(PrimitiveValueParameter<TValue> parameter) where TValue : struct
        {
            ParameterExpression pe = Expression.Parameter(typeof(TValue), parameter.GetFullName());
            return pe;
        }

        public static IEnumerable<ParameterExpression> GetExpressions<TValue>(this PrimitiveValueParameter<TValue> parameter) where TValue : struct
        {
            if (TryGetExpression(parameter, out ParameterExpression expression))
            {
                yield return expression;
            }

            else
            {
                expression = CreateExpression<TValue>(parameter);
                CacheExpression(parameter, expression);
                yield return expression;
            }
        }

        public static IEnumerable<ParameterExpression> GetExpressions(this ObjectParameter parameter)
        {
            return parameter.GetOwnedParameters().SelectMany(p => p.GetExpressions());
        }

        public static IEnumerable<ParameterExpression> GetExpressions(this InterfaceParameter parameter)
        {
            return parameter.GetOwnedParameters().SelectMany(p => p.GetExpressions());
        }

        public static IEnumerable<ParameterExpression> GetExpressions(this ArrayParameter parameter)
        {
            return parameter.GetOwnedParameters().SelectMany(p => p.GetExpressions());
        }

        public static IEnumerable<ParameterExpression> GetExpressions(this Parameter parameter)
        {
            switch (parameter)
            {
                case BooleanParameter p: return GetExpressions(p);
                case CharParameter p: return GetExpressions(p);
                case ByteParameter p: return GetExpressions(p);
                case SByteParameter p: return GetExpressions(p);
                case Int16Parameter p: return GetExpressions(p);
                case Int32Parameter p: return GetExpressions(p);
                case Int64Parameter p: return GetExpressions(p);
                case UInt16Parameter p: return GetExpressions(p);
                case UInt32Parameter p: return GetExpressions(p);
                case UInt64Parameter p: return GetExpressions(p);
                case SingleParameter p: return GetExpressions(p);
                case DoubleParameter p: return GetExpressions(p);

                case ObjectParameter p: return GetExpressions(p);
                case InterfaceParameter p: return GetExpressions(p);
                case ArrayParameter p: return GetExpressions(p);
                default:
                    throw new NotSupportedException();
            }
        }

        public static List<ParameterExpression> GetExpressionsForUsedParameters(this ParameterStore store)
        {

            // traverse all parameters in the parameter store
            // include only those which are actually used, e.g. their IDataElement was created...
            return store.GetUsedParameters().Select(p => p.)
        }
    }
}
