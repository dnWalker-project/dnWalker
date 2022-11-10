﻿using dnWalker.Symbolic;

using Microsoft.Z3;

using System.Collections.Generic;

using IVariable = dnWalker.Symbolic.IVariable;
using System;
using System.Diagnostics;

namespace dnWalker.Z3
{
    public partial class Z3Solver : ISolver
    {
        private class SolverContext : IDisposable
        {
            private int _freeSymbol;

            public readonly Context Z3;
            public readonly Expr NullExpr;
            public readonly Expr StringNullExpr;
            public readonly Sort LocSort;
            public readonly Solver Solver;


            public SolverContext()
            {
                _freeSymbol = 0;
                Context context = new Context();

                Solver = context.MkSolver();
                Z3 = context;
                LocSort = context.MkUninterpretedSort("loc");
                NullExpr = context.MkConst("null", LocSort);
                StringNullExpr = context.MkConst("string-null", Z3.StringSort);

                // ensure null and string-null are actually used...
                Solver.Assert(Z3.MkEq(NullExpr, context.MkConst("dummyLoc", LocSort)));
                Solver.Assert(Z3.MkEq(StringNullExpr, context.MkConst("dummyString", Z3.StringSort)));
            }

            public readonly Dictionary<IVariable, Expr> VariableMapping = new Dictionary<IVariable, Expr>();


            public Symbol Symbol()
            {
                return Z3.MkSymbol(_freeSymbol++);
            }

            public bool CheckSat()
            {
                Status status = Solver.Check();
                return status == Status.SATISFIABLE;
            }

            public void Dispose()
            {
                Z3.Dispose();
            }
        }


        public IModel? Solve(Constraint constraint)
        {

            using (SolverContext context = new SolverContext())
            {
                // setup variables & their mapping
                SetupVariableMapping(constraint, context);

                // assert constraint from pure terms
                AssertPureTerms(constraint, context);

                // assert constraints from heap terms
                AssertHeapTerms(constraint, context);
                AssertMemberRootPointerEqualities(constraint, context);
                AssertRootsWithMembersAreNotNull(constraint, context);

                // assert constraints from types of location variables
                AssertTypes(constraint, context);

                if (context.CheckSat())
                {
                    return BuildModel(constraint, context);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
