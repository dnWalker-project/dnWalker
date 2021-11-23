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

    using System.Text;
    using MMC.Data;
    using MMC.Util;
    using MMC.Collections;
    using System;

    /// An implementation of the sack of stuff.
    class PoolData  {

		/*
		Pool m_elementPool;		
		Pool m_oPool;
		Pool m_decPool;
		Pool m_lockPool;
		Pool m_idPool;
		 */


		Pool m_oPool;

		public CloningFastPool<IDataElement> m_elementPool;		
		public CloningFastPool<IDataElementContainer> m_decPool;
		public CloningFastPool<Lock> m_lockPool;
		public CloningFastPool<IInitData>  m_idPool;
		public FastPool<WrappedIntArray> m_listPool;
		public FastPool<ObjectSII> m_objectSIIPool; 

		public int GetInt(IDataElement de) {

			return m_elementPool.GetInt(de);
		}

		public int GetInt(IDataElementContainer c) {

			return m_decPool.GetInt(c);
		}

		public int GetInt(Lock l) {

			return m_lockPool.GetInt(l);
		}

		public int GetInt(WrappedIntArray l) {

			return m_listPool.GetInt(l);
		}

		public int GetInt(IInitData id) {
			var i = m_idPool.GetInt(id);
			return i;
		}

        internal PoolData Clone()
        {
            return new PoolData
            {
                m_decPool = m_decPool?.Clone(),
                m_elementPool = m_elementPool?.Clone(),
                m_idPool = m_idPool?.Clone(),
                m_listPool = m_listPool?.Clone(),
                m_lockPool = m_lockPool?.Clone(),
                m_objectSIIPool = m_objectSIIPool?.Clone(),
                m_oPool = m_oPool?.Clone()
            };
        }

        public int GetInt(object o) {

			return m_oPool.GetInt(o);
		}

		public int GetInt(ObjectSII ts) {

			return m_objectSIIPool.GetInt(ts);
		}

		public IDataElement GetElement(int i) {

			return (IDataElement)m_elementPool.GetObject(i);
		}

		public DataElementList GetDataElementList(int i) {

			return m_decPool.GetObject(i) as DataElementList;
		}

		public DataElementStack GetDataElementStack(int i) {

			return m_decPool.GetObject(i) as DataElementStack;
		}

		public WrappedIntArray GetList(int i) {

			return m_listPool.GetObject(i);
		}

		public Lock GetLock(int i) {

			return m_lockPool.GetObject(i) as Lock;
		}

		public IInitData GetInitData(int i) {

			return m_idPool.GetObject(i) as IInitData;
		}

		public object GetObject(int i) {

			return m_oPool.GetObject(i);
		}

		public ObjectSII GetObjectSII(int i) {

			return m_objectSIIPool.GetObject(i);
		}

		/*
		 * TODO: fix this tostring code */
		public override string ToString() {

			var sb = new StringBuilder();
//			sb.AppendFormat("Elements pool:\n{0}\n", m_elementPool.ToString());
//			sb.AppendFormat("Container pool:\n{0}\n", m_decPool.ToString());
//			sb.AppendFormat("List pool:\n{0}\n", m_listPool.ToString());
//			sb.AppendFormat("Lock pool:\n{0}\n", m_lockPool.ToString());
//			sb.AppendFormat("InitData pool:\n{0}\n", m_idPool.ToString());
//			sb.AppendFormat("Object pool:\n{0}\n", m_oPool.ToString());
			return sb.ToString();
		}

		public PoolData() {					
			

			var cilhcp = new CILElementHashCodeProvider();
			var cilcmp = new CILElementComparer();
			var cilprn = new CILElementPrinter();
			m_oPool = new Pool(cilhcp, cilcmp, cilprn, false);

			m_objectSIIPool = new FastPool<ObjectSII>(14);
			GetInt(ObjectSII.Empty); // let the empty sum to be the smallest array possible

			m_listPool = new FastPool<WrappedIntArray>(16);
			m_elementPool = new CloningFastPool<IDataElement>(10);
			m_decPool = new CloningFastPool<IDataElementContainer>(14);
			m_lockPool = new CloningFastPool<Lock>(10);
			m_idPool = new CloningFastPool<IInitData>(10);
		}
	} 

}
