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

using dnWalker.ChoiceGenerators;

namespace MMC.State
{
    using MMC.Data;
    using MMC.Util;
    using MMC.InstructionExec;
    using MMC.Collections;
    using System.Collections.Generic;
    using dnWalker;
    using System;

    public delegate void ChoiceGeneratorCreated(IChoiceGenerator choiceGenerator);

    /// <summary>
    /// An implementation of the active state of the virtual machine.
    /// </summary>
    public class ExplicitActiveState : IStorageVisitable, ICleanable
    {
        DynamicArea m_dyn;
        IStaticArea m_stat;
        ThreadPool m_tp;
        IGarbageCollector m_gc;
        private readonly Collapser m_stateConvertor;

        public event ChoiceGeneratorCreated ChoiceGeneratorCreated;

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

        public IConfig Configuration { get; }

        internal IInstructionExecProvider InstructionExecProvider { get; }

        public IGarbageCollector GarbageCollector => m_gc;

        public IStorageFactory StorageFactory { get; }

        internal ParentWatcher ParentWatcher { get; }

        internal ThreadObjectWatcher ThreadObjectWatcher { get; }

        public DefinitionProvider DefinitionProvider { get; }

        internal Collapser StateCollapser => m_stateConvertor;

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

        public Logger Logger { get; internal set; }
        
        internal StateStorage StateStorage { get; set; }

        /// <summary>
        /// Determine if we are "running" in the assembly to be checked.
        /// </summary>
        /// <remarks>This means the current thread is currently executing code that is contained in the main assembly.</remarks>
        /// <returns>True iff code from the main assembly is being executed.</returns>
        public bool IsAssemblyLocalState()
        {
            return CurrentMethod != null &&
                (CurrentMethod.Definition.DeclaringType.Module == //.Assembly ==
                 CurrentMethod.Cur.DefinitionProvider.AssemblyDefinition);
        }

        public delegate void DoSharingAnalysisRequestHandle();

        public event DoSharingAnalysisRequestHandle DoSharingAnalysisRequest;

        public void RequestSharingAnalysis()
        {
            DoSharingAnalysisRequest?.Invoke();
        }

        /// <summary>
        /// Callback method for visitors.
        /// </summary>
        /// <param name="visitor">A visitor visiting this state.</param>
        public void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
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
            var thread = ThreadPool.Threads[threadId];
            /*var cur = this;
            MethodState method = cur.ThreadPool.Threads[threadId].CurrentMethod;
            var instr = method.ProgramCounter;
            InstructionExecBase instrExec = InstructionExecProvider.GetExecFor(instr);

            return instrExec.Accessed(threadId, cur);*/
            return NextAccess(thread);
        }

        public MemoryLocation NextAccess(ThreadState thread)
        {
            var instr = thread.CurrentMethod.ProgramCounter;
            var instrExec = InstructionExecProvider.GetExecFor(instr);

            return instrExec.Accessed(thread, this);
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
            return $@"------------------------- DYNAMIC AREA -------------------------
{m_dyn}------------------------- STATIC  AREA -------------------------
{m_stat}------------------------- THREAD  POOL -------------------------
{m_tp}";
        }

        public void Reset()
        {
            m_dyn = new DynamicArea(this);
            m_stat = new StaticArea(this);
            m_tp = new ThreadPool(Logger);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal ExplicitActiveState(
            IConfig config,
            IInstructionExecProvider instructionExecProvider,
            DefinitionProvider definitionProvider,
            Logger logger)
        {
            DefinitionProvider = definitionProvider;
            Logger = logger;
            Configuration = config;
            InstructionExecProvider = instructionExecProvider;
            Reset();

            m_stateConvertor = new Collapser(this);

            /*
			 * Pick out the garbage collector. Note that only memoised GC and
			 * Mark & sweep are available. Reference counting is broken. */
            m_gc = config.MemoisedGC ?
                new IncrementalHeapVisitor(this) as IGarbageCollector :
                new MarkAndSweepGC() as IGarbageCollector;

            StorageFactory = new StorageFactory(this);
            ParentWatcher = new ParentWatcher(config, DefinitionProvider);
            ThreadObjectWatcher = new ThreadObjectWatcher();
        }

        private readonly Queue<int> m_emptyQueue = new Queue<int>(0);
        private IChoiceGenerator _choiceGenerator, _nextChoiceGenerator;

        public IChoiceGenerator ChoiceGenerator => _choiceGenerator;

        internal SchedulingData Collapse()
        {
            return Collapse(StateCollapser, StateStorage);
        }

        internal SchedulingData Collapse(Collapser stateCollapser, StateStorage m_stateStorage)
        {
            var cur = this;
            var collapsedCurrent = stateCollapser.CollapseCurrentState(this);

            var sd = new SchedulingData
            {
                Delta = collapsedCurrent.GetDelta()
            };

            int id = m_stateStorage.Count + 1;
            bool seenState = m_stateStorage.FindOrAdd(ref collapsedCurrent, ref id);

            /*
			 * Note: the collapsedCurrent stored in the hashtable can be
			 * different from the current collapsedCurrent, although they
			 * represent the same collapsedState. This is due to the 
			 * changingintvector, which also contains information about
			 * the reversed delta, which may be different for two 
			 * representative collapsed states */
            sd.ID = id;
            sd.State = collapsedCurrent;

            if (seenState)
            {
                // state is not new
                sd.Working = m_emptyQueue;
                m_stateStorage.StateRevisited(collapsedCurrent, sd, cur);
            }
            else
            {
                // state is new
                collapsedCurrent.ClearDelta(); // delta's do not need to be stored				
                sd.Enabled = cur.ThreadPool.RunnableThreadIds;

                if (sd.Enabled.Count > 0)
                {
                    sd.Working = sd.Enabled; //sd = sd.SetWorking(sd.Enabled);
                    sd.Done = new Queue<int>();
                }
                else
                {
                    //sd = sd.ClearWorking();
                    sd.Working = m_emptyQueue;
                    sd.Done = m_emptyQueue;
                }

                m_stateStorage.StateConstructed(collapsedCurrent, sd, cur);
            }

            return sd;
        }

        public bool TryTerminateThread(ThreadState thread)
        {
            if (_nextChoiceGenerator != null
                || (ChoiceGenerator?.Previous != null)
                || (ChoiceGenerator?.HasMoreChoices == true))
            {
                return false;
            }

            ThreadPool.TerminateThread(thread);
            return true;
        }

        public void SetNextChoiceGenerator(IChoiceGenerator choiceGenerator)
        {
            choiceGenerator.SetContext(this);

            ChoiceGeneratorCreated?.Invoke(choiceGenerator);

            _nextChoiceGenerator = choiceGenerator;
        }

        public bool Break()
        {
            return _nextChoiceGenerator != null;
            // return ((nextCg != null) || isIgnored);
            //throw new System.NotImplementedException();
        }

        public void Next()
        {
            if (_nextChoiceGenerator == null)
            {
                return;
            }

            _choiceGenerator = _nextChoiceGenerator;
            _nextChoiceGenerator = null;
        }
    }
}