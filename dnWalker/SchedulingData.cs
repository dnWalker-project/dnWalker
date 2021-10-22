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

using System.Linq;

namespace MMC.State
{
    using System.Collections.Generic;
    using MMC.Util;
    using MMC.Data;
    using TypeDefinition = dnlib.DotNet.TypeDef;

    /// <summary>
    /// A memory access is composed of a memory location and the thread that accesses it
    /// </summary>
    public struct MemoryAccess
    {
        public MemoryAccess(MemoryLocation ml, int thread)
        {
            MemoryLocation = ml;
            ThreadId = thread;
        }

        public bool IsThreadLocal
        {
            get { return MemoryLocation.Type == MemoryLocation.Null.Type; }
        }

        public MemoryLocation MemoryLocation { get; }

        public int ThreadId { get; }

        public override string ToString()
        {
            return ThreadId + " " + MemoryLocation;
        }

        public MemoryAccess Clone()
        {
            return new MemoryAccess(MemoryLocation.Clone(), ThreadId);
        }
    }

    /*
	 * It is important here that equals works here when we are talking about the same memory location
	 */

    public struct MemoryLocation
    {
        /// 2 is a locking object
        /// 1 is a static area object
        /// 0 is a heap object
        int m_type;
        int m_location;
        int m_offset;

        private readonly ExplicitActiveState cur;

        public static readonly MemoryLocation Null = new MemoryLocation(int.MaxValue, 0, 0, null);

        /*
		 * Memory location is a locking object
		 */
        public MemoryLocation(ObjectReference or, ExplicitActiveState cur)
            : this(2, (int)or.Location - 1, 0, cur)
        {
        }

        /*
		 * Memory location in the static area
		 */
        public MemoryLocation(int offset, TypeDefinition type, ExplicitActiveState cur)
            : this(1, cur.StaticArea.GetClassLocation(type), offset, cur)
        {
        }

        /*
		 * Memory location on the heap
		 */
        public MemoryLocation(int offset, ObjectReference or, ExplicitActiveState cur)
            : this(0, (int)or.Location - 1, offset, cur)
        {
        }

        public MemoryLocation(int type, int loc, int offset, ExplicitActiveState cur)
        {
            this.m_type = type;
            this.m_location = loc;
            this.m_offset = offset;
            this.cur = cur;
        }

        public int Type { get { return m_type; } }
        public int Location { get { return m_location; } }
        public int Offset { get { return m_offset; } }

        public override bool Equals(object obj)
        {
            var other = (MemoryLocation)obj;
            return m_type == other.m_type && m_location == other.m_location && m_offset == other.m_offset;
        }

        public override string ToString()
        {
            if (m_type == 0)
            {
                return "[Alloc(" + (m_location + 1) + ")." + m_offset + "]";
            }
            else if (m_type == 1)
            {
                var iac = cur.StaticArea.Classes[m_location];
                var type = iac.Type;
                return "[" + type.DeclaringType.Name + "." + m_offset + "]";
            }
            else if (m_type == 2)
            {
                return "[Lock Alloc(" + (m_location + 1) + ")]";
            }
            else
            {
                return "[null]";
            }
        }

        public override int GetHashCode()
        {
            return MMC.Collections.SingleIntHasher.Hash(m_type) ^ MMC.Collections.SingleIntHasher.Hash(m_location) ^ MMC.Collections.SingleIntHasher.Hash(m_offset);
        }

        public MemoryLocation Clone()
        {
            return new MemoryLocation(m_type, m_location, m_location, cur);
        }
    }

    // Should be a normal struct, but those are not nullable.
    public class SchedulingData
    {
        private readonly Queue<int> m_emptyQueue = new Queue<int>(0);

        public ISII SII { get; set; }

        public ISparseElement AllocAtttributes { get; set; }

        public MemoryAccess LastAccess { get; set; }

        public CollapsedStateDelta Delta { get; set; }

        public CollapsedState State { get; set; }

        public Queue<int> Enabled { get; set; }/* => m_enabled;/*
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }*/

        public Queue<int> Working { get; set; }

        public Queue<int> Done { get; set; }/* => m_done;/*
        {
            get { return m_done; }
            set { m_done = value; }
        }*/

        public int ID { get; set; }

        public int Dequeue()
        {
            var retval = Working.Dequeue();
            Done.Enqueue(retval);
            return retval;
        }

        public void Enqueue(int threadId)
        {
            if (!Working.Contains(threadId) && !Done.Contains(threadId))
                Working.Enqueue(threadId);
        }

        /* 
		 * TODO: make a new ToString() */
        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendFormat("Backtrack ID:       {0}\t", ID);
            //            sb.AppendFormat("Last chosen thread: {0}\t", this.m_lastChosenThread);
            //            sb.AppendFormat("Threads to run:     {0}\t\t", new ListToString(m_threadsToRun.ToArray()));
            //            sb.AppendFormat("- Delta:\n{0}\n", m_delta.ToString());
            return sb.ToString();
        }

        public SchedulingData()
        {
            Done = m_emptyQueue;
            Working = m_emptyQueue;
            Enabled = m_emptyQueue;
        }

        public SchedulingData Clone()
        {
            return new SchedulingData
            {
                AllocAtttributes = AllocAtttributes?.Clone(),
                ID = ID,
                Enabled = Enabled != null ? new Queue<int>(Enabled) : null,
                Delta = Delta.Clone(),
                Done = Done != null ? new Queue<int>(Done) : null,
                LastAccess = LastAccess.Clone(),
                State = State?.Clone(),
                Working = Working != null ? new Queue<int>(Working) : null,
                SII = SII
            };
        }
    }
}