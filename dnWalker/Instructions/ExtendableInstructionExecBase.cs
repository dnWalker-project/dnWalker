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
    public class ExtendableInstructionExecBase : InstructionExecBase
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

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            foreach (IInstructionExtension extension in _extensions)
            {
                if (extension.TryExecute(this, cur, out IIEReturnValue retValue))
                {
                    return retValue;
                }
            }

            return base.Execute(cur);
        }
    }
}
