using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface ISolver
    {
        /// <summary>
        /// Solves the precondition described by the two collections of terms.
        /// </summary>
        /// <param name="precondition">Represents the constraints which must be satisfied.</param>
        /// <returns>An instance of <see cref="IModel"/> which satisfies the precondition</returns>
        IModel? Solve(Constraint constraint);
    }

    public static class SolverExtensions
    {
        public static IModel? Solve(this ISolver solver, params Constraint[] constraints)
        {
            return solver.Solve(Constraint.Merge(constraints));
        }
        public static IModel? Solve(this ISolver solver, IEnumerable<Constraint> constraints)
        {
            return solver.Solve(Constraint.Merge(constraints));
        }
    }
}
