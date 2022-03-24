using dnlib.DotNet.Emit;

using dnWalker.Parameters;

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
    public class CEQ : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Ceq };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            // need to handle the null equality
            // bool isNull = obj == null;
            // AND
            // bool isNull = null == obj;
            // is translated in following ways:
            // ld* obj
            // ldnull 
            // ceq

            IDataElement isNullOperand = cur.EvalStack.Peek(1);

            if (!cur.TryGetParameterStore(out ParameterStore store) ||
                !isNullOperand.TryGetParameter(cur, out IReferenceTypeParameter refTypeParameter))
            {
                // we do nothing, since no parameter system is available OR the element in question is not parametrized
                return next(baseExecutor, cur);
            }

            IDataElement possiblyNull = cur.EvalStack.Peek(0);
            if (possiblyNull is not ObjectReference objRef ||
                !objRef.IsNull() ||
                objRef.HashCode != ObjectReference.Null.HashCode)
            {
                // the other operand is NOT global null
            }
        }
    }
}
