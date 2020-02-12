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
    using dnWalker.DataElements;

    public class ThreadState : IMustDispose, ICleanable, IStorageVisitable
    {
		public static int state_field_offset = LockManager.NoThread;

		CallStack m_callStack;
        System.Threading.ThreadState m_state; // Short-hand for the 'state' field.
        bool m_isDirty;
		int m_me; // an ID.

        private readonly ExplicitActiveState cur;

        // ---------------- Accessors and Short-hands -------------- 

        public CallStack CallStack
        {
			get { return m_callStack; }
			set { m_callStack = value; } // for restoring the state
		}

		public MethodState CurrentMethod
        {
			get
            {
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

        public ObjectReference ThreadObject { get; set; }

        public System.Exception UnhandledException { get; internal set; }

        private ObjectReference _exceptionReference;

        public ObjectReference ExceptionReference
        {
            get { return _exceptionReference; }
            set
            {
                _exceptionReference = value;
                if (ObjectReference.Null.Equals(_exceptionReference))
                {
                    UnhandledException = null;
                    return;
                }

                AllocatedObject exceptionObj = cur.DynamicArea.Allocations[_exceptionReference] as AllocatedObject;
                var messageField = cur.DefinitionProvider.GetFieldDefinition(typeof(System.Exception).FullName, "_message");
                UnhandledException = new System.Exception($"An exception of type {exceptionObj.Type.FullName} has been thrown " +
                    $"with message '{exceptionObj.Fields[(int)messageField.FieldOffset].ToString()}");
            }
        }

        // ---------------- State, Locking and Waiting -------------- 

        public System.Threading.ThreadState State
        {
			get { return m_state; }
			set {
				if (m_state == System.Threading.ThreadState.Stopped) {
					Debug.Assert(value != m_state, "Stopping already stopped thread.");
					if (value == System.Threading.ThreadState.Running || value == System.Threading.ThreadState.WaitSleepJoin) {
						cur.DynamicArea.SetPinnedAllocation(ThreadObject, true);
						ThreadObjectWatcher.Increment(m_me, ThreadObject, cur);
					}
				}
				if (value == System.Threading.ThreadState.Stopped)
					ReleaseObject();

				m_isDirty |= m_state != value;
				m_state = value;

				if (state_field_offset != LockManager.NoThread) {
					AllocatedObject theThreadObject =
						(AllocatedObject)cur.DynamicArea.Allocations[ThreadObject];
					Debug.Assert(theThreadObject != null,
							"Thread object should not be null when setting state (even to Stopped).");
					theThreadObject.Fields[state_field_offset] = new Int4((int)m_state);
				}
			}
		}

		public bool IsAlive
        {
            get
            {
                return m_state == System.Threading.ThreadState.Unstarted
                  || m_state == System.Threading.ThreadState.Running
                  || m_state == System.Threading.ThreadState.WaitSleepJoin;
            }
		}

		public bool IsRunnable
        { 
			get { return m_state == System.Threading.ThreadState.Running; }
		}

		public void WaitFor(int other)
        {
			m_state = System.Threading.ThreadState.WaitSleepJoin;
			m_isDirty |= WaitingFor != other;
			//m_isDirty = true;
			WaitingFor = other;
		}

        public void Awaken(Logger logger)
        {
            m_state = System.Threading.ThreadState.Running;
            //m_isDirty |= m_waitFor != LockManager.NoThread;
            m_isDirty = true;
            WaitingFor = LockManager.NoThread;
            logger.Debug("thread {0} woke up", m_me);
        }

        public int WaitingFor { get; set; }

        private IDataElement _retValue;
        public IDataElement RetValue
        {
            get
            {
                return _retValue;
            }
            internal set 
            {
                _retValue = value;

                if (value.Equals(ObjectReference.Null))
                {
                    return;
                }

                if (value is ObjectReference objectReference)
                {
                    _retValue = new ReturnValue(cur.DynamicArea.Allocations[objectReference], cur);
                }
            }
        }

        // ---------------- Cleanup and Cleanliness -------------- 

        void ReleaseObject()
        {
            cur.DynamicArea.SetPinnedAllocation(ThreadObject, false);
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
					ThreadObject.ToString(),
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
			ThreadObject = threadObj;
			m_state = System.Threading.ThreadState.Running;
			WaitingFor = LockManager.NoThread;
			m_isDirty = true;
			m_me = me;
			ExceptionReference = ObjectReference.Null;
            this.cur = cur;
			cur.DynamicArea.SetPinnedAllocation(ThreadObject, true);
			ThreadObjectWatcher.Increment(me, threadObj, cur);
		}
	}
}
