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

namespace MMC.State {

	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	
	using MMC;
	using MMC.Data;
	using MMC.Util;
	using MMC.Collections;
    using System.Linq;

    public delegate void NewThreadSpawned(ThreadState threadState);

    public class ThreadPool : ICleanable, IStorageVisitable 
    {
        private DirtyList m_dirty;
        private readonly Logger _logger;

        ////////////////////////////////////////////////////////////////////
        // Accessors
        ////////////////////////////////////////////////////////////////////

        public int CurrentThreadId 
        { 
            get; 
            set; 
        }

        public SparseReferenceList<ThreadState> Threads { get; }

        ////////////////////////////////////////////////////////////////////
        // ICleanable implementation and such.
        ////////////////////////////////////////////////////////////////////

        public DirtyList DirtyThreads 
        {
            get
            {
                // Add all changed threads to m_dirty and return the list.
                for (var thread_id = 0; thread_id < Threads.Length; ++thread_id)
                {
                    if (Threads[thread_id] != null && Threads[thread_id].IsDirty())
                    {
                        m_dirty.SetDirty(thread_id);
                    }
                }
                return m_dirty;
            }
		}

		public bool IsDirty() {

			var retval = m_dirty.Count > 0;
			for (var thread_id = 0; !retval && thread_id < Threads.Length; ++thread_id)
				retval = Threads[thread_id] != null && Threads[thread_id].IsDirty();
			return retval;
		}

        public void Clean()
        {
            //m_dirty = new DirtyList(m_tl.Length);
            m_dirty.Clean();
            foreach (var thread in Threads.Where(t => t != null))
            {
                thread.Clean();
            }
        }

		////////////////////////////////////////////////////////////////////
		// Query and enumerate threads.
		////////////////////////////////////////////////////////////////////

		public int GetThreadCount(System.Threading.ThreadState state)
        {
            return Threads.Count(t => t?.State == state);
        }

		public int GetThreadCount()
        {
            return Threads.Count(t => t != null);
        }

		public Queue<ThreadState> GetThreadCollection(System.Threading.ThreadState state)
        {
            return new Queue<ThreadState>(Threads.Where(t => t?.State == state));
		}

		public int FindOwningThread(IDataElement e) 
        {
			var retval = LockManager.NoThread;
			for (var i = 0; retval == LockManager.NoThread && i < Threads.Length; ++i) 
            {
                // For now, only look in thread object field. It would be nice to
                // have a more advanced implementation that looks into the call stacks,
                // and tries to find the data element there (of course, implement the
                // relevant code in MethodState, DataElementList, DataElementStack, etc.).
                if (Threads[i].ThreadObject.Equals(e))
                {
                    retval = i;
                }
			}
			return retval;
		}

		////////////////////////////////////////////////////////////////////
		// Create new threads.
		////////////////////////////////////////////////////////////////////

		public int NewThread(ExplicitActiveState cur, MethodState entry, ObjectReference threadObj)
        {
			var thread_id = Threads.Length;
            var newThread = new ThreadState(cur, threadObj, thread_id);
			Threads.Add(newThread);
			newThread.CallStack.Push(entry);
			_logger.Debug("spawned new thread with id {0}", thread_id);

			// these arguments were added by the parent thread, but they should be 
			// marked from the child thread for heap analysis
			ThreadObjectWatcher.DecrementAll(this, entry.Arguments, cur);
			ThreadObjectWatcher.IncrementAll(thread_id, entry.Arguments, cur);

            cur.RequestSharingAnalysis();

            RaiseNewThreadSpawned(newThread);

            return thread_id;
		}

        public event NewThreadSpawned OnNewThreadSpawned;

        private void RaiseNewThreadSpawned(ThreadState threadState)
        {
            OnNewThreadSpawned?.Invoke(threadState);
        }

        ////////////////////////////////////////////////////////////////////
        // Suspending threads.
        ////////////////////////////////////////////////////////////////////

        public void JoinThreads(int blocking_thread, int to_terminate) {

			var to_block = Threads[blocking_thread];
			Debug.Assert(to_block != null, "Thread to be blocked is null!?");
			var to_term = Threads[to_terminate];
			Debug.Assert(to_term != null, "Thread to be blocked is null!?");

			// This would be an MMC error: the thread to be blocked does a Join
			// call, but it's not running.
			Debug.Assert(to_block.State == System.Threading.ThreadState.Running, 
					"Thread to be blocked is not running.");

			// If the thread to wait on is not alive, skip the whole waiting thingy.
			// Issue a notice, and be done with it. In the most likely case the thread
			// is already stopped, and the waiting thread should simply continue.
			if (!to_term.IsAlive)
				_logger.Debug("thread {0} not waiting for {1}, since thread {1} is not alive.",
						blocking_thread, to_terminate);
			else {
				// Set the waiting-for and state fields of the blocking thread.
				to_block.WaitingFor = to_terminate;
				to_block.State = System.Threading.ThreadState.WaitSleepJoin;

				_logger.Debug("thread {0} is now waiting for thread {1}.",
						blocking_thread, to_terminate);

				CheckDeadlockFrom(blocking_thread);
			}
		}

		public void CheckDeadlockFrom(int begin_thread) {

			// Perform early deadlock detection by cycle detection in the
			// "waiting-for" graph.
			var current = begin_thread;
			var seen = new BitArray(Threads.Length, false);
			while (current != LockManager.NoThread && !seen[current]) {
				Debug.Assert(Threads[current] != null, 
						"some thread is waiting for some a null thread at offset "+current+".");
				seen[current] = true;
				current = Threads[current].WaitingFor;
			}
			if (current != LockManager.NoThread)
				_logger.Log(LogPriority.Severe,
						"cycle in waiting-for graph. deadlock!");
		}

        ////////////////////////////////////////////////////////////////////
        // Terminate and delete threads.
        ////////////////////////////////////////////////////////////////////

        public void TerminateThread(ThreadState theThread)
        {
            var thread_id = theThread.Id;
            Debug.Assert(theThread != null, "Terminating a null thread.");
            _logger.Debug("thread termination: {0}", thread_id);
            if (!theThread.CallStack.IsEmpty())
            {
                _logger.Warning("terminating a thread with a non-empty call stack.");
            }

            theThread.State = System.Threading.ThreadState.Stopped;

            //Explorer.ActivateGC = true;

            // Check for threads that called Join on the terminating thread, and wake them.
            foreach (var thread in Threads)
            {
                if (thread != null
                    && thread.State == System.Threading.ThreadState.WaitSleepJoin
                    && thread.WaitingFor == thread_id)
                {
                    thread.Awaken(_logger);
                }
            }
        }

		public void DeleteThread(int thread_id) 
        {
			if (thread_id < 0 && thread_id >= Threads.Length)
				throw new System.ArgumentException("thread ID not within bounds of thread list");

			var bokje = Threads[thread_id];
			if (bokje != null) {
				//bokje.Dispose(); 
				Threads[thread_id] = null;
				m_dirty.SetDirty(thread_id);
			}
		}

		public void SetThreadUpperBound(int first_to_die) 
        {
            for (var i = first_to_die; i < Threads.Length; ++i)
            {
                DeleteThread(i);
            }
		}

		////////////////////////////////////////////////////////////////////
		// Constructor and other boring stuff.
		////////////////////////////////////////////////////////////////////

		public void Accept(IStorageVisitor visitor, ExplicitActiveState cur) 
        {
			visitor.VisitThreadPool(this);
		}

		public override string ToString() 
        {
			var sb = new System.Text.StringBuilder();
            foreach (var thread in Threads)
            {
				if (thread != null) 
                {
					sb.AppendFormat("* Thread {0}: ", thread.Id);
					sb.Append(thread.ToString());
				}
			}
			return sb.ToString();
		}

        public ThreadPool(Logger logger)
        {
            Threads = new SparseReferenceList<ThreadState>();
            m_dirty = new DirtyList();
            _logger = logger;
        }

		////////////////////////////////////////////////////////////////////
		// Short-hands.
		////////////////////////////////////////////////////////////////////

		public ThreadState CurrentThread
        {
			get { return Threads[CurrentThreadId]; }
		}

		public Queue<ThreadState> RunnableThreads
        {
			get
            {
				return GetThreadCollection(System.Threading.ThreadState.Running);
			}
		}

        public Queue<int> RunnableThreadIds
        {
            get
            {
                return new Queue<int>(RunnableThreads.Select(t => t.Id));
            }
        }

        public int RunnableThreadCount
        {
			get { return GetThreadCount(System.Threading.ThreadState.Running); }
		}
	}
}
