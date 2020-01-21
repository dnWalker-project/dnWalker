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
    using MMC.Data;
    using MMC.Util;
    using MMC.InstructionExec;

    /// <summary>
    /// Singleton accessor class.
    /// </summary>
    static class ActiveState
    {
        /// <summary>
        /// A simple public field suffices here, no need to define a property.
        /// </summary>
        public static readonly ExplicitActiveState cur = new ExplicitActiveState(Config.Instance);
    }

    /// <summary>
    /// An implementation of the active state of the virtual machine.
    /// </summary>
    class ExplicitActiveState : IStorageVisitable, ICleanable
    {
        private IConfig _config;
        DynamicArea m_dyn;
        IStaticArea m_stat;
        ThreadPool m_tp;

        /// <summary>
        /// The allocation heap, i.e. the dynamic part of the state.
        /// </summary>
        public DynamicArea DynamicArea
        {
            get { return m_dyn; }
        }

        /// <summary>
        /// Loaded classes reside here, i.e. the static part.
        /// </summary>
        public IStaticArea StaticArea
        {
            get { return m_stat; }
        }

        /// <summary>
        /// Running sub-processes are kept in here.
        /// </summary>
        public ThreadPool ThreadPool
        {
            get { return m_tp; }
        }

        /// <summary>
        /// Currently running thread.
        /// </summary>
        public ThreadState CurrentThread
        {
            get { return m_tp.CurrentThread; }
        }

        /// <summary>
        /// Current location in the CIL image.
        /// </summary>
        public CILLocation CurrentLocation
        {
            get { return CurrentThread.CurrentLocation; }
        }

        /// <summary>
        /// ID of currently running thread.
        /// </summary>
        public int me
        {
            get { return m_tp.CurrentThreadId; }
        }

        /// <summary>
        /// Callstack of currently running thread.
        /// </summary>
        public CallStack CallStack
        {
            get { return m_tp.CurrentThread.CallStack; }
        }

        /// <summary>
        /// Top of callstack of currently running thread.
        /// </summary>
        public MethodState CurrentMethod
        {
            get { return CurrentThread.CurrentMethod; }
        }

        public IConfig Configuration => _config;

        /// <summary>
        /// Evaluation stack of top of callstack of currently running thread.
        /// </summary>
        public DataElementStack EvalStack
        {
            get
            {
                DataElementStack retval = null;
                if (CurrentMethod != null)
                    retval = CurrentMethod.EvalStack;
                return retval;
            }
        }

        /// <summary>
        /// Determine if we are "running" in the assembly to be checked.
        /// </summary>
        /// <remarks>This means the current thread is currently executing code that is contained in the main assembly.</remarks>
        /// <returns>True iff code from the main assembly is being executed.</returns>
        public bool IsAssemblyLocalState()
        {
            return CurrentMethod != null &&
                (CurrentMethod.Definition.DeclaringType.Module == //.Assembly ==
                 DefinitionProvider.dp.AssemblyDefinition);
        }

        /// <summary>
        /// Callback method for visitors.
        /// </summary>
        /// <param name="visitor">A visitor visiting this state.</param>
        public void Accept(IStorageVisitor visitor)
        {
            visitor.VisitActiveState(this);
        }

        /// <summary>
        /// Set all state-defining parts to be clean.
        /// </summary>
        public void Clean()
        {
            m_dyn.Clean();
            m_stat.Clean();
            m_tp.Clean();
        }

        /// <summary>
        /// Check if any of the four state-defining parts are dirty.
        /// </summary>
        public bool IsDirty()
        {
            return m_dyn.IsDirty() || m_stat.IsDirty() || m_tp.IsDirty();
        }

        public MemoryLocation NextAccess(int threadId)
        {
            var cur = ActiveState.cur;
            MethodState method = cur.ThreadPool.Threads[threadId].CurrentMethod;
            var instr = method.ProgramCounter;
            InstructionExecBase instrExec = InstructionExecProvider.iep.GetExecFor(instr);

            return instrExec.Accessed(threadId, cur);
        }

        /// <summary>
        /// Convert the active state to a somewhat large string representation.
        /// </summary>
        /// <remarks>
        /// Do not over-use this method. It is quite slow to print each and every
        /// explored state for example. For small examples this is fine, but for
        /// >100.000 states it turns out ~80% of the time it's building strings.
        /// </remarks>
        /// <returns>A string representation of the active state.</returns>
        public override string ToString()
        {
            return string.Format(@"------------------------- DYNAMIC AREA -------------------------
{0}------------------------- STATIC  AREA -------------------------
{1}------------------------- THREAD  POOL -------------------------
{2}",
                m_dyn.ToString(), m_stat.ToString(), m_tp.ToString());
        }

        public void Reset()
        {
            m_dyn = new DynamicArea(Configuration);
            m_stat = new StaticArea(Configuration);
            m_tp = new ThreadPool();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ExplicitActiveState(IConfig config)
        {
            _config = config;
            Reset();
        }
    }
}