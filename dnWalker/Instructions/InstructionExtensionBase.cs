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
    /// <summary>
    /// Base class for <see cref="IInstructionExtension"/> implementations. Does nothing, only caches the current instruction and explicit active state.
    /// </summary>
    public abstract class InstructionExtensionBase : IInstructionExtension
    {
        public abstract bool CanExecute(Code code);

        private InstructionExecBase _currentInstruction;
        private ExplicitActiveState _cur;

        protected InstructionExecBase CurrentInstruction
        {
            get
            {
                return _currentInstruction;
            }
        }

        protected ExplicitActiveState CurrentState
        {
            get
            {
                return _cur;
            }
        }

        public virtual void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            _currentInstruction = instruction;
            _cur = cur;
        }

        public virtual bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            _currentInstruction = instruction;
            _cur = cur;

            retValue = null;
            return false;
        }


        public virtual void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            _currentInstruction = instruction;
            _cur = cur;

        }
    }
}
