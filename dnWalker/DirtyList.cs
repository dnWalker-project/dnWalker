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

	using System.Collections.Generic;
	using MMC.Data;

	class DirtyList : ICleanable { 

		const int DEFAULT_INIT_SIZE = 128;

		DynamicBitArray m_isDirty;
		Queue<int>  m_dirtyStuff;

		public int Count {

			get { return m_dirtyStuff.Count; }
		}
		
		public void SetDirty(int index) {

			if (!m_isDirty[index]) {
				m_isDirty[index] = true;
				m_dirtyStuff.Enqueue(index);
			}
		}

		public bool IsDirty() {

			return m_dirtyStuff.Count > 0;
		}

		public void Clean() {

			m_isDirty.SetAll(false);
			m_dirtyStuff.Clear();
		}

		public override string ToString() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("{");
			foreach (int i in this) {
				if (sb.Length > 1)
					sb.Append(", ");
				sb.Append(i);
			}
			sb.Append("}");
			return sb.ToString();
		}
		
		public DirtyList(int initialSize) {
			
			m_isDirty = new DynamicBitArray(initialSize);
			m_dirtyStuff = new Queue<int>(initialSize);
		}

		public DirtyList() : this(DEFAULT_INIT_SIZE) {}

		public IEnumerator<int> GetEnumerator() {

			return m_dirtyStuff.GetEnumerator();
		}
	}
}
