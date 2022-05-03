using dnWalker.Symbolic;
using dnWalker.Symbolic.Utils;
using dnWalker.Symbolic.Variables;
using dnWalker.Symbolic.Expressions.Utils;

using Microsoft.Z3;

using dnlib.DotNet;

using IVariable = dnWalker.Symbolic.IVariable;
using System.Linq;
using System.Collections.Generic;
using System;

namespace dnWalker.Z3
{
    public partial class Z3Solver
    {
        /// <summary>
        /// Translates the heap terms into their pure projections and asserts them using the solver provided by the <paramref name="context"/>.
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        private static void AssertHeapTerms(Constraint constraint, ref SolverContext context)
        {
            Dictionary<IVariable, Expr> varLookup = context.VariableMapping;
            Solver solver = context.Solver;
            Context z3 = context.Z3;
            Expr nullExpr = context.NullExpr;

            // handle PoinToTerm only, TODO: add inductive terms... - too complex for now
            PointToTerm[] terms = constraint.HeapTerms.OfType<PointToTerm>().ToArray();

            // 1. the source variables must not be null
            for (int i = 0; i < terms.Length; ++i)
            {
                Expr locVar = varLookup[terms[i].Source];
                solver.Assert(z3.MkNot(z3.MkEq(locVar, nullExpr)));
            }

            // 2. the source variable must be different
            for (int i = 0; i < terms.Length; ++i)
            {
                Expr xi = varLookup[terms[i].Source];
                for (int j = i + 1; j < terms.Length; ++j)
                {
                    Expr xj = varLookup[terms[j].Source];
                    solver.Assert(z3.MkNot(z3.MkEq(xi, xj)));
                }
            }

            // 3. the attributes equality
            // x -> <TYPE>({ fld1 := var1, ... , fldN := varN })
            // x.fld1 = var1 && ... && x.fldN = varN
            for (int i = 0; i < terms.Length; ++i)
            {
                foreach (KeyValuePair<object, IVariable> attribute in terms[i].Attributes)
                {
                    IVariable leftVar = attribute.Key switch
                    {
                        IField fld => Variable.InstanceField(terms[i].Source, fld),
                        int index => Variable.ArrayElement(terms[i].Source, index),
                        //(IMethod method, int invocation) => Variable.MethodResult(terms[i].Source, method, invocation),
                        _ => throw new InvalidOperationException($"Provided attribute has invalid type: {attribute.Key.GetType()}")
                    };
                    IVariable rightVar = attribute.Value;

                    solver.Assert(z3.MkEq(varLookup[leftVar], varLookup[rightVar]));
                }
            }
        }

        /// <summary>
        /// Equality between member variables which represents the same member implies equality between the member variable roots.
        /// </summary>
        /// <remarks>
        /// ((x1.field = v1) && (x2.field = v2) && (v1 = v2)) => (x1 = x2)
        /// From separation logic
        /// (x.field = v1) <=> (x -> (field := v))
        /// </remarks>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        private static void AssertMemberRootPointerEqualities(Constraint constraint, ref SolverContext context)
        {
            ICollection<(IMemberVariable v1, IMemberVariable v2)> refEqualities = RefEqualityCollector.GetRefEqualities(constraint.PureTerms.Select(pt => pt.GetExpression()));
            foreach ((IMemberVariable v1, IMemberVariable v2) in refEqualities)
            {
                if (v1.IsSameMemberAs(v2))
                {
                    context.Solver.Assert(context.Z3.MkEq(context.VariableMapping[v1.Parent], context.VariableMapping[v2.Parent]));
                }
            }
        }
    }
}
