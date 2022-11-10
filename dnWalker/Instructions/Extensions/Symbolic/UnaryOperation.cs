using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public class UnaryOperation : IInstructionExecutor
    {
        private static readonly Dictionary<OpCode, Operator> _operatorLookup = new Dictionary<OpCode, Operator>()
        {
            [OpCodes.Neg] = Operator.Negate,
            [OpCodes.Not] = Operator.Not,
        };
        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _operatorLookup.Keys;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement operand = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!operand.TryGetExpression(cur, out Expression expression))
            {
                return retValue;
            }

            IDataElement result = cur.EvalStack.Peek();

            Operator op = _operatorLookup[baseExecutor.Instruction.OpCode];

            Expression resultExpression = Expression.MakeUnary(op, expression);
            result.SetExpression(cur, resultExpression);

            return retValue;
        }
    }
}
