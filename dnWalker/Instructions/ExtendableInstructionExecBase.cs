using dnlib.DotNet.Emit;

using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions
{
    public abstract class ExtendableInstructionExecBase : InstructionExecBase
    {
        public ExtendableInstructionExecBase(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr)
        {
            _extensions = new List<IInstructionExtension>();
        }
        public ExtendableInstructionExecBase(Instruction instr, object operand, InstructionExecAttributes atr, IEnumerable<IInstructionExtension> extensions) : base(instr, operand, atr)
        {
            _extensions = new List<IInstructionExtension>(extensions);
        }

        private readonly List<IInstructionExtension> _extensions;

        /// <summary>
        /// An ordered collection of extensions to use.
        /// </summary>
        public IList<IInstructionExtension> Extensions
        {
            get
            {
                return _extensions;
            }
        }

        /// <summary>
        /// This method, when overrideden, will provide the default execution behavior.
        /// </summary>
        /// <param name="cur"></param>
        /// <returns></returns>
        protected abstract IIEReturnValue ExecuteCore(ExplicitActiveState cur);

        public sealed override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // pre-execute
            foreach (IInstructionExtension extension in _extensions)
            {
                extension.PreExecute(this, cur);
            }

            // execute
            IIEReturnValue retValue = null;
            // in order to make sure that if extension fails to execute the instruction, the cur is not changed - ExplicitActiveState.MakeSavePoint() and ExpliciteActiveState.RestoreState(), and/or add trackin capabilities, e.g. evalstack.push(...) will be saved and we will be able to undo it...
            foreach (IInstructionExtension extension in _extensions)
            {
                if (extension.TryExecute(this, cur, out retValue))
                {
                    break;
                }
            }

            if (retValue == null)
            {
                retValue = ExecuteCore(cur);
            }

            // post-execute
            foreach (IInstructionExtension extension in _extensions)
            {
                extension.PostExecute(this, cur, retValue);
            }

            return retValue;
        }
    }
}
