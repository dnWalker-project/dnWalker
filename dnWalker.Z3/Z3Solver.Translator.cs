using dnWalker.Symbolic;

using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3
{
    public partial class Z3Solver
    {
        /// <summary>
        /// Translates the pure terms within the <paramref name="constraint"/> and asserts them using the solver 
        /// provided by the <paramref name="context"/>
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        private static void AssertPureTerms(Constraint constraint, SolverContext context)
        {
            Z3ExpressionTranslator translator = new Z3ExpressionTranslator(
                context.Z3,
                context.VariableMapping,
                context.NullExpr,
                context.StringNullExpr
                );

            BoolExpr[] z3Constraints = constraint.PureTerms
                .Select(pt => translator.Translate(pt.GetExpression()))
                .Cast<BoolExpr>()
                .ToArray();

            context.Solver.Assert(z3Constraints);
        }
    }
}
