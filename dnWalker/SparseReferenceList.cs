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

	class SparseReferenceList<T> where T : class {

		const int DEFAULT_SIZE = 16;
		T[] m_data;
		int m_length;

		public virtual T this[int index] {

			get { return (index < m_length) ? m_data[index] : null; }

			set {
				if (index >= m_data.Length) {
					T[] newArr = new T[index * 2];
					System.Array.Copy(m_data, newArr, index);
					m_data = newArr;
				}

				m_data[index] = value;

				// Update length.
				if (value == null && index == m_length - 1) {
					while (m_length >= 1 && m_data[m_length - 1] == null)
						--m_length;
					// m_length will be >= 0 here.
				} else if (value != null && index >= m_length)
					m_length = index + 1;
			}
		}

		public int Add(T o) {
			this[m_length] = o;
			return m_length - 1;
		}

		/// \brief Generate a string representation of the list.
		///
		/// The format looks like: <tt>[offset -> value, offset -> value,
		/// ...]</tt>, marking deleted elements as such.
		///
		/// \return A string representation of the list.
		public override string ToString() {

			StringBuilder sb = new StringBuilder("{");
			for (int i = 0; i < Length; ++i) {
				if (this[i] != null) {
					if (sb.Length > 1)
						sb.Append(", ");
					sb.AppendFormat("({0} -> {1})", i, m_data[i].ToString());
				}
			}
			sb.Append("}");
			return sb.ToString();
		}

		/// \brief Create a shallow copy of this list.
		/// \return A shallow copy of the list.
		public SparseReferenceList<T> Clone() {

			return new SparseReferenceList<T>(this);
		}


		public SparseReferenceList(SparseReferenceList<T> s) {

			if (s != null) {
				m_length = s.Length;
				m_data = new T[s.Length];
				System.Array.Copy(s.m_data, m_data, m_length);
			} else
				m_data = new T[DEFAULT_SIZE];
		}

		public SparseReferenceList(ICollection<T> col) {
			m_data = new T[col.Count];
			m_length = col.Count;

			foreach (T key in col)
				Add(key);
		}

		/// \brief Construct a new empty list with the specified capacity.
		///
		/// \param n Initial size.
		public SparseReferenceList(int n) {

			m_data = new T[n];
			m_length = 0;
		}

		/// Construct a new empty list of default capacity.
		public SparseReferenceList() : this(DEFAULT_SIZE) { }

		/// Capacity of underlying data, that is the largest element ever indexed.
		public int Length {

			get { return m_length; }
		}

	}

}
