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
        public void RegisterExtension(IInstructionExecutor instructionExtension)
        {
            foreach (OpCode opCode in instructionExtension.SupportedOpCodes)
            {
                ExtendPipeline(opCode, instructionExtension);
            }

        }

        private void ExtendPipeline(OpCode opCode, IInstructionExecutor extension)
        {
            if (!_pipelines.TryGetValue(opCode, out var pipeline))
            {
                pipeline = new List<IInstructionExecutor>();
                _pipelines[opCode] = pipeline;
            }

            pipeline.Add(extension);
        }

        private readonly Dictionary<OpCode, List<IInstructionExecutor>> _pipelines = new Dictionary<OpCode, List<IInstructionExecutor>>();


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
                OpCode opCode = instr.OpCode;

                if (_pipelines.TryGetValue(opCode, out var pipeline))
                {
                    extendableInstructionExec.Extensions = pipeline;
                }
            }

            return iExec;
        }
    }
}
