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

namespace MMC.State
{

    using System;
    using System.Linq;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;

    using dnWalker.TypeSystem;

    using MMC.Data;

    public delegate void MethodStateCallback(MethodState me);

    public class MethodState : IMustDispose, IStorageVisitable, IComparable, ICleanable
    {
        bool m_isDirty;
        /*
		 * If this method is the exception source, we know that when one returns/jumps
		 * back to this method because of exception object constructors, one has to
		 * find the suitable exception handler
		 */
        bool m_isExceptionSource;
        private readonly ExplicitActiveState cur;
        Instruction m_pc;
        //	Instruction m_finallyTarget;
        public ExplicitActiveState Cur => cur;

        public MethodState DeepCopy()
        {
            var copiedArgs = Arguments.StorageCopy() as DataElementList;
            var copiedLocals = Locals.StorageCopy() as DataElementList;
            var copiedStack = EvalStack.StorageCopy() as DataElementStack;

            var copy = new MethodState(Definition, copiedArgs, copiedLocals, copiedStack, cur);
            copy.OnDispose = this.OnDispose.Clone() as MethodStateCallback;
            return copy;
        }

        public MethodStateCallback OnDispose { get; set; }

        public DataElementList Arguments { get; set; }

        internal Instruction GetNextInstruction()
        {
            return Definition.Body.Instructions.FirstOrDefault(i => i.Offset > m_pc.Offset);
        }

        public DataElementList Locals { get; set; }

        public DataElementStack EvalStack { get; set; }

        public MethodDef Definition { get; set; }

        public Instruction ProgramCounter
        {

            get { return m_pc; }
            set
            {
                m_isDirty |= m_pc != value;
                m_pc = value;
            }
        }

        public bool IsExceptionSource
        {
            get { return m_isExceptionSource; }
            set
            {
                m_isExceptionSource = value;
                m_isDirty = true;
            }
        }

        public int PCOffset
        {
            get { return m_pc != null ? (int)m_pc.Offset : -1; }
        }

        public bool IsPrefixed { get; set; }

        public ITypeDefOrRef Constrained { get; set; }

        public ExceptionHandler NextFilterOrCatchHandler(Instruction instr, ITypeDefOrRef exceptionType)
        {
            ExceptionHandler retval = null;
            foreach (var eh in Definition.Body.ExceptionHandlers)
            {
                if ((eh.HandlerType == ExceptionHandlerType.Filter ||
                        (eh.HandlerType == ExceptionHandlerType.Catch && cur.DefinitionProvider.IsSubtype(exceptionType, eh.CatchType)))
                            && eh.TryStart.Offset <= instr.Offset && instr.Offset < eh.TryEnd.Offset)
                {
                    // First to encounter, or this EH has a smaller scope than
                    // the previously found one.
                    if (retval == null || retval.TryStart.Offset < eh.TryStart.Offset || retval.TryEnd.Offset > eh.TryEnd.Offset)
                    { 
                        retval = eh;
                    }
                }
            }

            return retval;
        }

        public ExceptionHandler NextFinallyOrFaultHandler(Instruction instr)
        {

            ExceptionHandler retval = null;
            foreach (var eh in Definition.Body.ExceptionHandlers)
            {
                if ((eh.HandlerType == ExceptionHandlerType.Finally || eh.HandlerType == ExceptionHandlerType.Fault)
                            && eh.TryStart.Offset <= instr.Offset && instr.Offset < eh.TryEnd.Offset)
                {
                    // First to encounter, or this EH has a smaller scope than
                    // the previously found one.
                    if (retval == null || retval.TryStart.Offset < eh.TryStart.Offset || retval.TryEnd.Offset > eh.TryEnd.Offset)
                        retval = eh;
                }
            }

            return retval;
        }

        public ExceptionHandler NextFinallyHandler(Instruction instr)
        {

            ExceptionHandler retval = null;
            foreach (var eh in Definition.Body.ExceptionHandlers)
            {
                if (eh.HandlerType == ExceptionHandlerType.Finally
                        && eh.TryStart.Offset <= instr.Offset && instr.Offset < eh.TryEnd.Offset)
                {
                    // First to encounter, or this EH has a smaller scope than
                    // the previously found one.
                    if (retval == null || retval.TryStart.Offset < eh.TryStart.Offset || retval.TryEnd.Offset > eh.TryEnd.Offset)
                        retval = eh;
                }
            }

            return retval;
        }

        public bool IsDirty()
        {

            return m_isDirty ||
                EvalStack.IsDirty() ||
                Locals.IsDirty() ||
                Arguments.IsDirty();
        }

        public void Clean()
        {

            m_isDirty = false;
            EvalStack.Clean();
            Locals.Clean();
            Arguments.Clean();
        }

        public void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
            visitor.VisitMethodState(this);
        }

        public void Dispose()
        {
            EvalStack.Dispose();
            Arguments.Dispose();
            Locals.Dispose();
            if (OnDispose != null)
                OnDispose(this);
        }

        public override string ToString()
        {

            return string.Format("{0}::{1}, pc={2:D4}, stack={3}, locals={4}, args={5}",
                    Definition.DeclaringType.Name,
                    Definition.Name,
                    (m_pc != null ? (int)m_pc.Offset : -1),
                    EvalStack.ToString(),
                    Locals.ToString(),
                    Arguments.ToString());
        }


        public override int GetHashCode()
        {

            var retval = Definition.GetHashCode();
            retval ^= HashMasks.MASK2;
            retval += Locals.GetHashCode();
            retval ^= HashMasks.MASK3;
            retval += Arguments.GetHashCode();
            retval ^= HashMasks.MASK4;
            retval += EvalStack.GetHashCode();
            retval ^= HashMasks.MASK5;
            retval += PCOffset;
            retval ^= HashMasks.MASK6;
            retval += (OnDispose == null ? -1 : 1);
            retval ^= HashMasks.MASK7;
            retval += (m_isExceptionSource ? -1 : 1);
            return retval;

        }

        public static int Compare(object a, object b)
        {

            var msa = a as MethodState;
            var msb = b as MethodState;

            var retval = msa.PCOffset - msb.PCOffset;

            if (retval == 0)
                retval = (msa.m_isExceptionSource == msb.m_isExceptionSource) ? 0 : 1;

            if (retval == 0)
                retval = msa.Definition.GetHashCode() - msb.Definition.GetHashCode();

            if (retval == 0)
                retval = msa.EvalStack.StackPointer - msb.EvalStack.StackPointer;

            for (var i = 0; retval == 0 && i < msa.Arguments.Length; ++i)
                retval = msa.Arguments[i].GetHashCode() - msb.Arguments[i].GetHashCode();

            for (var i = 0; retval == 0 && i < msa.Locals.Length; ++i)
                retval = msa.Locals[i].GetHashCode() - msb.Locals[i].GetHashCode();

            return retval;
        }

        public int CompareTo(object other)
        {

            return Compare(this, other);
        }

        public override bool Equals(object other)
        {

            return Compare(this, other) == 0;
        }

        internal void InitStructures()
        {
            if (EvalStack == null)
            {
                EvalStack = cur.StorageFactory.CreateStack(Definition.Body.MaxStack);
            }

            if (Locals == null)
            {
                Locals = cur.StorageFactory.CreateList(Definition.Body.Variables.Count);
                for (var i = 0; i < Locals.Length; ++i)
                {
                    Locals[i] = DataElement.GetNullValue(Definition.Body.Variables[i].Type);
                }
            }

            if (Arguments == null)
            {
                ParameterList parameters = Definition.Parameters;
                int cnt = parameters.Count;
                DataElementList args = cur.StorageFactory.CreateList(cnt);
                for (int i = 0; i < cnt; ++i)
                {
                    args[i] = DataElement.GetNullValue(parameters[i].Type);
                }

                Arguments = args;

                //throw new NotImplementedException("XX");/*
                //m_inArguments = cur.StorageFactory.CreateList(m_methodDefinition.Par amDefs.Count);
                //for (int i = 0; i < m_inArguments.Length; ++i)
                //    m_inArguments[i] = cur.DefinitionProvider.GetParameterNullOrDefaultValue(m_methodDefinition.Parame ters[i].ParamDef);*/
            }
        }

        internal MethodState(MethodDef meth, DataElementList pars, DataElementList locals, DataElementStack evalstack, ExplicitActiveState cur)
        {
            this.cur = cur;

            m_isDirty = true;
            Definition = meth;
            Locals = locals;
            Arguments = pars;
            EvalStack = evalstack;

            m_pc = meth.Body.Instructions[0]; // safe; body always contains 'ret'.
            m_isExceptionSource = false;

            ThreadObjectWatcher.IncrementAll(cur.ThreadPool.CurrentThreadId, pars, cur);
        }

        public MethodState(MethodDef meth, ExplicitActiveState cur)
            : this(meth, null, null, null, cur)
        {
            InitStructures();
        }

        public MethodState(MethodDef meth, DataElementList pars, ExplicitActiveState cur)
            : this(meth, pars, null, null, cur)
        {
            InitStructures();
        }
    }
}