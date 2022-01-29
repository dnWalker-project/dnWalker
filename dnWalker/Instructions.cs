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
    using System.Diagnostics;
    using System.Collections;

    using dnlib.DotNet.Emit;
    using MMC.Data;
    using MMC.State;
    using dnlib.DotNet;

    using MethodDefinition = dnlib.DotNet.MethodDef;
    using TypeDefinition = dnlib.DotNet.TypeDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;
    using ParameterDefinition = dnlib.DotNet.Parameter;
    using MMC.ICall;
    using dnWalker;
    using ThreadState = State.ThreadState;
    using dnWalker.TypeSystem;
    using System.Collections.Generic;

    //using FieldDefinition = dnlib.DotNet.Var;

    public class BREAK : InstructionExecBase
    {

        public BREAK(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }


    public class BranchInstructionExec : InstructionExecBase
    {
        public BranchInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            /*
			 * Prevents infinite looping with local vars 
			 * 
			 * VYN: disabled this, this cannot detect whether two threads fail to make progress, and run infinitely
			 */
            return true;
            //return cur.ThreadPool.GetThreadCount(System.Threading.ThreadState.Running) > 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            /*
			 * Same reason as for IsMultiThreadSafe(ExplicitActiveState cur), only we have to "fool" the 
			 * POR that a branch instruction is dependent in case of a single thread 
			 * 
			 * VYN: disabled this, this cannot detect whether two threads fail to make progress, and run infinitely
			 */
            return false;
            //return cur.ThreadPool.GetThreadCount(System.Threading.ThreadState.Running) == 1;
        }

        protected int CompareOperands(IDataElement a, IDataElement b)
        {
            if (Unsigned && a is INumericElement na && b is INumericElement nb)
            {
                if (na.Equals(nb))
                {
                    return 0;
                }

                return na.ToUnsignedInt8(CheckOverflow).CompareTo(nb.ToUnsignedInt8(CheckOverflow));

                //return ValueComparer.CompareUnsigned((INumericElement)a, (INumericElement)b);
            }

            return a.CompareTo(b);
        }
    }

    // Branch on equal.
    public class BEQ : BranchInstructionExec
    {
        public BEQ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            return CompareOperands(a, b) == 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Branch on equal.
    public class BGE : BranchInstructionExec
    {
        public BGE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            return CompareOperands(a, b) >= 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Branch on greater than.
    public class BGT : BranchInstructionExec
    {
        public BGT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            return CompareOperands(a, b) > 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Branch on less or equal.
    public class BLE : BranchInstructionExec
    {
        public BLE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();

            return CompareOperands(a, b) <= 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Branch on less than.
    public class BLT : BranchInstructionExec
    {
        public BLT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            return CompareOperands(a, b) < 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Branch on not equal.
    public class BNE : BranchInstructionExec
    {
        public BNE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            return CompareOperands(a, b) != 0 ?
                new JumpReturnValue((Instruction)Operand) :
                nextRetval;
        }
    }

    // Unconditional branch.
    public class BR : BranchInstructionExec
    {
        public BR(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return new JumpReturnValue((Instruction)Operand);
        }
    }

    // Branch on non-false or non-null.
    public class BRTRUE : BranchInstructionExec
    {
        public BRTRUE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            return a.ToBool() ? new JumpReturnValue((Instruction)Operand) : nextRetval;
        }
    }

    // Branch on false, null or zero.
    public class BRFALSE : BranchInstructionExec
    {
        public BRFALSE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            return !a.ToBool() ? new JumpReturnValue((Instruction)Operand) : nextRetval;
        }
    }

    // Switch
    public class SWITCH : BranchInstructionExec
    {
        public SWITCH(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            var targets = Operand as Instruction[];
            if (targets.Length == 0)
            {
                return nextRetval;
            }

            try
            {
                var index = (int)Convert.ChangeType(a, typeof(int));
                return index >= 0 && index < targets.Length ?
                    new JumpReturnValue(targets[index]) :
                    nextRetval;
            }
            catch (OverflowException o)
            {
                return nextRetval;
            }
        }
    }

    public class NOP : InstructionExecBase
    {
        public NOP(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return nextRetval;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            return false;
        }
    }

    /*
	 * In MMC, everything is executed as volatile anyway 
	 */
    public class VOLATILE : InstructionExecBase
    {
        public VOLATILE(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return nextRetval;
        }
    }

    public class READONLY : InstructionExecBase
    {
        public READONLY(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return nextRetval;
        }
    }

    public class LogicalIntsructionExec : InstructionExecBase
    {

        public LogicalIntsructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    // Bitwise AND
    public class AND : LogicalIntsructionExec
    {
        public AND(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = (IIntegerElement)cur.EvalStack.Pop();
            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.And(b));
            return nextRetval;
        }
    }

    // Bitwise NOT
    public class NOT : LogicalIntsructionExec
    {
        public NOT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Not());
            return nextRetval;
        }
    }

    // Bitwise OR
    public class OR : LogicalIntsructionExec
    {

        public OR(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            var b = (IIntegerElement)cur.EvalStack.Pop();
            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Or(b));
            return nextRetval;
        }
    }

    // Bitwise XOR
    public class XOR : LogicalIntsructionExec
    {

        public XOR(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            var b = (IIntegerElement)cur.EvalStack.Pop();
            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Xor(b));
            return nextRetval;
        }
    }

    // Shift left
    public class SHL : LogicalIntsructionExec
    {

        public SHL(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var value = cur.EvalStack.Pop();
            var shiftBy = (int)Convert.ChangeType(value, typeof(int));
            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Shl(shiftBy));
            return nextRetval;
        }
    }

    // Shift right
    public class SHR : LogicalIntsructionExec
    {
        public SHR(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var value = cur.EvalStack.Pop();
            var shiftBy = (int)Convert.ChangeType(value, typeof(int));
            var a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Shr(shiftBy));
            return nextRetval;
        }
    }

    /// <summary>
    /// Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result onto the evaluation stack.
    /// </summary>
    public class SHR_UN : LogicalIntsructionExec
    {
        public SHR_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var value = cur.EvalStack.Pop();
            var shiftBy = (int)Convert.ChangeType(value, typeof(int));
            var a = (ISignedIntegerElement)cur.EvalStack.Pop();
            var u = (IIntegerElement)a.ToUnsigned();
            cur.EvalStack.Push(u.Shr(shiftBy));
            return nextRetval;
        }
    }

    public class LoadInstructionExec : InstructionExecBase
    {

        public LoadInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    public class DUP : LoadInstructionExec
    {

        public DUP(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            cur.EvalStack.Push(cur.EvalStack.Peek());
            return nextRetval;
        }
    }

    public class LDARG : LoadInstructionExec
    {
        public LDARG(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is either an implicit Int4, or a ParameterDefinition object.
            int argIndex;
            if (HasImplicitOperand)
            {
                argIndex = ((Int4)Operand).Value;
            }
            else
            {
                argIndex = ((ParameterDefinition)Operand).ParamDef.Sequence;

                if (!cur.CurrentMethod.Definition.HasThis)
                {
                    argIndex--;
                }
            }

            cur.EvalStack.Push(cur.CurrentMethod.Arguments[argIndex]);
            return nextRetval;
        }
    }

    public class LDARGA : LoadInstructionExec
    {
        public LDARGA(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            int argIndex = (Operand as ParameterDefinition).ParamDef.Sequence;
            if (cur.CurrentMethod.Definition.IsStatic)
            {
                argIndex--;
            }

            cur.EvalStack.Push(new ArgumentPointer(cur.CurrentMethod, argIndex));

            return nextRetval;
        }
    }

    /*
	 * By VY: updated this so that it can load all by ECMA specified types of
	 * values
	 */
    public class LDC : LoadInstructionExec
    {
        public LDC(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Determine the desired type. The PushBehaviour enum is no
            // good here (it doesn't deal with the shorter forms).
            // See format in ECMA spec

            var tokens = Instruction.OpCode.Name.Split(new char[] { '.' });
            INumericElement toPush = null;

            // (Signed) others (formatted as described above)

            switch (tokens[1][0])
            {
                case 'i':
                    switch (tokens[1][1])
                    {
                        case '4':
                            if (UseShortForm)
                            {
                                toPush = new Int4((sbyte)Operand).ToShort(CheckOverflow);
                            }
                            else if (HasImplicitOperand)
                            {
                                toPush = (INumericElement)Operand;
                            }
                            else
                            {
                                toPush = new Int4((int)Operand);
                            }

                            break;
                        case '8':
                            toPush = new Int8((long)Operand);
                            break;
                    }

                    break;

                case 'r':
                    switch (tokens[1][1])
                    {
                        case '4':
                            toPush = new Float4((float)Operand);
                            break;
                        case '8':
                            toPush = new Float8((double)Operand);
                            break;
                    }

                    break;
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    public class LDFTN : LoadInstructionExec
    {
        public LDFTN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var method = Operand as MethodDefinition;
            cur.EvalStack.Push(new MethodPointer(method));
            return nextRetval;
        }
    }

    public class LDVIRTFTN : LoadInstructionExec
    {
        public LDVIRTFTN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var or = cur.EvalStack.Pop();
            var method = Operand as MethodDefinition;

            var toCall = or.FindVirtualMethod(method, cur);
            cur.EvalStack.Push(new MethodPointer(toCall));

            return nextRetval;
        }
    }

    public class LDIND : LoadInstructionExec
    {
        public LDIND(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var pop = cur.EvalStack.Pop();
            if (pop is IManagedPointer mmp)
            {
                cur.EvalStack.Push(mmp.Value);
                return nextRetval;
            }

            throw new NotSupportedException($"IManagedPointer expected, {pop?.GetType().FullName ?? "null"} found.");
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            var reference = cur.EvalStack.Peek();
            return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {

            var reference = cur.EvalStack.Peek();

            if (reference is ObjectFieldPointer)
            {
                var ofp = (ObjectFieldPointer)reference;
                var theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

                return theObject.ThreadShared;
            }
            else if (reference is StaticFieldPointer)
            {
                return true;
            }
            else
                return false;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var reference = thread.CurrentMethod.EvalStack.Peek();

            if (reference is ObjectFieldPointer)
            {
                var ofp = (ObjectFieldPointer)reference;
                return ofp.MemoryLocation;
            }
            else if (reference is StaticFieldPointer)
            {
                var sfp = (StaticFieldPointer)reference;
                return sfp.MemoryLocation;
            }

            return base.Accessed(thread, cur);
        }
    }


    public class LDOBJ : LDIND
    {

        public LDOBJ(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        /*
		public override IIEReturnValue Execute(ExplicitActiveState cur) {
			IManagedPointer mmp = cur.EvalStack.Pop() as IManagedPointer;
			cur.EvalStack.Push(mmp.Value);
			return nextRetval;
		}

		public override bool IsMultiThreadSafe(ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();
			return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
		}

		public override bool IsDependent(ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				AllocatedObject theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

				return theObject.ThreadShared;
			} else if (reference is StaticFieldPointer) {
				return true;
			} else
				return false;
		}

		public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				return ofp.MemoryLocation;
			} else if (reference is StaticFieldPointer) {
				StaticFieldPointer sfp = (StaticFieldPointer)reference;
				return sfp.MemoryLocation;
			}

			return base.Accessed(thread);
		}*/
    }

    public class LDLOC : LoadInstructionExec
    {
        public LDLOC(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr) { }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            int index;
            if (HasImplicitOperand)
                index = ((Int4)Operand).Value;
            else
                index = ((Local)Operand).Index;
            cur.EvalStack.Push(cur.CurrentMethod.Locals[index]);
            return nextRetval;
        }
    }

    public class LDLOCA : LoadInstructionExec
    {
        public LDLOCA(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var index = ((Local)Operand).Index;
            cur.EvalStack.Push(new LocalVariablePointer(cur.CurrentMethod, index));
            return nextRetval;
        }
    }

    public class LDNULL : LoadInstructionExec
    {
        public LDNULL(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            cur.EvalStack.Push(ObjectReference.Null);
            return nextRetval;
        }
    }

    public class LDSTR : LoadInstructionExec
    {
        public LDSTR(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            cur.EvalStack.Push(new ConstantString(Operand as string));
            return nextRetval;
        }
    }

    public class ObjectModelInstructionExec : InstructionExecBase
    {
        /// <summary>Loads a public class into the static area.</summary>
        /// <remarks>
        /// We return true if the calling thread is okay to continue
        /// operating on the public class. This is NOT the case if any of the
        /// following conditions hold:
        /// <nl>
        ///   <li> The public class was not loaded, AND the calling thread should
        ///        initialize the public class. If the public class needs no initializtion
        ///        (i.e. no cctor exists), we can continue. </li>
        ///   <li> The public class was not fully initialized, the current thread is
        ///        not currently initializing the public class (this can only be the
        ///        case in a forward-reference in the cctor itself), or someone
        ///        waiting for us. In this case we're put on wait, and should
        ///        not execute the instruction. </li>
        /// </nl>
        /// </remarks>
        /// <param name="type">The type of public class to load.</param>
        /// <returns>True iff we can continue to execute the instruction (i.e.
        /// it's okay to access the static fields).</returns>
        public bool LoadClass(TypeDefinition type, ExplicitActiveState cur)
        {
            var me = cur.ThreadPool.CurrentThreadId;

            var allow_access = true;

            var cls = cur.StaticArea.GetClass(type);
            if (!cls.Initialized)
            {
                cur.Logger.Debug("thread {0} wants access to uninitialized public class {1}", me, type.Name);
                var cctorDef = type.ResolveTypeDefThrow().FindStaticConstructor();
                if (cctorDef == null)
                {
                    // Trivial case, no initializtion needed.
                    cls.Initialized = true;
                }
                else if (cls.InitializingThread == LockManager.NoThread)
                {
                    cur.Logger.Debug("no thread is currently initializing the public class");
                    // We are the ones who have to do the initialization.
                    cls.InitializingThread = me;
                    var cctorState = new MethodState(
                        cctorDef,
                        cur.StorageFactory.CreateList(0),
                        cur);
                    cctorState.OnDispose = new MethodStateCallback(CctorDoneCallBack);
                    cur.CallStack.Push(cctorState);
                    cur.Logger.Debug("found public class constructor. pushed on call stack.");
                    // Do not allow access now. We should first execute the cctor.
                    allow_access = false;
                }
                else
                {
                    var tp = cur.ThreadPool;
                    var wait_for = cls.InitializingThread;
                    cur.Logger.Debug("thread {0} is currently initializing the public class", wait_for);

                    var wait_safe = wait_for != me;
                    if (wait_safe)
                    {
                        // Make sure we're not introducing a deadlock by waiting here.
                        var seen = new BitArray(tp.Threads.Length, false);
                        while (wait_safe && wait_for != LockManager.NoThread && !seen[wait_for])
                        {
                            seen[wait_for] = true;
                            wait_safe = wait_for != me;
                            wait_for = tp.Threads[wait_for].WaitingFor;
                        }
                        // If we hit a cycle, report it, but this should never happen.
                        if (!wait_safe && wait_for != LockManager.NoThread)
                            cur.Logger.Log(LogPriority.Severe,
                                    "indication of deadlock while public class loading");
                        // If it's safe to wait, do it. Else, we can touch the data.
                        if (wait_safe)
                        {
                            cls.AddWaitingThread(me, cur);
                            cur.Logger.Debug("now waiting for completion of cctor by other thread.");
                        }
                    }
                    if (!wait_safe)
                        cur.Logger.Debug("not safe to wait for cctor completion. continue.");
                    allow_access = !wait_safe;
                }
            }

            return allow_access;
        }

        public void CctorDoneCallBack(MethodState ms)
        {
            ms.Cur.Logger.Debug("completed running cctor. public class initialized");
            var type = GetTypeDefinition();
            var cur = ms.Cur;
            /*
			 * It is possible that during state decollapsion, that 
			 * this callback is called when the initialising thread 
			 * shrinks during decollapse.
			 * 
			 * This if prevents that the callback is actually executed
			 */
            if (cur.StaticArea.ClassLoaded(type))
            {
                var cls = cur.StaticArea.GetClass(type);
                cls.Initialized = true;
                cls.InitializingThread = LockManager.NoThread;
                cls.AwakenWaitingThreads(cur);
            }
        }

        public FieldDefinition GetFieldDefinition()
        {
            static void EnsureLayoutInfo(FieldDefinition fieldDef)
            {
                if (!fieldDef.FieldOffset.HasValue)
                {
                    IList<FieldDefinition> fields = fieldDef.DeclaringType.Fields;
                    for (var i = 0; i < fields.Count; i++)
                    {
                        if (fields[i] == fieldDef)
                        {
                            fieldDef.FieldOffset = (uint)i;
                            break;
                        }
                    }
                }
            }

            var fld = Operand as FieldDefinition;
            EnsureLayoutInfo(fld);

            return fld;
        }

        public TypeDefinition GetTypeDefinition()
        {
            //return cur.DefinitionProvider.GetTypeDefinition(GetFieldDefinition().DeclaringType);
            return GetFieldDefinition().DeclaringType;
        }

        /*
		 * By VY: this offset calculation also considers inheritance
		 */

        private int m_offset = -1;

        public int GetFieldOffset(ITypeDefOrRef superType)
        {
            if (m_offset >= 0)
            {
                return m_offset;
            }

            FieldDefinition fld = GetFieldDefinition();
            int typeOffset = 0;
            bool matched = false;
            int retval = 0;

            foreach (TypeDefinition typeDef in superType.InheritanceEnumerator())
            {
                /*
				 * We start searching for the right field from the declaring type,
				 * it is possible that the declaring type does not define field, therefore
				 * it might be possible that we have to search further for field in
				 * the inheritance tree, (hence matched), and this continues until
				 * a field is found which has the same offset and the same name 
				 */
                if (typeDef.FullName.Equals(fld.DeclaringType.FullName) || matched)
                {
                    if (fld.FieldOffset < typeDef.Fields.Count
                        && typeDef.Fields[(int)fld.FieldOffset].Name.Equals(fld.Name))
                    {
                        retval = (int)fld.FieldOffset;
                        break;
                    }

                    matched = true;
                }

                if (typeDef.BaseType != null && typeDef.BaseType.FullName != "System.Object") // if base type is System.Object, stop // TODO: make it so that it does not rely on full name check!!!!
                {
                    typeOffset += Math.Max(0, typeDef.Fields.Count - 1);
                }
            }

            m_offset = typeOffset + retval;

            Debug.Assert(m_offset >= 0, $"Offset for type {superType.FullName} is negative.");

            return m_offset;
        }

        public bool CheckBounds(AllocatedArray arr, Int4 idx)
        {
            return idx.Value >= 0 && idx.Value < arr.Fields.Length;
        }

        public ObjectModelInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    // ------------------------------ Arrays ------------------------------ 
    // Creating, load element, store element, get length.

    public class NEWARR : ObjectModelInstructionExec
    {
        public NEWARR(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var length = (Int4)cur.EvalStack.Pop();

            if (length.Value < 0)
            {
                return ThrowException(new OverflowException(), cur);
            }
            else
            {
                cur.EvalStack.Push(cur.DynamicArea.AllocateArray(
                    cur.DynamicArea.DeterminePlacement(),
                    Operand as ITypeDefOrRef,
                    length.Value));
            }
            return nextRetval;
        }
    }

    /// <summary>
    /// Attempts to cast an object passed by reference to the specified class.
    /// </summary>
    public class CASTCLASS : ObjectModelInstructionExec
    {
        public CASTCLASS(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var objRef = (IReferenceType)cur.EvalStack.Pop();
            var toCastToType = (ITypeDefOrRef)Operand;
            if (objRef.Equals(ObjectReference.Null))
            {
                cur.EvalStack.Push(objRef);
                return nextRetval;
            }

            if (objRef is ConstantString s)
            {
                var st = cur.DefinitionProvider.GetTypeDefinition("System.String");
                if (cur.DefinitionProvider.IsSubtype(st, toCastToType))
                {
                    cur.EvalStack.Push(objRef);
                }
                else
                {
                    return ThrowException(new InvalidCastException(), cur);
                }

                return nextRetval;
            }

            /*
             * TODO: make this work for arrays! See ECMA spec on this
             */
            var ao = cur.DynamicArea.Allocations[(int)objRef.Location] as AllocatedObject;
            if (cur.DefinitionProvider.IsSubtype(ao.Type, toCastToType))
            {
                cur.EvalStack.Push(objRef);
            }
            else
            {
                return ThrowException(new InvalidCastException(), cur);
            }

            return nextRetval;
        }
    }

    /*
	public class INITOBJ : ObjectModelInstructionExec {

		public INITOBJ(Instruction instr, object operand,
				InstructionExecAttributes atr)
			: base(instr, operand, atr) {
		}

		public override IIEReturnValue Execute(ExplicitActiveState cur) {
			IManagedPointer ptr = cur.EvalStack.Pop() as IManagedPointer;
			TypeReference typeRef = (TypeReference)Operand;

			if (typeRef.IsValueType) {
			} else {
				ptr.Value = ObjectReference.Null;
			}

			return nextRetval;
		}
	}*/

    public class LDELEMA : ObjectModelInstructionExec
    {
        public LDELEMA(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var idx = (Int4)cur.EvalStack.Pop();
            var objRef = (ObjectReference)cur.EvalStack.Pop();
            cur.EvalStack.Push(new ObjectFieldPointer(cur, objRef, idx.Value));
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            return false;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            return base.Accessed(thread, cur);
        }
    }

    public class LDELEM : ObjectModelInstructionExec
    {
        public LDELEM(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var ida = cur.EvalStack.Peek();
            var idx = (Int4)cur.EvalStack.Pop();
            var arrayRef = (ObjectReference)cur.EvalStack.Pop();

            var retval = nextRetval;
            var theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;

            if (theArray == null)
            {
                return ThrowException(new NullReferenceException(), cur);
            }

            if (CheckBounds(theArray, idx))
            {
                cur.EvalStack.Push(theArray.Fields[idx.Value]);
            }
            else
            {
                return ThrowException(new IndexOutOfRangeException(), cur);
            }

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;

            //return cur.ThreadPool.RunnableThreadCount == 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var length = cur.EvalStack.Length;

            var arrayRef = (ObjectReference)cur.EvalStack[length - 2];
            var theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;
            return theArray.ThreadShared;

        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var des = thread.CurrentMethod.EvalStack;
            var length = des.Length;

            var idx = (Int4)des.Peek();
            var arrayRef = (ObjectReference)des[length - 2];

            return new MemoryLocation(idx.Value, arrayRef, cur);
        }
    }

    public class STELEM : ObjectModelInstructionExec
    {

        public STELEM(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var val = cur.EvalStack.Pop();
            var idx = (Int4)cur.EvalStack.Pop();
            var arrayRef = (ObjectReference)cur.EvalStack.Pop();

            var retval = nextRetval;
            var theArray =
                cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;

            if (theArray == null)
            {
                return ThrowException(new System.NullReferenceException(), cur);
            }
            else if (CheckBounds(theArray, idx))
            {
                ObjectEscapePOR.UpdateReachability(theArray.ThreadShared, theArray.Fields[idx.Value], val, cur);

                cur.ParentWatcher.RemoveParentFromChild(arrayRef, theArray.Fields[idx.Value], cur.Configuration.MemoisedGC);
                theArray.Fields[idx.Value] = val;
                cur.ParentWatcher.AddParentToChild(arrayRef, val, cur.Configuration.MemoisedGC);
            }
            else
            {
                return ThrowException(new IndexOutOfRangeException(), cur);
            }

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
            //return cur.ThreadPool.RunnableThreadCount == 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var length = cur.EvalStack.Length;

            var arrayRef = (ObjectReference)cur.EvalStack[length - 3];
            var theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;
            return theArray.ThreadShared;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var des = thread.CurrentMethod.EvalStack;
            var length = des.Length;

            var idx = (Int4)des[length - 2];
            var arrayRef = (ObjectReference)des[length - 3];

            return new MemoryLocation(idx.Value, arrayRef, cur);
        }
    }

    /// <summary>
    /// Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
    /// </summary>
    public class LDLEN : ObjectModelInstructionExec
    {
        public LDLEN(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var arrayRef = (ObjectReference)cur.EvalStack.Pop();
            var theArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];

            if (theArray == null)
            {
                return ThrowException(new NullReferenceException(), cur);
            }
            else
            {
                cur.EvalStack.Push(new UnsignedInt4((uint)theArray.Fields.Length));
            }

            return nextRetval;
        }
    }

    // ------------------------------ Objects ------------------------------ 
    // Note that NEWOBJ is a sub-public class of CallInstructionExec, since it
    // actually calls something (a ctor), so it needs the functionality there.
    // We're putting these classes where they belong functionally, not where
    // they would be put "in the real world".


    /// <summary>
    /// Tests whether an object reference (type O) is an instance of a particular class.
    /// </summary>
    public class ISINST : ObjectModelInstructionExec
    {
        public ISINST(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var value = cur.EvalStack.Pop();
            var typeDef = Operand as ITypeDefOrRef;

            switch (value)
            {
                case ConstantString constantString:
                    var st = cur.DefinitionProvider.GetTypeDefinition("System.String");
                    if (cur.DefinitionProvider.IsSubtype(st, typeDef))
                    {
                        cur.EvalStack.Push(value);
                    }
                    else
                    {
                        cur.EvalStack.Push(ObjectReference.Null);
                    }
                    return nextRetval;
                case ObjectReference reference:
                    if (reference.Equals(ObjectReference.Null))
                    {
                        cur.EvalStack.Push(ObjectReference.Null);
                        return nextRetval;
                    }

                    var obj = cur.DynamicArea.Allocations[reference];
                    if (cur.DefinitionProvider.IsSubtype(obj.Type, typeDef))
                    {
                        cur.EvalStack.Push(reference);
                    }
                    else
                    {
                        cur.EvalStack.Push(ObjectReference.Null);
                    }

                    return nextRetval;
            }

            throw new NotImplementedException(value.GetType().FullName);
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    public class LDFLDA : ObjectModelInstructionExec
    {
        public LDFLDA(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var dataElement = cur.EvalStack.Pop();
            if (dataElement is LocalVariablePointer localVariablePointer)
            {
                dataElement = localVariablePointer.Value;
            }

            switch (dataElement)
            {
                case ObjectReference objRef:
                    var ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;
                    var offset = GetFieldOffset(ao.Type);
                    cur.EvalStack.Push(new ObjectFieldPointer(cur, objRef, offset));
                    return nextRetval;
            }

            throw new NotImplementedException(dataElement.GetType().FullName);
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            return false;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            return base.Accessed(thread, cur);
        }
    }

    public class LDFLD : ObjectModelInstructionExec
    {
        public LDFLD(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var reference = cur.EvalStack.Pop();

            AllocatedObject theObject;

            if (reference is ObjectReference objectReference)
            {
                theObject = (AllocatedObject)cur.DynamicArea.Allocations[objectReference];
            }
            else if (reference is MethodMemberPointer methodMemberPointer)
            {
                // Points to a ObjectReference
                theObject = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)(methodMemberPointer).Value];
            }
            else
            {
                cur.Logger.Warning("unknown reference type on stack: {0} ", reference.GetType());
                return nextRetval;
            }

            if (theObject == null)
            {
                return ThrowException(new System.NullReferenceException(), cur);
            }
            else
            {
                var offset = GetFieldOffset(theObject.Type);
                // FieldDefinition fld = GetFieldDefinition();
                cur.EvalStack.Push(theObject.Fields[offset]);
            }

            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var reference = cur.EvalStack.Peek();

            AllocatedObject theObject = null;

            var fieldDef = GetFieldDefinition();
            if (fieldDef.IsInitOnly)
            {
                return false;
            }

            if (reference is ObjectReference objectReference)
            {
                theObject = (AllocatedObject)cur.DynamicArea.Allocations[objectReference];
            }
            else if (reference is MethodMemberPointer methodMemberPointer)
            {
                // Points to a ObjectReference
                theObject = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)(methodMemberPointer).Value];
            }

            if (theObject == null)
            {
                throw new NullReferenceException("Allocated object not found for " + reference);
            }

            return theObject.ThreadShared;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var refVal = thread.CurrentMethod.EvalStack.Peek();
            if (refVal is LocalVariablePointer localVariablePointer)
            {
                refVal = localVariablePointer.Value;
            }

            switch (refVal)
            {
                case ObjectReference or:
                    var ao = cur.DynamicArea.Allocations[or] as AllocatedObject;
                    var offset = GetFieldOffset(ao.Type);
                    return new MemoryLocation(offset, or, cur);
            }

            throw new NotImplementedException(refVal.GetType().FullName);
            /*ObjectReference or;

            if (refVal is ObjectReference)
                or = (ObjectReference)refVal;
            else
                or = (ObjectReference)((MethodMemberPointer)refVal).Value;

            AllocatedObject ao = cur.DynamicArea.Allocations[or] as AllocatedObject;
            int offset = GetFieldOffset(ao.Type);

            return new MemoryLocation(offset, or, cur);*/
        }
    }

    public class STFLD : ObjectModelInstructionExec
    {

        public STFLD(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var val = cur.EvalStack.Pop();
            var toChange = cur.EvalStack.Pop();
            //FieldDefinition fld = GetFieldDefinition();

            if (toChange is LocalVariablePointer lvp)
            {
                toChange = lvp.Value;
            }

            // This is somewhat rancid, but for now we'll do it like this.
            if (toChange is ObjectReference objectReference)
            {
                // Change a field of an object.
                var theObject = cur.DynamicArea.Allocations[objectReference] as AllocatedObject;

                var offset = GetFieldOffset(theObject.Type);
                cur.ParentWatcher.RemoveParentFromChild(objectReference, theObject.Fields[offset], cur.Configuration.MemoisedGC);

                // Can be the case that an object reference was written, thereby changing the object graph
                ObjectEscapePOR.UpdateReachability(theObject.ThreadShared, theObject.Fields[offset], val, cur);

                theObject.Fields[offset] = val;
                cur.ParentWatcher.AddParentToChild(objectReference, val, cur.Configuration.MemoisedGC);
                return nextRetval;
            }

            throw new NotSupportedException($"unknown type storage destination: {toChange.GetType()}");
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var fieldDef = GetFieldDefinition();
            if (fieldDef.IsInitOnly)
            {
                return false;
            }

            var length = cur.EvalStack.Length;
            var toChange = cur.EvalStack[length - 2];

            if (toChange is ObjectReference)
            {
                var theObject = cur.DynamicArea.Allocations[(ObjectReference)toChange] as AllocatedObject;
                return theObject.ThreadShared;
            }

            return false;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var des = thread.CurrentMethod.EvalStack;

            var length = des.Length;
            var toChange = des[length - 2];

            if (toChange is ObjectReference)
            {
                var or = (ObjectReference)toChange;
                //FieldDefinition fld = GetFieldDefinition();
                var ao = cur.DynamicArea.Allocations[or] as AllocatedObject;
                var offset = GetFieldOffset(ao.Type);
                return new MemoryLocation(offset, or, cur);
            }

            return base.Accessed(thread, cur);
        }
    }

    public class BOX : ObjectModelInstructionExec
    {
        public BOX(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is a type definition of the wrapper type to use.
            var wrappedRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(),
                Operand as ITypeDefOrRef);
            var wrapped = (AllocatedObject)cur.DynamicArea.Allocations[wrappedRef];
            if (wrapped.Type.IsValueType)
            {
                wrapped.Fields[wrapped.ValueFieldOffset] = cur.EvalStack.Pop();
                cur.EvalStack.Push(wrappedRef);
            }
            else
            {
                // TODO hack
            }

            return nextRetval;
        }
    }

    public class UNBOX : ObjectModelInstructionExec
    {
        public UNBOX(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is a type definition of the wrapper type to use.
            var reference = (IReferenceType)cur.EvalStack.Pop();
            if (reference is ObjectReference objRef)
            {
                var wrapped = (AllocatedObject)cur.DynamicArea.Allocations[objRef];
                // Maybe a clone isn't even really needed here, the object pushed on the
                // stack will probably not live very long and the field will probably not
                // be updated while it's on the stack.
                if (wrapped.Type.IsValueType)
                {
                    cur.EvalStack.Push(wrapped.Fields[wrapped.ValueFieldOffset]);
                }
                else
                {
                    cur.EvalStack.Push(reference);
                }
            }
            else
            {
                cur.EvalStack.Push(reference);
            }
            return nextRetval;
        }
    }

    // ------------------------------ Classes ------------------------------ 

    public class LDSFLD : ObjectModelInstructionExec
    {
        public LDSFLD(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var retval = nincRetval;
            var fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            if (LoadClass(declType, cur))
            {
                var ac = cur.StaticArea.GetClass(declType);
                cur.EvalStack.Push(ac.Fields[(int)fld.FieldOffset]);
                retval = nextRetval;
            }

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
            //return cur.ThreadPool.RunnableThreadCount == 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            return !GetFieldDefinition().IsInitOnly;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            return new MemoryLocation((int)fld.FieldOffset, declType, cur);
        }
    }

    public class LDSFLDA : ObjectModelInstructionExec
    {
        public LDSFLDA(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var retval = nincRetval;
            var fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            if (LoadClass(declType, cur))
            {
                cur.EvalStack.Push(new StaticFieldPointer(cur, declType, (int)fld.FieldOffset));
                retval = nextRetval;
            }

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            return false;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            return base.Accessed(thread, cur);
        }
    }

    public class STSFLD : ObjectModelInstructionExec
    {

        public STSFLD(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var retval = nincRetval;
            var fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            if (LoadClass(declType, cur))
            {
                var val = cur.EvalStack.Pop();

                var ac = cur.StaticArea.GetClass(declType);
                ThreadObjectWatcher.Decrement(cur.ThreadPool.CurrentThreadId, ac.Fields[(int)fld.FieldOffset], cur);

                ObjectEscapePOR.UpdateReachability(true, ac.Fields[(int)fld.FieldOffset], val, cur);

                ac.Fields[(int)fld.FieldOffset] = val;

                ThreadObjectWatcher.Increment(cur.ThreadPool.CurrentThreadId, ac.Fields[(int)fld.FieldOffset], cur);

                retval = nextRetval;
            }

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
            //return cur.ThreadPool.RunnableThreadCount == 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            if (GetFieldDefinition().IsInitOnly)
                return false;
            else
                return true;
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            return new MemoryLocation((int)fld.FieldOffset, declType, cur);
        }
    }

    /// <summary>
    /// Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
    /// </summary>
    public class LDTOKEN : ObjectModelInstructionExec
    {
        public LDTOKEN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var retval = nincRetval;
            switch (Operand)
            {
                case TypeDefinition typeDef:
                    if (LoadClass(typeDef, cur))
                    {
                        retval = nextRetval;
                    }
                    cur.EvalStack.Push(new TypePointer(typeDef));
                    break;
                case MethodDefinition methodDef:
                    cur.EvalStack.Push(new MethodPointer(methodDef));
                    retval = nextRetval;
                    break;
                case FieldDefinition fieldDef:
                    cur.EvalStack.Push(new FieldHandle(fieldDef));
                    return nextRetval;
                default:
                    throw new NotSupportedException("LDTOKEN is currently unsupported with " + Operand.GetType());
            }

            // TODO: handle field and method definitions
            return retval;
        }
    }

    public class NumericInstructionExec : InstructionExecBase
    {

        public NumericInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    public class ADD : NumericInstructionExec
    {
        public ADD(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();

            IAddElement left;
            INumericElement right;

            /*
			 * Arithmetica on managed pointers is a little bit different.
			 * The offset is represented in bytes. Each (object|static|array) field 
			 * is seperated by 4 offset bytes, thus one integer
			 */

            if (a is IManagedPointer)
            {
                left = (IAddElement)a;
                right = (INumericElement)b;
            }
            else
            {
                left = (INumericElement)b;
                right = (INumericElement)a;
            }

            if (Unsigned)
            {
                left = ((ISignedIntegerElement)left).ToUnsigned();
                right = ((ISignedIntegerElement)right).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(left.Add(right, CheckOverflow));
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    /// <summary>
    /// Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
    /// </summary>
    public class ADD_OVF_UN : NumericInstructionExec
    {
        public ADD_OVF_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();

            IAddElement left;
            INumericElement right;

            /*
			 * Arithmetica on managed pointers is a little bit different.
			 * The offset is represented in bytes. Each (object|static|array) field 
			 * is seperated by 4 offset bytes, thus one integer
			 */

            if (a is IManagedPointer)
            {
                left = (IAddElement)a;
                right = (INumericElement)b;
            }
            else
            {
                left = (INumericElement)b;
                right = (INumericElement)a;
            }

            if (right is IRealElement && !(left is IRealElement))
            {
                IDataElement tmp = left;
                left = right;
                right = (INumericElement)tmp;
            }

            switch (left)
            {
                case Int4 i4:
                    var res4 = (UnsignedInt4)i4.ToUnsignedInt4(false).Add(right.ToUnsignedInt4(false), CheckOverflow);
                    cur.EvalStack.Push(res4.ToInt4(false));
                    return nextRetval;
                case Int8 i8:
                    var res8 = (UnsignedInt8)i8.ToUnsignedInt8(false).Add(right.ToUnsignedInt8(false), CheckOverflow);
                    cur.EvalStack.Push(res8.ToInt8(false));
                    return nextRetval;
            }

            if (Unsigned && left is ISignedIntegerElement && right is ISignedIntegerElement)
            {
                left = ((ISignedIntegerElement)left).ToUnsigned();
                right = ((ISignedIntegerElement)right).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(left.Add(right, CheckOverflow));
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    /* The DIV instruction is a numerical instruction, whereas the DIV.UN
	 * instuction is an integer instruction. No need to worry, the tables of
	 * the resulting types are the same when it comes to verifiable CIL code.
	 */
    public class DIV : NumericInstructionExec
    {
        public DIV(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = (INumericElement)cur.EvalStack.Pop();
            var a = (INumericElement)cur.EvalStack.Pop();

            if (Unsigned)
            {
                a = ((ISignedIntegerElement)a).ToUnsigned();
                b = ((ISignedIntegerElement)b).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(a.Div(b));
            }
            catch (DivideByZeroException e)
            {
                return ThrowException(e, cur);
            }
            catch (ArithmeticException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    public class MUL : NumericInstructionExec
    {

        public MUL(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = (INumericElement)cur.EvalStack.Pop();
            var a = (INumericElement)cur.EvalStack.Pop();

            if (Unsigned)
            {
                a = ((ISignedIntegerElement)a).ToUnsigned();
                b = ((ISignedIntegerElement)b).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(a.Mul(b, CheckOverflow));
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    public class REM : NumericInstructionExec
    {
        public REM(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = (INumericElement)cur.EvalStack.Pop();
            var a = (INumericElement)cur.EvalStack.Pop();

            if (Unsigned)
            {
                a = ((ISignedIntegerElement)a).ToUnsigned();
                b = ((ISignedIntegerElement)b).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(a.Rem(b));
            }
            catch (DivideByZeroException e)
            {
                return ThrowException(e, cur);
            }
            catch (ArithmeticException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    public class SUB : NumericInstructionExec
    {
        public SUB(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            var b = cur.EvalStack.Pop();

            var left = (ISubElement)b;
            var right = (INumericElement)a;

            if (Unsigned)
            {
                left = ((ISignedIntegerElement)left).ToUnsigned();
                right = ((ISignedIntegerElement)right).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(left.Sub(right, CheckOverflow));
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    /// <summary>
    /// Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.
    /// </summary>
    public class SUB_OVF_UN : NumericInstructionExec
    {
        public SUB_OVF_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = cur.EvalStack.Pop();
            var b = cur.EvalStack.Pop();

            var left = (ISubElement)b;
            var right = (INumericElement)a;

            left = ((ISignedIntegerElement)left).ToUnsigned();
            right = ((ISignedIntegerElement)right).ToUnsigned();

            try
            {
                var sub = left.Sub(right, CheckOverflow);
                switch (sub)
                {
                    case UnsignedInt4 ui4:
                        cur.EvalStack.Push(ui4.ToInt4(false));
                        break;
                    case UnsignedInt8 ui8:
                        cur.EvalStack.Push(ui8.ToInt8(false));
                        break;
                    default:
                        cur.EvalStack.Push(sub);
                        break;
                }
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            return nextRetval;
        }
    }

    public class NEG : NumericInstructionExec
    {
        public NEG(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var a = (ISignedNumericElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Neg());
            return nextRetval;
        }
    }

    public class CompareInstruction : InstructionExecBase
    {
        public CompareInstruction(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        protected int CompareOperands(IDataElement a, IDataElement b)
        {
            if (Unsigned && a is INumericElement na && b is INumericElement nb)
            {
                if (na.Equals(nb))
                {
                    return 0;
                }

                return na.ToUnsignedInt8(CheckOverflow).CompareTo(nb.ToUnsignedInt8(CheckOverflow));
            }

            return a.CompareTo(b);
        }
    }

    public class CEQ : CompareInstruction
    {
        public CEQ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            cur.EvalStack.Push(CompareOperands(a, b) == 0 ? new Int4(1) : new Int4(0));
            return nextRetval;
        }
    }

    public class CGT : CompareInstruction
    {
        public CGT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            cur.EvalStack.Push(CompareOperands(a, b) > 0 ? new Int4(1) : new Int4(0));
            return nextRetval;
        }
    }

    public class CLT : CompareInstruction
    {
        public CLT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var b = cur.EvalStack.Pop();
            var a = cur.EvalStack.Pop();
            cur.EvalStack.Push(CompareOperands(a, b) < 0 ? new Int4(1) : new Int4(0));
            return nextRetval;
        }
    }

    public class ExceptionHandlingInstructionExec : InstructionExecBase
    {
        public ExceptionHandlingInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    public class THROW : ExceptionHandlingInstructionExec
    {

        public THROW(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            /*
			 * leave only the exception reference on the stack
			 */
            var exceptionRef = cur.EvalStack.Pop();
            cur.EvalStack.PopAll();

            // signals all exception related instructions that an exception has been thrown
            cur.CurrentThread.ExceptionReference = (ObjectReference)exceptionRef;
            cur.CurrentMethod.IsExceptionSource = true;

            // now find the accompanied handler
            return ehLookupRetval;
        }
    }

    public class RETHROW : ExceptionHandlingInstructionExec
    {

        public RETHROW(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // now find the accompanied handler
            //TODO: zie endfiltert
            return ehLookupRetval;
        }
    }

    public class ENDFILTER : ExceptionHandlingInstructionExec
    {

        public ENDFILTER(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var val = (Int4)cur.EvalStack.Pop();
            cur.CallStack.Pop(); // pop off the methodstate made for only filtering

            if (val.Value == 0)
            {
                //TODO BUG: moet verder zoeken vanaf het waar de filter was gevonden, niet weer vanaf het begin
                return ehLookupRetval;
            }
            else
            {
                return finallyLookupRetval;
            }
        }
    }

    public class LEAVE : ExceptionHandlingInstructionExec
    {

        public LEAVE(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IIEReturnValue retval;
            ExceptionHandler eh;
            var current = cur.CurrentMethod;

            if (ObjectReference.Null.Equals(cur.CurrentThread.ExceptionReference))
            {
                eh = current.NextFinallyHandler(current.ProgramCounter);
            }
            else
            {
                eh = current.NextFinallyOrFaultHandler(current.ProgramCounter);
            }

            if (eh == null)
                retval = new JumpReturnValue((Instruction)Operand);
            else
                retval = new JumpReturnValue(eh.HandlerStart);

            // reset, stating that all exception issues are handled
            cur.CurrentThread.ExceptionReference = ObjectReference.Null;

            return retval;
        }
    }

    public class ENDFINALLY : ExceptionHandlingInstructionExec
    {
        public ENDFINALLY(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            /*
			 * TODO, two situations in which this method is called
			 * 1. we have found an exception handler and wish to execute all finally handlers
			 * in methods above us
			 * 
			 * 2. an exception has been handled, and we are currently finalising the current
			 * try catch block in the current method
			 */
            if (ObjectReference.Null.Equals(cur.CurrentThread.ExceptionReference))
            {
                return nextRetval;
            }
            else
                return finallyLookupRetval;
        }
    }

    /// <summary>
    /// Common base public class for all instructions that call a method.
    /// </summary>
    public class CallInstructionExec : InstructionExecBase
    {
        public CallInstructionExec(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        protected MethodDefinition Method => Operand as MethodDefinition;

        /// <summary>Check if a method is empty, i.e. it has no code.</summary>
        /// <param name="meth">The method.</param>
        /// <returns>False iff the method has code.</returns>
        protected bool IsEmptyMethod(MethodDefinition meth)
        {
            return meth.Body == null || meth.Body.Instructions.Count == 0;
        }

        /// <summary>Handle an empty method body.</summary>
        /// <remarks>
        /// <para>
        /// This checks if the empty body is a delegate invocation, or an
        /// internal call. Both are handled in this method.
        /// </para>
        /// <para>
        /// At the moment, many internal calls and asynchronous invocations of
        /// delegates are not handled.
        /// </para>
        /// </remarks>
        /// <param name="args">Arguments to the method to be called.</param>
        /// <param name="cur"></param>
        /// <returns>True iff the method has been properly handled.</returns>
        /// <seealso cref="IntCallManager"/>
        protected bool HandleEmptyMethod(ExplicitActiveState cur, DataElementList args)
        {
            var handled = false;
            DynamicAllocation thisObject = null;
            if (args.Length > 0 && args[0] is ObjectReference)
                thisObject = cur.DynamicArea.Allocations[(ObjectReference)args[0]];
            var methDef = Method;

            // Determine the type of call and simulate the behaviour.
            if ((methDef.Attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PinvokeImpl)
            {
            }

            if (methDef.FullName == "System.Int32 System.Runtime.CompilerServices.RuntimeHelpers::GetHashCode(System.Object)")
            {
                cur.EvalStack.Push(new Int4(args[0].GetHashCode()));
                return true;
            }

            if (methDef.FullName == "System.Threading.Thread System.Threading.Thread::GetCurrentThreadNative()")
            {
                ThreadHandlers.CurrentThread_internal(null, null, cur);
                return true;
            }

            // Delegate Invoke call.
            // TODO: Asynchronous delegate calls, i.e. BeginInvoke and EndInvoke.
            if (thisObject is AllocatedDelegate && methDef.Name == "Invoke")
            {
                cur.Logger.Log(LogPriority.Call, "invoke: call is a synchronous delegate call");
                // Create caller block, shifting parameters so delegate is dropped
                // from argument list.
                var calleePars = cur.StorageFactory.CreateList(args.Length - 1);
                for (var i = 0; i < calleePars.Length; ++i)
                    calleePars[i] = args[i + 1];
                var toCall =
                    ((AllocatedDelegate)thisObject).Method.Value;
                // Create frame for the indirectly called method.
                var calleeState = new MethodState(toCall, calleePars, cur);
                cur.CallStack.Push(calleeState);
                return true;
            }

            // Internal call. This is like a software interrupt (a.k.a. trap) in normal operating systems.
            if ((methDef.ImplAttributes & MethodImplAttributes.InternalCall) != 0)
            {
                cur.Logger.Log(LogPriority.Call, "{0}: call is an internal call", methDef.Name);
                handled = MMC.ICall.IntCallManager.icm.HandleICall(methDef, args);

                if (!handled)
                    cur.Logger.Message("Not (yet) implemented internal call: {0}", methDef.ToString());
            }
            return handled;
        }

        // TODO fix documentation of args
        /// <summary>Handle assertion violations.</summary>
        /// <remarks>
        /// <para>This handles calls System.Diagnostics.Debug.Assert(*).</para>
        /// <para>
        /// This is not a nice place to put this code. All code should be put
        /// in the ICM, which is not just an internal call manager any more,
        /// but this makes the approach much more generic. 
        /// The ICM should be adjusted to handle polymorphic calls for this to
        /// be possible.
        /// </para>
        /// </remarks>
        /// <param name="args">Arguments to the Assert call.</param>
        /// <returns>True iff the call was handled by this method.</returns>
        protected bool HandleAssertCall(DataElementList args, ExplicitActiveState cur, out bool violated)
        {
            var methDef = Operand as MethodDefinition;
            string name = methDef.Name;
            var decl = methDef.DeclaringType.Namespace + "." + methDef.DeclaringType.Name;
            var assert = name == "Assert" && decl == "System.Diagnostics.Debug";
            violated = false;

            if (assert)
            {
                violated = !args[0].ToBool();
                if (violated)
                {
                    if (args.Length > 1)
                    {
                        cur.Logger.Warning("short message: {0}", args[1].ToString());
                    }
                    if (args.Length > 2)
                    {
                        cur.Logger.Warning("long message: {0}", args[2].ToString());
                    }
                }
                else
                {
                    cur.Logger.Debug("assertion passed.");
                }
            }
            return assert;
        }

        /// <summary>
        /// Filter out specific calls.
        /// </summary>
        /// <remarks>This code shouldn't be. It was introduced as a quick hack, and survived several re-factorings.
        /// Like HandleAssertCall, this should be merged with the ICM.</remarks>
        /// <param name="cur"></param>
        /// <returns>True iff the call to be made is to be filtered out.</returns>
        protected bool FilterCall(ExplicitActiveState cur)
        {
            var methDef = Operand as MethodDefinition;
            string name = methDef.Name;
            var decl = methDef.DeclaringType.Namespace + "." + methDef.DeclaringType.Name;
            var filtered =
                //(name == "WriteLine" && decl == "System.Console")
                //|| 
                name == "StartupSetApartmentStateInternal";
            if (filtered)
            {
                cur.Logger.Log(LogPriority.Call, "{0}: method is filtered out, not executed", name);
            }
            return filtered;
        }

        /// <summary>Create an argument list.</summary>
        /// <remarks>
        /// This constructs a argument list of the correct size, and fills it
        /// with elements popped off the eval stack (in right to left order).
        /// </remarks>
        /// <returns>A list containing the arguments.</returns>
        protected DataElementList CreateArgumentList(ExplicitActiveState cur)
        {
            var methDef = Operand as MethodDefinition;
            var size = methDef.ParamDefs.Count + (methDef.HasThis ? 1 : 0);
            var retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size >= 0; --size)
                retval[size] = cur.EvalStack.Pop();

            return retval;
        }

        protected DataElementList CopyArgumentList(ExplicitActiveState cur, ThreadState thread)
        {
            var methDef = Operand as MethodDefinition;
            var size = methDef.Parameters.Count;// + (methDef.HasThis ? 1 : 0);
            var retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size >= 0; --size)
            {
                retval[size] = thread.CurrentMethod.EvalStack.Pop();
            }

            /*
			 * Restore the eval stack
			 */
            for (var i = 0; i < retval.Length; i++)
            {
                thread.CurrentMethod.EvalStack.Push(retval[i]);
            }

            return retval;
        }

        protected void CheckTailCall()
        {
        }

        /// <summary>Determine if this call is safe.</summary>
        /// <remarks>
        /// Usually calls are safe, but this may not be the case for some
        /// internal calls. In this case, return the value given by the ICM.
        /// </remarks>
        /// <returns>True iff the call is safe.</returns>
        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            var safe = true;
            var methDef = Operand as MethodDefinition;
            if ((methDef.ImplAttributes & MethodImplAttributes.InternalCall) != 0)
            {
                safe = IntCallManager.icm.IsMultiThreadSafe(methDef.Name);
            }

            return safe;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var methDef = Operand as MethodDefinition;
            if ((methDef.ImplAttributes & MethodImplAttributes.InternalCall) != 0)
            {
                var args = CopyArgumentList(cur, cur.ThreadPool.CurrentThread);
                return IntCallManager.icm.IsDependent(methDef, args);
            }
            else
            {
                return false;
            }
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var methDef = Operand as MethodDefinition;
            var args = CopyArgumentList(cur, thread);
            return MMC.ICall.IntCallManager.icm.HandleICallAccessed(methDef, args, thread.Id);
        }

        public override string ToString()
        {
            var method = Method;
            return base.ToString() + " " + method;
        }
    }

    public class JMP : InstructionExecBase
    {

        public JMP(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            var oldState = cur.CallStack.Pop();

            var methDef = Operand as MethodDefinition;
            var newState = new MethodState(methDef, oldState.Arguments, cur);
            cur.CallStack.Push(newState);

            return nincRetval;
        }
    }

    /// <summary>
    /// A CALL instruction.
    /// </summary>
    public class CALL : CallInstructionExec
    {
        public CALL(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr)
        {
        }

        /// <summary>
        /// Execute the CALL instruction.
        /// </summary>
        /// <returns></returns>
        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var methDef = Operand as MethodDefinition;

            // This pops the arguments of the stack. Even if we don't execute
            // the method. This is a good thing.
            var args = CreateArgumentList(cur);
            var retval = nextRetval;

            // Skip certain calls.
            bool violated;
            if (!HandleAssertCall(args, cur, out violated) && !FilterCall(cur))
            {
                var bypass = NativePeer.Get(methDef.DeclaringType);
                if (bypass != null)
                {
                    if (bypass.TryGetValue(methDef, args, cur, out var returnValue))
                    {
                        return returnValue;
                    }
                }

                if (methDef.DeclaringType.IsValueType && methDef.Name == "ToString") // TODO
                {
                    cur.EvalStack.Push(DataElement.CreateDataElement(args[0].ToString(), cur.DefinitionProvider));
                    return nextRetval;
                }

                // Check for empty body (stub).
                if (IsEmptyMethod(methDef))
                {
                    cur.Logger.Log(LogPriority.Call, "{0}: instance call to method with no body.", methDef.FullName);
                    if (!HandleEmptyMethod(cur, args))
                    {
                        cur.Logger.Log(LogPriority.Call, "{0}: unhandled empty method call.", methDef.FullName);
                    }
                }
                else
                {
                    // Create new frame. Note we still return nextRetval, so
                    // the call instruction is not re-executed after returning.
                    //
                    // Before the scheduler cleanup we returned a jump target
                    // from this call (as a up-casted Instruction instance,
                    // yuck). This is BAD idea since it requires nasty hacks
                    // (to update the PC) to get right and is poorly readable.
                    var called = new MethodState(methDef, args, cur);
                    this.CheckTailCall();
                    cur.CallStack.Push(called);
                }
            }
            else
            {
                // assertion could be violated, if so, then stop the exploration
                if (violated)
                    retval = assertionViolatedRetval;

                // Gotcha: don't forget to dispose of the arguments if we don't
                // use them.
                args.Dispose();
            }

            return retval;
        }
    }

    /// A CALLI instruction.
    public class CALLI : CallInstructionExec
    {

        public CALLI(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        /// <summary>
        /// Execute the CALLI instruction.
        /// </summary>
        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // See MMC.InstructionExec.CALL.Execute(...) for comments.

            var methPtr = (MethodPointer)cur.EvalStack.Pop();
            var methDef = methPtr.Value;

            var args = CreateArgumentList(cur);

            if (!FilterCall(cur))
            {
                // Assumption: CALLI targets have a body.
                var called = new MethodState(methDef, args, cur);
                this.CheckTailCall();
                cur.CallStack.Push(called);
            }
            else
            {
                //ThreadObjectWatcher.RemoveAllInContainer(cur.ThreadPool.CurrentThreadId, args);
                args.Dispose();
            }

            return nextRetval;
        }
    }

    /// <summary>
    /// A CALLVIRT instruction.
    /// </summary>
    public class CALLVIRT : CallInstructionExec
    {
        public CALLVIRT(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Virtual calls use the run-time type of an object to determine
            // the location of the method to be called. It always requires
            // a this pointer, but we use the CreateArgumentList.
            var methDef = Operand as MethodDefinition;
            var args = CreateArgumentList(cur);

            if (methDef.FullName == "System.Void System.Threading.Thread::Start()")
            {
                var threadId = cur.ThreadPool.FindOwningThread(args[0]);
                if (threadId == LockManager.NoThread)
                {
                    throw new NotSupportedException("Owning thread not found.");
                }
                cur.ThreadPool.Threads[threadId].State = System.Threading.ThreadState.Running;
                return nextRetval;
            }

            // Skip certain calls.
            if (!FilterCall(cur))
            {
                var bypass = NativePeer.Get(methDef.DeclaringType);
                if (bypass != null)
                {
                    if (bypass.TryGetValue(methDef, args, cur, out var returnValue))
                    {
                        return returnValue;
                    }
                }
                // Check for empty body. This will catch the virtual calls to e.g.
                // the Invoke method of delegate types.
                /*
				 * The below has been commented out by Viet Yen Nguyen, because I do not know 
				 * what purpose this precisely serves...
				 */
                /*if (IsEmptyMethod(methDef)) {
					cur.Logger.Log(LogPriority.Warning, "{0}: virtual call to method with no body", methDef.Name);
					if (!HandleEmptyMethod(args))
						cur.Logger.Warning("{0}: unhandled virtual method call", methDef.Name);
				}*/

                // Normal virtual call: get run-time, and start searching for method
                // definition there (tarversing the object-tree upward).
                /*
				TypeReference type;
				if (args[0].WrapperName != "")
					// Element is a value type which should be wrapped.
					// TODO: Deal with instances of System.Array.
					type = cur.DefinitionProvider.GetTypeDefinition(args[0].WrapperName);
				else {
					// Else get type from dynamic area.
					Debug.Assert(args[0] != null, "No object to call on. How can this be static?");
					AllocatedObject theObject = (AllocatedObject)(
							cur.DynamicArea.Allocations[(ObjectReference)args[0]]);
					Debug.Assert(theObject != null,
							"BAD! Ohhhh sooooo bad! Object to perform CALLVIRT on is null. " +
							"Got object from reference " + args[0].ToString());
					Debug.Assert(theObject.Type != null, "No type set for object.");
					type = theObject.Type;
				}*/
                // Search inheritence tree for most derived implementation.
                MethodDefinition toCall = null;
                var constrained = cur.CurrentMethod.Constrained;
                if (cur.CurrentMethod.IsPrefixed
                    && constrained?.IsValueType == true
                    && methDef.DeclaringType == constrained)
                {
                    toCall = methDef;
                }
                else
                {
                    toCall = args[0].FindVirtualMethod(methDef, cur);
                }

                cur.CurrentMethod.IsPrefixed = false;
                cur.CurrentMethod.Constrained = null;

                var called = new MethodState(toCall, args, cur);
                this.CheckTailCall();
                cur.CallStack.Push(called);
                //cur.Logger.Log(LogPriority.Call, "{0}: found most derived definition in type {1}",
                //		methDef.Name, type.Name);		

            }
            else
            {
                // End Filter If //
                //ThreadObjectWatcher.RemoveAllInContainer(cur.ThreadPool.CurrentThreadId, args);
                args.Dispose();
            }

            return nextRetval;
        }
    }

    /// <summary>
    /// Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the evaluation stack.
    /// </summary>
    public class NEWOBJ : CallInstructionExec
    {
        public NEWOBJ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Basically the same as a normal call, except first we need
            // to create a new object, and pass it as argument 0 (the
            // rest of the arguments are on the stack, last on top)
            var methDef = Operand as MethodDefinition;

            // Note that this loop condition is "i > 0", not "i >= 0". This is because
            // we need to fill in args[0] (i.e. the this pointer) ourselves.
            var args = cur.StorageFactory.CreateList(methDef.ParamDefs.Count + 1);
            for (var i = args.Length - 1; i > 0; --i)
            {
                args[i] = cur.EvalStack.Pop();
            }

            var nativePeer = NativePeer.Get(methDef.DeclaringType);
            if (Method.IsConstructor 
                && nativePeer != null
                && nativePeer.TryConstruct(methDef, args, cur))
            {
                return nextRetval;
            }

            /*if (Method.DeclaringType.FullName == "System.Threading.Thread" && Method.IsConstructor)
            {
                ThreadHandlers.Thread_internal(Method, args, cur);

                //MMC.ICall.IntCallManager.
                //ObjectReference threadObjectRef = cur.DynamicArea.AllocateObject(
                //cur.DynamicArea.DeterminePlacement(false),
                //cur.DefinitionProvider.GetTypeDefinition("System.Threading.Thread"));*

                //cur.ThreadPool.NewThread(null, threadObjectRef);
                //return threadObjectRef;
                //return cur.EvalStack.Pop();
                return nextRetval;
            }*/

            // Check for empty body.
            if (IsEmptyMethod(methDef))
            {
                if ((args[1] is ObjectReference) && (args[2] is MethodPointer))
                {
                    IDataElement newDel = cur.DynamicArea.AllocateDelegate(
                        cur.DynamicArea.DeterminePlacement(),
                        (ObjectReference)args[1],
                        (MethodPointer)args[2]);
                    cur.EvalStack.Push(newDel);
                    cur.Logger.Log(LogPriority.Call, "constructor call for delegate handled by ves");
                }
                else
                {
                    cur.Logger.Warning("empty constructor, but does not look like delegate");
                }

                args.Dispose();
            }
            else
            {
                // Normal constructor call, create this pointer.
                args[0] = cur.DynamicArea.AllocateObject(
                        cur.DynamicArea.DeterminePlacement(),
                        methDef.DeclaringType);
                // Constructor calls should leave object reference on the stack.
                cur.EvalStack.Push(args[0]);
                // Call the constructor.
                var called = new MethodState(methDef, args, cur);
                cur.CallStack.Push(called);
            }

            return nextRetval;
        }

        // NEWOBJ calls are safe. Since a new object is being created just now,
        // only one thread can own a reference to it.
        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    /// <summary>
    /// Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto 
    /// the caller's evaluation stack.
    /// </summary>
    public class RET : InstructionExecBase
    {
        public RET(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // If there's something on the evaluation stack of the callee, we
            // need to push this on the eval stack of the caller. This is how
            // return values are passed. The program counter should be set to
            // the previous method, just after the last (call) instruction
            // executed. Note the Main method is also terminated with a 'ret'.
            // NOTE: Popping has a side-effect! If reference counting is enabled,
            // it calls the Dispose() method on the method state, which, in turn,
            // calls Dispose() on all members (like locals and argument lists).
            var callee = cur.CallStack.Pop();
            if (cur.CallStack.StackPointer >= /* was > before */ 0 && callee.EvalStack.StackPointer > 0)
            {
                var retValue = callee.EvalStack.Pop();
                cur.CurrentThread.RetValue = retValue;
                cur.EvalStack?.Push(retValue);
            }

            ThreadObjectWatcher.DecrementAll(callee.Arguments, cur);
            ThreadObjectWatcher.DecrementAll(callee.Locals, cur);

            /*
			 * If the underlying method state threw an exception, and we are now returning to it,
			 * means that we return from the ctor of the thrown exception and that we should
			 * start finding the appropiate handlers, if any
			 */
            if (cur.CallStack.StackPointer > 0 && cur.CurrentMethod.IsExceptionSource)
                return ehLookupRetval;
            else
                return nextRetval;
        }

        /// <summary>
        /// Return is always safe. Locks are released in MethodState.Dispose().
        /// </summary>
        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    public class ConvertInstructionExec : InstructionExecBase
    {

        public ConvertInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    public class CKFINITE : ConvertInstructionExec
    {
        public CKFINITE(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var ida = (IRealElement)cur.EvalStack.Pop();

            if (ida.IsFinite())
            {
                cur.EvalStack.Push(ida);
            }
            else
            {
                /*
				 * For some reason, Microsoft's IL_BVT tests raise an
				 * overflow exception instead of an arithmeticexception
				 * as specified in the ECMA standard. Overflow is a subtype of
				 * arithmetic anyway, so we just follow Microsoft here
				 */
                return ThrowException(new ArithmeticException(), cur);
            }

            return nextRetval;
        }
    }

    public class CONV : ConvertInstructionExec
    {
        public CONV(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Determine the desired type. The PushBehaviour enum is no
            // good here (it doesn't deal with the shorter forms).
            // Format of conv.* instructions are as follows:
            // conv.[iru][1248], where not all combinations are legal.
            var tokens = Instruction.OpCode.Name.Split(new char[] { '.' });
            IDataElement toPush = null;
            var popped = cur.EvalStack.Pop();

            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            // put aside the ovf part in the instruction
            if (CheckOverflow)
            {
                tokens[1] = tokens[2];
            }

            try
            {
                // (Signed) others (formatted as described above)
                switch (tokens[1][0])
                {
                    case 'i':
                        if (tokens[1].Length == 1) // conv.i (native int = i4)
                        {
                            toPush = a.ToInt4(CheckOverflow);
                        }
                        else
                        {
                            switch (tokens[1][1])
                            {
                                case '1': // i1, byte
                                    toPush = a.ToInt4(false).ToByte(CheckOverflow);
                                    break;
                                case '2': // i2, short
                                    toPush = a.ToInt4(false).ToShort(CheckOverflow);
                                    break;
                                case '4': // i4, int
                                    toPush = a.ToInt4(CheckOverflow);
                                    break;
                                case '8': // i8, long
                                    toPush = Unsigned ? (IDataElement)a.ToUnsignedInt8(CheckOverflow) : a.ToInt8(CheckOverflow);
                                    break;
                                default: // error
                                    break;
                            }
                        }
                        break;
                    case 'u':
                        if (tokens[1].Length == 1) // conv.u (native unsigned int = u4)
                        {
                            toPush = a.ToUnsignedInt4(CheckOverflow);
                        }
                        else
                        {
                            switch (tokens[1][1])
                            {
                                case '1': // u1, ubyte
                                    switch (a)
                                    {
                                        case Float4 f4:
                                            toPush = f4.ToByte(CheckOverflow);
                                            break;
                                        case Float8 f8:
                                            toPush = f8.ToByte(CheckOverflow);
                                            break;
                                        default:
                                            toPush = a.ToUnsignedInt4(false).ToByte(CheckOverflow);
                                            break;
                                    }
                                    // Float only - new Int4((byte)Convert.ChangeType(a, typeof(byte)));//
                                    break;
                                case '2': // u2, ushort
                                    toPush = a.ToUnsignedInt4(false).ToShort(CheckOverflow);
                                    break;
                                case '4': // u4, uint
                                    toPush = a.ToUnsignedInt4(CheckOverflow);
                                    break;
                                case '8': // u8, ulong
                                    toPush = a.ToUnsignedInt8(CheckOverflow);
                                    break;
                                default: // error
                                    break;
                            }
                        }
                        break;
                    case 'r':
                        if (tokens[1].Length > 1)
                        {
                            switch (tokens[1][1])
                            {
                                case '4': // r4, float
                                    toPush = a.ToFloat4(CheckOverflow);
                                    break;
                                case '8': // r8, double
                                    toPush = a.ToFloat8(CheckOverflow);
                                    break;
                                default: // error
                                    break;
                            }
                        }

                        if (tokens.Length == 3)
                        {
                            // opcode is conv.r.un

                            /*
							 * SPEC deviation!
							 * 
							 * ECMA says we should choose appropiately between
							 * float4 and float8, the latter should be chosen
							 * for no loss of representation. We keep it on the 
							 * safe side, and always do a float8
							 */
                            toPush = a.ToFloat8(CheckOverflow);
                        }
                        break;
                }
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the signed value on top of the evaluation stack to signed int64, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_I8 : ConvertInstructionExec
    {
        public CONV_OVF_I8(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is UnsignedInt4 ui4)
                {
                    cur.EvalStack.Push(a.ToInt4(false).ToInt8(CheckOverflow));
                    return nextRetval;
                }

                cur.EvalStack.Push(a.ToInt8(CheckOverflow));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the signed value on top of the evaluation stack to signed native int, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_I : ConvertInstructionExec
    {
        public CONV_OVF_I(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                cur.EvalStack.Push(a.ToInt4(false));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the signed value on top of the evaluation stack to signed native int, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_I_UN : ConvertInstructionExec
    {
        public CONV_OVF_I_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                cur.EvalStack.Push(a.ToInt4(CheckOverflow));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the unsigned value on top of the evaluation stack to signed int64, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_I8_UN : ConvertInstructionExec
    {
        public CONV_OVF_I8_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                switch (a)
                {
                    case Int4 i4:
                        cur.EvalStack.Push(i4.ToUnsignedInt4(false).ToInt8(false)); // todo why?
                        return nextRetval;
                    default:
                        var unsigned = a.ToUnsignedInt8(true).ToInt8(false);//.Value;
                        cur.EvalStack.Push(unsigned);// new Int8((long)unsigned));
                        return nextRetval;
                }
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the unsigned value on top of the evaluation stack to unsigned native int, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_U_UN : ConvertInstructionExec
    {
        public CONV_OVF_U_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int4 i4)
                {
                    var uintptr = (UIntPtr)a.ToUnsignedInt4(false).Value;
                    cur.EvalStack.Push(DataElement.CreateDataElement(uintptr, cur.DefinitionProvider));
                }
                /*else if (a is Int8 i8)
                {
                    var uintptr = (UIntPtr)a.ToUnsignedInt8(false).Value;
                    cur.EvalStack.Push(cur.DefinitionProvider.CreateDataElement(uintptr));
                }*/
                else
                {
                    var unsigned = a.ToUnsignedInt8(false).Value;
                    cur.EvalStack.Push(new UnsignedInt8(checked(unsigned)));
                }
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the unsigned value on top of the evaluation stack to unsigned int32, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_U4_UN : ConvertInstructionExec
    {
        public CONV_OVF_U4_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                var unsigned = a.ToUnsignedInt4(false).Value;
                cur.EvalStack.Push(new UnsignedInt4(checked(unsigned)));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the unsigned value on top of the evaluation stack to unsigned int64, throwing OverflowException on overflow.
    /// </summary>
    public class CONV_OVF_U8_UN : ConvertInstructionExec
    {
        public CONV_OVF_U8_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int4 i4)
                {
                    var value = a.ToUnsignedInt4(false).ToUnsignedInt8(false);
                    cur.EvalStack.Push(value);
                }
                else
                {
                    var unsigned = a.ToUnsignedInt8(false).Value;
                    cur.EvalStack.Push(new UnsignedInt8(checked(unsigned)));
                }
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the value on top of the evaluation stack to float32.
    /// </summary>
    public class CONV_R4 : ConvertInstructionExec
    {
        public CONV_R4(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int8 || a is UnsignedInt8)
                {
                    cur.EvalStack.Push(a.ToInt8(false).ToFloat4(false));
                }
                else if (a is Float8 || a is Float4)
                {
                    cur.EvalStack.Push(a.ToFloat4(false));
                }
                else
                {
                    cur.EvalStack.Push(a.ToInt4(false).ToFloat4(false));
                }

                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the value on top of the evaluation stack to float64.
    /// </summary>
    public class CONV_R8 : ConvertInstructionExec
    {
        public CONV_R8(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int8 || a is UnsignedInt8)
                {
                    cur.EvalStack.Push(a.ToInt8(false).ToFloat8(false));
                }
                else if (a is Float8 || a is Float4)
                {
                    cur.EvalStack.Push(a.ToFloat8(false));
                }
                else
                {
                    cur.EvalStack.Push(a.ToInt4(false).ToFloat8(false));
                }

                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the unsigned integer value on top of the evaluation stack to float32.
    /// </summary>
    public class CONV_R_UN : ConvertInstructionExec
    {
        public CONV_R_UN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int8 || a is UnsignedInt8)
                {
                    cur.EvalStack.Push(a.ToUnsignedInt8(false).ToFloat4(false));
                    return nextRetval;
                }

                cur.EvalStack.Push(a.ToUnsignedInt4(false).ToFloat4(false));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    public class CONV_I8 : ConvertInstructionExec
    {
        public CONV_I8(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is UnsignedInt4 ui4)
                {
                    cur.EvalStack.Push(ui4.ToInt4(false).ToInt8(false));
                    return nextRetval;
                }

                cur.EvalStack.Push(a.ToInt8(false));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the value on top of the evaluation stack to unsigned int16, and extends it to int32.
    /// </summary>
    public class CONV_U2 : ConvertInstructionExec
    {
        public CONV_U2(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is IRealElement r)
                {
                    cur.EvalStack.Push(DataElement.CreateDataElement((ushort)r.ToFloat8(false).Value, cur.DefinitionProvider));
                    return nextRetval;
                }

                cur.EvalStack.Push(DataElement.CreateDataElement((ushort)a.ToInt8(false).Value, cur.DefinitionProvider));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    /// <summary>
    /// Converts the value on top of the evaluation stack to unsigned int64, and extends it to int64.
    /// </summary>
    public class CONV_U8 : ConvertInstructionExec
    {
        public CONV_U8(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            var a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int4 i4)
                {
                    cur.EvalStack.Push(a.ToUnsignedInt4(false).ToUnsignedInt8(false));
                    return nextRetval;
                }

                cur.EvalStack.Push(a.ToUnsignedInt8(false));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                return ThrowException(e, cur);
            }

            cur.EvalStack.Push(toPush);

            return nextRetval;
        }
    }

    public class StoreInstructionExec : InstructionExecBase
    {
        public StoreInstructionExec(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
    }

    public class STARG : StoreInstructionExec
    {
        public STARG(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is a VariableDefinition. In all but the rarest of cases
            // starg.s is used, which has a more efficient encoding for the first
            // 256 arguments (programmers defining method with that many arguments
            // should be in the circus).
            int index = ((ParameterDefinition)Operand).ParamDef.Sequence;
            if (!cur.CurrentMethod.Definition.HasThis)
            {
                index--;
            }

            var ide = cur.EvalStack.Pop();

            /*
			 * For heap analysis, TODO this should be done in a much cleaner fashion... */
            if (ide is ObjectReference)
            {
                var oldIde = cur.CurrentMethod.Arguments[index];
                ThreadObjectWatcher.Decrement(cur.ThreadPool.CurrentThreadId, (ObjectReference)oldIde, cur);
                ThreadObjectWatcher.Increment(cur.ThreadPool.CurrentThreadId, (ObjectReference)ide, cur);

                //if (!oldIde.Equals(ObjectReference.Null))
                //	Explorer.ActivateGC = true;
            }

            cur.CurrentMethod.Arguments[index] = ide;

            return nextRetval;
        }
    }

    public class STLOC : StoreInstructionExec
    {
        public STLOC(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr) {}

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is either a number 0-3, or a VariableDefinition, which
            // has a property Index, which is the number we want.
            int index;
            if (HasImplicitOperand)
                index = ((Int4)Operand).Value;
            else
                index = ((Local)Operand).Index;

            var ide = cur.EvalStack.Pop();

            // For heap analysis, TODO this should be done in a much cleaner fashion ...
            if (ide is ObjectReference objRef)
            {
                var oldIde = cur.CurrentMethod.Locals[index];
                ThreadObjectWatcher.Decrement(cur.ThreadPool.CurrentThreadId, (ObjectReference)oldIde, cur);
                ThreadObjectWatcher.Increment(cur.ThreadPool.CurrentThreadId, objRef, cur);
                //if (!oldIde.Equals(ObjectReference.Null))
                //	Explorer.ActivateGC = true;
            }

            cur.CurrentMethod.Locals[index] = ide;

            return nextRetval;
        }
    }

    // Not really 'storage'... :-)
    public class POP : StoreInstructionExec
    {

        public POP(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            cur.EvalStack.Pop();
            return nextRetval;
        }
    }

    public class STOBJ : STIND
    {

        public STOBJ(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }
        /*
		public override IIEReturnValue Execute(ExplicitActiveState cur) {
			IDataElement val = cur.EvalStack.Pop();
			IManagedPointer mmp = cur.EvalStack.Pop() as IManagedPointer;
			mmp.Value = val;
			return nextRetval;
		}

		public override bool IsMultiThreadSafe(ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();
			return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
		}

		public override bool IsDependent(ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				AllocatedObject theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

				return theObject.ThreadShared;
			} else if (reference is StaticFieldPointer) {
				return true;
			} else
				return false;
		}

		public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				return ofp.MemoryLocation;
			} else if (reference is StaticFieldPointer) {
				StaticFieldPointer sfp = (StaticFieldPointer)reference;
				return sfp.MemoryLocation;
			}

			return base.Accessed(thread);
		}*/
    }

    public class STIND : StoreInstructionExec
    {
        public STIND(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var val = cur.EvalStack.Pop();
            var mmp = cur.EvalStack.Pop() as IManagedPointer;
            mmp.Value = val;
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            var length = cur.EvalStack.Length;
            var reference = cur.EvalStack[length - 2];
            return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            var length = cur.EvalStack.Length;
            var reference = cur.EvalStack[length - 2];

            if (reference is ObjectFieldPointer)
            {
                var ofp = (ObjectFieldPointer)reference;
                var theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

                return theObject.ThreadShared;
            }
            else if (reference is StaticFieldPointer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override MemoryLocation Accessed(ThreadState thread, ExplicitActiveState cur)
        {
            var length = thread.CurrentMethod.EvalStack.Length;
            var reference = thread.CurrentMethod.EvalStack[length - 2];

            if (reference is ObjectFieldPointer)
            {
                var ofp = (ObjectFieldPointer)reference;
                return ofp.MemoryLocation;
            }
            else if (reference is StaticFieldPointer)
            {
                var sfp = (StaticFieldPointer)reference;
                return sfp.MemoryLocation;
            }

            return base.Accessed(thread, cur);
        }
    }

    /// <summary>
    /// Initializes each field of the value type at a specified address to a null reference 
    /// or a 0 of the appropriate primitive type.
    /// </summary>
    public class INITOBJ : InstructionExecBase
    {
        public INITOBJ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var reference = cur.EvalStack.Pop();
            if (reference is LocalVariablePointer localVariablePointer)
            {
                var typeTok = (ITypeDefOrRef)Operand;
                localVariablePointer.Value = cur.DynamicArea.AllocateObject(cur.DynamicArea.DeterminePlacement(), typeTok);
                return nextRetval;
            }

            throw new NotSupportedException($"IManagedPointer expected, {reference?.GetType().FullName ?? "null"} found.");
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true; // similar as NEWOBJ
        }
    }

    public class CONSTRAINED : InstructionExecBase
    {
        public CONSTRAINED(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            cur.CurrentMethod.IsPrefixed = true;
            cur.CurrentMethod.Constrained = Operand as ITypeDefOrRef;
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    public class UNALIGNED : InstructionExecBase
    {
        public UNALIGNED(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            cur.CurrentMethod.IsPrefixed = true;
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    /// <summary>
    /// Pushes the size, in bytes, of a supplied value type onto the evaluation stack.
    /// </summary>
    public class SIZEOF : InstructionExecBase
    {
        public SIZEOF(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var type = (ITypeDefOrRef)Operand;
            if (type.IsValueType)
            {
                cur.EvalStack.Push(new Int4(cur.DefinitionProvider.SizeOf(type.ToTypeSig())));
            }
            else
            {
                cur.EvalStack.Push(new Int4(cur.DefinitionProvider.SizeOf(cur.DefinitionProvider.BaseTypes.IntPtr)));
            }

            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    /// <summary>
    /// Copies the value type located at the address of an object (type &, or native int) to the address 
    /// of the destination object (type &, or native int).
    /// </summary>
    public class CPOBJ : InstructionExecBase
    {
        public CPOBJ(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // The two object references are popped from the stack; 
            var ref2 = cur.EvalStack.Pop();
            var ref1 = cur.EvalStack.Pop();

            // the value type at the address of the source object is copied to the address of the destination object.
            if (ref2 is MethodMemberPointer mmp2 && ref1 is MethodMemberPointer mmp1)
            {
                mmp1.Value = mmp2.Value;
                return nextRetval;
            }

            throw new NotImplementedException("CPOBJ " + ref2.GetType().FullName + " & " + ref1.GetType().FullName);
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }
}