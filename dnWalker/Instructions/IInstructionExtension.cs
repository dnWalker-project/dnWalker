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
        /// <summary>
        /// Determines wheter the extension can execute instruction with the supplied <paramref name="code"/>.
        /// </summary>
        public bool CanExecute(Code code);
    }

    public interface ITryExecuteInstructionExtension : IInstructionExtension
    {
        /// <summary>
        /// Tries to execute the operation in place of the default execution.
        /// It may not be invoked when the extended instruction is executed - another extension with higher priority can execute the instruction before.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="cur"></param>
        /// <param name="retValue"></param>
        /// <returns><code>true</code> if the operation was executed, <code>false</code> if execution did not proceed. If returns <code>false</code> the <see cref="ExplicitActiveState.CallStack"/> and <see cref="ExplicitActiveState.EvalStack"/> remains unchanged.</returns>
        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue);
    }

    public interface IPreExecuteInstructionExtension : IInstructionExtension
    {
        /// <summary>
        /// Initializes inner state of the extension for the instruction. Must not edit cur.
        /// Will always be invoked everytime the extended instruction is executed.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="cur"></param>
        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur);
    }

    public interface IPostExecuteInstructionExtension : IInstructionExtension
    {

        /// <summary>
        /// Can do some operation on the eval and call stack after the execution iself - attach some metadata, do statistics etc. 
        /// Will always be invoked everytime the extended instruction is executed.
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="cur"></param>
        /// <param name="retValue"></param>
        public void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue);
    }
}
