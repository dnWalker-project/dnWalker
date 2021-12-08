using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        private const string Parameter2Expression = "PRM2EXPR";

        public static IDictionary<string, ParameterExpression> GetExpressionLookup(this ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(Parameter2Expression, out IDictionary<string, ParameterExpression> lookup))
            {
                lookup = new Dictionary<string, ParameterExpression>();
                cur.PathStore.CurrentPath.SetPathAttribute(Parameter2Expression, lookup);
            }

            return lookup;
        }

        public static Expression GetValueExpression(this IPrimitiveValueParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"V{parameter.Id.ToString("x8")}"; // VALUE parameter name in expression is in format V<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {

                expression = Expression.Parameter(Type.GetType(parameter.TypeName), name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetIsNullExpression(this IReferenceTypeParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"N{parameter.Id.ToString("x8")}"; // NULL parameter name in expression is in format N<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {

                expression = Expression.Parameter(typeof(bool), name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetLengthExpression(this IArrayParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"L{parameter.Id.ToString("x8")}"; // LENGTH parameter name in expression is in format L<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {

                expression = Expression.Parameter(typeof(uint), name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetReferenceEqualsExpression(this IReferenceTypeParameter p1, IReferenceTypeParameter p2, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name1 = $"E{p1.Id.ToString("x")}{p2.Id.ToString("x8")}"; // REF EQUAL parameter name in expression is in format E<ID1 AS HEX NUMBER><ID2 AS HEX NUMBER>
            string name2 = $"E{p2.Id.ToString("x")}{p1.Id.ToString("x8")}"; // REF EQUAL parameter name in expression is in format E<ID1 AS HEX NUMBER><ID2 AS HEX NUMBER>

            ParameterExpression expression = null;
            if (lookup.TryGetValue(name1, out expression) || lookup.TryGetValue(name2, out expression))
            {
                return expression;
            }
            else
            {
                expression = Expression.Parameter(typeof(bool), name1);
                lookup[name1] = expression;
                return expression;
            }

        }
    }
}
