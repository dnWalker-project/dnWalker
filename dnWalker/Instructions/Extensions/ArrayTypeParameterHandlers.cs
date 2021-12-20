using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MMC.InstructionExec.InstructionExecBase;

namespace dnWalker.Instructions.Extensions
{
    public class LDLEN_ParameterHandler : ITryExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(LDLEN);
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            IDataElement dataElement = cur.EvalStack.Peek();

            if (dataElement.TryGetParameter(cur, out IArrayParameter array))
            {
                // if (array.GetIsNull()) throw NullReferenceException();

                cur.EvalStack.Pop();
                UnsignedInt4 lengthDE = cur.GetLengthDataElement(array);
                cur.EvalStack.Push(lengthDE);

                retValue = nextRetval;
                return true;
            }

            retValue = null;
            return false;
        }
    }
}
