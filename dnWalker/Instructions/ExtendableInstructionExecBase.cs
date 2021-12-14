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
        }
        public ExtendableInstructionExecBase(Instruction instr, object operand, InstructionExecAttributes atr, IEnumerable<IInstructionExtension> extensions) : base(instr, operand, atr)
        {
        }

        private IReadOnlyList<IInstructionExtension> _extensions = null;

        /// <summary>
        /// An ordered collection of extensions to use.
        /// </summary>
        public IReadOnlyList<IInstructionExtension> Extensions
        {
            get
            {
                return _extensions;
            }
            set
            {
                _extensions = value;
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
            if (_extensions != null)
            {
                // pre-execute
                foreach (IPreExecuteInstructionExtension extension in _extensions.OfType<IPreExecuteInstructionExtension>())
                {
                    extension.PreExecute(this, cur);
                }

                // execute
                IIEReturnValue retValue = null;
                // in order to make sure that if extension fails to execute the instruction, the cur is not changed - ExplicitActiveState.MakeSavePoint() and ExpliciteActiveState.RestoreState(), and/or add trackin capabilities, e.g. evalstack.push(...) will be saved and we will be able to undo it...
                foreach (ITryExecuteInstructionExtension extension in _extensions.OfType<ITryExecuteInstructionExtension>())
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
                foreach (IPostExecuteInstructionExtension extension in _extensions.OfType<IPostExecuteInstructionExtension>())
                {
                    extension.PostExecute(this, cur, retValue);
                }

                return retValue;
            }
            else
            {
                return ExecuteCore(cur);
            }
        }
    }
}
