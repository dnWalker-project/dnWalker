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
            return ThreadId + " " + MemoryLocation.ToString();
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

        public static readonly MemoryLocation Null = new MemoryLocation(int.MaxValue, 0, 0);

        /*
		 * Memory location is a locking object
		 */
        public MemoryLocation(ObjectReference or)
            : this(2, (int)or.Location - 1, 0)
        {
        }

        /*
		 * Memory location in the static area
		 */
        public MemoryLocation(int offset, TypeDefinition type)
            : this(1, ActiveState.cur.StaticArea.GetClassLocation(type), offset)
        {
        }

        /*
		 * Memory location on the heap
		 */
        public MemoryLocation(int offset, ObjectReference or)
            : this(0, (int)or.Location - 1, offset)
        {
        }

        public MemoryLocation(int type, int loc, int offset)
        {
            this.m_type = type;
            this.m_location = loc;
            this.m_offset = offset;
        }

        public int Type { get { return m_type; } }
        public int Location { get { return m_location; } }
        public int Offset { get { return m_offset; } }


        public override bool Equals(object obj)
        {
            MemoryLocation other = (MemoryLocation)obj;
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
                AllocatedClass iac = ActiveState.cur.StaticArea.Classes[(int)m_location];
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
    }


    // Should be a normal struct, but those are not nullable.
    public class SchedulingData
    {

        int m_id;
        Queue<int> m_enabled;
        Queue<int> m_done;
        Queue<int> m_working;

        //CollapsedState m_delta;
        ISparseElement m_allocAttr;
        CollapsedStateDelta m_delta;
        CollapsedState m_collapsedState;

        MemoryAccess m_access;
        ISII m_sii;

        public ISII SII
        {
            get { return m_sii; }
            set { m_sii = value; }
        }

        public ISparseElement AllocAtttributes
        {
            get { return m_allocAttr; }
            set { m_allocAttr = value; }
        }

        public MemoryAccess LastAccess
        {
            get { return m_access; }
            set { m_access = value; }
        }

        public CollapsedStateDelta Delta
        {

            get { return m_delta; }
            set { m_delta = value; }
        }

        public CollapsedState State
        {

            get { return m_collapsedState; }
            set { m_collapsedState = value; }
        }

        public Queue<int> Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        public Queue<int> Working
        {
            get { return m_working; }
            set { m_working = value; }
        }

        public Queue<int> Done
        {
            get { return m_done; }
            set { m_done = value; }
        }

        public int ID
        {

            get { return m_id; }
            set { m_id = value; }
        }

        public int Dequeue()
        {
            int retval = Working.Dequeue();
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
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat("Backtrack ID:       {0}\t", m_id);
            //            sb.AppendFormat("Last chosen thread: {0}\t", this.m_lastChosenThread);
            //            sb.AppendFormat("Threads to run:     {0}\t\t", new ListToString(m_threadsToRun.ToArray()));
            //            sb.AppendFormat("- Delta:\n{0}\n", m_delta.ToString());
            return sb.ToString();
        }

    }
}