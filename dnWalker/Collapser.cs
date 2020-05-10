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

namespace MMC.State
{
    internal class Collapser
    {
        private readonly StateCollapser m_storer;
        private readonly StateDecollapser m_restorer;
        internal readonly PoolData m_pool_data;

        public CollapsedState CollapseCurrentState(ExplicitActiveState cur)
        {
            return m_storer.GetStorableState(cur);
        }

        public void Reset(CollapsedState s)
        {
            m_storer.Reset(s);
        }

        public void DecollapseByDelta(ExplicitActiveState cur, CollapsedStateDelta s)
        {
            m_restorer.RestoreState(cur, s);
        }

        public Collapser(ExplicitActiveState cur)
        {
            m_pool_data = new PoolData();
            m_storer = new StateCollapser(m_pool_data);
            m_restorer = new StateDecollapser(m_pool_data);
        }

        internal Collapser(PoolData poolData)
        {
            m_pool_data = poolData;
            m_storer = new StateCollapser(m_pool_data);
            m_restorer = new StateDecollapser(m_pool_data);
        }

        internal PoolData PoolData => m_pool_data;
    }
}
