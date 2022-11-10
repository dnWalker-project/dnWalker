using dnlib.DotNet.Emit;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class STELEM : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCoders = new[]
        {
            OpCodes.Stelem,

            OpCodes.Stelem_I,
            OpCodes.Stelem_I1,
            OpCodes.Stelem_I2,
            OpCodes.Stelem_I4,
            OpCodes.Stelem_I8,

            OpCodes.Stelem_R4,
            OpCodes.Stelem_R8,

            OpCodes.Stelem_Ref,
        };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedCoders;

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }
}
