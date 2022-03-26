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
    public class BRTRUE : Branch
    {
        private static readonly OpCode[] _supportedOpCodes = new OpCode[] { OpCodes.Brtrue, OpCodes.Brtrue_S };
        public override IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedOpCodes;
            }
        }


        public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement operandDE = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (!operandDE.TryGetExpression(cur, out Expression expression))
            {
                return retValue;
            }

            expression = expression.AsBoolean();

            Instruction nextInstruction = GetNextInstruction(retValue, cur);

            // nextInstruction != null => will branch => the condition is FALSE => we need to negate it for the path constraint
            Expression condition = nextInstruction != null ? Expression.Not(expression) : expression;

            SetPathConstraint(baseExecutor, nextInstruction, cur, condition);

            return retValue;
        }
    }
}
