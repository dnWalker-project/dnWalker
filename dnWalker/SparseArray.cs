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

	/*
		Everything here represents a sparse array of elements that is 
		modelled as a linked list. This sparse array design is meant for
		read only sparse arrays
	*/

	using System.Runtime.CompilerServices;
	using System.Runtime.InteropServices;
	using MMC.Collections;

    public interface ISparseElement {
		int Index { get; }
		int DeltaVal { get; }
		ISparseElement Next { get; }
        ISparseElement Clone();
    }


    public struct SparseElement : ISparseElement {

		public SparseElement(int i, int v, ISparseElement next) {
			m_index = i;
			m_deltaVal = v;
			m_next = next;
		}

		public int Index {
			get { return m_index; }
		}

		public int DeltaVal {
			get { return m_deltaVal; }
		}


		public ISparseElement Next {
			get { return m_next; }
		}

        public ISparseElement Clone()
        {
            return new SparseElement(m_index, m_deltaVal, m_next?.Clone());
        }

        private readonly int m_index;
        private readonly int m_deltaVal;
		private readonly ISparseElement m_next;
    }
}