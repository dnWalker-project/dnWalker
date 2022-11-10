using dnWalker.Symbolic;

using dnWalker.Symbolic.Variables;
using dnWalker.Symbolic.Expressions.Utils;

using Microsoft.Z3;

using dnlib.DotNet;

using IVariable = dnWalker.Symbolic.IVariable;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Z3
{
    public partial class Z3Solver
    {
        /// <summary>
        /// Translates the heap terms into their pure projections and asserts them using the solver provided by the <paramref name="context"/>.
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        private static void AssertHeapTerms(Constraint constraint, SolverContext context)
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
        /// Equality between variables implies equality between same member variables.
        /// </summary>
        /// <remarks>
        /// (x1 = x2) => (x1.field == x2.field)
        /// From separation logic
        /// (x.field = v1) <=> (x -> (field := v))
        /// </remarks>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        private static void AssertMemberRootPointerEqualities(Constraint constraint, SolverContext context)
        {
            // ref equalities from pure terms
            ICollection<(IVariable x1, IVariable x2)> refEqualities = RefEqualityCollector.GetRefEqualities(constraint.PureTerms.Select(pt => pt.GetExpression()));

            // we may add new ref equlity... system which checks for already handled equalities & enables adding new ones
            Queue<(IVariable x1, IVariable x2)> toHandle = new Queue<(IVariable x1, IVariable x2)>(refEqualities);
            HashSet<(IVariable x1, IVariable x2)> handled = new HashSet<(IVariable x1, IVariable x2)>();

            Dictionary<IVariable, List<IMemberVariable>> memberVariables = new Dictionary<IVariable, List<IMemberVariable>>();

            while (TryGetEquality(out IVariable? x1, out IVariable? x2))
            {
                List<IMemberVariable> members1 = GetMemberVariables(x1);
                List<IMemberVariable> members2 = GetMemberVariables(x2);

                foreach (IMemberVariable m1 in members1)
                {
                    foreach (IMemberVariable m2 in members2)
                    {
                        if (m1.IsSameMemberAs(m2))
                        {
                            BoolExpr x12Equals = context.Z3.MkEq(context.VariableMapping[x1], context.VariableMapping[x2]);
                            BoolExpr m12Equals = context.Z3.MkEq(context.VariableMapping[m1], context.VariableMapping[m2]);

                            context.Solver.Assert(context.Z3.MkImplies(x12Equals, m12Equals));

                            if (!m1.Type.IsPrimitive && !m1.Type.IsString())
                            {
                                toHandle.Enqueue((m1, m2));
                            }

                            // we found a matching member variable, there cannot be another => break
                            break;
                        }
                    }
                }
            }

            bool TryGetEquality([NotNullWhen(true)]out IVariable? x1, [NotNullWhen(true)] out IVariable? x2)
            {
                if (toHandle.TryDequeue(out (IVariable, IVariable)eq) && handled.Add(eq))
                {
                    x1 = eq.Item1;
                    x2 = eq.Item2;
                    return true;
                }

                x1 = null;
                x2 = null;
                return false;
            }

            List<IMemberVariable> GetMemberVariables(IVariable variable)
            {
                if (!memberVariables.TryGetValue(variable, out List<IMemberVariable>? memberVars))
                {
                    memberVars = context.VariableMapping.Keys.OfType<IMemberVariable>().Where(mv => mv.Parent.Equals(variable)).ToList();
                    memberVariables[variable] = memberVars;
                }

                return memberVars;
            }
        }
    }
}
