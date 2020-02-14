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
    using System.Diagnostics;

    using MMC.Data;
    using MMC.Util;
    using dnWalker.DataElements;
    using MMC.InstructionExec;

    public class ThreadState : IMustDispose, ICleanable, IStorageVisitable
    {
        public static int state_field_offset = LockManager.NoThread;
        private System.Threading.ThreadState m_state; // Short-hand for the 'state' field.
        private bool m_isDirty;
        private readonly ExplicitActiveState cur;

        // ---------------- Accessors and Short-hands -------------- 
        public int Id { get; }

        public CallStack CallStack { get; set; }

        public MethodState CurrentMethod
        {
            get
            {
                if (CallStack.IsEmpty())
                {
                    return null;
                }

                return CallStack.Peek();
            }
        }

        public CILLocation CurrentLocation
        {
            get
            {
                return new CILLocation(CurrentMethod.ProgramCounter, CurrentMethod.Definition);
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
            set
            {
                if (m_state == System.Threading.ThreadState.Stopped)
                {
                    Debug.Assert(value != m_state, "Stopping already stopped thread.");
                    if (value == System.Threading.ThreadState.Running
                        || value == System.Threading.ThreadState.WaitSleepJoin)
                    {
                        cur.DynamicArea.SetPinnedAllocation(ThreadObject, true);
                        ThreadObjectWatcher.Increment(Id, ThreadObject, cur);
                    }
                }
                if (value == System.Threading.ThreadState.Stopped)
                {
                    ReleaseObject();
                }

                m_isDirty |= m_state != value;
                m_state = value;

                if (state_field_offset != LockManager.NoThread)
                {
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
            logger.Debug("thread {0} woke up", Id);
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
            ThreadObjectWatcher.Decrement(Id, ThreadObject, cur);
        }

        public void Dispose()
        {
            ReleaseObject();
            CallStack.Dispose();
        }

        public bool IsDirty()
        {
            // This is okay for checking whether the thread state is dirty,
            // but note lower frames may be dirty as well.
            MethodState top = CurrentMethod;
            return m_isDirty || CallStack.IsDirty() || (top != null && top.IsDirty());
        }

        public void Clean()
        {
            m_isDirty = false;
            CallStack.Clean();
        }

        // ---------------- Miscellaneaous and Helpers -------------- 

        public void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
            visitor.VisitThreadState(this);
        }

        public override string ToString()
        {
            int currentWaitFor = WaitingFor;

            for (int i = 0; i < cur.DynamicArea.Allocations.Length; i++)
            {
                DynamicAllocation ida = cur.DynamicArea.Allocations[i];
                if (ida != null && ida.Locked)
                {
                    Lock l = ida.Lock;
                    if (l.ReadyQueue.Contains(Id) || l.WaitQueue.Contains(Id))
                    {
                        currentWaitFor = l.Owner;
                        break;
                    }
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("object: {0}, state: {1}{2}, {3}, call stacks:\n",
                ThreadObject.ToString(),
                m_state,
                currentWaitFor != LockManager.NoThread ? " waiting for " + currentWaitFor : "",
                IsDirty() ? "dirty" : "clean");
            for (int j = 0; j < CallStack.StackPointer; ++j)
            {
                sb.AppendFormat("    {0}\n", CallStack[j].ToString());
            }
            if (CallStack.IsEmpty())
            {
                sb.Append("    Empty call stack.\n");
            }
            return sb.ToString();
        }

        public ThreadState(ExplicitActiveState cur, ObjectReference threadObj, int me)
        {
            CallStack = cur.StorageFactory.CreateCallStack();
            ThreadObject = threadObj;
            m_state = System.Threading.ThreadState.Running;
            WaitingFor = LockManager.NoThread;
            m_isDirty = true;
            Id = me;
            ExceptionReference = ObjectReference.Null;
            this.cur = cur;
            cur.DynamicArea.SetPinnedAllocation(ThreadObject, true);
            ThreadObjectWatcher.Increment(me, threadObj, cur);
        }

        public bool ExecuteStep(IInstructionExecProvider instructionExecProvider, 
            Logger logger,
            IConfig config,
            StatefulDynamicPOR m_dpor,
            out bool threadTerm)
        {
            ThreadState thread = this;

            cur.ThreadPool.CurrentThreadId = thread.Id;

            MethodState currentMethod = thread.CurrentMethod;
            var currentInstrExec = instructionExecProvider.GetExecFor(currentMethod.ProgramCounter);
            bool continueExploration;
            IIEReturnValue ier;
            bool canForward;

            do
            {
                logger.Trace(currentInstrExec.ToString());
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
                    currentInstrExec = instructionExecProvider.GetExecFor(currentMethod.ProgramCounter);
                }
                else
                {
                    currentInstrExec = null;
                }

                canForward = currentInstrExec != null 
                    && cur.CurrentThread.IsRunnable
                    && (currentInstrExec.IsMultiThreadSafe(cur) || cur.ThreadPool.RunnableThreadCount == 1);

                /*
				 * Optimization related to merging states that have only one outgoing transition,
				 * need to ensure stateful DPOR correctness
				 */
                if (canForward
                    && config.UseStatefulDynamicPOR
                    && !currentInstrExec.IsMultiThreadSafe(cur)
                    && cur.ThreadPool.RunnableThreadCount == 1)
                {
                    MemoryLocation ml = cur.NextAccess(thread);
                    m_dpor.ExpandSelectedSet(new MemoryAccess(ml, thread.Id));
                }
            } while (canForward);

            if (thread.CallStack.IsEmpty())
            {
                cur.ThreadPool.TerminateThread(thread);
                threadTerm = true;
            }
            else
            {
                threadTerm = false;
            }

            return continueExploration;
        }
    }
}