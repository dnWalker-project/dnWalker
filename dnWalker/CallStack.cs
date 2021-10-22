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

	using System.Collections.Generic;
	using System.Diagnostics;
	using MMC.Collections;
	using MMC.Util;
	using MMC.Data;



    public class CallStack : SparseReferenceList<MethodState>, IMustDispose, ICleanable {

		int m_stackptr = 0;
		bool m_isDirty = true;

		public bool IsEmpty() {

			return m_stackptr == 0;
		}

		public override MethodState this[int index] {

			get { return base[index]; }
			set {
				var old_value = base[index];
				if (old_value != value) {
					m_isDirty = true;
					if (old_value != null)
						old_value.Dispose();
				}
				base[index] = value;
			}
		}

		/// <summary>Stack pointer is always one above last value, i.e. the place to write to.</summary>
		public int StackPointer {

			get { return m_stackptr; }
			set { 
				if (value < 0)
					throw new System.ArgumentOutOfRangeException("StackPointer < 0");
				m_isDirty |= m_stackptr != value;
				while (value < m_stackptr)
					Pop();
				m_stackptr = value;
			}
		}

		public DirtyList DirtyFrames {

			get {
				var retval = new DirtyList(m_stackptr);
				for (var i = m_stackptr-1; i >= 0 && this[i].IsDirty(); --i)
					retval.SetDirty(i);
				return retval;
			}
		}

		public void Push(MethodState s) {

			base[m_stackptr++] = s;
		}

		public MethodState Pop() {
			--m_stackptr;
			var retval = base[m_stackptr];
			Debug.Assert(retval != null, "popped a null from the callstack!");
			retval.Dispose();
			m_isDirty = true;

			return retval;
		}

		public MethodState Peek() {
			return base[m_stackptr-1];
		}

		public void Dispose() {

			StackPointer = 0;
		}

		public bool IsDirty() {

			var retval = m_isDirty;
			if (!retval && m_stackptr > 0)
				retval = base[m_stackptr-1].IsDirty();
			return retval;
		}

		public void Clean() {

			m_isDirty = false;
			for (var i = m_stackptr-1; i >= 0 && this[i].IsDirty(); --i)
				this[i].Clean();
		}

		public IEnumerator<MethodState> GetEnumerator() {
			for (var i = m_stackptr - 1; i >= 0; i--)
				yield return this[i];
		}

		public CallStack() : base() { }
		public CallStack(ICollection<MethodState> col) : base(col) {

			StackPointer = col.Count;
		}
	}
}
