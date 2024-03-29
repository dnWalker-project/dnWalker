﻿using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class BinaryBranch : IInstructionExecutor
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

        private static (Expression fallThrough, Expression branch) BuildChoices(OpCode operation, Expression lhs, Expression rhs)
        {
            Operator op = _operatorLookup[operation];
            Expression fallThrough = Expression.MakeBinary(op.Negate(), lhs, rhs);
            Expression branch = Expression.MakeBinary(op, lhs, rhs);

            return (fallThrough, branch);
        }

        public IEnumerable<OpCode> SupportedOpCodes => _operatorLookup.Keys;

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!ExpressionUtils.GetExpressions(cur, lhs, rhs, out Expression lhsExpression, out Expression rhsExpression))
            {
                return retValue;
            }

            Operator op = _operatorLookup[baseExecutor.Instruction.OpCode];
            Expression nextCondition = Expression.MakeBinary(op.Negate(), lhsExpression, rhsExpression);
            Expression jumpCondition = Expression.MakeBinary(op, lhsExpression, rhsExpression);

            DecisionHelper.JumpOrNext(cur, retValue, jumpCondition, nextCondition);

            return retValue;
        }
    }
}
