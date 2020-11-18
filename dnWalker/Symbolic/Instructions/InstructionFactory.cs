using dnlib.DotNet.Emit;
using MMC.InstructionExec;
using System;

namespace dnWalker.Symbolic.Instructions
{
    public class InstructionFactory : dnWalker.Factories.InstructionFactory
    {
        public override InstructionExecBase CreateInstructionExec(Instruction instr)
        {
            string[] tokens = instr.OpCode.Name.Split(new char[] { '.' });

            // Before doing anything else, check if we have an implementing class for this type of instruction.
            string name = "dnWalker.Symbolic.Instructions." + (string.Join("_", tokens)).ToUpper();
            Type t = Type.GetType(name);
            if (t == null)
            {
                name = "dnWalker.Symbolic.Instructions." + tokens[0].ToUpper();
                t = Type.GetType(name);
            }

            if (t == null)
            {
                return base.CreateInstructionExec(instr);
            }

            return CreateInstructionExec(t, tokens, instr);
        }
    }
}
