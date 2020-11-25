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

using System;

namespace MMC
{
	using System.Text;
	using System.Collections.Generic;
	using System.Timers;
	using MMC.State;
	using MMC.Util;
	using MMC.InstructionExec;
    using dnlib.DotNet.Emit;
    using dnWalker.ChoiceGenerators;
    using dnWalker;
    using dnWalker.Traversal;

    /// <summary>
    /// Handler for events that indicate the exploration of a state.
    /// </summary>
    public delegate void StateEventHandler(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur);

    /// <summary>
    /// Handler for deadlock events.
    /// </summary>
    public delegate void DeadlockEventHandler(SchedulingData sd);

    /// <summary>
    /// Handler for backtracking events.
    /// </summary>
    public delegate void BacktrackEventHandler(Stack<SchedulingData> stack, SchedulingData fromSD, ExplicitActiveState cur);

    /// <summary>
    /// Handler for scheduling events, at the moment only a chosen thread.
    /// </summary>
    public delegate void PickThreadEventHandle(SchedulingData sd, int chosen);

    public delegate void InstructionExecuted(CILLocation cilLocation);

    /// <summary>
    /// Handler for the event that the exploration stops.
    /// </summary>
    public delegate void ExplorationHaltEventHandle(IIEReturnValue ier);

	public class Explorer
    {
		public bool DoSharingAnalysis { get; set; }

		public bool m_continue = false;
		public bool m_measureMemory = true;

		/// <summary>
		/// A new state was found (or constructed) while exploring.
		/// </summary>
		protected event StateEventHandler StateConstructed;

		/// <summary>
		/// A state has been re-visited.
		/// </summary>
		protected event StateEventHandler StateRevisited;

		/// <summary>
		/// Exploration backtracked.
		/// </summary>
		protected event BacktrackEventHandler Backtracked;

		/// <summary>
		/// Exploration backtracked start
		/// </summary>
		protected event BacktrackEventHandler BacktrackStart;

		/// <summary>
		/// Exploration backtracked stop
		/// </summary>
		protected event BacktrackEventHandler BacktrackStop;

		/// <summary>
		/// A deadlock occured.
		/// </summary>
		protected event DeadlockEventHandler Deadlocked;

		/// <summary>
		/// A thread was scheduled.
		/// </summary>
		protected event PickThreadEventHandle ThreadPicked;

		/// <summary>
		/// Exploration was halted (e.g. because of an unhandled exception).
		/// </summary>
		protected event ExplorationHaltEventHandle ExplorationHalted;

        public event InstructionExecuted InstructionExecuted;

        private readonly StateStorage m_stateStorage;
        private readonly StatefulDynamicPOR m_dpor;
		private readonly ObjectEscapePOR m_spor;
        private readonly Timer m_explorationTimer;
        private readonly Timer m_memoryTimer;
        private readonly IInstructionExecProvider _instructionExecProvider;
        private readonly LinkedList<CollapsedState> m_atomicStates;
        private ExplorationLogger _explorationLogger;
        private IChoiceStrategy _strategy;
        private readonly PathStore _pathStore;

        public Explorer(ExplicitActiveState cur, IStatistics statistics, Logger Logger, IConfig config, PathStore pathStore = null)
        {
            Statistics = statistics;
            this.Logger = Logger;
            this.cur = cur;

            Config = config;
            _instructionExecProvider = cur.InstructionExecProvider;            
            
            cur.DoSharingAnalysisRequest += () => DoSharingAnalysis = true;

            var size = Math.Max(20, config.StateStorageSize);
            size = Math.Min(5, size);

            _explorationLogger = new ExplorationLogger(Statistics, this);
            var el = _explorationLogger;

            _pathStore = pathStore ?? new PathStore();

            /*
			 * Logging
			 */
            StateConstructed += new StateEventHandler(el.LogNewState);
            StateRevisited += new StateEventHandler(el.LogRevisitState);
            Backtracked += new BacktrackEventHandler(el.LogBacktrack);
            BacktrackEventHandler dummy = new BacktrackEventHandler(el.LogBacktrackDummy);
            BacktrackStart += dummy;
            BacktrackStop += dummy;
            Deadlocked += new DeadlockEventHandler(el.LogDeadlock);
            ThreadPicked += new PickThreadEventHandle(el.LogPickedThread);
            ExplorationHalted += new ExplorationHaltEventHandle(el.ExplorationHalted);
            //cur.ChoiceGeneratorCreated += OnChoiceGeneratorCreated;

            if (DotWriter.IsEnabled())
            {
                StateConstructed += new StateEventHandler(el.GraphNewState);
                StateRevisited += new StateEventHandler(el.GraphRevisitState);
                Backtracked += new BacktrackEventHandler(el.GraphBacktrack);
            }

            if (config.UseStatefulDynamicPOR)
            {
                m_dpor = new StatefulDynamicPOR(config);
                Backtracked += new BacktrackEventHandler(m_dpor.Backtracked);
                StateConstructed += new StateEventHandler(m_dpor.OnNewState);
                StateRevisited += new StateEventHandler(m_dpor.OnSeenState);
                ThreadPicked += new PickThreadEventHandle(m_dpor.ThreadPicked);
            }

            if (config.UseObjectEscapePOR)
            {
                m_spor = new ObjectEscapePOR();
                BacktrackStart += new BacktrackEventHandler(m_spor.CheckStoreThreadSharingData);
                StateConstructed += new StateEventHandler(m_spor.StoreThreadSharingData);
                BacktrackStop += new BacktrackEventHandler(m_spor.RestoreThreadSharingData);
            }
            
            // execution path tracking
            BacktrackStart += new BacktrackEventHandler(_pathStore.BacktrackStart);
            StateConstructed += new StateEventHandler(_pathStore.StateConstructed);
            BacktrackStop += new BacktrackEventHandler(_pathStore.BacktrackStop);
            cur.ThreadPool.OnNewThreadSpawned += _pathStore.NewThreadSpawned;
            cur.ThreadPool.OnNewThreadSpawned += threadState => threadState.InstructionExecuted += InstructionExecuted;

            // TODO CurrentThread was created in advance, explicit event registration/firing
            _pathStore.NewThreadSpawned(cur.CurrentThread);            

            if (!double.IsInfinity(config.MaxExploreInMinutes))
            {
                m_explorationTimer = new Timer();
                m_explorationTimer.Elapsed += OnTimedEvent;
                m_explorationTimer.Interval = config.MaxExploreInMinutes * 60 * 1000;
                m_explorationTimer.Enabled = true;
            }

            if (!double.IsInfinity(config.OptimizeStorageAtMegabyte))
            {
                m_atomicStates = new LinkedList<CollapsedState>();
                StateConstructed += new StateEventHandler(CheckAtomicOnNewState);
                Backtracked += new BacktrackEventHandler(CheckAtomicOnBacktrack);
            }

            if (config.OneTraceAndStop)
            {
                Backtracked += new BacktrackEventHandler(OnBacktrackAndStop);
            }

            m_memoryTimer = new Timer();
            m_memoryTimer.Elapsed += OnTimedMemoryEvent;
            m_memoryTimer.Interval = 5 * 1000;
            m_memoryTimer.Enabled = true;

            // hashtable
            m_stateStorage = new StateStorage(size)
            {
                StateConstructed = StateConstructed,
                StateRevisited = StateRevisited
            };

            var schedulingChoiceGenerator = new SchedulingChoiceGenerator(
                this,
                Statistics,
                m_stateStorage,
                m_spor,
                BacktrackStart,
                Backtracked,
                BacktrackStop,
                ThreadPicked);

            _strategy = new DefaultChoiceStrategy(
                cur,
                BacktrackStart,
                Backtracked,
                BacktrackStop);

            _strategy.RegisterChoiceGenerator(schedulingChoiceGenerator);

            cur.ChoiceStrategy = _strategy;

            cur.CurrentThread.State = (int)System.Threading.ThreadState.Running;
        }

        public IConfig Config { get; }

        public Logger Logger { get; }

        public PathStore PathStore => _pathStore;

        private IStatistics Statistics { get; }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Logger.Notice("Ran out of time");
            m_continue = false;
        }

		private void OnTimedMemoryEvent(object source, ElapsedEventArgs e)
        {
			m_measureMemory = true;
		}

        private void OnBacktrackAndStop(Stack<SchedulingData> stack, SchedulingData fromSD, ExplicitActiveState cur)
        {
            m_continue = false;
        }

		public int GetDFSStackSize()
        {
            return -42;// m_dfs.Count;
		}

        public System.Exception GetUnhandledException()
        {
            return cur.CurrentThread.UnhandledException;
        }

        public bool Run()
        {
            bool logAssert = false;
            bool logDeadlock = false;
            bool noErrors = false;

            Statistics.Start();
            m_continue = true;

            Logger.Notice("Exploration starts now");

            cur.StateStorage = m_stateStorage;
            cur.PathStore = _pathStore;

            InitializeStaticGlobals(cur);

            do
            {
                if (!SetExecutingThread(out ThreadState thread))
                {
                    break; // no thread was selected 
                }

                // Execute instructions
                noErrors = ExecuteNextStep(thread);

                // Check for specification dissatifaction
                if (!noErrors)
                {
                    // assertion violations?
                    Statistics.AssertionViolation();
                    if (!logAssert)
                    {
                        Logger.Message("Assertion violation detected");
                    }
                    logAssert = true;

                    if (Config.StopOnError)
                    {
                        break;
                    }
                }
                else if (CheckDeadlock())
                {
                    // deadlock found?
                    Statistics.Deadlock();
                    if (!logDeadlock)
                    {
                        Logger.Message("Deadlock detected");                        
                    }
                    logDeadlock = true;
                    noErrors = false;

                    Deadlocked(null);

                    if (Config.StopOnError)
                    {
                        break;
                    }
                }

                Advance();
                
                Statistics.MaxHashtableSize(m_stateStorage.Count);
                Statistics.MaxHeapArray(cur.DynamicArea.Allocations.Length);

                MemoryLimiting();
            } while (m_continue);

            Logger.Message("End of story: explored the whole state space");

            _explorationLogger.Dispose();

            return noErrors;
        }

        private void InitializeStaticGlobals(ExplicitActiveState cur)
        {
            dnWalker.NativePeers.SystemConsole.Init(cur);
        }

        private void Advance()
        {
            cur.Advance(out m_continue);
        }

        /// <summary>
        /// Measures memory and performs actions on it
        /// </summary>
        private void MemoryLimiting()
        {
            if (m_measureMemory)
            {
                long memUsed = GC.GetTotalMemory(false);
                Statistics.MeasureMemory(memUsed);

                /// If ex post facto transition merging is enabled...
                if (!double.IsInfinity(Config.OptimizeStorageAtMegabyte) && (memUsed / 1024 / 1024) > Config.OptimizeStorageAtMegabyte)
                {
                    int count = m_atomicStates.Count;

                    foreach (CollapsedState cs in m_atomicStates)
                    {
                        m_stateStorage.Remove(cs);
                    }
                    m_atomicStates.Clear();

                    long memAfter = GC.GetTotalMemory(true);

                    Logger.Notice("Optimized hashtable from " + memUsed / 1024 / 1024 + " Mb to " + memAfter / 1024 / 1024 + " Mb by removing " + count + " states");
                    memUsed = memAfter;
                }

                /// If memory limiting is enabled...
                if (!double.IsInfinity(Config.MemoryLimit) && (memUsed / 1024 / 1024) > Config.MemoryLimit)
                {
                    Logger.Notice("Ran out of memory");
                    m_continue = false;
                }

                m_measureMemory = false;
            }
        }

        /// <summary>
        /// The error trace consists of thread id's on the current DFS stack. Used by the TracingExplorer
        /// </summary>
        public Stack<int> GetErrorTrace()
        {
            return new Stack<int>();// (_choiceGenerator as IScheduler).GetErrorTrace();
        }

        public virtual void PrintTransition() { }

        /// <summary>
        /// Execute one (unsafe) instruction, followed by 0 or more safe instructions,
        /// where safe are intrathread instructions or where the is only one thread left for execution
        /// </summary>
        /// <param name="thread"></param>
        /// <param name="threadTerm"></param>
        /// <returns></returns>
        public bool ExecuteStep(ThreadState thread, out bool threadTerm)
        {
            return thread.ExecuteStep(_instructionExecProvider, Logger, Config, m_dpor, out threadTerm);
        }

        private bool SetExecutingThread(out ThreadState thread)
        {
            thread = _strategy.GetExecutingThread();
            return thread != null;
        }

        public bool ExecuteNextStep(ThreadState thread)
        {
            bool noErrors;
            bool threadTerm;

            if (Config.OneTraceAndStop)
            {
                PrintTransition(thread);
            }

            //do
            //{
            noErrors = ExecuteStep(thread, out threadTerm);
            /*
                var threadId = m_spor.GetPersistentThread(this);
                if (threadId < 0)
                {
                    break;
                }
                thread = cur.ThreadPool.Threads[threadId];
            } while (noErrors && !threadTerm);*/

            return noErrors;
        }

		/*
		 * This method is not really pretty, but it works...
		 */
		public void PrintTransition(ThreadState thread) 
        {
			MethodState currentMethod = thread.CurrentMethod;
			Instruction instr = currentMethod.ProgramCounter;
			string operandString = instr.Operand == null ?
				"" :
				CILElementPrinter.FormatCILElement(instr.Operand);

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("- thread: {0} ", thread.Id);

			sb.AppendFormat("{0:D4} {1} {2} on stack {3}, RunnableThreadCount={4} ",
				instr.Offset,
				instr.OpCode.Name,
				operandString,
				currentMethod.EvalStack.ToString(),
				cur.ThreadPool.RunnableThreadCount);

			Console.WriteLine(sb.ToString());
		}

        public void CheckAtomicOnNewState(CollapsedState collapsedCurrent, SchedulingData sd, ExplicitActiveState state)
        {
            if (!double.IsInfinity(Config.OptimizeStorageAtMegabyte) && state.ThreadPool.RunnableThreadCount == 1)
            {
                m_atomicStates.AddLast(collapsedCurrent);
            }
        }

        public void CheckAtomicOnBacktrack(Stack<SchedulingData> stack, SchedulingData parent, ExplicitActiveState cur)
        {
            if (!double.IsInfinity(Config.OptimizeStorageAtMegabyte))
            {
                // check if singleton transition
                if (parent.Working.Count == 0 && parent.Done.Count == 1)
                    m_atomicStates.AddLast(parent.State);
            }
        }

		public bool CheckDeadlock()
        {
			bool allStopped = true;
			if (cur.ThreadPool.RunnableThreadCount == 0)
				for (int i = 0; allStopped && i < cur.ThreadPool.Threads.Length; ++i)
					allStopped = cur.ThreadPool.Threads[i].State == System.Threading.ThreadState.Stopped;

			return !allStopped;
		}

		// DEBUG method 
		public string DebugSharedObjects
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < cur.DynamicArea.Allocations.Length; i++)
                {
                    DynamicAllocation ida = cur.DynamicArea.Allocations[i];
                    if (ida != null)
                    {
                        sb.AppendFormat("{0} FirstThread={1} ThreadShared={2}\n", i + 1, ida.HeapAttribute, ida.ThreadShared);
                        sb.AppendLine();
                    }
                }
                return sb.ToString();
            }
        }

        public ExplicitActiveState cur { get; }
    }
}