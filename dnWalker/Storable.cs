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

	interface IStorable {

		IStorable StorageCopy();
		bool ReadOnly { get; set; }
	}

	interface ICleanable {

		bool IsDirty();
		void Clean();
	}

	// System.IDisposable is not used, since that's for objects that are
	// used in a 'using' block (e.g. streams).
	
	interface IMustDispose {

		void Dispose();
	}

	interface IDataElementContainer : IMustDispose, ICleanable, IStorable {

		IDataElement this[int index] { get; set; }
		int Length { get; }
	}
}
