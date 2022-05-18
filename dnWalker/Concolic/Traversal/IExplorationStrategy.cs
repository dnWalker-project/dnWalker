
using dnlib.DotNet;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public interface IExplorationStrategy
    {
        /// <summary>
        /// Determine whether specified constraint node has been explored.
        /// </summary>
        /// <param name="constraintNode"></param>
        /// <returns>True if already explored, false otherwise.</returns>
        bool ShouldBeExplored(ConstraintNode constraintNode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constraintNode"></param>
        void OnNodeExplored(ConstraintNode constraintNode);

        void OnUnsatisfiableNodePruned(ConstraintNode constraintNode);

        void NewIteration();

        void Initialize(ExplicitActiveState activeState, MethodDef entryPoint);
    }
}
