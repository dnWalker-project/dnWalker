using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class BinaryBranch : Branch
    {
        private static readonly Dictionary<OpCode, ExpressionType> _operatorLookup = new Dictionary<OpCode, ExpressionType>()
        {
            [OpCodes.Beq] = ExpressionType.Equal,
            [OpCodes.Beq_S] = ExpressionType.Equal,

            [OpCodes.Bgt] = ExpressionType.GreaterThan,
            [OpCodes.Bgt_S] = ExpressionType.GreaterThan,
            [OpCodes.Bgt_Un] = ExpressionType.GreaterThan,
            [OpCodes.Bgt_Un_S] = ExpressionType.GreaterThan,

            [OpCodes.Bge] = ExpressionType.GreaterThanOrEqual,
            [OpCodes.Bge_S] = ExpressionType.GreaterThanOrEqual,
            [OpCodes.Bge_Un] = ExpressionType.GreaterThanOrEqual,
            [OpCodes.Bge_Un_S] = ExpressionType.GreaterThanOrEqual,

            [OpCodes.Blt] = ExpressionType.LessThan,
            [OpCodes.Blt_S] = ExpressionType.LessThan,
            [OpCodes.Blt_Un] = ExpressionType.LessThan,
            [OpCodes.Blt_Un_S] = ExpressionType.LessThan,

            [OpCodes.Ble] = ExpressionType.LessThanOrEqual,
            [OpCodes.Ble_S] = ExpressionType.LessThanOrEqual,
            [OpCodes.Ble_Un] = ExpressionType.LessThanOrEqual,
            [OpCodes.Ble_Un_S] = ExpressionType.LessThanOrEqual,

            [OpCodes.Bne_Un] = ExpressionType.NotEqual,
            [OpCodes.Bne_Un_S] = ExpressionType.NotEqual,
        };

        private static ExpressionType Negate(ExpressionType expressionType)
        {
            return expressionType switch
            {
                ExpressionType.Equal => ExpressionType.NotEqual,
                ExpressionType.GreaterThan => ExpressionType.LessThanOrEqual,
                ExpressionType.GreaterThanOrEqual => ExpressionType.GreaterThan,
                ExpressionType.LessThan => ExpressionType.GreaterThanOrEqual,
                ExpressionType.LessThanOrEqual => ExpressionType.GreaterThan,
                ExpressionType.NotEqual => ExpressionType.Equal,
                _ => throw new NotSupportedException(),
            };
        }

        private static Expression BuildExpression(OpCode operation, bool isTrue, Expression lhs, Expression rhs)
        {
            ExpressionType op = _operatorLookup[operation];
            if (!isTrue) op = Negate(op);

            return Expression.MakeBinary(op, lhs, rhs);
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
            Expression condition = BuildExpression(baseExecutor.Instruction.OpCode, nextInstruction != null, lhsExpression ?? lhs.AsExpression(), rhsExpression ?? rhs.AsExpression());

            SetPathConstraint(baseExecutor, nextInstruction, cur, condition);

            return retValue;
        }
    }
}
