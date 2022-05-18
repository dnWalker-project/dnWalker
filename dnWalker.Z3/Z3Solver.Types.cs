using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Utils;

using Microsoft.Z3;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVariable = dnWalker.Symbolic.IVariable;

namespace dnWalker.Z3
{
    public partial class Z3Solver
    {
        /// <summary>
        /// Constructs and asserts constraint based on variable types.
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="context"></param>
        /// <remarks>
        /// Each reference type variable has its type. Two variables of incompatible types cannot be equal.
        /// This method sorts the reference type variables into groups per their type and enforces non equality.
        /// TODO: make it accept inheritance.
        /// </remarks>
        private static void AssertTypes(Constraint constraint, ref SolverContext context)
        {
            Dictionary<IVariable, Expr> varLookup = context.VariableMapping;
            Dictionary<TypeSig, List<IVariable>> groups = new Dictionary<TypeSig, List<IVariable>>(TypeEqualityComparer.Instance);
            Solver solver = context.Solver;
            Context z3 = context.Z3;
            Expr nullExpr = context.NullExpr;

            // sort into groups
            foreach (IVariable variable in varLookup.Keys)
            {
                TypeSig varType = variable.Type;
                if (!varType.IsPrimitive && !varType.IsString())
                {
                    List<IVariable> group = GetGroup(varType);
                    group.Add(variable);
                }
            }

            // ensure the inequalities
            List<IVariable>[] groupsArr = groups.Values.ToArray();
            for (int i = 0; i < groupsArr.Length; ++i)
            {
                List<IVariable> gi = groupsArr[i];
                for (int j = i + 1; j < groupsArr.Length; ++j)
                {
                    List<IVariable> gj = groupsArr[j];

                    AddInequalities(gi, gj);
                }
            }



            List<IVariable> GetGroup(TypeSig type)
            {
                if (!groups.TryGetValue(type, out var group))
                {
                    group = new List<IVariable>();
                    groups.Add(type, group);
                }

                return group;
            }

            void AddInequalities(List<IVariable> gi, List<IVariable> gj)
            {
                for (int k = 0; k < gi.Count; ++k)
                {
                    Expr x_ik = varLookup[gi[k]];
                    for (int l = 0; l < gj.Count; ++l)
                    {
                        Expr x_jl = varLookup[gj[l]];

                        // (x_ik != x_jl) || ((x_jk == null) && (x_jl == null)) 

                        solver.Assert(
                            z3.MkOr(
                                z3.MkNot(z3.MkEq(x_ik, x_jl)), 
                                z3.MkAnd(
                                    z3.MkEq(x_ik, nullExpr),
                                    z3.MkEq(x_jl, nullExpr))
                                ));
                    }
                }
            }
        }

    }
}
