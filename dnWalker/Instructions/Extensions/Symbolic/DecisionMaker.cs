using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public static class DecisionMaker
    {
        public delegate Expression EdgeExpressionFactory(ExplicitActiveState cur, ControlFlowEdge edge);
        public delegate Expression EdgeExpressionFactory<T>(ExplicitActiveState cur, ControlFlowEdge edge, T context);
        public delegate Expression EdgeExpressionFactory<T1,T2>(ExplicitActiveState cur, ControlFlowEdge edge, T1 ctx1, T2 ctx2);
        public delegate Expression EdgeExpressionFactory<T1,T2,T3>(ExplicitActiveState cur, ControlFlowEdge edge, T1 ctx1, T2 ctx2, T3 ctx3);

        private static InstructionBlockNode GetControlFlowNode(ExplicitActiveState cur)
        {
            //ControlFlowGraph graph = ;
            //explorer.Graph.GetInstructionNode(cur.CurrentLocation.Instruction);
            return cur.PathStore.ControlFlowGraphProvider.Get(cur.CurrentLocation.Method).GetInstructionNode(cur.CurrentLocation.Instruction); 
        }

        /// <summary>
        /// Finds the index within provided successors array, such that it matches the provided return value.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="successors"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        private static int FindChosenSuccessor(ExplicitActiveState cur, ControlFlowNode current, ControlFlowNode[] successors, IIEReturnValue returnValue)
        {
            for (int i = 0; i < successors.Length; ++i)
            {
                if (IsMatch(cur, current.GetEdgeTo(successors[i]), returnValue))
                {
                    return i;
                }
            }

            return -1;

            static bool IsMatch(ExplicitActiveState cur, ControlFlowEdge edge, IIEReturnValue returnValue)
            {
                return (edge, returnValue) switch
                {
                    (NextEdge _, NextReturnValue _) => true,
                    (JumpEdge je, JumpReturnValue jrv) => ((InstructionBlockNode)je.Target).Contains(jrv.Target),
                    (ExceptionEdge ex, ExceptionHandlerLookupReturnValue _) =>
                        dnlib.DotNet.TypeEqualityComparer.Instance.Equals(
                            ((AllocatedObject)cur.DynamicArea.Allocations[cur.CurrentThread.ExceptionReference]).Type,
                            ex.ExceptionType),
                    _ => false
                };

            }
        }

        public static void MakeDecision<TContext>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<TContext> expressionBuilder, TContext context)
        {
            InstructionBlockNode currentNode = GetControlFlowNode(cur);
            ControlFlowNode[] successors = currentNode.Successors.ToArray();

            Expression[] choiceExpression = new Expression[successors.Length];
            for (int i = 0; i < successors.Length; ++i)
            {
                choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), context);
            }

            int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

            MakeDecision(cur, decision, successors, choiceExpression);
        }
        public static void MakeDecision<T1,T2>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<T1,T2> expressionBuilder, T1 ctx1, T2 ctx2)
        {
            InstructionBlockNode currentNode = GetControlFlowNode(cur);
            ControlFlowNode[] successors = currentNode.Successors.ToArray();

            Expression[] choiceExpression = new Expression[successors.Length];
            for (int i = 0; i < successors.Length; ++i)
            {
                choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), ctx1, ctx2);
            }

            int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

            MakeDecision(cur, decision, successors, choiceExpression);
        }
        public static void MakeDecision<T1, T2, T3>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<T1,T2,T3> expressionBuilder, T1 ctx1, T2 ctx2, T3 ctx3)
        {
            InstructionBlockNode currentNode = GetControlFlowNode(cur);
            ControlFlowNode[] successors = currentNode.Successors.ToArray();

            Expression[] choiceExpression = new Expression[successors.Length];
            for (int i = 0; i < successors.Length; ++i)
            {
                choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), ctx1, ctx2, ctx3);
            }

            int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

            MakeDecision(cur, decision, successors, choiceExpression);
        }

        public static void MakeDecision(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory expressionBuilder)
        {
            InstructionBlockNode currentNode = GetControlFlowNode(cur);
            ControlFlowNode[] successors = currentNode.Successors.ToArray();

            Expression[] choiceExpression = new Expression[successors.Length];
            for (int i = 0; i < successors.Length; ++i)
            {
                choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]));
            }

            int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

            MakeDecision(cur, decision, successors, choiceExpression);
        }


        private static void MakeDecision(ExplicitActiveState cur, int decision, ControlFlowNode[] choiceTargets, Expression[] choiceExpressions)
        {
            cur.Services.GetService<ConstraintTreeExplorer>().MakeDecision(cur, decision, choiceTargets, choiceExpressions);
        }

    }
}
