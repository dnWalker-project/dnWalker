using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class BinaryBranch : Branch
    {
        private static readonly Dictionary<OpCode, Operator> _operatorLookup = new Dictionary<OpCode, Operator>()
        {
            [OpCodes.Beq] = Operator.Equal,
            [OpCodes.Beq_S] = Operator.Equal,

            [OpCodes.Bgt] = Operator.GreaterThan,
            [OpCodes.Bgt_S] = Operator.GreaterThan,
            [OpCodes.Bgt_Un] = Operator.GreaterThan,
            [OpCodes.Bgt_Un_S] = Operator.GreaterThan,

            [OpCodes.Bge] = Operator.GreaterThanOrEqual,
            [OpCodes.Bge_S] = Operator.GreaterThanOrEqual,
            [OpCodes.Bge_Un] = Operator.GreaterThanOrEqual,
            [OpCodes.Bge_Un_S] = Operator.GreaterThanOrEqual,

            [OpCodes.Blt] = Operator.LessThan,
            [OpCodes.Blt_S] = Operator.LessThan,
            [OpCodes.Blt_Un] = Operator.LessThan,
            [OpCodes.Blt_Un_S] = Operator.LessThan,

            [OpCodes.Ble] = Operator.LessThanOrEqual,
            [OpCodes.Ble_S] = Operator.LessThanOrEqual,
            [OpCodes.Ble_Un] = Operator.LessThanOrEqual,
            [OpCodes.Ble_Un_S] = Operator.LessThanOrEqual,

            [OpCodes.Bne_Un] = Operator.NotEqual,
            [OpCodes.Bne_Un_S] = Operator.NotEqual,
        };

        private static Operator Negate(Operator expressionType)
        {
            return expressionType switch
            {
                Operator.Equal => Operator.NotEqual,
                Operator.GreaterThan => Operator.LessThanOrEqual,
                Operator.GreaterThanOrEqual => Operator.LessThan,
                Operator.LessThan => Operator.GreaterThanOrEqual,
                Operator.LessThanOrEqual => Operator.GreaterThan,
                Operator.NotEqual => Operator.Equal,
                _ => throw new NotSupportedException(),
            };
        }

        private static (Expression fallThrough, Expression branch) BuildChoices(OpCode operation, Expression lhs, Expression rhs)
        {
            Operator op = _operatorLookup[operation];
            Expression fallThrough = Expression.MakeBinary(Negate(op), lhs, rhs);
            Expression branch = Expression.MakeBinary(op, lhs, rhs);

            return (fallThrough, branch);
        }

        public override IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _operatorLookup.Keys;
            }
        }


        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            IIEReturnValue retValue = next(baseExecutor, cur);

            bool lhsSymbolic = lhs.TryGetExpression(cur, out Expression lhsExpression);
            bool rhsSymbolic = rhs.TryGetExpression(cur, out Expression rhsExpression);

            if (!lhsSymbolic && !rhsSymbolic)
            {
                return retValue;
            }

            Instruction nextInstruction = GetNextInstruction(retValue, cur);

            (Expression fallThrough, Expression branch) = BuildChoices(baseExecutor.Instruction.OpCode, lhsExpression ?? lhs.AsExpression(), rhsExpression ?? rhs.AsExpression());

            cur.GetConstraintTree().MakeDecision(nextInstruction != null ? 1 : 0, fallThrough, branch);

            //SetPathConstraint(baseExecutor, nextInstruction, cur, condition);

            return retValue;
        }
    }
}
