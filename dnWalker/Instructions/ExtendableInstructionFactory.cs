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
        //private readonly IList<IInstructionExtension> _extensions = new List<IInstructionExtension>();

        public void RegisterExtension(IInstructionExtension instructionExtension)
        {
            //_extensions.Add(instructionExtension);
            foreach (Type supportedInstruction in instructionExtension.SupportedInstructions)
            {
                ExtendPipeline(supportedInstruction, instructionExtension);
            }

        }

        private void ExtendPipeline(Type instructionType, IInstructionExtension extension)
        {
            if (!_pipelines.TryGetValue(instructionType, out var pipeline))
            {
                pipeline = new List<IInstructionExtension>();
                _pipelines[instructionType] = pipeline;
            }

            pipeline.Add(extension);
        }

        private readonly Dictionary<Type, List<IInstructionExtension>> _pipelines = new Dictionary<Type, List<IInstructionExtension>>();


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
                Type instructionType = extendableInstructionExec.GetType();

                if (_pipelines.TryGetValue(instructionType, out var pipeline))
                {
                    extendableInstructionExec.Extensions = pipeline;
                }
            }

            return iExec;
        }
    }
}
