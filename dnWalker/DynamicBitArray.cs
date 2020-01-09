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

	// If only the standard, or the Mono developers, would not define their classes sealed,
	// I could actually keep all the getters from the BitArray class and not redirect all those
	// calls. Keep this as minimal as possible.


	using System.Collections;

	class DynamicBitArray {

		const int DEFAULT_INITIAL_SIZE = 32;
		const int DELTA = 32;

		BitArray m_fakeBase;

		public bool this[int index] {

			get {
				return index < m_fakeBase.Length && m_fakeBase[index];
			}

			set {
				// Does not fit && expand if value is true (otherwise false will be returned anyway).
				if (m_fakeBase.Length <= index && value) {
					m_fakeBase.Length = index + DELTA;
					m_fakeBase[index] = true;

				} else
					// Fits. Write value.
					m_fakeBase[index] = value;
			}
		}
		
		public void SetAll(bool val) {

			m_fakeBase.SetAll(val);
		}

		public DynamicBitArray(int initialSize) {

			m_fakeBase = new BitArray(initialSize);
		}

		public DynamicBitArray() : this(DEFAULT_INITIAL_SIZE) { }
	}
}
