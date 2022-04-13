using dnlib.DotNet.Emit;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions
{
    public delegate IIEReturnValue InstructionExecution(InstructionExecBase instruction, ExplicitActiveState cur);
    public interface IInstructionExecutor
    {
        /// <summary>
        /// Determines whether the extension can be injected into the instruction execution.
        /// </summary>
        public IEnumerable<OpCode> SupportedOpCodes { get; }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next);
    }

}
