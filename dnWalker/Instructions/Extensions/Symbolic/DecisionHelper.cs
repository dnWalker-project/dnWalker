using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.DataElements;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public static class DecisionHelper
    {
        public delegate Expression EdgeExpressionFactory(ExplicitActiveState cur, ControlFlowEdge edge);
        public delegate Expression EdgeExpressionFactory<T>(ExplicitActiveState cur, ControlFlowEdge edge, T context);
        public delegate Expression EdgeExpressionFactory<T1, T2>(ExplicitActiveState cur, ControlFlowEdge edge, T1 ctx1, T2 ctx2);
        public delegate Expression EdgeExpressionFactory<T1, T2, T3>(ExplicitActiveState cur, ControlFlowEdge edge, T1 ctx1, T2 ctx2, T3 ctx3);

        private static InstructionBlockNode GetCurrentControlFlowNode(ExplicitActiveState cur)
        {
            //ControlFlowGraph graph = ;
            //explorer.Graph.GetInstructionNode(cur.CurrentLocation.Instruction);
            return cur.PathStore.ControlFlowGraphProvider.Get(cur.CurrentLocation.Method).GetInstructionNode(cur.CurrentLocation.Instruction);
        }

        ///// <summary>
        ///// Finds the index within provided successors array, such that it matches the provided return value.
        ///// </summary>
        ///// <param name="current"></param>
        ///// <param name="successors"></param>
        ///// <param name="returnValue"></param>
        ///// <returns></returns>
        //private static int FindChosenSuccessor(ExplicitActiveState cur, ControlFlowNode current, ControlFlowNode[] successors, IIEReturnValue returnValue)
        //{
        //    for (int i = 0; i < successors.Length; ++i)
        //    {
        //        if (IsMatch(cur, current.GetEdgeTo(successors[i]), returnValue))
        //        {
        //            return i;
        //        }
        //    }

        //    return -1;

        //    static bool IsMatch(ExplicitActiveState cur, ControlFlowEdge edge, IIEReturnValue returnValue)
        //    {
        //        return (edge, returnValue) switch
        //        {
        //            (NextEdge _, NextReturnValue _) => true,
        //            (JumpEdge je, JumpReturnValue jrv) => ((InstructionBlockNode)je.Target).Contains(jrv.Target),
        //            (ExceptionEdge ex, ExceptionHandlerLookupReturnValue _) =>
        //                dnlib.DotNet.TypeEqualityComparer.Instance.Equals(
        //                    ((AllocatedObject)cur.DynamicArea.Allocations[cur.CurrentThread.ExceptionReference]).Type,
        //                    ex.ExceptionType),
        //            _ => false
        //        };

        //    }
        //}

        //public static void MakeDecision<TContext>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<TContext> expressionBuilder, TContext context)
        //{
        //    InstructionBlockNode currentNode = GetControlFlowNode(cur);
        //    ControlFlowNode[] successors = currentNode.Successors.ToArray();

        //    Expression[] choiceExpression = new Expression[successors.Length];
        //    for (int i = 0; i < successors.Length; ++i)
        //    {
        //        choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), context);
        //    }

        //    int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

        //    MakeDecision(cur, decision, successors, choiceExpression);
        //}
        //public static void MakeDecision<T1, T2>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<T1, T2> expressionBuilder, T1 ctx1, T2 ctx2)
        //{
        //    InstructionBlockNode currentNode = GetControlFlowNode(cur);
        //    ControlFlowNode[] successors = currentNode.Successors.ToArray();

        //    Expression[] choiceExpression = new Expression[successors.Length];
        //    for (int i = 0; i < successors.Length; ++i)
        //    {
        //        choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), ctx1, ctx2);
        //    }

        //    int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

        //    MakeDecision(cur, decision, successors, choiceExpression);
        //}
        //public static void MakeDecision<T1, T2, T3>(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory<T1, T2, T3> expressionBuilder, T1 ctx1, T2 ctx2, T3 ctx3)
        //{
        //    InstructionBlockNode currentNode = GetControlFlowNode(cur);
        //    ControlFlowNode[] successors = currentNode.Successors.ToArray();

        //    Expression[] choiceExpression = new Expression[successors.Length];
        //    for (int i = 0; i < successors.Length; ++i)
        //    {
        //        choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]), ctx1, ctx2, ctx3);
        //    }

        //    int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

        //    MakeDecision(cur, decision, successors, choiceExpression);
        //}

        //public static void MakeDecision(ExplicitActiveState cur, IIEReturnValue returnValue, EdgeExpressionFactory expressionBuilder)
        //{
        //    InstructionBlockNode currentNode = GetControlFlowNode(cur);
        //    ControlFlowNode[] successors = currentNode.Successors.ToArray();

        //    Expression[] choiceExpression = new Expression[successors.Length];
        //    for (int i = 0; i < successors.Length; ++i)
        //    {
        //        choiceExpression[i] = expressionBuilder(cur, currentNode.GetEdgeTo(successors[i]));
        //    }

        //    int decision = FindChosenSuccessor(cur, currentNode, successors, returnValue);

        //    MakeDecision(cur, decision, successors, choiceExpression);
        //}


        //private static void MakeDecision(ExplicitActiveState cur, int decision, ControlFlowNode[] choiceTargets, Expression[] choiceExpressions)
        //{
        //    cur.Services.GetService<ConstraintTreeExplorer>().MakeDecision(cur, decision, choiceTargets, choiceExpressions);
        //}

        private static dnlib.DotNet.ITypeDefOrRef GetCurrentException(ExplicitActiveState cur)
        {
            if (!cur.CurrentThread.ExceptionReference.IsNull())
            {
                return ((AllocatedObject)cur.DynamicArea.Allocations[cur.CurrentThread.ExceptionReference]).Type;
            }

            return null;
        }

        public static void ArrayElementAccess(ExplicitActiveState cur, IIEReturnValue returnValue, Expression arrayExpression, Expression indexExpression)
        {
            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

            Debug.Assert(choiceTargets.Length == 3, "For ArrayElementAccess there should be exactly 3 options.");

            ExpressionFactory ef = cur.GetExpressionFactory();
            IVariable arrayVariable = ((VariableExpression)arrayExpression).Variable;

            Expression nullExcCondition = Expression.MakeEqual(arrayExpression, ef.NullExpression);
            Expression outOfRangeExcCondition = Expression.MakeAnd(
                    Expression.MakeNot(nullExcCondition),
                    Expression.MakeOr(
                        Expression.MakeLessThan(indexExpression, ef.MakeIntegerConstant(0)),
                        Expression.MakeGreaterThanOrEqual(indexExpression, Expression.MakeVariable(Variable.ArrayLength(arrayVariable)))));
            Expression nextCondition = Expression.MakeAnd(Expression.MakeNot(nullExcCondition), Expression.MakeNot(outOfRangeExcCondition));

            Expression[] conditions = new Expression[3];
            for (int i = 0; i < conditions.Length; ++i)
            {
                conditions[i] = choiceTargets[i] switch
                {
                    NextEdge _ => nextCondition,
                    ExceptionEdge { ExceptionType.FullName : "System.NullReferenceException" } => nullExcCondition,
                    ExceptionEdge { ExceptionType.FullName : "System.IndexOutOfRangeException" } => outOfRangeExcCondition,
                    { } e => throw new InvalidOperationException($"Invalid control flow edge: {e}")
                };
            }

            int decision = 0;
            for (int i = 0; i < choiceTargets.Length; ++i)
            {
                if (choiceTargets[i] is NextEdge && returnValue is NextReturnValue)
                {
                    decision = i;
                    break;
                }

                if (choiceTargets[i] is ExceptionEdge edge && GetCurrentException(cur)?.Equals(edge.ExceptionType) == true)
                {
                    decision = i;
                    break;
                }
            }

            MakeDecision(cur, decision, choiceTargets, conditions);
        }

        public static void JumpOrNext(ExplicitActiveState cur, IIEReturnValue returnValue, Expression jumpCondition, Expression nextCondition)
        {
            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

            Debug.Assert(choiceTargets.Length == 2, "For JumpOrNext there should be exactly 2 options.");

            if (choiceTargets[0] is NextEdge)
            {
                int index = returnValue is NextReturnValue ? 0 : 1;
                MakeDecision(cur, index, choiceTargets, new Expression[] { nextCondition, jumpCondition });
            }
            else
            {
                int index = returnValue is NextReturnValue ? 1 : 0;
                MakeDecision(cur, index, choiceTargets, new Expression[] { jumpCondition, nextCondition });
            }
        }

        public static void ThrowZeroOrNext(ExplicitActiveState cur, IIEReturnValue returnValue, Expression valueExpression)
        {
            ExpressionFactory ef = cur.GetExpressionFactory();

            if (!valueExpression.Type.IsInteger())
            {
                // we throw DivideByZero exception only if the divider is integer value

                //ControlFlowEdge[] nextEdge = GetCurrentControlFlowNode(cur).OutEdges.OfType<NextEdge>().ToArray();
                //Expression[] condition = new Expression[] { ef.MakeBooleanConstant(true) };

                //MakeDecision(cur, 0, nextEdge, condition);

                // this code makes sens - one ConstraintNode per CFGNode, but it needlessly makes the strategy to repeat
                // should be used, if there is check 
                // if (constraintNode.Condition == TrueCondition || constraintNode.Condition == FalseCondition) => skip it

                return;
            }
            else
            {
                ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

                Debug.Assert(choiceTargets.Length == 2, "For ThrowZeroOrNext there should be exactly 2 options.");


                Expression isZero = Expression.MakeEqual(valueExpression, ef.MakeIntegerConstant(0));
                Expression isNotZero = Expression.MakeNotEqual(valueExpression, ef.MakeIntegerConstant(0));

                if (choiceTargets[0] is NextEdge)
                {
                    int index = returnValue is NextReturnValue ? 0 : 1;
                    MakeDecision(cur, index, choiceTargets, new Expression[] { isNotZero, isZero });
                }
                else
                {
                    int index = returnValue is NextReturnValue ? 1 : 0;
                    MakeDecision(cur, index, choiceTargets, new Expression[] { isZero, isNotZero });
                }
            }
        }

        public static void ThrowNullOrNext(ExplicitActiveState cur, IIEReturnValue returnValue, Expression instanceExpression)
        {
            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

            Debug.Assert(choiceTargets.Length == 2, "For ThrowNullOrNext there should be exactly 2 options.");

            ExpressionFactory ef = cur.GetExpressionFactory();

            Expression isNull = Expression.MakeEqual(instanceExpression, ef.NullExpression);
            Expression isNotNull = Expression.MakeNotEqual(instanceExpression, ef.NullExpression);

            if (choiceTargets[0] is NextEdge)
            {
                int index = returnValue is NextReturnValue ? 0 : 1;
                MakeDecision(cur, index, choiceTargets, new Expression[] { isNotNull, isNull });
            }
            else
            {
                int index = returnValue is NextReturnValue ? 1 : 0;
                MakeDecision(cur, index, choiceTargets, new Expression[] { isNull, isNotNull });
            }
        }

        public static void Switch(ExplicitActiveState cur, IIEReturnValue returnValue, Expression expression, Instruction[] targets)
        {
            int n = targets.Length;

            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

            Debug.Assert(choiceTargets.Length == n + 1, "For Switch there should be exactly (n + 1) options, where n is number of cases");

            ExpressionFactory ef = cur.GetExpressionFactory();

            Expression[] conditions = new Expression[n + 1];
            for (int i = 0; i < choiceTargets.Length; i++)
            {
                conditions[i] = choiceTargets[i] switch
                {
                    // next edge => default case or no jump => value must be greater than or equal to the number of cases
                    NextEdge => Expression.MakeGreaterThanOrEqual(expression, ef.MakeIntegerConstant(n)),
                    JumpEdge j => Expression.MakeEqual(expression, ef.MakeIntegerConstant(Array.IndexOf(targets, ((InstructionBlockNode)j.Target).Header))),
                    { } e => throw new InvalidOperationException($"Invalid control flow edge: {e}")
                };
            }

            int decision = 0;
            for (int i = 0; i < choiceTargets.Length; ++i)
            {
                if (choiceTargets[i] is NextEdge && returnValue is NextReturnValue)
                {
                    decision = i;
                    break;
                }

                if (choiceTargets[i] is JumpEdge edge && 
                    returnValue is JumpReturnValue j && 
                    ((InstructionBlockNode)edge.Target).Header == j.Target)
                {
                    decision = i;
                    break;
                }
            }

            MakeDecision(cur, decision, choiceTargets, conditions);
        }

        public static void MakeDecision(ExplicitActiveState cur, int decision, ControlFlowEdge[] choiceTargets, Expression[] choiceExpressions)
        {
            cur.Services.GetService<ConstraintTreeExplorer>().MakeDecision(cur, decision, choiceTargets, choiceExpressions);
        }


        //public static void AddConstraint(ExplicitActiveState cur, Expression constraintExpression)
        //{
        //    ConstraintTreeExplorer ctExplorer = cur.Services.GetService<ConstraintTreeExplorer>();
        //    ctExplorer.Current.Constraint.AddExpressionConstraint(constraintExpression);
        //}

        //public static void AddConstraints(ExplicitActiveState cur, Expression constraintExpression1, Expression constraintExpression2)
        //{
        //    ConstraintTreeExplorer ctExplorer = cur.Services.GetService<ConstraintTreeExplorer>();
        //    ctExplorer.Current.AddConstraint(constraintExpression1);
        //    ctExplorer.Current.AddConstraint(constraintExpression2);
        //}

        //public static void AddConstraint(ExplicitActiveState cur, params Expression[] constraintExpressions)
        //{
        //    ConstraintTreeExplorer ctExplorer = cur.Services.GetService<ConstraintTreeExplorer>();
        //    ctExplorer.Current.AddConstraints(constraintExpressions);
        //}

        //public static void AddVariableRangeConstraint(ExplicitActiveState cur, VariableExpression variable, Expression min, Expression max)
        //{
        //    AddConstraints(cur, Expression.MakeGreaterThanOrEqual(variable, min), Expression.MakeLessThanOrEqual(variable, max));
        //}
    }
}
