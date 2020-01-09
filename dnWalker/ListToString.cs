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

	interface IToStringConvertor {

		string ToString(object o);
	}

	public delegate bool ObjectFilter(IList lst, int i);

	/// \brief Simple class that can be used as a string representation of a list.
	///
	/// Alas, I cannot inherit from System.String, since it's sealed. This
	/// class gives a snapshot string representation of a list, i.e. it does
	/// not update its data if the list is changed. An alternative use if to
	/// use this class as a convertor without state. Note that arrays are treated
	/// as lists.
	class ListToString : IToStringConvertor {

		string m_str;

		public static implicit operator string(ListToString a2s) {

			return a2s.ToString();
		}

		static bool AlwaysTrue(IList lst, int i) {

			return true;
		}

		public static bool NotNull(IList lst, int i) {

			return lst[i] != null;
		}

		public override string ToString() {

			System.Diagnostics.Debug.Assert(m_str != null, "Incorrect use of ListToString!");
			return m_str;
		}

		public static string Format(IList data, ObjectFilter filter, int from, int length) {

			// Yes, I know this method could use less locals. But let the
			// compiler/VES figure this out. "The competent programmer [...]
			// avoids clever tricks like the plague." -- Edsger Dijkstra.
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int to = from + length;
			for (int i=from; i < to; ++i) {
				if (sb.Length > 0)
					sb.Append(", ");
				if (filter(data, i))
					sb.Append(data[i].ToString());
			}
			sb.Insert(0, '[');
			sb.Append("]");
			return sb.ToString();
		}

		// And a bunch of convenience methods ...
		// Makes you want to kick the guy who decided not to put default
		// parameters in the C# standard in the nuts. FTI, a not really
		// convincing reason for this is given at:
		// http://blogs.msdn.com/csharpfaq/archive/2004/03/07/85556.aspx
		// Basically, it boils down to "we don't want to let go of the
		// programmers hand", a common practice.
	
		public static string Format(IList data, int from, int length) {

			return Format(data, new ObjectFilter(AlwaysTrue), from, length);
		}

		public static string Format(IList data, ObjectFilter filter) {

			return Format(data, filter, 0, data.Count);
		}

		public static string Format(IList data) {

			return Format(data, 0, data.Count);
		}

		public string ToString(object o) {

			return Format( (IList)o );
		}

		public ListToString(IList data, ObjectFilter filter) {

			m_str = Format(data, filter);
		}

		public ListToString(IList data) {

			m_str = Format(data);
		}

		public ListToString() {
		
			m_str = null;
		}
	}
}
