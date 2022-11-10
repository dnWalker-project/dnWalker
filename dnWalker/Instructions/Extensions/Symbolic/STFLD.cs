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
    public abstract partial class STFLD : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new[] { OpCodes.Stfld };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedCodes;

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }
}
