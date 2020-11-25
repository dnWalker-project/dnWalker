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
        Dictionary<string, object> Solve(Expression expression);
    }
}
