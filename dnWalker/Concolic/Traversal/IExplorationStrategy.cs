
using dnlib.DotNet;

using dnWalker.Configuration;
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
        /// <summary>
        /// Invoked before the execution.
        /// </summary>
        /// <param name="activeState"></param>
        /// <param name="entryPoint"></param>
        void Initialize(ExplicitActiveState activeState, MethodDef entryPoint, IConfiguration configuration);

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
