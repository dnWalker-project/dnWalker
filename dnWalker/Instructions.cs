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
            //return cur.ThreadPool.GetThreadCount(MMC.ThreadStatus.Running) > 1;
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
            //return cur.ThreadPool.GetThreadCount(MMC.ThreadStatus.Running) == 1;
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();

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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement a = cur.EvalStack.Pop();
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
            Instruction[] targets = Operand as Instruction[];
            if (targets.Length == 0)
            {
                return nextRetval;
            }

            var index = (int)Convert.ChangeType(a, typeof(int));
            return index >= 0 && index < targets.Length ?
                new JumpReturnValue(targets[index]) : nextRetval;
        }
    }

    public class NOP : InstructionExecBase
    {
        public NOP(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            return nextRetval;
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
            IIntegerElement b = (IIntegerElement)cur.EvalStack.Pop();
            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
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

            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
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

            IIntegerElement b = (IIntegerElement)cur.EvalStack.Pop();
            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
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

            IIntegerElement b = (IIntegerElement)cur.EvalStack.Pop();
            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
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
            int shiftBy = (int)Convert.ChangeType(value, typeof(int));
            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Shl(shiftBy));
            return nextRetval;
        }
    }

    // Shift right
    public class SHR : LogicalIntsructionExec
    {
        public SHR(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            var value = cur.EvalStack.Pop();
            int shiftBy = (int)Convert.ChangeType(value, typeof(int));
            IIntegerElement a = (IIntegerElement)cur.EvalStack.Pop();
            cur.EvalStack.Push(a.Shr(shiftBy));
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
                argIndex = ((Int4)Operand).Value;
            else
            {
                argIndex = ((ParameterDefinition)Operand).ParamDef.Sequence;

                if (!cur.CurrentMethod.Definition.HasThis)
                    argIndex--;
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

            string[] tokens = Instruction.OpCode.Name.Split(new char[] { '.' });
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
            MethodDefinition method = Operand as MethodDefinition;
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
            ObjectReference or = (ObjectReference)cur.EvalStack.Pop();
            MethodDefinition method = Operand as MethodDefinition;

            MethodDefinition toCall = cur.DefinitionProvider.SearchVirtualMethod(method, or, cur);
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
            IDataElement reference = cur.EvalStack.Peek();
            return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {

            IDataElement reference = cur.EvalStack.Peek();

            if (reference is ObjectFieldPointer)
            {
                ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
                AllocatedObject theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

                return theObject.ThreadShared;
            }
            else if (reference is StaticFieldPointer)
            {
                return true;
            }
            else
                return false;
        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            IDataElement reference = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack.Peek();

            if (reference is ObjectFieldPointer)
            {
                ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
                return ofp.MemoryLocation;
            }
            else if (reference is StaticFieldPointer)
            {
                StaticFieldPointer sfp = (StaticFieldPointer)reference;
                return sfp.MemoryLocation;
            }

            return base.Accessed(threadId, cur);
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

		public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				return ofp.MemoryLocation;
			} else if (reference is StaticFieldPointer) {
				StaticFieldPointer sfp = (StaticFieldPointer)reference;
				return sfp.MemoryLocation;
			}

			return base.Accessed(threadId);
		}*/
    }

    public class LDLOC : LoadInstructionExec
    {
        public LDLOC(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

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
            int index = ((Local)Operand).Index;
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
        /// \brief Loads a public class into the static area.
        ///
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
        ///
        /// \param type The type of public class to load.
        /// \return True iff we can continue to execute the instruction (i.e.
        /// it's okay to access the static fields).
        public bool LoadClass(TypeDefinition type, ExplicitActiveState cur)
        {
            int me = cur.ThreadPool.CurrentThreadId;

            bool allow_access = true;

            AllocatedClass cls = cur.StaticArea.GetClass(type);
            if (!cls.Initialized)
            {
                cur.Logger.Debug("thread {0} wants access to uninitialized public class {1}", me, type.Name);
                MethodDefinition cctorDef = cur.DefinitionProvider.SearchMethod(".cctor", type);
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
                    MethodState cctorState = new MethodState(
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
                    ThreadPool tp = cur.ThreadPool;
                    int wait_for = cls.InitializingThread;
                    cur.Logger.Debug("thread {0} is currently initializing the public class", wait_for);

                    bool wait_safe = wait_for != me;
                    if (wait_safe)
                    {
                        // Make sure we're not introducing a deadlock by waiting here.
                        BitArray seen = new BitArray(tp.Threads.Length, false);
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
            TypeDefinition type = GetTypeDefinition();
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
                AllocatedClass cls = cur.StaticArea.GetClass(type);
                cls.Initialized = true;
                cls.InitializingThread = LockManager.NoThread;
                cls.AwakenWaitingThreads(cur);
            }
        }

        public FieldDefinition GetFieldDefinition()
        {
            FieldDefinition fld = Operand as FieldDefinition;
            // Lookup layout information if it's not available.
            if (!fld.HasLayoutInfo)
            {
                fld = DefinitionProvider.GetFieldDefinition(fld);
                //throw new NotImplementedException("GetFieldDefinition");
            }

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

            foreach (TypeDefinition typeDef in DefinitionProvider.InheritanceEnumerator(superType))
            {
                /*
				 * We start searching for the right field from the declaringtype,
				 * it is possible that the declaring type does not define fld, therefore
				 * it might be possible that we have to search further for fld in
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

                if (typeDef.BaseType != null && typeDef.BaseType.FullName != "System.Object") // if base type is System.Object, stop
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

        public NEWARR(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            Int4 length = (Int4)cur.EvalStack.Pop();

            if (length.Value < 0)
            {
                RaiseException("System.OverflowException", cur);
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


    public class CASTCLASS : ObjectModelInstructionExec
    {

        public CASTCLASS(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            ObjectReference objRef = (ObjectReference)cur.EvalStack.Pop();
            var toCastToType = (ITypeDefOrRef)Operand;

            /*
			 * TODO: make this work for arrays! See ECMA spec on this
			 */
            AllocatedObject ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;

            if (cur.DefinitionProvider.IsSubtype(ao.Type, toCastToType))
                cur.EvalStack.Push(objRef);
            else
            {
                RaiseException("System.InvalidCastException", cur);
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
            Int4 idx = (Int4)cur.EvalStack.Pop();
            ObjectReference objRef = (ObjectReference)cur.EvalStack.Pop();
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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            return base.Accessed(threadId, cur);
        }
    }

    public class LDELEM : ObjectModelInstructionExec
    {

        public LDELEM(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement ida = cur.EvalStack.Peek();
            Int4 idx = (Int4)cur.EvalStack.Pop();
            ObjectReference arrayRef = (ObjectReference)cur.EvalStack.Pop();

            IIEReturnValue retval = nextRetval;
            AllocatedArray theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;

            if (theArray == null)
                RaiseException("System.NullReferenceException", cur);
            else if (CheckBounds(theArray, idx))
                cur.EvalStack.Push(theArray.Fields[idx.Value]);
            else
                RaiseException("System.IndexOutOfRangeException", cur);

            return retval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;

            //return cur.ThreadPool.RunnableThreadCount == 1;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {

            int length = cur.EvalStack.Length;

            ObjectReference arrayRef = (ObjectReference)cur.EvalStack[length - 2];
            AllocatedArray theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;
            return theArray.ThreadShared;

        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            DataElementStack des = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack;
            int length = des.Length;

            Int4 idx = (Int4)des.Peek();
            ObjectReference arrayRef = (ObjectReference)des[length - 2];

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
            IDataElement val = cur.EvalStack.Pop();
            Int4 idx = (Int4)cur.EvalStack.Pop();
            ObjectReference arrayRef = (ObjectReference)cur.EvalStack.Pop();

            IIEReturnValue retval = nextRetval;
            AllocatedArray theArray =
                cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;

            if (theArray == null)
            {
                RaiseException("System.NullReferenceException", cur);
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
                RaiseException("System.IndexOutOfRangeException", cur);
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
            int length = cur.EvalStack.Length;

            ObjectReference arrayRef = (ObjectReference)cur.EvalStack[length - 3];
            AllocatedArray theArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;
            return theArray.ThreadShared;
        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            DataElementStack des = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack;
            int length = des.Length;

            Int4 idx = (Int4)des[length - 2];
            ObjectReference arrayRef = (ObjectReference)des[length - 3];

            return new MemoryLocation(idx.Value, arrayRef, cur);
        }
    }

    public class LDLEN : ObjectModelInstructionExec
    {

        public LDLEN(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            ObjectReference arrayRef = (ObjectReference)cur.EvalStack.Pop();
            AllocatedArray theArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];

            if (theArray == null)
            {
                RaiseException("System.NullReferenceException", cur);
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

    public class ISINST : ObjectModelInstructionExec
    {

        public ISINST(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement reference = cur.EvalStack.Pop();
            DynamicAllocation obj = (DynamicAllocation)cur.DynamicArea.Allocations[(ObjectReference)reference];

            var typeDef = Operand as ITypeDefOrRef;

            if (cur.DefinitionProvider.IsSubtype(obj.Type, typeDef))
                cur.EvalStack.Push(reference);
            else
                cur.EvalStack.Push(ObjectReference.Null);

            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return true;
        }
    }

    public class LDFLDA : ObjectModelInstructionExec
    {
        public LDFLDA(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            ObjectReference objRef = (ObjectReference)cur.EvalStack.Pop();
            AllocatedObject ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;
            int offset = GetFieldOffset(ao.Type);

            cur.EvalStack.Push(new ObjectFieldPointer(cur, objRef, offset));
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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            return base.Accessed(threadId, cur);
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
            IDataElement reference = cur.EvalStack.Pop();

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
                RaiseException("System.NullReferenceException", cur);
            }
            else
            {
                int offset = GetFieldOffset(theObject.Type);
                //			FieldDefinition fld = GetFieldDefinition();
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
            IDataElement reference = cur.EvalStack.Peek();

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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            IDataElement refVal = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack.Peek();
            ObjectReference or;

            if (refVal is ObjectReference)
                or = (ObjectReference)refVal;
            else
                or = (ObjectReference)((MethodMemberPointer)refVal).Value;

            AllocatedObject ao = cur.DynamicArea.Allocations[or] as AllocatedObject;
            int offset = GetFieldOffset(ao.Type);

            return new MemoryLocation(offset, or, cur);
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
            IDataElement val = cur.EvalStack.Pop();
            IDataElement toChange = cur.EvalStack.Pop();
            //FieldDefinition fld = GetFieldDefinition();

            // This is somewhat rancid, but for now we'll do it like this.
            if (toChange is ObjectReference objectReference)
            {
                // Change a field of an object.
                AllocatedObject theObject =
                    cur.DynamicArea.Allocations[objectReference] as AllocatedObject;

                int offset = GetFieldOffset(theObject.Type);
                cur.ParentWatcher.RemoveParentFromChild(objectReference, theObject.Fields[offset], cur.Configuration.MemoisedGC);

                /// Can be the case that an object reference was written, thereby changing the object graph
                ObjectEscapePOR.UpdateReachability(theObject.ThreadShared, theObject.Fields[offset], val, cur);

                theObject.Fields[offset] = val;
                cur.ParentWatcher.AddParentToChild(objectReference, val, cur.Configuration.MemoisedGC);
            }
            else if (toChange is LocalVariablePointer)
            {
                // Change by pointer to local variable.
                LocalVariablePointer lvp = (LocalVariablePointer)toChange;
                lvp.Value = val;
            }
            else
                cur.Logger.Warning("unknown type storage destination: {0}", toChange.GetType());

            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            return false;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {

            FieldDefinition fieldDef = GetFieldDefinition();
            if (fieldDef.IsInitOnly)
                return false;


            int length = cur.EvalStack.Length;
            IDataElement toChange = cur.EvalStack[length - 2];

            if (toChange is ObjectReference)
            {
                AllocatedObject theObject = cur.DynamicArea.Allocations[(ObjectReference)toChange] as AllocatedObject;
                return theObject.ThreadShared;
            }

            return false;
        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            DataElementStack des = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack;

            int length = des.Length;
            IDataElement toChange = des[length - 2];

            if (toChange is ObjectReference)
            {
                ObjectReference or = (ObjectReference)toChange;
                //FieldDefinition fld = GetFieldDefinition();
                AllocatedObject ao = cur.DynamicArea.Allocations[or] as AllocatedObject;
                int offset = GetFieldOffset(ao.Type);
                return new MemoryLocation(offset, or, cur);
            }

            return base.Accessed(threadId, cur);
        }
    }

    public class BOX : ObjectModelInstructionExec
    {
        public BOX(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is a type definition of the wrapper type to use.
            ObjectReference wrappedRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(),
                Operand as ITypeDefOrRef);
            AllocatedObject wrapped = (AllocatedObject)cur.DynamicArea.Allocations[wrappedRef];
            wrapped.Fields[wrapped.ValueFieldOffset] = cur.EvalStack.Pop();
            cur.EvalStack.Push(wrappedRef);
            return nextRetval;
        }
    }

    public class UNBOX : ObjectModelInstructionExec
    {
        public UNBOX(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Operand is a type definition of the wrapper type to use.
            ObjectReference wrappedRef = (ObjectReference)cur.EvalStack.Pop();
            AllocatedObject wrapped = (AllocatedObject)cur.DynamicArea.Allocations[wrappedRef];
            // Maybe a clone isn't even really needed here, the object pushed on the
            // stack will probably not live very long and the field will probably not
            // be updated while it's on the stack.
            cur.EvalStack.Push(wrapped.Fields[wrapped.ValueFieldOffset]);
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
            IIEReturnValue retval = nincRetval;
            FieldDefinition fld = GetFieldDefinition();
            var declType = GetTypeDefinition();

            if (LoadClass(declType, cur))
            {
                AllocatedClass ac = cur.StaticArea.GetClass(declType);
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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
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
            IIEReturnValue retval = nincRetval;
            FieldDefinition fld = GetFieldDefinition();
            TypeDefinition declType = GetTypeDefinition();

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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            return base.Accessed(threadId, cur);
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
            IIEReturnValue retval = nincRetval;
            FieldDefinition fld = GetFieldDefinition();
            TypeDefinition declType = GetTypeDefinition();

            if (LoadClass(declType, cur))
            {
                IDataElement val = cur.EvalStack.Pop();

                AllocatedClass ac = cur.StaticArea.GetClass(declType);
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

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            FieldDefinition fld = GetFieldDefinition();
            TypeDefinition declType = GetTypeDefinition();

            return new MemoryLocation((int)fld.FieldOffset, declType, cur);
        }
    }

    public class LDTOKEN : ObjectModelInstructionExec
    {
        public LDTOKEN(Instruction instr, object operand, InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IIEReturnValue retval = nincRetval;
            if (Operand is TypeDefinition)
            {
                TypeDefinition typeDef = Operand as TypeDefinition;
                if (LoadClass(typeDef, cur))
                {
                    retval = nextRetval;
                }
                cur.EvalStack.Push(new TypePointer(typeDef));
            }
            else
            {
                throw new NotSupportedException("LDTOKEN is currently unsupported");
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

        public ADD(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();

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
            catch (OverflowException)
            {
                RaiseException("System.OverflowException", cur);
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

        public DIV(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            INumericElement b = (INumericElement)cur.EvalStack.Pop();
            INumericElement a = (INumericElement)cur.EvalStack.Pop();

            if (Unsigned)
            {
                a = ((ISignedIntegerElement)a).ToUnsigned();
                b = ((ISignedIntegerElement)b).ToUnsigned();
            }

            try
            {
                /*if (b.Equals(b.Zero))
                {
                // TODO optimize!
                }*/

                cur.EvalStack.Push(a.Div(b));
            }
            catch (DivideByZeroException e)
            {
                RaiseException(e.GetType().FullName, cur);
            }
            catch (ArithmeticException e)
            {
                RaiseException(e.GetType().FullName, cur);
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
            INumericElement b = (INumericElement)cur.EvalStack.Pop();
            INumericElement a = (INumericElement)cur.EvalStack.Pop();

            if (Unsigned)
            {
                a = ((ISignedIntegerElement)a).ToUnsigned();
                b = ((ISignedIntegerElement)b).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(a.Mul(b, CheckOverflow));
            }
            catch (OverflowException)
            {
                RaiseException("System.OverflowException", cur);
            }

            return nextRetval;
        }
    }

    public class REM : NumericInstructionExec
    {

        public REM(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            INumericElement b = (INumericElement)cur.EvalStack.Pop();
            INumericElement a = (INumericElement)cur.EvalStack.Pop();

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
                RaiseException(e.GetType().FullName, cur);
            }
            catch (ArithmeticException e)
            {
                RaiseException(e.GetType().FullName, cur);
            }

            return nextRetval;
        }
    }

    public class SUB : NumericInstructionExec
    {

        public SUB(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement a = cur.EvalStack.Pop();
            IDataElement b = cur.EvalStack.Pop();

            ISubElement left = (ISubElement)b;
            INumericElement right = (INumericElement)a;

            if (Unsigned)
            {
                left = ((ISignedIntegerElement)left).ToUnsigned();
                right = ((ISignedIntegerElement)right).ToUnsigned();
            }

            try
            {
                cur.EvalStack.Push(left.Sub(right, CheckOverflow));
            }
            catch (OverflowException)
            {
                RaiseException("System.OverflowException", cur);
            }

            return nextRetval;
        }
    }

    public class NEG : NumericInstructionExec
    {

        public NEG(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            ISignedNumericElement a = (ISignedNumericElement)cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement b = cur.EvalStack.Pop();
            IDataElement a = cur.EvalStack.Pop();
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
            IDataElement exceptionRef = cur.EvalStack.Pop();
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
            Int4 val = (Int4)cur.EvalStack.Pop();
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
            MethodState current = cur.CurrentMethod;

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

        public ENDFINALLY(Instruction instr, object operand,
                InstructionExecAttributes atr)
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

    /// Common base public class for all instructions that call a method.
    public class CallInstructionExec : InstructionExecBase
    {

        public CallInstructionExec(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        protected MethodDefinition Method => Operand as MethodDefinition;

        /// Check if a method is empty, i.e. it has no code.
        ///
        /// \param meth The method.
        /// \return False iff the method has code.
        protected bool IsEmptyMethod(MethodDefinition meth)
        {

            return (meth.Body == null || meth.Body.Instructions.Count == 0);
        }

        /// Handle an empty method body.
        ///
        /// This checks if the empty body is a delegate invocation, or an
        /// internal call. Both are handled in this method.
        ///
        /// At the moment, many internal calls and asynchronous invocations of
        /// delegates are not handled.
        ///
        /// \paran args Arguments to the method to be called.
        /// \return True iff the method has been properly handled.
        /// \sa IntCallManager
        protected bool HandleEmptyMethod(ExplicitActiveState cur, DataElementList args)
        {
            bool handled = false;
            DynamicAllocation thisObject = null;
            if (args.Length > 0 && args[0] is ObjectReference)
                thisObject = cur.DynamicArea.Allocations[(ObjectReference)args[0]];
            MethodDefinition methDef = Method;

            // Determine the type of call and simulate the behaviour.
            if ((methDef.Attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PinvokeImpl)
            {
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
                DataElementList calleePars = cur.StorageFactory.CreateList(args.Length - 1);
                for (int i = 0; i < calleePars.Length; ++i)
                    calleePars[i] = args[i + 1];
                MethodDefinition toCall =
                    ((AllocatedDelegate)thisObject).Method.Value;
                // Create frame for the indirectly called method.
                MethodState calleeState = new MethodState(toCall, calleePars, cur);
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
        /// \brief Handle assertion violations.
        ///
        /// This handles calls System.Diagnostics.Debug.Assert(*).
        ///
        /// This is not a nice place to put this code. All code should be put
        /// in the ICM, which is not just an internal call manager any more,
        /// but this makes the approach much more generic. 
        /// The ICM should be adjusted to handle polymorphic calls for this to
        /// be possible.
        ///
        /// \param args Arguments to the Assert call.
        /// \return True iff the call was handled by this method.
        protected bool HandleAssertCall(DataElementList args, ExplicitActiveState cur, out bool violated)
        {
            MethodDefinition methDef = Operand as MethodDefinition;
            string name = methDef.Name;
            string decl = methDef.DeclaringType.Namespace + "." + methDef.DeclaringType.Name;
            bool assert = name == "Assert" && decl == "System.Diagnostics.Debug";
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
            MethodDefinition methDef = Operand as MethodDefinition;
            string name = methDef.Name;
            string decl = methDef.DeclaringType.Namespace + "." + methDef.DeclaringType.Name;
            bool filtered =
                (name == "WriteLine" && decl == "System.Console")
                || name == "StartupSetApartmentStateInternal";
            if (filtered)
            {
                cur.Logger.Log(LogPriority.Call, "{0}: method is filtered out, not executed", name);
            }
            return filtered;
        }

        /// \brief Create an argument list.
        ///
        /// This constructs a argument list of the correct size, and fills it
        /// with elements popped off the eval stack (in right to left order).
        ///
        /// \return A list containing the arguments.
        protected DataElementList CreateArgumentList(ExplicitActiveState cur)
        {
            MethodDefinition methDef = Operand as MethodDefinition;
            int size = methDef.ParamDefs.Count + (methDef.HasThis ? 1 : 0);
            DataElementList retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size >= 0; --size)
                retval[size] = cur.EvalStack.Pop();

            return retval;
        }

        protected DataElementList CopyArgumentList(ExplicitActiveState cur, int threadId)
        {
            MethodDefinition methDef = Operand as MethodDefinition;
            int size = methDef.Parameters.Count + (methDef.HasThis ? 1 : 0);
            DataElementList retval = cur.StorageFactory.CreateList(size);

            // Topmost stack element is last argument (this ptr is also on stack).
            for (--size; size >= 0; --size)
                retval[size] = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack.Pop();

            /*
			 * Restore the eval stack
			 */
            for (int i = 0; i < retval.Length; i++)
                cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack.Push(retval[i]);

            return retval;
        }


        protected void CheckTailCall()
        {

        }

        /// Determine if this call is safe.
        ///
        /// Usually calls are safe, but this may not be the case for some
        /// internal calls. In this case, return the value given by the ICM.
        ///
        /// \return True iff the call is safe.
        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            bool safe = true;
            MethodDefinition methDef = Operand as MethodDefinition;
            if ((methDef.ImplAttributes & MethodImplAttributes.InternalCall) != 0)
                safe = MMC.ICall.IntCallManager.icm.IsMultiThreadSafe(methDef.Name);

            return safe;
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            MethodDefinition methDef = Operand as MethodDefinition;
            if ((methDef.ImplAttributes & MethodImplAttributes.InternalCall) != 0)
            {
                DataElementList args = CopyArgumentList(cur, cur.ThreadPool.CurrentThreadId);
                return MMC.ICall.IntCallManager.icm.IsDependent(methDef, args);
            }
            else
                return false;
        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            MethodDefinition methDef = Operand as MethodDefinition;
            DataElementList args = CopyArgumentList(cur, threadId);
            return MMC.ICall.IntCallManager.icm.HandleICallAccessed(methDef, args, threadId);
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

            MethodState oldState = cur.CallStack.Pop();

            MethodDefinition methDef = Operand as MethodDefinition;
            MethodState newState = new MethodState(methDef, oldState.Arguments, cur);
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
            MethodDefinition methDef = Operand as MethodDefinition;

            // This pops the arguments of the stack. Even if we don't execute
            // the method. This is a good thing.
            DataElementList args = CreateArgumentList(cur);
            IIEReturnValue retval = nextRetval;

            // Skip certain calls.
            bool violated;
            if (!HandleAssertCall(args, cur, out violated) && !FilterCall(cur))
            {
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
                    var bypass = NativePeer.Get(methDef);
                    if (bypass != null)
                    {
                        if (bypass.TryGetValue(args, cur, out var dataElement))
                        {
                            cur.EvalStack.Push(dataElement);
                            return retval;
                        }
                    }

                    // Create new frame. Note we still return nextRetval, so
                    // the call instruction is not re-executed after returning.
                    //
                    // Before the scheduler cleanup we returned a jump target
                    // from this call (as a up-casted Instruction instance,
                    // yuck). This is BAD idea since it requires nasty hacks
                    // (to update the PC) to get right and is poorly readable.
                    MethodState called = new MethodState(methDef, args, cur);
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

            MethodPointer methPtr = (MethodPointer)cur.EvalStack.Pop();
            MethodDefinition methDef = methPtr.Value;

            DataElementList args = CreateArgumentList(cur);

            if (!FilterCall(cur))
            {
                // Assumption: CALLI targets have a body.
                MethodState called = new MethodState(methDef, args, cur);
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

        public CALLVIRT(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            // Virtual calls use the run-time type of an object to determine
            // the location of the method to be called. It always requires
            // a this pointer, but we use the CreateArgumentList.
            MethodDefinition methDef = Operand as MethodDefinition;
            DataElementList args = CreateArgumentList(cur);

            if (methDef.FullName == "System.Void System.Threading.Thread::Start()")
            {
                var threadId = cur.ThreadPool.FindOwningThread(args[0]);
                if (threadId == LockManager.NoThread)
                {
                    throw new NotSupportedException("Owning thread not found.");
                }
                cur.ThreadPool.Threads[threadId].State = (int)System.Threading.ThreadState.Running;
                return nextRetval;
            }

            // Skip certain calls.
            if (!FilterCall(cur))
            {
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

                MethodDefinition toCall = cur.DefinitionProvider.SearchVirtualMethod(methDef, (ObjectReference)args[0], cur);

                MethodState called = new MethodState(toCall, args, cur);
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

    /// A NEWOBJ instruction.
    public class NEWOBJ : CallInstructionExec
    {

        public NEWOBJ(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            // Basically the same as a normal call, except first we need
            // to create a new object, and pass it as argument 0 (the
            // rest of the arguments are on the stack, last on top)
            MethodDefinition methDef = Operand as MethodDefinition;

            // Note that this loop condition is "i > 0", not "i >= 0". This is because
            // we need to fill in args[0] (i.e. the this pointer) ourselves.
            DataElementList args = cur.StorageFactory.CreateList(methDef.ParamDefs.Count + 1);
            for (int i = args.Length - 1; i > 0; --i)
            {
                args[i] = cur.EvalStack.Pop();
            }

            if (Method.DeclaringType.FullName == "System.Threading.Thread" && Method.IsConstructor)
            {
                ThreadHandlers.Thread_internal(Method, args, cur);

                //MMC.ICall.IntCallManager.
                /*ObjectReference threadObjectRef = cur.DynamicArea.AllocateObject(
                    cur.DynamicArea.DeterminePlacement(false),
                    cur.DefinitionProvider.GetTypeDefinition("System.Threading.Thread"));*/

                //cur.ThreadPool.NewThread(null, threadObjectRef);
                //return threadObjectRef;
                //return cur.EvalStack.Pop();
                return nextRetval;
            }

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
                MethodState called = new MethodState(methDef, args, cur);
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

    /// A RET instruction.
    public class RET : InstructionExecBase
    {

        public RET(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        /// Execute the RET instruction.
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
            MethodState callee = cur.CallStack.Pop();
            if (cur.CallStack.StackPointer > 0 && callee.EvalStack.StackPointer > 0)
            {
                cur.EvalStack.Push(callee.EvalStack.Pop());
            }
            else
            {
                if (callee.Definition.ReturnType.IsValueType 
                    || callee.Definition.ReturnType == callee.Definition.ReturnType.Module.CorLibTypes.String)
                {
                    cur.CurrentThread.RetValue = callee.EvalStack.Top(); // TODO a little bit hacky
                }
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

        /// Return is always safe. 
        /// Locks are released in MethodState.Dispose().
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

        public CKFINITE(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IRealElement ida = (IRealElement)cur.EvalStack.Pop();

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
                RaiseException("System.OverflowException", cur);
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
            string[] tokens = Instruction.OpCode.Name.Split(new char[] { '.' });
            IDataElement toPush = null;
            IDataElement popped = cur.EvalStack.Pop();

            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                var unsigned = a.ToUnsignedInt4(false).Value;
                cur.EvalStack.Push(new Int8(checked(unsigned)));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                if (a is Int4 i4)
                {
                    var uintptr = (UIntPtr)a.ToUnsignedInt4(false).Value;
                    cur.EvalStack.Push(cur.DefinitionProvider.CreateDataElement(uintptr));
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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

            try
            {
                var unsigned = a.ToUnsignedInt4(false).Value;
                cur.EvalStack.Push(new UnsignedInt4(checked(unsigned)));
                return nextRetval;
            }
            catch (OverflowException e)
            {
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            IDataElement popped = cur.EvalStack.Pop();
            IDataElement toPush = null;
            INumericElement a = (popped is IManagedPointer) ? (popped as IManagedPointer).ToInt4() : (INumericElement)popped;

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
                RaiseException("System.OverflowException", cur);
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
            int index = ((ParameterDefinition)Operand).Index;
            if (!cur.CurrentMethod.Definition.HasThis)
                index--;

            IDataElement ide = cur.EvalStack.Pop();

            /*
			 * For heap analysis, TODO this should be done in a much cleaner fashion... */
            if (ide is ObjectReference)
            {
                IDataElement oldIde = cur.CurrentMethod.Arguments[index];
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

        public STLOC(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {

            // Operand is either a number 0-3, or a VariableDefinition, which
            // has a property Index, which is the number we want.
            int index;
            if (HasImplicitOperand)
                index = ((Int4)Operand).Value;
            else
                index = ((Local)Operand).Index;
            
            IDataElement ide = cur.EvalStack.Pop();

            /*
			 * For heap analysis, TODO this should be done in a much cleaner fashion... */
            if (ide is ObjectReference)
            {
                IDataElement oldIde = cur.CurrentMethod.Locals[index];
                ThreadObjectWatcher.Decrement(cur.ThreadPool.CurrentThreadId, (ObjectReference)oldIde, cur);
                ThreadObjectWatcher.Increment(cur.ThreadPool.CurrentThreadId, (ObjectReference)ide, cur);

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

		public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur) {
			IDataElement reference = cur.EvalStack.Peek();

			if (reference is ObjectFieldPointer) {
				ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
				return ofp.MemoryLocation;
			} else if (reference is StaticFieldPointer) {
				StaticFieldPointer sfp = (StaticFieldPointer)reference;
				return sfp.MemoryLocation;
			}

			return base.Accessed(threadId);
		}*/
    }

    public class STIND : StoreInstructionExec
    {

        public STIND(Instruction instr, object operand,
                InstructionExecAttributes atr)
            : base(instr, operand, atr)
        {
        }

        public override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IDataElement val = cur.EvalStack.Pop();
            IManagedPointer mmp = cur.EvalStack.Pop() as IManagedPointer;
            mmp.Value = val;
            return nextRetval;
        }

        public override bool IsMultiThreadSafe(ExplicitActiveState cur)
        {
            int length = cur.EvalStack.Length;
            IDataElement reference = cur.EvalStack[length - 2];
            return !(reference is ObjectFieldPointer || reference is StaticFieldPointer);
        }

        public override bool IsDependent(ExplicitActiveState cur)
        {
            int length = cur.EvalStack.Length;
            IDataElement reference = cur.EvalStack[length - 2];


            if (reference is ObjectFieldPointer)
            {
                ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
                AllocatedObject theObject = (AllocatedObject)cur.DynamicArea.Allocations[ofp.MemoryLocation.Location];

                return theObject.ThreadShared;
            }
            else if (reference is StaticFieldPointer)
            {
                return true;
            }
            else
                return false;
        }

        public override MemoryLocation Accessed(int threadId, ExplicitActiveState cur)
        {
            int length = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack.Length;
            IDataElement reference = cur.ThreadPool.Threads[threadId].CurrentMethod.EvalStack[length - 2];

            if (reference is ObjectFieldPointer)
            {
                ObjectFieldPointer ofp = (ObjectFieldPointer)reference;
                return ofp.MemoryLocation;
            }
            else if (reference is StaticFieldPointer)
            {
                StaticFieldPointer sfp = (StaticFieldPointer)reference;
                return sfp.MemoryLocation;
            }

            return base.Accessed(threadId, cur);
        }
    }

}