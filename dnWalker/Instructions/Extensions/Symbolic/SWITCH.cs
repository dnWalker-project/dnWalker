using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
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
    public class SWITCH : IInstructionExecutor
    {
        private static readonly OpCode[] _opCodes = new OpCode[] { OpCodes.Switch };

        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _opCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            IDataElement value = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(baseExecutor, cur);

            if (ExpressionUtils.GetExpressions(cur, value, out Expression expression))
            {
                Instruction[] targets = (Instruction[])baseExecutor.Operand;

                DecisionHelper.Switch(cur, ((INumericElement)value).ToInt4(false).Value, expression, targets);
            }

            return retValue;
        }
    }
}
