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

namespace MMC.Collections {

	//	using C5;
	using System.Text;
	using System.Collections.Generic;

	/// Just an intarray wrapped around and class
	sealed class WrappedIntArray {
		int[] m_arr;

		public WrappedIntArray(WrappedIntArray wia, int length)
			: this(length) {
			int traversalSize = System.Math.Min(length, wia.Length);
			System.Array.Copy(wia.m_arr, m_arr, traversalSize);
		}

		public WrappedIntArray(int size) {
			m_arr = new int[size];
		}

		public int this[int index] {
			get { return m_arr[index]; }

			set {m_arr[index] = value; }
		}

		public int Length {
			get { return m_arr.Length; }
		}

		public WrappedIntArray Clone() {
			return new WrappedIntArray(this, Length);
		}

		public override bool Equals(object obj) {
			WrappedIntArray other = obj as WrappedIntArray;
			bool retval = MMC.Util.IntArrayHashHelper.CompareIntArrays(m_arr, other.m_arr) == 0;
			return retval;
		}

		public override int GetHashCode() {
			return ArrayIntHasher.GetHashCodeIntArray(m_arr);
		}
	}
}
