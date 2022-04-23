using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Z3.LinqBinding;

namespace dnWalker
{
    public interface ISolver
    {
        Dictionary<string, object> Solve(Expression expression, IList<ParameterExpression> parameters);// => new Dictionary<string, object>();
        IEnumerable<Valuation> Solve(IEnumerable<dnWalker.Symbolic.Expressions.Expression> constraints) => Enumerable.Empty<Valuation>();
    }
}
