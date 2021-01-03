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
	using System.Collections;
	using MMC.Collections;
	using MMC.Data;
	using MMC.Util;

    /// <summary>
    /// Data structure to store the object heap, using a (generated) list structure.
    /// </summary>
    public sealed class AllocationList : SparseReferenceList<DynamicAllocation>, IEnumerable, ICleanable
    {
		private const int INITIAL_SIZE = 64;

        /// <summary>
        /// Indexer keeping track of changes.
        /// </summary>
        /// <param name="index">Offset in the linear heap.</param>
        /// <returns>The Allocation at the specified offset.</returns>
        public override DynamicAllocation this[int index]
        {
            get { return base[index]; }
            set
            {
                if (base[index] != value)
                {
                    DirtyLocations.SetDirty(index);
                }

                base[index] = value;
            }
        }

		/// <summary>
		/// Syntactic sugar for the offset-based indexer.
		/// </summary>
		public DynamicAllocation this[IReferenceType obj]
        {
			get { return base[(int)obj.Location - 1]; }
			set { this[(int)obj.Location - 1] = value; }
		}

        /// <summary>Location that have changed.</summary>
        /// <remarks>
        ///	Either by pointing to a diffent allocation (changed reference), or
        ///	the allocation pointed to is changed (changed value).
        ///	</remarks>
        public DirtyList DirtyLocations { get; }

        /// <summary>
        /// Enumerator for all allocations.
        /// </summary>
        public IEnumerator GetEnumerator()
        {
			return new AllocationListEnumerator(this);
		}

		/// <summary>
        /// Check if this allocation list is dirty.
        /// </summary>
		/// <returns>True iff at least one allocation has changed.</returns>
		/// \sa DirtyLocations
		public bool IsDirty()
        {
			return DirtyLocations.Count > 0;
		}

		/// <summary>
		/// Set all allocations to clean.
		/// </summary>
		public void Clean()
        {
			//m_dirtyLocs = new DirtyList(Length);
			DirtyLocations.Clean();
		}

        /// <summary>
        /// Constructor for an empty allocation list.
        /// </summary>
        /// <remarks>
        /// Create a SparseAllocationList as a base class, using a pre-set
        /// initial size.
        /// </remarks>
        public AllocationList() : base(INITIAL_SIZE)
        {
			DirtyLocations = new DirtyList(INITIAL_SIZE);
		}

		/// <summary>
		/// Enumerator for the allocation list.
		/// </summary>
		private class AllocationListEnumerator : IEnumerator
        {
			private readonly AllocationList m_al;
            private int m_cur;

			/// <summary>Current object in iteration.</summary>
			/// <returns>Object at the cursor, not null.</returns>
			public object Current
            {
				get { return m_al[m_cur]; }
			}

            /// <summary>Move iteration to next allocation, skipping gaps.</summary>
            /// <returns>True if a new object has been found, false otherwise (end of iteration).</returns>
            public bool MoveNext()
            {
				bool retval = false;
				for (++m_cur; !retval && m_cur < m_al.Length; ++m_cur)
					retval = m_al[m_cur] != null;
				if (retval)
					--m_cur;
				return retval;
			}

			/// <summary>
			/// Reset the iterator to the begin.
			/// </summary>
			public void Reset()
            {
				m_cur = -1;
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			public AllocationListEnumerator(AllocationList al)
            {
				m_al = al;
			}
		}
	}
}
