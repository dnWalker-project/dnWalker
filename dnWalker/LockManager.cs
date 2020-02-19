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
    using System.Collections.Generic;
    using System.Diagnostics;
    using MMC.Data;

    public class LockManager
    {
        /// Constant for 'not owner'. Nullable types would be nice, but...
        public const int NoThread = -1;
        private readonly ExplicitActiveState cur;

        public LockManager(ExplicitActiveState cur)
        {
            this.cur = cur;
        }

        /// <summary>Check if an allocation has an associated locking thread. [const]</summary>
        ///
        /// <param name="obj">Reference to the allocation.</param>
        /// <returns>True iff a thread owns the lock on this allocation.</returns>
        public bool IsLocked(ObjectReference obj)
        {
            DynamicAllocation alloc = cur.DynamicArea.Allocations[obj];
            Debug.Assert(alloc != null, "Checking lock on null allocation.");
            return alloc.Locked;
        }

        /// <summary>Get the lock associated with an allocation.</summary>
        /// <remarks>
        /// This creates a new (empty) lock if none exists (done by the
        /// Allocation class).
        /// </remarks>
        /// <param name="obj">Reference to the allocation.</param>
        /// <returns>The associated lock (possibly new).</returns>
        public Lock GetLock(ObjectReference obj)
        {
            DynamicAllocation alloc = cur.DynamicArea.Allocations[obj];
            Debug.Assert(alloc != null, "Getting lock on null allocation.");
            return alloc.Lock;
        }

        /// <summary>Attempt to acquire the lock on an allocation for a thread.</summary>
        /// <remarks>
        /// One thread can own more than one lock on an allocation. To release
        /// the lock completely the number of Release() calls should equal the
        /// number of Acquire() calls.
        /// </remarks>
        /// <param name="obj">Reference to the allocation.</param>
        /// <param name="thread_id">The number of the thread to acquire the lock for.</param>
        /// <returns>True iff the lock was succesfully acquired.</returns>
        public bool Acquire(ObjectReference obj, int thread_id)
        {
            Lock l = GetLock(obj);
            bool retval = l.Owner == thread_id || l.Owner == NoThread;
            if (retval)
            {
                l.Owner = thread_id;
                l.Count++;
            }
            return retval;
        }

        public bool IsAcquireable(ObjectReference obj, int thread_id)
        {
            Lock l = GetLock(obj);
            return l.Owner == thread_id || l.Owner == NoThread;
        }

        /// <summary>Notify (all) waiting threads.</summary>
        /// <remarks>
        /// This moves one (or all, if all is true) thread from the waiting
        /// queue to the ready queue.
        /// </remarks>
        /// <param name="obj">Reference to the allocation.</param>
        /// <param name="all">Notify all waiting threads if true. Just one otherwise.</param>
        public void Pulse(ObjectReference obj, bool all)
        {
            Lock l = GetLock(obj);
            if (l.HasWaitQueue())
            {
                Queue<int> waitQueue = l.WaitQueue;
                Queue<int> readyQueue = l.ReadyQueue;
                int max = (all ? waitQueue.Count : 1);
                while (max-- > 0 && waitQueue.Count > 0)
                {
                    readyQueue.Enqueue(waitQueue.Dequeue());
                }
            }
        }

        /// <summary>Release the lock on an allocation.</summary>
        /// <remarks>
        /// If the lock is completely released (i.e. the number of Release()
        /// calls equals the number of Acquire() calls), the ownership of the
        /// lock is automatically passed to the first thread in the ready
        /// queue.
        /// </remarks>
        /// <param name="obj">Reference to the allocation.</param>
        public void Release(ObjectReference obj)
        {
            Lock l = GetLock(obj);
            l.Count--;
            // If lock is fully released...
            if (l.Count == 0)
            {
                if (l.HasReadyQueue())
                {
                    // Let first in line in the ready queue acquire the lock.
                    l.Owner = l.ReadyQueue.Dequeue();
                    l.Count = 1;
                }
                else
                {
                    l.Owner = NoThread;
                }
            }
        }
    }
}