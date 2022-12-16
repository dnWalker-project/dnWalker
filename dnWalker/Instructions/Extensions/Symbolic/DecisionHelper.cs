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

        public static void Assert(ExplicitActiveState cur, bool passed, Expression passCondition, Expression violationCondition)
        {
            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();

            Debug.Assert(choiceTargets.Length == 2, "For Assert there should be exactly 2 options.");

            if (choiceTargets[0] is NextEdge)
            {
                int index = passed ? 0 : 1;
                MakeDecision(cur, index, choiceTargets, new Expression[] { passCondition, violationCondition });
            }
            else
            {
                int index = passed ? 1 : 0;
                MakeDecision(cur, index, choiceTargets, new Expression[] { violationCondition, passCondition });
            }
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

            Expression nullExpression = instanceExpression.Type.IsString() ? ef.StringNullExpression : ef.NullExpression;

            Expression isNull = Expression.MakeEqual(instanceExpression, nullExpression);
            Expression isNotNull = Expression.MakeNotEqual(instanceExpression, nullExpression);

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

        public static void Switch(ExplicitActiveState cur, int index, Expression expression, Instruction[] targets)
        {
            int n = targets.Length;

            ControlFlowEdge[] choiceTargets = GetCurrentControlFlowNode(cur).OutEdges.ToArray();
            // [0]   ... 1st target
            // [1]   ... 2nd target
            // [n-1] ... n-th target
            // [n]   ... next instruction

            Debug.Assert(choiceTargets.Length == n + 1, "For Switch there should be exactly (n + 1) options, where n is number of cases");

            ExpressionFactory ef = cur.GetExpressionFactory();

            Expression[] conditions = new Expression[n + 1];
            for (int i = 0; i < targets.Length; i++)
            {
                conditions[i] = Expression.MakeEqual(expression, ef.MakeIntegerConstant(i));
            }
            conditions[n] = Expression.MakeGreaterThanOrEqual(expression, ef.MakeIntegerConstant(n));

            int decision = Math.Min(index, n);

            MakeDecision(cur, decision, choiceTargets, conditions);
        }

        public static void MakeDecision(ExplicitActiveState cur, int decision, ControlFlowEdge[] choiceTargets, params Expression[] choiceExpressions)
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
