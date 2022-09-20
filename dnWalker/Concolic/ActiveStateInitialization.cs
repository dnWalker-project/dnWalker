using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public static class ActiveStateInitialization
    {
        public static ConstraintTreeExplorer InitializeConcolicExploration(this ExplicitActiveState cur, MethodDef entryPoint, IExplorationStrategy strategy)
        {
            ControlFlowGraph entryCfg = cur.PathStore.ControlFlowGraphProvider.Get(entryPoint);
            IReadOnlyList<ConstraintTree> constraintTrees = ConstraintTree.UnfoldConstraints(entryPoint, entryCfg.EntryPoint);
            ConstraintTreeExplorer constraintTree = new ConstraintTreeExplorer(strategy, constraintTrees);

            cur.Services.RegisterService(constraintTree);

            strategy.Initialize(cur, entryPoint);

            return constraintTree;
        }
        public static ConstraintTreeExplorer InitializeConcolicExploration(this ExplicitActiveState cur, MethodDef entryPoint, IExplorationStrategy strategy, IEnumerable<IReadOnlyModel> inputModels)
        {
            ControlFlowGraph entryCfg = cur.PathStore.ControlFlowGraphProvider.Get(entryPoint);

            ExpressionFactory ef = cur.GetExpressionFactory();
            IReadOnlyList<ConstraintTree> constraintTrees = ConstraintTree.UnfoldConstraints(entryPoint, entryCfg.EntryPoint, inputModels, ef);
            ConstraintTreeExplorer constraintTree = new ConstraintTreeExplorer(strategy, constraintTrees);

            cur.Services.RegisterService(constraintTree);

            strategy.Initialize(cur, entryPoint);

            return constraintTree;
        }
    }
}
