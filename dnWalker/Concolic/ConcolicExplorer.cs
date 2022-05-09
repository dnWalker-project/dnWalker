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

            // TODO: handle via configuration
            ConstraintTreeExplorer constraintTree = new ConstraintTreeExplorer(new AllPathsCoverage());
            //ConstraintTreeExplorer constraintTree = new ConstraintTreeExplorer(new AllConditionsCoverage());

            ExplicitActiveState cur = CreateActiveState();
            cur.Services.RegisterService(constraintTree);

            Traversal.PathStore pathStore = PathStore;

            while (constraintTree.TryGetNextConstraintNode(out ConstraintNode currentConstraintNode))
            {
                // get the input model
                IModel model = Solver.Solve(currentConstraintNode.GetPrecondition());

                if (model == null)
                {
                    // UNSAT => try another precondition
                    currentConstraintNode.MarkUnsatisfiable();
                    continue;
                }

                // SAT => start the execution
                // TODO: just restore cur up until the decision point & update values per the expressions and model

                // new execution path
                pathStore.ResetPath(true);

                // setup explicit active state
                cur.Reset();
                
                SimpleStatistics statistics = new SimpleStatistics();
                // creating the explorer before main thread is created
                // - no need to do explicit (a.k.a. error prone) registration of events
                MMC.Explorer explorer = new MMC.Explorer(cur, statistics, Logger, GetConfiguration(), PathStore);

                MethodState mainState = new MethodState(entryPoint, cur);
                cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, Logger));

                // setup input variables & attach model
                cur.Initialize(model);


                // run the model checker
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

            // TODO: make it as a explorer extension...
            ConstraintTreeExplorerWriter.Write(constraintTree, "constraintTree.dot");
        }
    }
}
