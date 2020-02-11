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

    public class ThreadPool : ICleanable, IStorageVisitable {

		SparseReferenceList<ThreadState>	m_tl;
		int					m_curThread;
		DirtyList			m_dirty;
        private readonly Logger _logger;

        ////////////////////////////////////////////////////////////////////
        // Accessors
        ////////////////////////////////////////////////////////////////////

        public int CurrentThreadId {

			get { return m_curThread; }
			set { m_curThread = value; }
		}

		public SparseReferenceList<ThreadState> Threads {

			get { return m_tl; }
		}

		////////////////////////////////////////////////////////////////////
		// ICleanable implementation and such.
		////////////////////////////////////////////////////////////////////

		public DirtyList DirtyThreads {

			get {
				// Add all changed threads to m_dirty and return the list.
				for (int thread_id = 0; thread_id < m_tl.Length; ++thread_id)
					if (m_tl[thread_id] != null && m_tl[thread_id].IsDirty())
						m_dirty.SetDirty(thread_id);
				return m_dirty;
			}
		}

		public bool IsDirty() {

			bool retval = m_dirty.Count > 0;
			for (int thread_id = 0; !retval && thread_id < m_tl.Length; ++thread_id)
				retval = m_tl[thread_id] != null && m_tl[thread_id].IsDirty();
			return retval;
		}

		public void Clean() {

			//m_dirty = new DirtyList(m_tl.Length);
			m_dirty.Clean();
			for (int thread_id = 0; thread_id < m_tl.Length; ++thread_id)
				if (m_tl[thread_id] != null)
					m_tl[thread_id].Clean();
		}

		////////////////////////////////////////////////////////////////////
		// Query and enumerate threads.
		////////////////////////////////////////////////////////////////////

		public int GetThreadCount(System.Threading.ThreadState state)
        {
			int retval = 0;
			for (int i=0; i < m_tl.Length; ++i)
				if (m_tl[i] != null && m_tl[i].State == state)
					++retval;
			return retval;
		}

		public int GetThreadCount()
        {
			int retval = 0;
			for (int i=0; i < m_tl.Length; ++i)
				if (m_tl[i] != null) ++retval;
			return retval;
		}

		public Queue<int> GetThreadCollection(System.Threading.ThreadState state)
        {
			Queue<int> retval = new Queue<int>(m_tl.Length);
			for (int i=0; i < m_tl.Length; ++i)
				if (m_tl[i] != null && m_tl[i].State == state)
					retval.Enqueue(i);
			return retval;
		}

		public int FindOwningThread(IDataElement e) {

			int retval = LockManager.NoThread;
			for (int i = 0; retval == LockManager.NoThread && i < m_tl.Length; ++i) {
				// For now, only look in thread object field. It would be nice to
				// have a more advanced implementation that looks into the call stacks,
				// and tries to find the data element there (of course, implement the
				// relevant code in MethodState, DataElementList, DataElementStack, etc.).
				if (m_tl[i].ThreadObject.Equals(e))
					retval = i;
			}
			return retval;
		}

		////////////////////////////////////////////////////////////////////
		// Create new threads.
		////////////////////////////////////////////////////////////////////

		public int NewThread(ExplicitActiveState cur, MethodState entry, ObjectReference threadObj)
        {
			int thread_id = m_tl.Length;
			ThreadState newThread = new ThreadState(cur, threadObj, thread_id);
			m_tl.Add(newThread);
			newThread.CallStack.Push(entry);
			_logger.Debug("spawned new thread with id {0}", thread_id);

			// these arguments were added by the parent thread, but they should be 
			// marked from the child thread for heap analysis
			ThreadObjectWatcher.DecrementAll(this, entry.Arguments, cur);
			ThreadObjectWatcher.IncrementAll(thread_id, entry.Arguments, cur);

            cur.RequestSharingAnalysis();

            return thread_id;
		}

		////////////////////////////////////////////////////////////////////
		// Suspending threads.
		////////////////////////////////////////////////////////////////////

		public void JoinThreads(int blocking_thread, int to_terminate) {

			ThreadState to_block = m_tl[blocking_thread];
			Debug.Assert(to_block != null, "Thread to be blocked is null!?");
			ThreadState to_term = m_tl[to_terminate];
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
			int current = begin_thread;
			BitArray seen = new BitArray(m_tl.Length, false);
			while (current != LockManager.NoThread && !seen[current]) {
				Debug.Assert(m_tl[current] != null, 
						"some thread is waiting for some a null thread at offset "+current+".");
				seen[current] = true;
				current = m_tl[current].WaitingFor;
			}
			if (current != LockManager.NoThread)
				_logger.Log(LogPriority.Severe,
						"cycle in waiting-for graph. deadlock!");
		}

        ////////////////////////////////////////////////////////////////////
        // Terminate and delete threads.
        ////////////////////////////////////////////////////////////////////

        public void TerminateThread(int thread_id)
        {
            ThreadState theThread = m_tl[thread_id];
            Debug.Assert(theThread != null, "Terminating a null thread.");
            _logger.Debug("thread termination: {0}", thread_id);
            if (!theThread.CallStack.IsEmpty())
            {
                _logger.Warning("terminating a thread with a non-empty call stack.");
            }

            theThread.State = System.Threading.ThreadState.Stopped;

            //Explorer.ActivateGC = true;

            // Check for threads that called Join on the terminating thread, and wake them.
            for (int i = 0; i < m_tl.Length; ++i)
            {
                if (m_tl[i] != null && m_tl[i].State == System.Threading.ThreadState.WaitSleepJoin
                    && m_tl[i].WaitingFor == thread_id)
                {
                    m_tl[i].Awaken(_logger);
                }
            }
        }

		public void DeleteThread(int thread_id) {

			if (thread_id < 0 && thread_id >= m_tl.Length)
				throw new System.ArgumentException("thread ID not within bounds of thread list");

			ThreadState bokje = m_tl[thread_id];
			if (bokje != null) {
				//bokje.Dispose(); 
				m_tl[thread_id] = null;
				m_dirty.SetDirty(thread_id);
			}
		}

		public void SetThreadUpperBound(int first_to_die) {

			for (int i=first_to_die; i < m_tl.Length; ++i)
				DeleteThread(i);
		}

		////////////////////////////////////////////////////////////////////
		// Constructor and other boring stuff.
		////////////////////////////////////////////////////////////////////

		public void Accept(IStorageVisitor visitor, ExplicitActiveState cur) {

			visitor.VisitThreadPool(this);
		}

		public override string ToString() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i=0; i < m_tl.Length; ++i) {
				if (m_tl[i] != null) {
					sb.AppendFormat("* Thread {0}: ", i);
					sb.Append(m_tl[i].ToString());
				}
			}
			return sb.ToString();
		}

        public ThreadPool(Logger logger)
        {
            m_tl = new SparseReferenceList<ThreadState>();
            m_dirty = new DirtyList();
            _logger = logger;
        }

		////////////////////////////////////////////////////////////////////
		// Short-hands.
		////////////////////////////////////////////////////////////////////

		public ThreadState CurrentThread
        {
			get { return m_tl[m_curThread]; }
		}

		public Queue<int> RunnableThreads
        {
			get
            {
				return GetThreadCollection(System.Threading.ThreadState.Running);
			}
		}

		public int RunnableThreadCount
        {
			get { return GetThreadCount(System.Threading.ThreadState.Running); }
		}
	}
}
