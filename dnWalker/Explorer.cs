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
using System.Collections.Generic;
using System.Text;
using MMC.Data;


namespace MMC {

	using System.Text;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Timers;
	using MMC.State;
	using MMC.Util;
	using MMC.InstructionExec;
	using MMC.Collections;
    using dnlib.DotNet.Emit;

    /// Handler for events that indicate the exploration of a state.
    delegate void StateEventHandler(CollapsedState collapsed, SchedulingData sd);
	/// Handler for deadlock events.
	delegate void DeadlockEventHandler(SchedulingData sd);
	/// Handler for backtracking events.
	delegate void BacktrackEventHandler(Stack<SchedulingData> stack, SchedulingData fromSD);
	/// Handler for scheduling events, at the moment only a chosen thread.
	delegate void PickThreadEventHandle(SchedulingData sd, int chosen);
	/// Handler for the event that the exploration stops.
	delegate void ExplorationHaltEventHandle(IIEReturnValue ier);

	class Explorer {

		public static bool DoSharingAnalysis = false;

		public bool m_continue = false;
		public bool m_measureMemory = true;

		/// A new state was found (or constructed) while exploring.
		protected event StateEventHandler StateConstructed;
		/// A state has been re-visited.
		protected event StateEventHandler StateRevisited;
		/// Exploration backtracked.
		protected event BacktrackEventHandler Backtracked;
		/// Exploration backtracked start
		protected event BacktrackEventHandler BacktrackStart;
		/// Exploration backtracked stop
		protected event BacktrackEventHandler BacktrackStop;
		/// A deadlock occured.
		protected event DeadlockEventHandler Deadlocked;
		/// A thread was scheduled.
		protected event PickThreadEventHandle ThreadPicked;
		/// Exploration was halted (e.g. because of an unhandled exception).
		protected event ExplorationHaltEventHandle ExplorationHalted;

		Stack<SchedulingData> m_dfs;
		Collapser m_stateConvertor;
		FastHashtable<CollapsedState, int> m_stateStorage;
		readonly Queue<int> m_emptyQueue = new Queue<int>(0);
		StatefulDynamicPOR m_dpor;
		ObjectEscapePOR m_spor;
		IGarbageCollector m_gc;
		Timer m_explorationTimer;
		Timer m_memoryTimer;

        private readonly IConfig _config;
        private readonly IInstructionExecProvider _instructionExecProvider;

        LinkedList<CollapsedState> m_atomicStates;

		public Explorer(IConfig config, IInstructionExecProvider instructionExecProvider)
        {
            _config = config;
            _instructionExecProvider = instructionExecProvider;
            // DFS stack
            m_dfs = new Stack<SchedulingData>();
			m_stateConvertor = new Collapser();

			// hashtable
			m_stateStorage = new FastHashtable<CollapsedState, int>(20);

			ExplorationLogger el = new ExplorationLogger(this);

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

			if (DotWriter.IsEnabled()) {
				StateConstructed += new StateEventHandler(el.GraphNewState);
				StateRevisited += new StateEventHandler(el.GraphRevisitState);
				Backtracked += new BacktrackEventHandler(el.GraphBacktrack);
			}

			if (config.UseStatefulDynamicPOR) {
				m_dpor = new StatefulDynamicPOR(m_dfs, config);
				Backtracked += new BacktrackEventHandler(m_dpor.Backtracked);
				StateConstructed += new StateEventHandler(m_dpor.OnNewState);
				StateRevisited += new StateEventHandler(m_dpor.OnSeenState);
				ThreadPicked += new PickThreadEventHandle(m_dpor.ThreadPicked);
			}

			if (config.UseObjectEscapePOR) {
				m_spor = new ObjectEscapePOR();
				BacktrackStart += new BacktrackEventHandler(m_spor.CheckStoreThreadSharingData);
				StateConstructed += new StateEventHandler(m_spor.StoreThreadSharingData);
				BacktrackStop += new BacktrackEventHandler(m_spor.RestoreThreadSharingData);
			}

			if (!double.IsInfinity(config.MaxExploreInMinutes)) {
				m_explorationTimer = new Timer();
				m_explorationTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
				m_explorationTimer.Interval = config.MaxExploreInMinutes * 60 * 1000;
				m_explorationTimer.Enabled = true;
			}

			if (!double.IsInfinity(config.OptimizeStorageAtMegabyte)) {
				m_atomicStates = new LinkedList<CollapsedState>();
				StateConstructed += new StateEventHandler(this.CheckAtomicOnNewState);
				Backtracked += new BacktrackEventHandler(this.CheckAtomicOnBacktrack);
			}

			if (config.OneTraceAndStop)
				Backtracked += new BacktrackEventHandler(this.OnBacktrackAndStop);

			m_memoryTimer = new Timer();
			m_memoryTimer.Elapsed += new ElapsedEventHandler(OnTimedMemoryEvent);
			m_memoryTimer.Interval = 5 * 1000;
			m_memoryTimer.Enabled = true;

			/*
			 * Pick out the garbage collector. Note that only memoised GC and
			 * Mark & sweep are available. Reference counting is broken. */
			m_gc = (config.MemoisedGC) ?
								   IncrementalHeapVisitor.ihv as IGarbageCollector :
								   MarkAndSweepGC.msgc as IGarbageCollector;
		}

        public IConfig Config => _config;

		private void OnTimedEvent(object source, ElapsedEventArgs e) {
			Logger.l.Notice("Ran out of time");
			m_continue = false;
		}

		private void OnTimedMemoryEvent(object source, ElapsedEventArgs e) {
			m_measureMemory = true;
		}

		private void OnBacktrackAndStop(Stack<SchedulingData> stack, SchedulingData fromSD) {
			m_continue = false;
		}

		public int GetDFSStackSize() {
			return this.m_dfs.Count;
		}

		public bool Run() {
			bool logAssert = false;
			bool logDeadlock = false;
			bool noErrors;
			bool dummyBool;
			int threadId = 0;

			//Statistics.s.Start();
			m_continue = true;

			Logger.l.Notice("Exploration starts now");

			do {
                var cur = ActiveState.cur;
                /*
				 * Execute instructions. 
				 */
                if (_config.UseObjectEscapePOR)
					noErrors = ExecutePorStep(threadId);
				else
					noErrors = ExecuteStep(threadId, out dummyBool);

				/*
				 * Check for specification dissatifaction
				 */
				if (!noErrors) { // assertion violations?
					Statistics.s.AssertionViolation();
					if (!logAssert)
						Logger.l.Message("Assertion violation detected");
					logAssert = true;

					if (_config.StopOnError)
						break;
				} else if (CheckDeadlock()) { // deadlock found?
					Statistics.s.Deadlock();
					if (!logDeadlock)
						Logger.l.Message("Deadlock detected");
					logDeadlock = true;
					noErrors = false;

					if (_config.StopOnError)
						break;
				}

				/*
				 * Run garbage collection
				 */
				m_gc.Run();


				/*
				 * Do a state matching, store if unmatched,
				 * if matched, a backtracking is initiated by
				 * returning an empty working queue 
				 */
				SchedulingData sd = UpdateHashtable(cur);

				BacktrackStart(m_dfs, sd);
				while (sd.Working.Count == 0 && m_dfs.Count > 0) {
					// apply the reverse delta
					m_stateConvertor.DecollapseByDelta(sd.Delta);
					Backtracked(m_dfs, sd);
					sd = m_dfs.Pop();
				}
				BacktrackStop(m_dfs, sd);

				m_stateConvertor.Reset(sd.State);
				cur.Clean();

				/*
				 * either the recently explored sd can be pushed on the stack, 
				 * or the last popped sd from the stack is not fully explored */
				if (sd.Working.Count > 0) {
					m_dfs.Push(sd);
					threadId = SelectRunnableThread(sd); // for the next round		

					// update last access information 
					// (used by dynamic POR + tracing explorer) 
					MemoryLocation ml = cur.NextAccess(threadId);
					sd.LastAccess = new MemoryAccess(ml, threadId);

					ThreadPicked(sd, threadId);
				} else {
					Logger.l.Message("End of story: explored the whole state space");
				}

				Statistics.s.MaxHashtableSize(m_stateStorage.Count);
				Statistics.s.MaxHeapArray(cur.DynamicArea.Allocations.Length);

				MemoryLimiting();

				Statistics.s.BacktrackStackDepth(m_dfs.Count);

			} while (m_dfs.Count > 0 && m_continue == true);

			return noErrors;
		}

		/// Measures memory and performs actions on it 
		private void MemoryLimiting() {
			if (m_measureMemory) {
				long memUsed = System.GC.GetTotalMemory(false);
				Statistics.s.MeasureMemory(memUsed);
				
				/// If ex post facto transition merging is enabled...
				if (!Double.IsInfinity(_config.OptimizeStorageAtMegabyte) && (memUsed / 1024 / 1024) > _config.OptimizeStorageAtMegabyte) {
					int count = m_atomicStates.Count;

					foreach (CollapsedState cs in m_atomicStates)
						m_stateStorage.Remove(cs);
					m_atomicStates.Clear();

					long memAfter = System.GC.GetTotalMemory(true);

					Logger.l.Notice("Optimized hashtable from " + memUsed / 1024 / 1024 + " Mb to " + memAfter / 1024 / 1024 + " Mb by removing " + count + " states");
					memUsed = memAfter;
				}

				/// If memory limiting is enabled...
				if (!Double.IsInfinity(_config.MemoryLimit) && (memUsed / 1024 / 1024) > _config.MemoryLimit) {
					Logger.l.Notice("Ran out of memory");
					this.m_continue = false;
				}
				m_measureMemory = false;
			}
		}

		/// The error trace consists of thread id's on the current
		/// DFS stack. Used by the TracingExplorer
		public Stack<int> GetErrorTrace() {
			Stack<int> retval = new Stack<int>();
			foreach (SchedulingData sd in m_dfs)
				retval.Push(sd.LastAccess.ThreadId);

			return retval;
		}

		protected virtual int SelectRunnableThread(SchedulingData sd) {
			int retval = sd.Dequeue();
			return retval;
		}

		public bool OnStack(CollapsedState s) {
			foreach (SchedulingData sd in m_dfs) {
				if (sd.State.Equals(s))
					return true;
			}

			return false;
		}

        public SchedulingData UpdateHashtable(ExplicitActiveState cur)
        {
            var collapsedCurrent = m_stateConvertor.CollapseCurrentState(cur);

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
                StateRevisited(collapsedCurrent, sd);
            }
            else
            {
                // state is new
                collapsedCurrent.ClearDelta(); // delta's do not need to be stored				
                sd.Enabled = cur.ThreadPool.RunnableThreads;

                if (sd.Enabled.Count > 0)
                {
                    sd.Working = sd.Enabled;
                    sd.Done = new Queue<int>();
                }
                else
                {
                    sd.Working = m_emptyQueue;
                    sd.Done = m_emptyQueue;
                }

                StateConstructed(collapsedCurrent, sd);
            }

            return sd;
        }

		public virtual void PrintTransition() {}

        /*
		 * Execute one (unsafe) instruction, followed by 0 or more safe instructions,
		 * where safe are intrathread instructions or where the is only one thread left 
		 * for execution */
        public bool ExecuteStep(int threadId, out bool threadTerm)
        {
            ActiveState.cur.ThreadPool.CurrentThreadId = threadId;

            MethodState currentMethod = ActiveState.cur.CurrentMethod;
            InstructionExecBase currentInstrExec = _instructionExecProvider.GetExecFor(ActiveState.cur.CurrentMethod.ProgramCounter);
            bool continueExploration;
            IIEReturnValue ier;
            bool canForward = false;

            do
            {
                var cur = ActiveState.cur;

                PrintTransition();
                //Console.Out.WriteLine(currentInstrExec.ToString());
                ier = currentInstrExec.Execute(cur);

                currentMethod.ProgramCounter = ier.GetNextInstruction(currentMethod);
                /* if a RET was performed, currentMethod.ProgramCounter is null
				 * and the PC should be set to programcounter of the current method, i.e.,
				 * the method jumped to
				 */
                currentMethod = cur.CurrentMethod;
                continueExploration = ier.ContinueExploration(currentMethod);

                if (currentMethod != null && continueExploration)
                {
                    currentInstrExec = _instructionExecProvider.GetExecFor(currentMethod.ProgramCounter);
                }
                else
                {
                    currentInstrExec = null;
                }

                canForward = currentInstrExec != null && cur.CurrentThread.IsRunnable
                    && (currentInstrExec.IsMultiThreadSafe(cur) || cur.ThreadPool.RunnableThreadCount == 1);

                /*
				 * Optimization related to merging states that have only one outgoing transition,
				 * need to ensure stateful DPOR correctness
				 */
                if (canForward
                        && _config.UseStatefulDynamicPOR
                        && !currentInstrExec.IsMultiThreadSafe(cur)
                        && cur.ThreadPool.RunnableThreadCount == 1)
                {
                    MemoryLocation ml = cur.NextAccess(threadId);
                    m_dpor.ExpandSelectedSet(new MemoryAccess(ml, threadId));
                }
            } while (canForward);

            if (ActiveState.cur.CallStack.IsEmpty())
            {
                ActiveState.cur.ThreadPool.TerminateThread(threadId);
                threadTerm = true;
            }
            else
            {
                threadTerm = false;
            }

            return continueExploration;
        }

		public bool ExecutePorStep(int threadId) {
			bool noErrors;
			bool threadTerm;

			if (_config.OneTraceAndStop)
				PrintTransition(threadId);

			do {
				noErrors = ExecuteStep(threadId, out threadTerm);
				threadId = m_spor.GetPersistentThread();
			} while (noErrors && !threadTerm && threadId >= 0);


			return noErrors;
		}

		/*
		 * This method is not really pretty, but it works...
		 */
		public void PrintTransition(int tid) {
			MethodState currentMethod = ActiveState.cur.ThreadPool.Threads[tid].CurrentMethod;
			Instruction instr = currentMethod.ProgramCounter;
			bool isRet = instr.OpCode.Code == Code.Ret;
			string operandString = (instr.Operand == null ?
					"" :
					CILElementPrinter.FormatCILElement(instr.Operand));

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("- thread: {0} ", tid);

			sb.AppendFormat("{0:D4} {1} {2} on stack {3}, RunnableThreadCount={4} ",
							instr.Offset,
							instr.OpCode.Name,
							operandString,
							currentMethod.EvalStack.ToString(),
							ActiveState.cur.ThreadPool.RunnableThreadCount
				);

			System.Console.WriteLine(sb.ToString());
		}

		public void CheckAtomicOnNewState(CollapsedState collapsedCurrent, SchedulingData sd) {
			if (!Double.IsInfinity(_config.OptimizeStorageAtMegabyte) && ActiveState.cur.ThreadPool.RunnableThreadCount == 1)
				m_atomicStates.AddLast(collapsedCurrent);
		}

		public void CheckAtomicOnBacktrack(Stack<SchedulingData> stack, SchedulingData parent) {
			if (!Double.IsInfinity(_config.OptimizeStorageAtMegabyte)) {
				// check if singleton transition
				if (parent.Working.Count == 0 && parent.Done.Count == 1)
					m_atomicStates.AddLast(parent.State);
			}
		}


		public bool CheckDeadlock() {

			bool allStopped = true;
			if (ActiveState.cur.ThreadPool.RunnableThreadCount == 0)
				for (int i = 0; allStopped && i < ActiveState.cur.ThreadPool.Threads.Length; ++i)
					allStopped = ActiveState.cur.ThreadPool.Threads[i].State == (int)System.Threading.ThreadState.Stopped;

			return !allStopped;
		}

		// DEBUG method 
		public String DebugSharedObjects {
			get {
				String result = "";

				for (int i = 0; i < ActiveState.cur.DynamicArea.Allocations.Length; i++) {
					DynamicAllocation ida = ActiveState.cur.DynamicArea.Allocations[i];

					if (ida != null) {
						result += String.Format("{0} FirstThread={1} ThreadShared={2}\n", i + 1, ida.HeapAttribute, ida.ThreadShared).ToString();
					}
				}
				return result;
			}
		}
		
		/// String of last accessed fields on the DFS stack
		public String DebugLastAccessed {
			get {
				String result = "";
				foreach (SchedulingData sd in m_dfs) {
					result += sd.LastAccess.ToString() + "\n";
				}

				return result;
			}
		}

		/// String of working sets on the DFS stack
		public String DebugWorkingSets() {
			String result = "";
			foreach (SchedulingData sd in m_dfs) {
				result += ListToString.Format(new ArrayList(sd.Enabled.ToArray())) + "\n";
			}

			return result;
		}
	}
}