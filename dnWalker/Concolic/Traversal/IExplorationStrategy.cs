
using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public interface IExplorationStrategy
    {
        ///// <summary>
        ///// Determine whether specified constraint node has been explored.
        ///// </summary>
        ///// <param name="constraintNode"></param>
        ///// <returns>True if already explored, false otherwise.</returns>
        //bool ShouldBeExplored(ConstraintNode constraintNode);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="constraintNode"></param>
        //void OnNodeExplored(ConstraintNode constraintNode);

        //void OnUnsatisfiableNodePruned(ConstraintNode constraintNode);

        //void NewIteration();

        /// <summary>
        /// Invoked before the execution.
        /// </summary>
        /// <param name="activeState"></param>
        /// <param name="entryPoint"></param>
        void Initialize(ExplicitActiveState activeState, MethodDef entryPoint);

        /// <summary>
        /// Produces next input model.
        /// </summary>
        /// <param name="solver"></param>
        /// <param name="inputModel"></param>
        /// <returns>True if new input model was created, false if exploration is finished.</returns>
        bool TryGetNextSatisfiableNode(ISolver solver, out ConstraintNode node,  out IModel inputModel);

        /// <summary>
        /// Adds new discovered node for the search.
        /// </summary>
        /// <param name="nodes"></param>
        void AddDiscoveredNode(ConstraintNode node);

        /// <summary>
        /// Adds explored node.
        /// </summary>
        void AddExploredNode(ConstraintNode node);
    }
}
