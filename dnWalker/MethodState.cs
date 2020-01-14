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
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using dnlib.DotNet;
    using dnlib.DotNet.Emit;
    using MMC.Data;
    using MMC.Util;

    delegate void MethodStateCallback(MethodState me);


    class MethodState : IMustDispose, IStorageVisitable, IComparable, ICleanable
    {

        MethodDef m_methodDefinition;
        DataElementList m_locals;
        DataElementList m_inArguments;
        DataElementStack m_evalStack;
        MethodStateCallback m_onDipose;
        bool m_isDirty;
        /*
		 * If this method is the exception source, we know that when one returns/jumps
		 * back to this method because of exception object constructors, one has to
		 * find the suitable exception handler
		 */
        bool m_isExceptionSource;

        Instruction m_pc;
        //	Instruction m_finallyTarget;


        public MethodState DeepCopy()
        {
            DataElementList copiedArgs = m_inArguments.StorageCopy() as DataElementList;
            DataElementList copiedLocals = m_locals.StorageCopy() as DataElementList;
            DataElementStack copiedStack = m_evalStack.StorageCopy() as DataElementStack;

            MethodState copy = new MethodState(m_methodDefinition, copiedArgs, copiedLocals, copiedStack);
            copy.m_onDipose = this.m_onDipose.Clone() as MethodStateCallback;
            return copy;
        }


        public MethodStateCallback OnDispose
        {

            get { return m_onDipose; }
            set { m_onDipose = value; }
        }

        public DataElementList Arguments
        {

            get { return m_inArguments; }
            set { m_inArguments = value; }
        }

        internal Instruction GetNextInstruction()
        {
            return m_methodDefinition.Body.Instructions.FirstOrDefault(i => i.Offset > m_pc.Offset);
        }

        public DataElementList Locals
        {

            get { return m_locals; }
            set { m_locals = value; }
        }

        public DataElementStack EvalStack
        {

            get { return m_evalStack; }
            set { m_evalStack = value; }
        }

        public MethodDef Definition
        {

            get { return m_methodDefinition; }
            set { m_methodDefinition = value; }
        }

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

            get { return (m_pc != null ? (int)m_pc.Offset : -1); }
        }

        public ExceptionHandler NextFilterOrCatchHandler(Instruction instr, ITypeDefOrRef exceptionType)
        {

            ExceptionHandler retval = null;
            foreach (ExceptionHandler eh in m_methodDefinition.Body.ExceptionHandlers)
            {
                if ((eh.HandlerType == ExceptionHandlerType.Filter ||
                        (eh.HandlerType == ExceptionHandlerType.Catch && DefinitionProvider.dp.IsSubtype(exceptionType, eh.CatchType)))
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

        public ExceptionHandler NextFinallyOrFaultHandler(Instruction instr)
        {

            ExceptionHandler retval = null;
            foreach (ExceptionHandler eh in m_methodDefinition.Body.ExceptionHandlers)
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
            foreach (ExceptionHandler eh in m_methodDefinition.Body.ExceptionHandlers)
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
                m_evalStack.IsDirty() ||
                m_locals.IsDirty() ||
                m_inArguments.IsDirty();
        }

        public void Clean()
        {

            m_isDirty = false;
            m_evalStack.Clean();
            m_locals.Clean();
            m_inArguments.Clean();
        }

        public void Accept(IStorageVisitor visitor)
        {

            visitor.VisitMethodState(this);
        }

        public void Dispose()
        {

            m_evalStack.Dispose();
            m_inArguments.Dispose();
            m_locals.Dispose();
            if (m_onDipose != null)
                m_onDipose(this);
        }

        public override string ToString()
        {

            return string.Format("{0}::{1}, pc={2:D4}, stack={3}, locals={4}, args={5}",
                    m_methodDefinition.DeclaringType.Name,
                    m_methodDefinition.Name,
                    (m_pc != null ? (int)m_pc.Offset : -1),
                    m_evalStack.ToString(),
                    m_locals.ToString(),
                    m_inArguments.ToString());
        }


        public override int GetHashCode()
        {

            int retval = m_methodDefinition.GetHashCode();
            retval ^= HashMasks.MASK2;
            retval += m_locals.GetHashCode();
            retval ^= HashMasks.MASK3;
            retval += m_inArguments.GetHashCode();
            retval ^= HashMasks.MASK4;
            retval += m_evalStack.GetHashCode();
            retval ^= HashMasks.MASK5;
            retval += PCOffset;
            retval ^= HashMasks.MASK6;
            retval += (m_onDipose == null ? -1 : 1);
            retval ^= HashMasks.MASK7;
            retval += (m_isExceptionSource ? -1 : 1);
            return retval;

        }

        public static int Compare(object a, object b)
        {

            MethodState msa = a as MethodState;
            MethodState msb = b as MethodState;

            int retval = msa.PCOffset - msb.PCOffset;

            if (retval == 0)
                retval = (msa.m_isExceptionSource == msb.m_isExceptionSource) ? 0 : 1;

            if (retval == 0)
                retval = msa.Definition.GetHashCode() - msb.Definition.GetHashCode();

            if (retval == 0)
                retval = msa.EvalStack.StackPointer - msb.EvalStack.StackPointer;

            for (int i = 0; retval == 0 && i < msa.Arguments.Length; ++i)
                retval = msa.Arguments[i].GetHashCode() - msb.Arguments[i].GetHashCode();

            for (int i = 0; retval == 0 && i < msa.Locals.Length; ++i)
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
            if (m_evalStack == null)
                m_evalStack = StorageFactory.sf.CreateStack(m_methodDefinition.Body.MaxStack);

            if (m_locals == null)
            {
                m_locals = StorageFactory.sf.CreateList(m_methodDefinition.Body.Variables.Count);
                for (int i = 0; i < m_locals.Length; ++i)
                    m_locals[i] = DefinitionProvider.dp.GetNullValue(
                            m_methodDefinition.Body.Variables[i].Type);
            }

            if (m_inArguments == null)
            {
                throw new NotImplementedException("XX");/*
                m_inArguments = StorageFactory.sf.CreateList(m_methodDefinition.Par amDefs.Count);
                for (int i = 0; i < m_inArguments.Length; ++i)
                    m_inArguments[i] = DefinitionProvider.dp.GetParameterNullOrDefaultValue(m_methodDefinition.Parame ters[i].ParamDef);*/
            }
        }

        internal MethodState(MethodDef meth, DataElementList pars, DataElementList locals, DataElementStack evalstack)
        {
            m_isDirty = true;
            m_methodDefinition = meth;
            m_locals = locals;
            m_inArguments = pars;
            m_evalStack = evalstack;

            m_pc = meth.Body.Instructions[0]; // safe; body always contains 'ret'.
            m_isExceptionSource = false;

            ThreadObjectWatcher.IncrementAll(ActiveState.cur.ThreadPool.CurrentThreadId, pars);
        }

        public MethodState(MethodDef meth, DataElementList pars)
            : this(meth, pars, null, null)
        {
            InitStructures();
        }
    }
}