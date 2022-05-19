using dnlib.DotNet;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    /// <summary>
    /// A tree which tracks the decisions in a execution path.
    /// </summary>
    public class ConstraintTree
    {
        private readonly ConstraintNode _root;

        public ConstraintTree(Constraint precondition, ControlFlowNode entryNode)
        {
            _root = new PreconditionNode(this, precondition, entryNode);
        }

        public ConstraintNode Root => _root;

        public static IEnumerable<ConstraintTree> UnfoldConstraints(MethodDef entryPoint, ControlFlowNode entryNode)
        {
            // infer preconditions, for example from attributes over arguments
            // ensure 'this' is not null
            // etc

            ConstraintTree[] trees = new ConstraintTree[1];

            trees[0] = new ConstraintTree(new Constraint(), entryNode);

            return trees;
        }
    }
}
