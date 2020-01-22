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

	// This implementation of a placement strategy to find and avoid symmetric
	// heaps is taken from the JPF2. It is not 100% the same as the algorithm
	// described in the articles by the JPF writers.
	//
	// The difference lies in the fact that we're not really keeping track of
	// an occurence count for each of the (CIL) instruction that allocate an
	// object on the heap. Instead, we "try" the occurence number, which isn't
	// really an occurence count but rather just some counter unrelated to the
	// number of times an instruction is executed. We do this by starting at
	// zero, and then increment the counter as needed.
	//
	// This way we'll get an ordered list of keys, i.e. for some fixed thread t
	// and instruction t, we'll get (t,i,0), (t,i,1), and so on. Each key maps
	// to a unique offset in the heap. If an offset is taken by another
	// allocation, the next key is tried. Else, the offset is returned and
	// used.
	//
	// So far we haven't throught of an example where this fake "occurence"
	// count is a problem.
	//
	// We have, however, ran into problems using real occurence counts. The
	// main problem is: occurence counts are part of the state, and thus if
	// they change, the state is also changed. Effectively, this means we
	// cannot include the occurence counts in state comparison without
	// compromising the entire goal of symmetry reduction: reducting the number
	// of unique states. If we exclude the occurence counts from the state, we
	// get basically the same thing as we implemented here, and what was
	// implemented in the JPF, i.e. take the first free slot in the mapping.

	struct PlacementMappingKey {

		public int ThreadId;
		public MMC.Util.CILLocation LineNumber;
		public int Occurence;

		// TODO use better hashing
		public override int GetHashCode() {

			int retval = ThreadId;
			retval ^= MMC.HashMasks.MASK1;
			retval += LineNumber.GetHashCode();
			retval ^= MMC.HashMasks.MASK4;
			retval += Occurence;
			return retval;
		}

		public override bool Equals(object other) {

			PlacementMappingKey o = (PlacementMappingKey)other;
			return o.ThreadId == ThreadId && 
				o.Occurence == Occurence &&
				o.LineNumber.Equals(LineNumber);
		}

		public override string ToString() {

			return string.Format("({0}, {1}, {2})",
					LineNumber.ToString(), ThreadId, Occurence);
		}
	}

	/// Type to store the mapping used in the symmetry reduction.
	interface IPlacementMapping {

		int GetLocation();
	}

	/// An implementation of the IPlacementMapping type.
	class PlacementMapping : IPlacementMapping {

		System.Collections.Hashtable m_map;

		/// Determine the position for the current allocation.
		///
		/// This queries the active state for its data. If a mapping of the
		/// current allocation taking place to a location was not found, it
		/// queries the heap for a free slot, stores it, and returns it.
		/// Otherwise, a previously stored location is returned.
		///
		/// As a side effect, the occurence count of the current instuction in
		/// the current thread is incremented.
		///
		/// \sa PlacementMappingKey
		public int GetLocation() {

			int retval = -1;
            var cur = ActiveState.cur;

			PlacementMappingKey key = new PlacementMappingKey();
			key.LineNumber = cur.CurrentLocation;
			key.ThreadId = cur.ThreadPool.CurrentThreadId;
			key.Occurence = 0;


			while (retval == -1) {
//				MonoModelChecker.Message("trying key {0}", key.ToString());
				object wrapped = m_map[key];
				if (wrapped == null) {
					// Empty mapping. Store and return.
					retval = cur.DynamicArea.FreeSlot();
//					MonoModelChecker.Message("new mapping to {0}", retval);
					m_map[key] = retval;
				}
				else {
					// Got existing mapping, see if it's free.
					retval = (int)wrapped;
//					MonoModelChecker.Message("existing mapping to {0}", retval);
					if (cur.DynamicArea.Allocations[retval] != null) {
//						MonoModelChecker.Message("ocupado. trying next.");
						retval = -1;
						key.Occurence++;
					}
				}
			}

			return retval;
		}

		/// Create a new placement mapping.
		public PlacementMapping() {

			m_map = new System.Collections.Hashtable();
		}
	}
}
