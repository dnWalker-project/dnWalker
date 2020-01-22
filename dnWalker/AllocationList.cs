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

	using System;
	using System.Collections;
	using System.Diagnostics;
	using MMC.Collections;
	using MMC.Data;
	using MMC.Util;

    /// Data structure to store the object heap, using a (generated) list structure.
    public sealed class AllocationList : SparseReferenceList<DynamicAllocation>, IEnumerable, ICleanable {

		const int INITIAL_SIZE = 64;
		DirtyList m_dirtyLocs;

		/// \brief Indexer keeping track of changes.
		///
		/// \param index Offset in the linear heap.
		/// \return	The Allocation at the specified offset.
		public override DynamicAllocation this[int index] {

			get { return base[index]; }
			set {
				if (base[index] != value)
					m_dirtyLocs.SetDirty(index);

				base[index] = value;
			}
		}

		/// Syntactic sugar for the offset-based indexer.
		public DynamicAllocation this[ObjectReference obj] {

			get { return base[(int)obj.Location - 1]; }
			set { this[(int)obj.Location - 1] = value; }
		}

		///	\brief Location that have changed.
		///	Either by pointing to a diffent allocation (changed reference), or
		///	the allocation pointed to is changed (changed value).
		public DirtyList DirtyLocations {

			get { return m_dirtyLocs; }
		}

		/// Enumerator for all allocations.
		public IEnumerator GetEnumerator() {

			return new AllocationListEnumerator(this);
		}

		/// \brief Check if this allocation list is dirty.
		///
		/// \return True iff at least one allocation has changed.
		/// \sa DirtyLocations
		public bool IsDirty() {

			return m_dirtyLocs.Count > 0;
		}

		/// Set all allocations to clean.
		public void Clean() {

			//m_dirtyLocs = new DirtyList(Length);
			m_dirtyLocs.Clean();
		}

		/// \brief Constructor for an empty allocation list.
		///
		/// Create a SparseAllocationList as a base class, using a pre-set
		/// initial size.
		public AllocationList()
			: base(INITIAL_SIZE) {

			m_dirtyLocs = new DirtyList(INITIAL_SIZE);
		}

		/// Enumerator for the allocation list.
		private class AllocationListEnumerator : IEnumerator {

			AllocationList m_al;
			int m_cur;

			/// \brief Current object in iteration.
			///
			/// \return Object at the cursor, not null.
			public object Current {

				get { return m_al[m_cur]; }
			}

			/// \brief Move iteration to next allocation, skipping gaps.
			///
			/// \return True if a new object has been found, false otherwise
			/// 		(end of iteration).
			public bool MoveNext() {

				bool retval = false;
				for (++m_cur; !retval && m_cur < m_al.Length; ++m_cur)
					retval = m_al[m_cur] != null;
				if (retval)
					--m_cur;
				return retval;
			}

			/// Reset the iterator to the begin.
			public void Reset() {

				m_cur = -1;
			}

			/// Constructor.
			public AllocationListEnumerator(AllocationList al) {

				m_al = al;
			}
		}
	} 
}
