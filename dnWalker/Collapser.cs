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

	using MMC.Data;
	using MMC.Util;
	using MMC.Collections;
	using Mono.Cecil;

	class Collapser {

		public static PoolData POOL_DATA = new PoolData();
		StateCollapser m_storer;
		StateDecollapser m_restorer;

		public CollapsedState CollapseCurrentState() {
			return m_storer.GetStorableState();
		}

		public void Reset(CollapsedState s) {
			m_storer.Reset(s);
		}

		public void DecollapseByDelta(CollapsedStateDelta s) {
			m_restorer.RestoreState(s);
		}

		public Collapser() {
			m_storer = new StateCollapser(POOL_DATA);
			m_restorer = new StateDecollapser(POOL_DATA);
		}
	}
}
