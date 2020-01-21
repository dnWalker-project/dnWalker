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
    using dnlib.DotNet;
    using MMC.Data;
    using System;

    /// <summary>
    /// Types of allocations.
    /// </summary>
    /// <remarks>VY: I believe that in the end, there should only be two kinds of allocations, namely objects and classes</remarks>
    internal enum AllocationType
    {
        Object,
        Array,
        Delegate, // TODO: this one should be refactored away as delegates are actualy objects
        Class,
        Unknown
    }

    /// <summary>
    /// An abstract base class for all allocations.
    /// </summary>
    internal abstract class Allocation : IStorageVisitable, ICleanable, IMustDispose
    {
        /// <summary>
        /// Print at most this many fields.
        /// </summary>
        public const int max_printed_fields = 4;

        private Lock m_lock;

        protected readonly IConfig _config;

        /// <summary>
        /// A self-describing property. Should be overridden.
        /// </summary>
        public virtual AllocationType AllocationType
        {
            get { return AllocationType.Unknown; }
        }

        /// <summary>
        /// Returns the amount of fields/elements in this allocation
        /// </summary>
        public abstract int InnerSize
        {
            get;
        }

        /// <summary>
        /// The type of the allocations (check subclass for semantics).
        /// </summary>
        public ITypeDefOrRef Type { get; }

        /// <summary>
        /// The lock associated with the allocation.
        /// </summary>
        public Lock Lock
        {
            get
            {
                if (m_lock == null)
                {
                    m_lock = new Lock();
                }

                return m_lock;
            }
            set
            {
                m_lock = value;
            }
        }

        /// <summary>
        /// True iff this allocation is locked.
        /// </summary>
        public bool Locked
        {
            get { return m_lock != null && m_lock.Count > 0; }
        }

        public abstract bool IsDirty();
        public abstract void Clean();
        public abstract void Dispose();
        public abstract void Accept(IStorageVisitor visitor);

        protected Allocation(ITypeDefOrRef typeDef, IConfig config)
        {
            Type = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
            m_lock = null;
            _config = config;
        }

        protected Allocation(IConfig config)
        {
            m_lock = null;
            _config = config;
        }
    }

    internal abstract class DynamicAllocation : Allocation
    {
        /// <summary>
        /// Name of the value field of value type wrappers.
        /// </summary>
        public const string VALUE_FIELD_NAME = "m_value";

        /*
		 * Used for Mark & Sweep GC and POR using object escape analysis:
		 * UNMARKED = known as unreachable from the callstacks
		 * between 0 and int.MaxValue = the thread id that can reach this object
		 * SHARED = reachable from multiple threads, i.e., thread-shared
		 */
        public const int UNMARKED = -100;
        public const int SHARED = int.MaxValue;
        private int m_refCount;
        private int m_pinCount;

        /// <summary>
        /// Do not use! Use DynamicArea::SetPinnedLocation(int loc, bool pin) instead.
        /// </summary>
        public bool Pinned
        {
            get { return m_pinCount > 0; }
            set
            {
                if (value)
                    ++m_pinCount;
                else
                    --m_pinCount;
            }
        }

        /// <summary>
        /// Reference count.
        /// </summary>
        public int RefCount
        {
            get
            {
                // Hack: always return 1 if we're not keeping track of reference counts,
                // so allocations are not always left out of the collapse procedure.
                return _config.UseRefCounting ? m_refCount : 1;
            }
            set { m_refCount = value; }
        }

        protected DynamicAllocation(ITypeDefOrRef typeDef, IConfig config) : base(typeDef, config)
        {
            m_refCount = 0;

            /// This ensures that newly created allocations are seen
            /// as inconsistent and therefore become processed by the 
            /// the MGC algorithm
            if (_config.MemoisedGC)
            {
                Rhs = int.MaxValue;
                Depth = int.MinValue;
            }

            HeapAttribute = UNMARKED;
        }

        protected DynamicAllocation(IConfig config) : base(config)
        {
            m_refCount = 0;

            /// This ensures that newly created allocations are seen
            /// as inconsistent and therefore become processed by the 
            /// the MGC algorithm
            if (_config.MemoisedGC)
            {
                Rhs = int.MaxValue;
                Depth = int.MinValue;
            }

            HeapAttribute = UNMARKED;
        }

        public bool ThreadShared
        {
            get { return HeapAttribute == SHARED; }
        }

        /// <summary>
        /// See chapter on POR using object escape analysis in VY's thesis
        /// </summary>
        public int HeapAttribute { get; set; }

        /// <summary>
        /// See chapter on Memoised GC in VY's thesis
        /// </summary>
        public int Rhs { get; set; }

        public int Depth { get; set; }

        public int Key
        {
            get { return Math.Min(Depth, Rhs); }
        }
    }
}