using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Instructions;
using dnWalker.Instructions.Extensions;
using dnWalker.Parameters;
using dnWalker.Symbolic;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    /// <summary>
    /// Runs exploration with help of constraint tree.
    /// </summary>
    public class ConcolicExplorer : ExplorerBase
    {
        public ConcolicExplorer(IDefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver) : base(definitionProvider, config, logger, solver)
        {
        }

        protected override void RunCore(MethodDef entryPoint, IDictionary<string, object> data = null)
        {
            // TODO gather precondition for the entry point
            // unfold it into multiple constraint objects
            // IEnumerable<Constraint> unfoledPreconditions = entryPoint.GetPrecondition().Unfold();
            // ConstraintTreeExplorer constraintTree = new ConstraintTree(unfoldedPreconditions);
            ConstraintTreeExplorer constraintTree = new ConstraintTreeExplorer();
            
            ExplicitActiveState cur = CreateActiveState();
            cur.Services.RegisterService(constraintTree);

            Traversal.PathStore pathStore = PathStore;

            while (constraintTree.TryGetNextPrecondition(out Constraint precondition))
            {
                // get the input model
                IModel model = Solver.Solve(precondition);

                if (model == null)
                {
                    // UNSAT => try another precondition
                    continue;
                }

                // SAT => start the execution
                // TODO: just restore cur up until the decision point & update values per the expressions and model

                // new execution path
                pathStore.ResetPath(true);

                // setup explicit active state
                cur.Reset();
                MethodState mainState = new MethodState(entryPoint, cur);
                cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, Logger));

                // setup input variables & attach model
                cur.Initialize(model);

                // run the model checker
                SimpleStatistics statistics = new SimpleStatistics();
                MMC.Explorer explorer = new MMC.Explorer(cur, statistics, Logger, GetConfiguration(), PathStore);

                // TODO: remove the ParameterStore and use Model
                // OnIterationStarted(new IterationStartedEventArgs(IterationCount, ParameterStore));
                explorer.InstructionExecuted += PathStore.OnInstructionExecuted;
                explorer.Run();
                explorer.InstructionExecuted -= PathStore.OnInstructionExecuted;

                Path currentPath = pathStore.CurrentPath;
                ConstraintNode node = constraintTree.Current;
                OnPathExplored(currentPath);

                currentPath.Model = model;
                currentPath.PathConstraint = node.GetPrecondition();

                // TODO: remove the ParameterStore and use Model
                // OnIterationFinished(new IterationFinishedEventArgs(IterationCount, ParameterStore, PathStore.CurrentPath));
            }

        }
    }
}
