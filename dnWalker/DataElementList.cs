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

namespace MMC.Data {

	using System.Diagnostics;
	using MMC.State;
	using MMC.Util;
	using MMC.Collections;

	class DataElementList : IDataElementContainer {

		protected IDataElement[] m_elements;
		protected bool m_isDirty;
		protected bool m_isReadonly;

		public virtual IDataElement this[int index] {

			get { return m_elements[index]; }

			set {
				Debug.Assert(!m_isReadonly, "Changing read-only data element list.");
				m_isDirty |= m_elements[index] != value;

				IDataElement oldVal = m_elements[index];
				m_elements[index] = value;
			}
		}

		public int Length {

			get { return m_elements.Length; }
		}

		public bool ReadOnly {

			get { return m_isReadonly; }
			set {
				Debug.Assert(!m_isReadonly, "Changing read-only data element list.");
				m_isReadonly = value;
			}
		}

		public bool IsDirty() {

			return m_isDirty;
		}

		public virtual void Dispose() {

		}

		public void Clean() {

			m_isDirty = false;
		}

		public virtual IStorable StorageCopy() {

			IDataElement[] retval_elements = new IDataElement[m_elements.Length];
			System.Array.Copy(m_elements, retval_elements, retval_elements.Length);
			return new DataElementList(retval_elements);
		}


		public override string ToString() {
			return new ListToString(m_elements);
		}


		public override int GetHashCode() {
			return ArrayIntHasher.GetHashCodeDataElementContainer(this, this.Length);
		}

		public override bool Equals(object other) {

			DataElementList o = other as DataElementList;
			bool equal = o != null && Length == o.Length;
			for (int i = 0; equal && i < m_elements.Length; ++i)
				equal = o[i].Equals(m_elements[i]);

			return equal;
		}

		public DataElementList(int length) {

			m_elements = new IDataElement[length];
			m_isDirty = true;
			m_isReadonly = false;
		}

		protected DataElementList(IDataElement[] elements) {

			m_elements = elements;
			m_isDirty = true;
			m_isReadonly = false;
		}
	}

}
