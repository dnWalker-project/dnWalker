using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public static partial class ParameterExtensions
    {
        public static List<ParameterExpression> GetParametersAsExpressions(this ParameterStore parameterStore)
        {
            return parameterStore.RootParameters
                .SelectMany(p => p.GetParameterExpressions())
                .Where(e => e != null)
                .ToList();
        }

        //public static IEnumerable<ParameterExpression> GetParameterExpressions(this Parameter parameter)
        //{

        //}

        //public static bool TryGetSingleExpressions(this Parameter parameter, out ParameterExpression expression)
        //{
        //    return false;
        //}
    }
}
