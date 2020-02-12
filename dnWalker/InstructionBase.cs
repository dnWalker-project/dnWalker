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

        // Boolean attributes.
        InstructionExecAttributes m_flags;

        // Properties for access to private fields.
        public Instruction Instruction { get; }

        public object Operand { get; set; }

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

        public InstructionExecBase(Instruction instr, object operand, InstructionExecAttributes atr)
        {
            Instruction = instr;
            Operand = operand;
            m_flags = atr;
        }

        public virtual IIEReturnValue Execute(ExplicitActiveState cur)
        {
            cur.Logger.Warning("Execution for instruction not implemented.");
            return null;
        }

        public IIEReturnValue ThrowException(Exception ex, ExplicitActiveState cur)
        {
            var exceptionType = cur.DefinitionProvider.GetTypeDefinition(ex.GetType().FullName);

            ObjectReference exceptionRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(),
                exceptionType);

            var exceptionObject = cur.DynamicArea.Allocations[exceptionRef];
            //exceptionObject.From(ex);

            cur.CurrentThread.ExceptionReference = exceptionRef;
            cur.CurrentThread.UnhandledException = ex;
            cur.CurrentMethod.IsExceptionSource = true;

            // Constructor calls should leave object reference on the stack.
            cur.EvalStack.Push(exceptionRef);

            return ehLookupRetval;
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

        public override string ToString()
        {
            return $"{base.ToString()} IL_{Instruction.Offset:X4} {Instruction.OpCode.Name}";
        }
    }
}