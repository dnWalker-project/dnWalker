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

	using System.Diagnostics;
	using MMC.Util;
	using MMC.Data;

    public struct CollapsedStateDelta {

		public CollapsedStateDelta(ISparseElement allocD, ISparseElement classD, ISparseElement thrdsD, int bound) {
			m_allocDelta = allocD;
			m_classDelta = classD;
			m_trdsDelta = thrdsD;

			m_threadsUpperbound = bound;
		}

		ISparseElement m_allocDelta;
		ISparseElement m_classDelta;
		ISparseElement m_trdsDelta;
		int m_threadsUpperbound; // the amount of threads in this state


		public ISparseElement Allocations {
			get { return m_allocDelta; }
		}

		public ISparseElement Classes {
			get { return m_classDelta; }
		}

		public ISparseElement Threads {
			get { return m_trdsDelta; }
		}

		public int ThreadsUpperBound {
			get { return m_threadsUpperbound; }
		}
	}

    public class CollapsedState
    {
		ChangingIntVector m_alloc;
		ChangingIntVector m_class;
		ChangingIntVector m_trds;

        public bool OnStack
        {
			get { return SchedulingData != null; }
		}

        public SchedulingData SchedulingData { get; set; }

        public ChangingIntVector Allocations { get { return m_alloc; } }
		public ChangingIntVector Classes { get { return m_class; } }
		public ChangingIntVector Threads { get { return m_trds; } }

        public ISII SII { get; set; }

        public CollapsedState Clone()
        {
			return new CollapsedState(
				m_alloc.WriteBack(),
				m_class.WriteBack(),
				m_trds.WriteBack());
		}

		public void ClearDelta() {
			m_alloc.ClearDelta();
			m_class.ClearDelta();
			m_trds.ClearDelta();
		}

		public CollapsedStateDelta GetDelta()
        {
			return new CollapsedStateDelta(
				m_alloc.GetReverseDelta(),
				m_class.GetReverseDelta(),
				m_trds.GetReverseDelta(), m_trds.Length);
		}

		public override string ToString()
        {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.AppendFormat("allocations   : {0}\nclasses       : {1}\nthreads       : {2}\n",
				m_alloc.ToString(), m_class.ToString(), m_trds.ToString());
			return sb.ToString();
		}

		public override bool Equals(object other)
        {
			CollapsedState o = other as CollapsedState;
			return m_trds.Equals(o.Threads) &&
				m_alloc.Equals(o.Allocations) &&
				m_class.Equals(o.Classes);
		}

		public override int GetHashCode()
        {
			return Collections.SingleIntHasher.Hash(m_alloc.GetHashCode() ^ m_class.GetHashCode() ^ m_trds.GetHashCode());
		}

        protected CollapsedState(ChangingIntVector alloc,
            ChangingIntVector cls,
            ChangingIntVector trds)
        {
            m_alloc = alloc;
            m_class = cls;
            m_trds = trds;
        }

        public CollapsedState(ExplicitActiveState cur)
        {
            m_alloc = new ChangingIntVector(cur.DynamicArea.Allocations.Length);
            m_class = new ChangingIntVector(cur.StaticArea.Classes.Length);
            m_trds = new ChangingIntVector(cur.ThreadPool.Threads.Length);
        }
	}
}
