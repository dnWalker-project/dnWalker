using dnlib.DotNet.Emit;

using MMC.InstructionExec;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions
{
    public class ExtendableInstructionFactory : dnWalker.Factories.InstructionFactory
    {
        private readonly Dictionary<Code, IList<IInstructionExtension>> _extensions = new Dictionary<Code, IList<IInstructionExtension>>();

        public void RegisterExtension(IInstructionExtension instructionExtension)
        {
            Code c = instructionExtension.Operation;
            if (!_extensions.TryGetValue(c, out IList<IInstructionExtension> extensions))
            {
                extensions = new List<IInstructionExtension>();
                _extensions[c] = extensions;
            }

            extensions.Add(instructionExtension);
        }


        public override InstructionExecBase CreateInstructionExec(Instruction instr)
        {
            var tokens = instr.OpCode.Name.Split(new char[] { '.' });

            // Before doing anything else, check if we have an implementing class for this type of instruction.
            var name = "dnWalker.Instructions." + (string.Join("_", tokens)).ToUpper();
            var t = Type.GetType(name);
            if (t == null)
            {
                name = "dnWalker.Instructions." + tokens[0].ToUpper();
                t = Type.GetType(name);
            }

            InstructionExecBase iExec;
            if (t == null)
            {
                iExec = base.CreateInstructionExec(instr);
            }
            else
            {
                iExec = CreateInstructionExec(t, tokens, instr);
            }

            if (iExec is ExtendableInstructionExecBase extendableInstructionExec)
            {
                foreach (IInstructionExtension instructionExtension in _extensions[instr.OpCode.Code])
                {
                    extendableInstructionExec.Extensions.Add(instructionExtension);
                }
            }

            return iExec;
        }
    }
}
