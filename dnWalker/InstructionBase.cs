/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

namespace MMC.InstructionExec
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using MMC.Data;
    using MMC.State;
    using dnlib.DotNet.Emit;
    using dnlib.DotNet;

    [Flags]
    public enum InstructionExecAttributes : int
    { // using less than word size is useless

        None = 0x0000,
        // P3, p.2: Stored as int4, but only consider lowest 16 bits.
        ShortForm = 0x0001,
        // Operation implicitly defines an operand (e.g. ldc.i4.0 defines 0).
        ImplicitOperand = 0x0002,
        // Should this instruction executor check for overflows?
        CheckOverflow = 0x0004,
        // Use unsigned arithmetic (unset flag means signed arithmetic).
        Unsigned = 0x0008
    }

    public class InstructionExecBase
    {

        // Static return types for next and no-increment. Those do not contain data,
        // so no need for more than one copy.
        public static IIEReturnValue nextRetval = new NextReturnValue();
        public static IIEReturnValue nincRetval = new NoIncrementReturnValue();
        public static IIEReturnValue assertionViolatedRetval = new AssertionViolatedReturnValue();
        public static IIEReturnValue ehLookupRetval = new ExceptionHandlerLookupReturnValue();
        public static IIEReturnValue finallyLookupRetval = new ExceptionFinallyReturnValue();

        // Operand (either implicit or explicit)
        object m_operand;
        // Boolean attributes.
        InstructionExecAttributes m_flags;
        // Reference to the instruction itself. 
        Instruction m_instr;

        // Properties for access to private fields.
        public Instruction Instruction
        {

            get { return m_instr; }
        }

        public object Operand
        {

            get { return m_operand; }
            set { m_operand = value; }
        }

        public bool UseShortForm
        {

            get { return (m_flags & InstructionExecAttributes.ShortForm) != 0; }
        }

        public bool HasImplicitOperand
        {

            get { return (m_flags & InstructionExecAttributes.ImplicitOperand) != 0; }
        }

        public bool CheckOverflow
        {

            get { return (m_flags & InstructionExecAttributes.CheckOverflow) != 0; }
        }

        public bool Unsigned
        {

            get { return (m_flags & InstructionExecAttributes.Unsigned) != 0; }
        }

        public InstructionExecBase(Instruction instr, object operand,
                InstructionExecAttributes atr)
        {

            m_instr = instr;
            m_operand = operand;
            m_flags = atr;
        }

        public virtual IIEReturnValue Execute(ExplicitActiveState cur)
        {

            Logger.l.Warning("Execution for instruction not implemented.");
            return null;
        }

        // ------------------------ Static members --------------------------------

        /// \brief Factory for instruction executors.
        /// 
        /// This method parses the name and operand of the given instruction
        /// and returns an object that can be used to execute the instruction
        /// on a given machine state. The parsing method used here is quite
        /// crude, and could use some refinement.
        /// 
        /// \param instr The instruction to build the executor for.
        /// \return The executor.
        ///
        public static InstructionExecBase CreateInstructionExec(Instruction instr)
        {

            string[] tokens = instr.OpCode.Name.Split(new char[] { '.' });

            // Before doing anything else, check if we have an implementing
            // class for this type of instruction.
            string name = "MMC.InstructionExec." + tokens[0].ToUpper();
            Type t = Type.GetType(name);

            if (t == null)
            {
                throw new System.Exception("No IE found for " + name);
                //return null;
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
                        operand = new Int4((int)(c - '0'));
                }
                else if (lastToken.ToUpper().Equals("M1"))
                    operand = new Int4(-1);
                attr |= InstructionExecAttributes.ImplicitOperand;
            }

            // Check if we should use regard the stack elements as unsigned.
            for (int i = 1; i < tokens.Length; ++i)
            {
                if (tokens[i] == "un")
                    attr |= InstructionExecAttributes.Unsigned;
                else if (tokens[i] == "ovf")
                    attr |= InstructionExecAttributes.CheckOverflow;
            }

            // If no implicit operand was found, we pass the operand of
            // the instruction, and let the instruction executor figure
            // out what to do with it.
            if (operand == null)
                operand = instr.Operand;
            // Lookup definitions.
            if (operand is IMethod)
            {// && !(operand is MethodDefinition)) {
                operand = DefinitionProvider.dp.GetMethodDefinition(operand as IMethod);
                if (operand == null)
                    Logger.l.Warning("failed to lookup method reference");
            }
            else if (operand is IField field)
            {//&& !(operand is FieldDefinition)) {
                operand = DefinitionProvider.dp.GetFieldDefinition(field);// foperand as FieldReference);
                if (operand == null)
                    Logger.l.Warning("failed to lookup field reference");
            }
            else if (operand is ITypeDefOrRef typeDefOrRef)
            {//&& !(operand is TypeDefinition)) {
                operand = DefinitionProvider.dp.GetTypeDefinition(typeDefOrRef);// operand as TypeReference);
                if (operand == null)
                    Logger.l.Warning("failed to lookup type reference");
            }

            // Check for short form.
            if (instr.OpCode.OperandType == OperandType.ShortInlineBrTarget ||
                    instr.OpCode.OperandType == OperandType.ShortInlineI ||
                    instr.OpCode.OperandType == OperandType.ShortInlineR ||
                    instr.OpCode.OperandType == OperandType.ShortInlineVar //||
                    //instr.OpCode.OperandType == OperandTypeShortInlineParam
                    )
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

        public void RaiseException(string type, ExplicitActiveState cur)
        {
            var exceptionType = DefinitionProvider.dp.GetTypeDefinition(type);

            ObjectReference exceptionRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(cur),
                exceptionType);

            cur.CurrentThread.ExceptionReference = exceptionRef;
            cur.CurrentMethod.IsExceptionSource = true;

            // Constructor calls should leave object reference on the stack.
            cur.EvalStack.Push(exceptionRef);

            /*
			 * lookup the constructor with no arguments
			 */
            MethodDef noArgsCtor = null;
            foreach (var constructor in exceptionType.FindInstanceConstructors())
            {
                if (constructor.Parameters.Count == 0)
                {
                    noArgsCtor = constructor;
                    break;
                }
            }

            // Call the constructor.
            MethodState called = new MethodState(noArgsCtor, cur.StorageFactory.CreateSingleton(exceptionRef), cur);
            cur.CallStack.Push(called);
        }

        public virtual bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            // Note that the current implementation of safe vs. unsafe
            // instruction is NOT enough to guarentee termination of even very
            // simple single-threaded programs like 'while (true) ;'.  The
            // reason for this is simple: no check is made whether intermediate
            // states are visited.
            // These example are usually not interesting to model-check anyway,
            // but a fix is to make more instructions 'unsafe', like JMP in
            // this case.
            return true;
        }

        /*
		 * This is overloaded by specific instructions for POR using object escape analysis
		 */
        public virtual bool IsDependent(ExplicitActiveState cur)
        {
            return false;
        }

        public virtual MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            return MemoryLocation.Null;
        }
    }
}