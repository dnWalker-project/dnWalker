using dnlib.DotNet.Emit;

using dnWalker.Parameters;
using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Parameters
{
    public class LDLEN : IInstructionExecutor
    {
        private readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Ldlen };
        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
        {
            // gather the operand
            IDataElement dataElement = cur.EvalStack.Peek();

            IIEReturnValue retValue = next(instruction, cur);

            if (retValue is ExceptionHandlerLookupReturnValue)
            {
                // an exception was thrown => the array is null => do nothing
                return retValue;
            }

            if (cur.TryGetParameterStore(out ParameterStore store))
            {
                dataElement.TryGetParameter(cur, out IArrayParameter arrayParameter);

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(arrayParameter.GetLengthExpression(cur), cur);
            }

            return retValue;
        }
    }
}
