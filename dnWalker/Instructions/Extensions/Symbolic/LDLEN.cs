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
    public abstract partial class LDLEN : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new[] { OpCodes.Ldlen };

        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }
}
