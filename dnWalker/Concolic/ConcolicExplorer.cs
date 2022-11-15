using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Instructions;
using dnWalker.Instructions.Extensions;
using dnWalker.Symbolic;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ConcolicExplorer(IDefinitionProvider definitionProvider, IConfiguration config, Logger logger, ISolver solver) : base(definitionProvider, config, logger, solver)
        {
        }

        protected override ExplorationResult RunCore(MethodDef entryPoint, PathStore pathStore, ExplicitActiveState cur, IDictionary<string, object> data = null)
        {
            DateTime explorationStart = DateTime.Now;

            IConfiguration configuration = Configuration;

            ConstraintTreeExplorer constraintTrees = cur.InitializeConcolicExploration(entryPoint, configuration.CreateStrategy(), configuration.GetInputModels(entryPoint, DefinitionProvider));

            // TODO: make it as a explorer extension...
            if (!ControlFlowGraphWriter.TryWrite(ControlFlowGraph.Build(entryPoint), "cfg.dot")) Logger.Warning("CFG writer failed.");

            List<ExplorationIterationResult> iterationResults = new List<ExplorationIterationResult>();
            int currentIteration = 0;
            int maxIterations = GetMaxIterations(configuration);
                
            DateTime iterationStarted = DateTime.Now;
            while (constraintTrees.TryGetNextInputModel(Solver, out IModel inputModel))
            {
                NextIterationOrThrow(ref currentIteration, maxIterations);

                // SAT => start the execution
                // TODO: just restore cur up until the decision point & update values per the expressions and model

                // new execution path
                Path currentPath = pathStore.ResetPath(true);

                // setup explicit active state
                cur.Reset();

                SimpleStatistics statistics = new SimpleStatistics();
                // creating the explorer before main thread is created
                // - no need to do explicit (a.k.a. error prone) registration of events
                MMC.Explorer explorer = new MMC.Explorer(cur, statistics, Logger, Configuration, pathStore);

                MethodState mainState = new MethodState(entryPoint, cur);
                cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, Logger));

                // setup input variables & attach model
                cur.Initialize(inputModel);

                OnIterationStarted(new IterationStartedEventArgs(currentIteration, inputModel));


                // run the model checker
                explorer.Run();

                // the path may have changed??
                currentPath = pathStore.CurrentPath;
                OnPathExplored(currentPath);
                
                // build iteration result
                ConstraintNode node = constraintTrees.Current;
                Constraint preCondition = inputModel.Precondition;
                Constraint postCondition = node.GetPrecondition();
                SymbolicContext symbolicContext = currentPath.GetSymbolicContext();

                DateTime iterationFinished = DateTime.Now;

                ExplorationIterationResult iterationResult = new ExplorationIterationResult(currentIteration, currentPath, symbolicContext, preCondition, postCondition, explorer.Statistics, iterationStarted, iterationFinished);
                iterationResults.Add(iterationResult);

                OnIterationFinished(new IterationFinishedEventArgs(iterationResult, currentPath));
                iterationStarted = DateTime.Now;
            }

            DateTime explorationFinished = DateTime.Now;

            // TODO: make it as a explorer extension...
            if (!ConstraintTreeExplorerWriter.TryWrite(constraintTrees, $"constraintTree.dot")) Logger.Warning("ConstraintTree writer failed.");

            ExplorationResult result = new ExplorationResult(entryPoint, iterationResults, constraintTrees.Trees, pathStore.MethodTracerProvider.Get(entryPoint), explorationStart, explorationFinished);
            return result;

            static void NextIterationOrThrow(ref int currentIteration, int maxIterations)
            {
                if (currentIteration >= maxIterations) throw new MaxIterationsExceededException(currentIteration);
                currentIteration++;
            }

            static int GetMaxIterations(IConfiguration config)
            {
                int maxIterations = config.MaxIterations();
                if (maxIterations <= 0) return int.MaxValue;
                return maxIterations;
            }
        }

    }
}
