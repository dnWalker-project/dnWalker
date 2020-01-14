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

    using dnlib.DotNet;
    using MMC.Data;
	using MMC.Util;
	using System;
	
		
	/// VY: I believe that in the end, there should only be two kinds of 
	/// allocations, namely objects and classes

	/// Types of allocations.
	enum AllocationType : int {

		Object,
		Array, 
		Delegate, // TODO: this one should be refactored away as delegates are actualy objects
		Class,
		Unknown
	}

    /// An abstract base class for all allocations.
    abstract class Allocation : IStorageVisitable, ICleanable, IMustDispose
    {

        /// Print at most this many fields.
        public const int max_printed_fields = 4;

        private ITypeDefOrRef m_typeDef;
        Lock m_lock;


        /// A self-describing property. Should be overridden.
        public virtual AllocationType AllocationType
        {

            get { return AllocationType.Unknown; }
        }

        /// Returns the amount of fields/elements in this allocation
        public abstract int InnerSize
        {
            get;
        }

        /// The type of the allocations (check subclass for semantics).
        public ITypeDefOrRef Type
        {
            get { return m_typeDef; }
        }

        /// The lock associated with the allocation.
        public Lock Lock
        {

            get
            {
                if (m_lock == null)
                    m_lock = new Lock();
                return m_lock;
            }
            set
            {
                m_lock = value;
            }
        }

        /// True iff this allocation is locked.
        public bool Locked
        {
            get { return m_lock != null && m_lock.Count > 0; }
        }

        public abstract bool IsDirty();
        public abstract void Clean();
        public abstract void Dispose();
        public abstract void Accept(IStorageVisitor visitor);

        public Allocation(ITypeDefOrRef typeDef)
        {
            m_typeDef = typeDef ?? throw new ArgumentNullException(nameof(typeDef));
            m_lock = null;
        }

        protected Allocation()
        {
            m_lock = null;
        }
    }

	abstract class DynamicAllocation : Allocation {

		/// Name of the value field of value type wrappers.
		public const string VALUE_FIELD_NAME = "m_value";
		
		/*
		 * Used for Mark & Sweep GC and POR using object escape analysis:
		 * UNMARKED = known as unreachable from the callstacks
		 * between 0 and int.MaxValue = the thread id that can reach this object
		 * SHARED = reachable from multiple threads, i.e., thread-shared
		 */ 
		public const int UNMARKED = -100;
		public const int SHARED = int.MaxValue;
		int m_heapAttr;
		
		int m_refCount;
		int m_pinCount;

		// For memoised GC
		int m_depth;
		int m_rhs;

		

		/// Do not use! Use DynamicArea::SetPinnedLocation(int loc, bool pin) instead.
		public bool Pinned {

			get { return m_pinCount > 0; }
			set {
				if (value)
					++m_pinCount;
				else
					--m_pinCount;
			}
		}

		/// Reference count.
		
		public int RefCount {

			get {
				// Hack: always return 1 if we're not keeping track of reference counts,
				// so allocations are not always left out of the collapse procedure.
				return (Config.UseRefCounting ? m_refCount : 1);
			}
			set { m_refCount = value; }
		}

		public DynamicAllocation(ITypeDefOrRef typeDef) : base(typeDef)
        {
			m_refCount = 0;

			/// This ensures that newly created allocations are seen
			/// as inconsistent and therefore become processed by the 
			/// the MGC algorithm
			if (Config.MemoisedGC) {
				m_rhs = int.MaxValue;
				m_depth = int.MinValue;
			}

			m_heapAttr = UNMARKED;
		}

        protected DynamicAllocation() : base()
        {
            m_refCount = 0;

            /// This ensures that newly created allocations are seen
            /// as inconsistent and therefore become processed by the 
            /// the MGC algorithm
            if (Config.MemoisedGC)
            {
                m_rhs = int.MaxValue;
                m_depth = int.MinValue;
            }

            m_heapAttr = UNMARKED;
        }

        public bool ThreadShared {
			get { return m_heapAttr == SHARED; }
		}

		/// See chapter on POR using object escape analysis in VY's thesis
		public int HeapAttribute {
			get { return m_heapAttr; }
			set { m_heapAttr = value; }
		}
		
		/// See chapter on Memoised GC in VY's thesis
		public int Rhs {
			get { return m_rhs; }
			set { m_rhs = value; }
		}

		public int Depth {
			get { return m_depth; }
			set { m_depth = value; }
		}

		public int Key {
			get { return Math.Min(m_depth, m_rhs); }
		}
	}
}
