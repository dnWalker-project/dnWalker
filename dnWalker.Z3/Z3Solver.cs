using dnWalker.Symbolic;

using Microsoft.Z3;

using System.Collections.Generic;

using IVariable = dnWalker.Symbolic.IVariable;
using System;
using System.Diagnostics;

namespace dnWalker.Z3
{
    public partial class Z3Solver : ISolver
    {
        private ref struct SolverContext
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
            SolverContext context = new SolverContext(); // ref struct cannot implement IDisposable
            try
            {
                // setup variables & their mapping
                SetupVariableMapping(constraint, ref context);

                // assert constraint from pure terms
                AssertPureTerms(constraint, ref context);

                // assert constraints from heap terms
                AssertHeapTerms(constraint, ref context);
                AssertMemberRootPointerEqualities(constraint, ref context);

                // assert constraints from types of location variables
                AssertTypes(constraint, ref context);

                if (context.CheckSat())
                {
                    return BuildModel(ref context);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception: {e.GetType()}:{e.Message}");
                throw;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
