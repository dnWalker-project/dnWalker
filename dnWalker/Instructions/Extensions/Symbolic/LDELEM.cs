using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class LDELEM : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedOpCodes = new[]
        {
            OpCodes.Ldelem,
            OpCodes.Ldelema,

            OpCodes.Ldelem_I,
            OpCodes.Ldelem_I1,
            OpCodes.Ldelem_I2,
            OpCodes.Ldelem_I4,
            OpCodes.Ldelem_I8,

            OpCodes.Ldelem_U1,
            OpCodes.Ldelem_U2,
            OpCodes.Ldelem_U4,

            OpCodes.Ldelem_R4,
            OpCodes.Ldelem_R8,

            OpCodes.Ldelem_Ref,
        };



        public IEnumerable<OpCode> SupportedOpCodes => _supportedOpCodes;

        public abstract IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }
}
