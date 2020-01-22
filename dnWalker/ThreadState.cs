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

	using System.Collections.Generic;
	using System.Collections;
	using System.Diagnostics;
	
	using MMC.Data;
	using MMC.Util;

    public class ThreadState : IMustDispose, ICleanable, IStorageVisitable {

		public static int state_field_offset = LockManager.NoThread;

		CallStack m_callStack;
		ObjectReference m_threadObj;
		ObjectReference m_exceptionObj; // ref is stored here in case of exception
		int m_state; // Short-hand for the 'state' field.
        bool m_isDirty;
		int m_me; // an ID.

        private readonly ExplicitActiveState cur;

        // ---------------- Accessors and Short-hands -------------- 

        public CallStack CallStack {

			get { return m_callStack; }
			set { m_callStack = value; } // for restoring the state
		}

		public MethodState CurrentMethod {

			get {
				if (m_callStack.IsEmpty())
					return null;
				else
					return m_callStack.Peek();
			}
		}

		public CILLocation CurrentLocation {

			get {
				return new CILLocation(
				  CurrentMethod.ProgramCounter,
				  CurrentMethod.Definition);
			}
		}

		public ObjectReference ThreadObject {

			get { return m_threadObj; }
			set { m_threadObj = value; }
		}

		public ObjectReference ExceptionReference {

			get { return this.m_exceptionObj; }
			set { this.m_exceptionObj = value; }
		}

		// ---------------- State, Locking and Waiting -------------- 

		public int State {

			get { return m_state; }
			set {
				if (m_state == MMC.ThreadStatus.Stopped) {
					Debug.Assert(value != m_state, "Stopping already stopped thread.");
					if (value == MMC.ThreadStatus.Running || value == MMC.ThreadStatus.WaitSleepJoin) {
						cur.DynamicArea.SetPinnedAllocation(m_threadObj, true);
						ThreadObjectWatcher.Increment(m_me, m_threadObj, cur);
					}
				}
				if (value == MMC.ThreadStatus.Stopped)
					ReleaseObject();

				m_isDirty |= m_state != value;
				m_state = value;

				if (state_field_offset != LockManager.NoThread) {
					AllocatedObject theThreadObject =
						(AllocatedObject)cur.DynamicArea.Allocations[m_threadObj];
					Debug.Assert(theThreadObject != null,
							"Thread object should not be null when setting state (even to Stopped).");
					theThreadObject.Fields[state_field_offset] = new Int4(m_state);
				}
			}
		}

		public bool IsAlive {

			get { return ThreadStatus.IsAlive(m_state); }
		}

		public bool IsRunnable {

			get { return m_state == ThreadStatus.Running; }
		}

		public void WaitFor(int other) {

			m_state = MMC.ThreadStatus.WaitSleepJoin;
			m_isDirty |= WaitingFor != other;
			//m_isDirty = true;
			WaitingFor = other;
		}

        public void Awaken()
        {
            m_state = MMC.ThreadStatus.Running;
            //m_isDirty |= m_waitFor != LockManager.NoThread;
            m_isDirty = true;
            WaitingFor = LockManager.NoThread;
            Logger.l.Debug("thread {0} woke up", m_me);
        }

        public int WaitingFor { get; set; }

        // ---------------- Cleanup and Cleanliness -------------- 

        void ReleaseObject()
        {
            cur.DynamicArea.SetPinnedAllocation(m_threadObj, false);
            ThreadObjectWatcher.Decrement(m_me, ThreadObject, cur);
        }

        public void Dispose()
        {
            ReleaseObject();
            m_callStack.Dispose();
        }

		public bool IsDirty() {

			// This is okay for checking whether the thread state is dirty,
			// but note lower frames may be dirty as well.
			MethodState top = CurrentMethod;
			return m_isDirty || m_callStack.IsDirty() || (top != null && top.IsDirty());
		}

		public void Clean() {

			m_isDirty = false;
			m_callStack.Clean();
		}

		// ---------------- Miscellaneaous and Helpers -------------- 

		public void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
			visitor.VisitThreadState(this);
		}

		public override string ToString() {

			int currentWaitFor = WaitingFor;

			for (int i = 0; i < cur.DynamicArea.Allocations.Length; i++) {
				DynamicAllocation ida = cur.DynamicArea.Allocations[i];
				if (ida != null && ida.Locked) {
					Lock l = ida.Lock;
					if (l.ReadyQueue.Contains(m_me) || l.WaitQueue.Contains(m_me)) {
						currentWaitFor = l.Owner;
						break;
					}
				}
			}

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendFormat("object: {0}, state: {1}{2}, {3}, call stacks:\n",
					m_threadObj.ToString(),
					(System.Threading.ThreadState)m_state,
					(currentWaitFor != LockManager.NoThread ? " waiting for " + currentWaitFor : ""),
					(IsDirty() ? "dirty" : "clean"));
			for (int j = 0; j < m_callStack.StackPointer; ++j)
				sb.AppendFormat("    {0}\n", m_callStack[j].ToString());
			if (m_callStack.IsEmpty())
				sb.Append("    Empty call stack.\n");
			return sb.ToString();
		}

		public ThreadState(ExplicitActiveState cur, ObjectReference threadObj, int me)
        {
			m_callStack = cur.StorageFactory.CreateCallStack();
			m_threadObj = threadObj;
			m_state = MMC.ThreadStatus.Running;
			WaitingFor = LockManager.NoThread;
			m_isDirty = true;
			m_me = me;
			m_exceptionObj = ObjectReference.Null;
            this.cur = cur;
			cur.DynamicArea.SetPinnedAllocation(m_threadObj, true);
			ThreadObjectWatcher.Increment(me, threadObj, cur);
		}
	}
}
