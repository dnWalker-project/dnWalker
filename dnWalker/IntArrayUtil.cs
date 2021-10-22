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

	using System.Collections;
	using System.Diagnostics;
	using System.Text;
	using MMC.Data;
	using MMC.Collections;

	class IntArrayHashHelper : IComparer, IHashCodeProvider {

		public int GetHashCode(object o) {
			return ArrayIntHasher.GetHashCodeIntArray((int[])o);
		}

		public new bool Equals(object a, object b) {

			return Compare(a, b) == 0;
		}

		public int Compare(object a, object b) {

			var ll = (int[])a;
			var kk = (int[])b;
			return CompareIntArrays(ll, kk);
		}

		public static int CompareIntArrays(int[] ll, int[] kk) {

			var retval = ll.Length - kk.Length;
			for (var i = 0; retval == 0 && i < ll.Length; i++)
				retval = ll[i] - kk[i];
			return retval;
		}
	}

	class IntArray {

		public static readonly int[] Empty = new int[0] { };
		/*
		public static int[] Create(int length, int initial_value) {

			int[] retval = new int[length];
			FillFrom(retval, 0, initial_value);
			return retval;
		}

		public static int[] Clone(int[] arr) {

			int[] retval = new int[arr.Length];
			System.Array.Copy(arr, 0, retval, 0, arr.Length);
			return retval;
		}*/

		/*
		public static void FillFrom(int[] arr, int index, int val) {

			for (; index < arr.Length; ++index)
				arr.SetValue(val, index);
		}*/
		/*
		public static int[] GetRange(int[] source, int start_index, int length) {

			if (length < 0)
				throw new System.ArgumentOutOfRangeException("length");
			if (start_index < 0 || start_index >= source.Length)
				throw new System.ArgumentOutOfRangeException("start_index");
			int[] retval = new int[length];
			System.Array.Copy(source, start_index, retval, 0, length);
			return retval;
		}

		public static int[] Expand(int[] source, int length, int new_val) {

			if (length < source.Length)
				throw new System.ArgumentException("length < source.Length");
			int[] retval = new int[length];
			System.Array.Copy(source, 0, retval, 0, source.Length);
			FillFrom(retval, source.Length, new_val);
			return retval;
		}*/

		public static int[] GrowArray(int[] arr, int min_length) {

			var newlength = (arr.Length > 0 ? arr.Length : 1);
			while (newlength <= min_length)
				newlength *= 2;
			var retval = new int[newlength];
			System.Array.Copy(arr, 0, retval, 0, arr.Length);
			//FillFrom(retval, arr.Length, new_val);
			return retval;
		}
	}
}
