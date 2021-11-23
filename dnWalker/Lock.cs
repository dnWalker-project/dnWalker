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
	using MMC.Data;
	using MMC.Util;
	using MMC.Collections;

    /// <summary>
    /// A simple and minimal lock implementation.
    /// </summary>
    public class Lock : IStorable, ICleanable
    {
		bool m_isReadonly;
		bool m_isDirty;
		int m_owner;
		int m_count;
		Queue<int> m_waitQueue;
		Queue<int> m_readyQueue;

		/// <summary>
		/// The ID of the thread owning this lock.
		/// </summary>
		public int Owner
        {

            get { return m_owner; }
            set
            {
                m_isDirty |= m_owner != value;
                m_owner = value;
            }
        }

		/// <summary>The number of locks.</summary>
		///
		/// In other words, the number of Acquire() calls minus the number of
		/// Release() calls.
		public int Count
        {
            get { return m_count; }
            set
            {
                if (value >= 0)
                {
                    m_isDirty |= m_count != value;
                    m_count = value;
                }
                else
                {
                    // This should really never happen. If it does, it's probably a bug in this
                    // application. Otherwise, there's something wrong with (g)mcs.
                    throw new System.NotSupportedException("trying to set a lock count to less than zero! " +
                        "This should really never happen. If it does, it's probably a bug in this application. " +
                        "Otherwise, there's something wrong with (g)mcs.");
                    //Logger.l.Warning("trying to set a lock count to less than zero!");
                }
            }
        }

		/// <summary>Check if this lock has an associated wait queue. [const]</summary>
		///
		/// This operation does not create one on-demand, use WaitQueue for that.
		/// A positive result means no threads are waiting on this allocation, it does
		/// not mean the wait queue as an object does not exist. In other words, an
		/// empty wait queue is regarded the same as none at all.
		///
		/// <returns>True iff at least one thread is waiting on the specified allocation.</returns>
		public bool HasWaitQueue() {

			return m_waitQueue != null && m_waitQueue.Count > 0;
		}

		/// <summary>Get the wait queue for an allocation.</summary>
		///
		/// <returns>The associated wait queue, or a new (empty) one if none was</returns>
		/// 		previously defined.
		public Queue<int> WaitQueue {

			get {
				if (m_waitQueue == null)
					m_waitQueue = new Queue<int>();
				m_isDirty = true;
				return m_waitQueue;
			}
		}

		/// <summary>Check if this lock has an associated ready queue. [const]</summary>
		///
		/// This operation does not create one on-demand, use ReadyQueue for that.
		/// A positive result means no threads are ready on this allocation, it does
		/// not mean the ready queue as an object does not exist. In other words, an
		/// empty ready queue is regarded the same as none at all.
		///
		/// <returns>True iff at least one thread is ready on the specified allocation.</returns>
		public bool HasReadyQueue() {

			return m_readyQueue != null && m_readyQueue.Count > 0;
		}

		/// <summary>Get the ready queue for an allocation.</summary>
		///
		/// <param name="obj">Reference to the allocation.</param>
		/// <returns>The associated ready queue, or a new (empty) one if none was</returns>
		/// 		previously defined.
		public Queue<int> ReadyQueue {

			get {
				if (m_readyQueue == null)
					m_readyQueue = new Queue<int>();
				m_isDirty = true;
				return m_readyQueue;
			}
		}

		public override int GetHashCode() {

			var retval = m_owner;
			retval ^= HashMasks.MASK2;
			retval += m_count;

			if (HasReadyQueue())
				retval ^= ArrayIntHasher.GetHashCodeEnumerable(m_readyQueue, m_readyQueue.Count);				

			retval ^= HashMasks.MASK3;

			if (HasWaitQueue())
				retval ^= ArrayIntHasher.GetHashCodeEnumerable(m_waitQueue, m_waitQueue.Count);
			
			return retval;
		}

		public override bool Equals(object other) {

			var o = other as Lock;
			var equal = o != null &&
				o.Owner == Owner &&
				o.Count == Count &&
				o.HasReadyQueue() == HasReadyQueue() &&
				o.HasWaitQueue() == HasWaitQueue();
			if (equal && HasReadyQueue()) {
				var o1 = ReadyQueue.ToArray();
				var o2 = o.ReadyQueue.ToArray();
				equal = o1.Length == o2.Length;
				for (var i = 0; equal && i < o1.Length; ++i)
					equal = o1[i] == o2[i];
			}
			if (equal && HasWaitQueue()) {
				var o1 = WaitQueue.ToArray();
				var o2 = o.WaitQueue.ToArray();
				equal = o1.Length == o2.Length;
				for (var i = 0; equal && i < o1.Length; ++i)
					equal = o1[i] == o2[i];
			}
			return equal;
		}

		public bool ReadOnly {

			get { return m_isReadonly; }
			set {
				if (m_isReadonly)
					throw new System.InvalidOperationException("Lock is read-only.");
				m_isReadonly = value;
			}
		}

		/// <summary>Create a deep copy of this lock.</summary>
		///
		/// <returns>A clone.</returns>
		public IStorable StorageCopy() {

			var rdy = (HasReadyQueue() ? new Queue<int>(ReadyQueue) : null);
			var wait = (HasWaitQueue() ? new Queue<int>(WaitQueue) : null);

			return new Lock(m_owner, m_count, rdy, wait);
		}

		public override string ToString() {

			return string.Format("thread {0}, {1} times", m_owner, m_count);
		}

		public void Clean() {

			m_isDirty = false;
		}

		public bool IsDirty() {

			return m_isDirty;
		}

		/// <summary>Create a new lock.</summary>
		///
		/// By default, the lock count is zero, and no one owns the thread.
		/// Both the ready and wait queues are empty.
		public Lock() : this(LockManager.NoThread, 0, null, null) { }

		/// <summary>Create a new lock.</summary>
		///
		/// <param name="onwer">Owner of the lock.</param>
		/// <param name="count">The amount of locks the owner has.</param>
		/// <param name="rdy">Ready queue (keeps reference, not values).</param>
		/// <param name="wait">Wait queue (keeps reference, not values).</param>
		protected Lock(int owner, int count, Queue<int> rdy, Queue<int> wait) {

			m_owner = owner;
			m_count = count;
			m_readyQueue = rdy;
			m_waitQueue = wait;
			m_isDirty = true;
		}
	}
}
