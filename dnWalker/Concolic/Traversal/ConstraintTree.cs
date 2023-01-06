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
            _root = new ConstraintNode(this, null, null, precondition);
        }

        public ConstraintNode Root => _root;

        public static IReadOnlyList<ConstraintTree> UnfoldConstraints(MethodDef entryPoint, ControlFlowNode entryNode, ExpressionFactory expressionFactory = null)
        {
            ExpressionFactory ef = expressionFactory ?? ExpressionFactory.Default;
            // infer preconditions, for example from attributes over arguments
            // ensure 'this' is not null
            // etc

            ConstraintTree[] trees = new ConstraintTree[1];

            if (entryPoint.HasThis)
            {
                Expression thisIsNotNull = ef.MakeNotEqual(ef.MakeVariable(Variable.MethodArgument(entryPoint.Parameters[0])), ef.NullExpression);

                trees[0] = new ConstraintTree(new Constraint(new PureTerm[] { new BooleanExpressionTerm(thisIsNotNull) }, Array.Empty<HeapTerm>()), entryNode);
            }
            else
            {
                trees[0] = new ConstraintTree(new Constraint(), entryNode);
            }


            return trees;
        }

        public static IReadOnlyList<ConstraintTree> UnfoldConstraints(MethodDef entryPoint, ControlFlowNode entryNode, IEnumerable<Constraint> constraints, ExpressionFactory expressionFactory = null)
        {
            ExpressionFactory ef = expressionFactory ?? ExpressionFactory.Default;

            List<ConstraintTree> unfolded = new List<ConstraintTree>();
            foreach (Constraint constraint in constraints)
            {
                unfolded.Add(new ConstraintTree(constraint, entryNode));
            }

            if (unfolded.Count == 0)
            {
                return UnfoldConstraints(entryPoint, entryNode);
            }

            return unfolded;
        }

        public IEnumerable<ConstraintNode> EnumerateNodes()
        {
            Stack<ConstraintNode> frontier = new Stack<ConstraintNode>();
            frontier.Push(_root);

            while (frontier.TryPop(out ConstraintNode cn))
            {
                yield return cn;

                foreach (ConstraintNode child in cn.Children)
                {
                    frontier.Push(child);
                }
            }
        }
    }
}
