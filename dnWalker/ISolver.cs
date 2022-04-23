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
    public readonly struct SolverResult
    {
        public IEnumerable<Valuation> Valuations { get; }
        public Status Status { get; }

        private SolverResult(IEnumerable<Valuation> valuations, Status status)
        {
            Valuations = valuations ?? throw new ArgumentNullException(nameof(valuations));
            Status = status;
        }

        public static readonly SolverResult Unsatisfiable = new SolverResult();
        public static SolverResult Satisfiable(IEnumerable<Valuation> valuations)
        {
            return new SolverResult(valuations, Status.Satisfiable);
        }
    }

    public enum Status
    {
        Unsatisfiable,
        Satisfiable
    }


    public interface ISolver
    {
        Dictionary<string, object> Solve(Expression expression, IList<ParameterExpression> parameters);// => new Dictionary<string, object>();
        SolverResult Solve(IEnumerable<dnWalker.Symbolic.Expressions.Expression> constraints) => SolverResult.Unsatisfiable;
    }
}
