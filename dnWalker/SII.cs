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

using System;
using System.Collections.Generic;
using System.Text;
using MMC.Data;


namespace MMC {

	using System.Text;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using MMC.State;
	using MMC.Util;
	using MMC.InstructionExec;
	using MMC.Collections;
	
	/// A full blown ObjectSII represents the acceses to an object
	///
	/// A set of threads is represented by an integer, where the
	/// bit-index represents a thread identifier. This means that 
	/// the max. num of thread is 32. This suffices for now, as verifying
	/// models with more threads is likely going out of memory very quickly
	class ObjectSII {
		protected int[] m_fields;
		int sum = 0; // additional test for faster equality

		public static readonly ObjectSII Empty = new ObjectSII(0);

		public ObjectSII(int maxOffset) {
			m_fields = new int[maxOffset];
		}

		public int Length {
			get { return m_fields.Length; }
		}

		public int this[int index] {
			get {
				/*
				 * Could be possible that length does not match with that is on the heap.
				 * happens when we grab a semantically equivalent smaller array from the collapsing pool
				 * (also see the Equals method in this class)
				 */ 
				if (index < m_fields.Length)
					return m_fields[index];
				else
					return CollectionConstants.NotSet;
			}
			set {
				/*
				 * Could be possible that length does not match with that is on the heap.
				 * happens when we grab a semantictally equivalent smaller array from the collapsing pool 
				 */ 
				if (index >= m_fields.Length) 
					m_fields = IntArray.GrowArray(m_fields, index);
				
				int oldVal = m_fields[index];
				m_fields[index] = value;
				sum = sum - oldVal + value;
			}
		}

		public override bool Equals(object obj) {
			if (obj != null) {
				ObjectSII otherOsii = obj as ObjectSII;

				if ((sum | otherOsii.sum) == 0)
					/*
					 * If the sums are zero, then all values are initialised to 0, meaning
					 * no threads accessed this object. For sake of equality, distinguishing between
					 * them does not matter 
					 */
					return true;
				else {
					bool equal = sum == otherOsii.sum;

					int[] smallest;
					int[] biggest;

					if (m_fields.Length < otherOsii.m_fields.Length) {
						smallest = m_fields;
						biggest = otherOsii.m_fields;
					} else {
						biggest = m_fields;
						smallest = otherOsii.m_fields;
					}

					for (int i = 0; equal && i < smallest.Length; i++)
						equal = smallest[i] == biggest[i];

					for (int i = smallest.Length; equal && i < biggest.Length; i++)
						equal = biggest[i] == 0;

					return equal;
				}
			} else
				return false;
		}

		public override int GetHashCode() {
			return ArrayIntHasher.GetHashCodeIntArray(m_fields);
		}
	}

    public interface ISII {
		IEnumerator<MemoryAccess> GetEnumerator();
	}
	
	
	/// Collapsed SII, see the chapter on Collapsing Interleaving Information
	/// in VY's thesis
	class CollapsedSII : ISII {
		readonly internal ISparseElement[] m_list;

		public CollapsedSII(SimplifiedSII sii) {
			m_list = new ISparseElement[sii.m_accesses.Length];
			

			for (int k = 0; k < sii.m_accesses.Length; k++) {
				ObjectSII[] currOther = sii.m_accesses[k];
				ISparseElement curr = m_list[k];
				for (int i = 0; i < currOther.Length; i++) {
					ObjectSII otherOsii = currOther[i];
					if (otherOsii != null)
						curr = new SparseElement(i, Collapser.POOL_DATA.GetInt(otherOsii), curr);
				}

				m_list[k] = curr;
			}
		}

		public IEnumerator<MemoryAccess> GetEnumerator() {
			for (int i = 0; i < m_list.Length; i++) {

				for (ISparseElement curr = m_list[i]; curr != null; curr = curr.Next) {
					ObjectSII osii = Collapser.POOL_DATA.GetObjectSII(curr.DeltaVal);
					for (int k = 0; k < osii.Length; k++) {
						int threadset = osii[k];
						if (threadset != 0) {
							for (int threadId = 0; threadId < 32; threadId++) {
								int mask = (1 << threadId);
								if ((threadset & mask) == mask) {
									MemoryLocation hml = new MemoryLocation(i, curr.Index, k);
									yield return new MemoryAccess(hml, threadId);
								}
							}
						}
					}
				}
			}
		}
	}

	/// A simplifiedSII is a summarised II for a given state
	class SimplifiedSII : ISII {

		/*
		 * internal access flag allows the collapsedSII to access these fileds */
		internal readonly ObjectSII[][] m_accesses;


		public SimplifiedSII() {
			m_accesses = new ObjectSII[3][];
			m_accesses[0] = new ObjectSII[ActiveState.cur.DynamicArea.Allocations.Length];
			m_accesses[1] = new ObjectSII[ActiveState.cur.StaticArea.Classes.Length];
			m_accesses[2] = new ObjectSII[ActiveState.cur.DynamicArea.Allocations.Length];
		}

		/// Returns the size (in terms of fields) of a given memory location
		private int GetOffsetSize(int type, int loc) {
			if (type == 0) {
				DynamicAllocation da = ActiveState.cur.DynamicArea.Allocations[loc];
				return (da as Allocation).InnerSize;
			} else if (type == 2) {
				return 1;
			} else {
				return ActiveState.cur.StaticArea.Classes[loc].Fields.Length;
			}
		}

		private ObjectSII GetOrCreate(int type, int loc) {
			ObjectSII retval = m_accesses[type][loc];

			if (retval == null) {
				retval = new ObjectSII(GetOffsetSize(type, loc)); // clone it 
				m_accesses[type][loc] = retval;
			}

			return retval;
		}

		/// Checks whether a memory location truly exists is the ActiveState
		///
		/// This method should be moved to ActiveState class
		private bool Exists(MemoryAccess ma) {
			int type = ma.MemoryLocation.Type;
			int loc = ma.MemoryLocation.Location;
			int offset = ma.MemoryLocation.Offset;

			if (type == 0) {
				DynamicAllocation da = ActiveState.cur.DynamicArea.Allocations[loc];
				return da != null;
			} else if (type == 2) {
				DynamicAllocation da = ActiveState.cur.DynamicArea.Allocations[loc];
				return da != null;
			} else {
				return true;
//				throw new System.InvalidProgramException("not implemented stateful dynamic POR Exists for static memory accesses yes");
			}
		}

		/// Merges a memory access with this SII
		private void Merge(MemoryAccess ma) {
			int type = ma.MemoryLocation.Type;
			int loc = ma.MemoryLocation.Location;
			int offset = ma.MemoryLocation.Offset;

			/*
			 * The access is outside the current memory range,
			 * no use to merge it, 
			 * 
			 * or the access is on an object that does not exist
			 * in the current state, there is no need to merge it
			 */ 
			if (loc >= m_accesses[type].Length || !Exists(ma))
				return;

			ObjectSII osii = GetOrCreate(type, loc);
			osii[offset] |= (1 << ma.ThreadId);
		}

		/// Merges this SII with an other, with exception of orAccess
		///
		/// More on this in VY's thesis
		public void Merge(ISII other, MemoryAccess orAccess) {
			foreach (MemoryAccess ma in other) {
				if (!ma.MemoryLocation.Equals(orAccess.MemoryLocation)) 
					this.Merge(ma);
			}

			if (!orAccess.IsThreadLocal)
				this.Merge(orAccess);
		}


		/*
		 * Oh well, the following could be programmed nicer, but this works 
		 */
		public IEnumerator<MemoryAccess> GetEnumerator() {
			for (int i = 0; i < m_accesses.Length; i++) {
				ObjectSII[] curr = m_accesses[i];

				for (int j = 0; j < curr.Length; j++) {
					ObjectSII osii = curr[j];

					if (osii != null) {
						for (int k = 0; k < osii.Length; k++) {
							int threadset = osii[k];
							if (threadset != 0) {
								for (int threadId = 0; threadId < 32; threadId++) {
									int mask = (1 << threadId);
									if ((threadset & mask) == mask) {
										MemoryLocation hml = new MemoryLocation(i, j, k);
										yield return new MemoryAccess(hml, threadId);
									}
								}
							}
						}
					}
				}
			}
		}

		public override string ToString() {
			String result = "";
			foreach (MemoryAccess ma in this) {
				result += String.Format("{0}, <{1}, {2}, {3}>\n", ma.ThreadId, ma.MemoryLocation.Type, ma.MemoryLocation.Location, ma.MemoryLocation.Offset);
			}

			return result;
		}
	}
}
