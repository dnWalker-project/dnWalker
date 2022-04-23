using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Instructions;
using dnWalker.Instructions.Extensions;
using dnWalker.Parameters;
using dnWalker.Symbolic;
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
            ConstraintTreeExplorer constraintTreeExplorer = new ConstraintTreeExplorer();

            while (constraintTreeExplorer.TryGetNextPrecondition(out IPrecondition precondition))
            {
                // get the input model
                IModel model = precondition.Solve(Solver);

                // setup explicit active state
                ExplicitActiveState cur = CreateActiveState();
                MethodState mainState = new MethodState(entryPoint, cur);
                cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, Logger));
                cur.CurrentThread.InstructionExecuted += PathStore.OnInstructionExecuted;

                // setup input variables & attach model
                cur.Initialize(model);

                cur.AttachService(constraintTreeExplorer);

                // run the model checker
                SimpleStatistics statistics = new SimpleStatistics();
                MMC.Explorer explorer = new MMC.Explorer(cur, statistics, Logger, GetConfiguration(), PathStore);

                // TODO: remove the ParameterStore and use Model
                // OnIterationStarted(new IterationStartedEventArgs(IterationCount, ParameterStore));
                explorer.InstructionExecuted += PathStore.OnInstructionExecuted;
                explorer.Run();
                explorer.InstructionExecuted -= PathStore.OnInstructionExecuted;

                OnPathExplored(PathStore.CurrentPath);

                // TODO: remove the ParameterStore and use Model
                // OnIterationFinished(new IterationFinishedEventArgs(IterationCount, ParameterStore, PathStore.CurrentPath));

                PathStore.ResetPath();
            }

        }
    }
}
