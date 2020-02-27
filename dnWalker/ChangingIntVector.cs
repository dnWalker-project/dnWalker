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

namespace MMC.Util {


	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using MMC.Collections;

    public class ChangingIntVector {

		public const int deleted = CollectionConstants.Deleted;
		public const int not_set = CollectionConstants.NotSet;
		public const int default_size = 16;

		/// The linked list of delta's 
		ISparseElement m_delta;
		int[] m_currVals;

		/// Get the maximum of both lists (use this to iterate).
		public int Length {
			get { return m_currVals.Length; }
		}

		/// <summary>Indexer for elements.</summary>
		///
		/// First, try to get the value from the new values list. If that
		/// fails, return the old value. If the value has been deleted, not_set
		/// is returned.
		public int this[int index] {

			get { return m_currVals[index]; }

			set {
				if (value != not_set && index >= m_currVals.Length)
					m_currVals = IntArray.GrowArray(m_currVals, index);

				int oldVal = m_currVals[index];

				if (oldVal == value)
					return;

				m_delta = oldVal == not_set ? new SparseElement(index, deleted, m_delta) : new SparseElement(index, oldVal, m_delta);

				m_currVals[index] = value;
			}
		}


		/// <summary>Return a list of values as returned by the indexer.</summary>
		///
		/// <returns>Old values overwritten with new values.</returns>
		public ChangingIntVector WriteBack() {

			int[] retval = new int[m_currVals.Length];
			System.Array.Copy(m_currVals, retval, m_currVals.Length);
			return new ChangingIntVector(retval);
		}


		/// <summary>Get the old values for which a new value exists.</summary>
		///
		/// <returns>The reverse delta.</returns>
		public ISparseElement GetReverseDelta() {

			return m_delta;
		}

		/// <summary>Compare two lists.</summary>
		///
		/// <param name="other">The list to compare to (should be IChangingIntVector!).</param>
		/// <returns>True iff the lists are equal.</returns>
		public override bool Equals(object other) {

			ChangingIntVector o = other as ChangingIntVector;
			bool equal = true;

			// First, check the common part.
			int i;
			int min_length = System.Math.Min(this.Length, o.Length);
			for (i = 0; equal && i < min_length; ++i)
				equal = this[i] == o[i]; // deleted values are catched in indexer.

			// Check the largest list for the rest: it should only contain
			// not_set and/or deleted values.
			ChangingIntVector largest_list = (this.Length > o.Length ? this : o);
			for ( ; equal && i < largest_list.Length; ++i)
				equal = largest_list[i] == not_set;

			return equal;
		}

		/// <summary>Get the hash code of a list.</summary>
		public override int GetHashCode() 
        {
			return ArrayIntHasher.GetHashCodeIntArray(m_currVals);	
		}

		public void ClearDelta() 
        {
			m_delta = null;
		}

		/// <summary>Get a string representation of the list.</summary>
		///
		/// The format is: 0 -> 23, 1 -> 322, etc.
		///
		/// <returns>The formatted list.</returns>
		public override string ToString() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int val;
			for (int i = 0; i < this.Length; ++i) {
				val = this[i];
				if (val != not_set)
					sb.AppendFormat("{0}{1}->{2}", (sb.Length > 0 ? ", " : ""), i, val);
			}
			return sb.ToString();
		}

		/// Create a ChangingIntVector.
		///
		/// <param name="initial_length">Initial length of the list.</param>
		public ChangingIntVector(int initial_length) {

			m_currVals = new int[initial_length];
		}

		public ChangingIntVector()
			: this(default_size) {
		}

		/// Create a ChangingIntVector.
		///
		/// The passed array is NOT copied, it is referenced. 
		///
		/// <param name="old_vals">An array containing the old values.</param>
		protected ChangingIntVector(int[] old_vals) {

			m_currVals = old_vals;
		}

	}
}