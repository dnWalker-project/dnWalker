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
    public interface IInstructionExtension
    {
        public Code Operation { get; }

        /// <summary>
        /// Tries to execute the operation in place of the default execution.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="cur"></param>
        /// <param name="retValue"></param>
        /// <returns><code>true</code> if the operation was executed, <code>false</code> if execution did not proceed. If returns <code>false</code> the <see cref="ExplicitActiveState.CallStack"/> and <see cref="ExplicitActiveState.EvalStack"/> remains unchanged.</returns>
        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue);
    }
}
