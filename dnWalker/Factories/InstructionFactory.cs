﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using MMC;
using MMC.Data;
using MMC.InstructionExec;
using System;
using System.Reflection;

namespace dnWalker.Factories
{
    public interface IInstructionFactory
    {
        InstructionExecBase CreateInstructionExec(Instruction instr);
    }

    public class InstructionFactory : IInstructionFactory
    {
        /// <summary>
        /// Factory for instruction executors.
        /// </summary>
        /// <remarks>
        /// This method parses the name and operand of the given instruction
        /// and returns an object that can be used to execute the instruction
        /// on a given machine state. The parsing method used here is quite
        /// crude, and could use some refinement.
        /// </remarks>
        /// <param name="instr">The instruction to build the executor for.</param>
        /// <returns>The executor.</returns>
        public virtual InstructionExecBase CreateInstructionExec(Instruction instr)
        {
            string[] tokens = instr.OpCode.Name.Split(new char[] { '.' });

            // Before doing anything else, check if we have an implementing class for this type of instruction.
            string name = "MMC.InstructionExec." + (string.Join("_", tokens)).ToUpper();
            Type t = Type.GetType(name);
            if (t == null)
            {
                name = "MMC.InstructionExec." + tokens[0].ToUpper();
                t = Type.GetType(name);
            }

            if (t == null)
            {
                throw new Exception("No instruction executor found for " + name);
            }

            InstructionExecAttributes attr = InstructionExecAttributes.None;
            object operand = null;

            // Check for possible implicit operand (always digit or m1/M1).
            string lastToken = tokens[tokens.Length - 1];
            if (instr.OpCode.OperandType == OperandType.InlineNone)
            {
                if (lastToken.Length == 1)
                {
                    char c = lastToken[0];
                    if (c >= '0' && c <= '9')
                    {
                        operand = new Int4(c - '0');
                    }
                }
                else if (lastToken.Equals("M1", StringComparison.OrdinalIgnoreCase))
                {
                    operand = new Int4(-1);
                }
                attr |= InstructionExecAttributes.ImplicitOperand;
            }

            // Check if we should use regard the stack elements as unsigned.
            for (int i = 1; i < tokens.Length; ++i)
            {
                if (tokens[i] == "un")
                {
                    attr |= InstructionExecAttributes.Unsigned;
                }
                else if (tokens[i] == "ovf")
                {
                    attr |= InstructionExecAttributes.CheckOverflow;
                }
            }

            // If no implicit operand was found, we pass the operand of
            // the instruction, and let the instruction executor figure
            // out what to do with it.
            if (operand == null)
            {
                operand = instr.Operand;
            }

            // Lookup definitions.
            switch (operand)
            {
                case ITypeDefOrRef typeDefOrRef:
                    operand = DefinitionProvider.GetTypeDefinition(typeDefOrRef)
                        ?? throw new NullReferenceException("Type not resolved: " + operand);
                    break;
                case IMethod method:
                    operand = method.ResolveMethodDef()
                        ?? throw new NullReferenceException("Method not resolved: " + operand);
                    break;
                case IField field:
                    operand = DefinitionProvider.GetFieldDefinition(field)
                        ?? throw new NullReferenceException("Field not resolved: " + operand);
                    break;
            }

            // Check for short form.
            if (instr.OpCode.OperandType == OperandType.ShortInlineBrTarget ||
                instr.OpCode.OperandType == OperandType.ShortInlineI ||
                instr.OpCode.OperandType == OperandType.ShortInlineR ||
                instr.OpCode.OperandType == OperandType.ShortInlineVar) //||instr.OpCode.OperandType == OperandTypeShortInlineParam
            {
                attr |= InstructionExecAttributes.ShortForm;
            }

            // Create an InstructionExec object, using reflection.
            return (InstructionExecBase)t.InvokeMember(null,
                BindingFlags.DeclaredOnly |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.CreateInstance,
                null, null, new object[] { instr, operand, attr });
        }
    }
}
