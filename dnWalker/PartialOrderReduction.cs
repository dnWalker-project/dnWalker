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


using System;
using System.Collections.Generic;

namespace MMC {
	using MMC.State;
	using MMC.Data;
	using MMC.Util;
	using MMC.InstructionExec;
	using MMC.Collections;
	using System.Collections;
    using dnlib.DotNet.Emit;

    /// This is L in the stateful dynamic POR algorithm outlined in the 
    /// Collapsing Interleaving Information of VY's thesis
    class LastTransitionMapper {

		public static FastHashtable<MemoryLocation, Stack<SchedulingData>> m_lasttrans = new FastHashtable<MemoryLocation,Stack<SchedulingData>>(10);

		public SchedulingData Get(MemoryLocation ml) {

			Stack<SchedulingData> list;

			if (m_lasttrans.Find(ml, out list) && list.Count != 0)
				return list.Peek();
			else
				return null;
		}

		public void Put(SchedulingData sd) {
			if (sd.LastAccess.IsThreadLocal)
				return;

			MemoryLocation ml = sd.LastAccess.MemoryLocation;			
			Stack<SchedulingData> list;

			if (!m_lasttrans.Find(ml, out list)) {
				list = new Stack<SchedulingData>();
				m_lasttrans.UncheckedAdd(ml, list);
			}

			list.Push(sd);
		}

		public void Remove(SchedulingData sd) {
			if (sd.LastAccess.IsThreadLocal)
				return;

			MemoryLocation ml = sd.LastAccess.MemoryLocation;
			Stack<SchedulingData> list;

			if (m_lasttrans.Find(ml, out list)) {
				SchedulingData poppedSD = list.Pop();
				System.Diagnostics.Debug.Assert(poppedSD == sd, "The SD's are different, should not be possible.");
			}
		}
	}

	class StatefulDynamicPOR {
		private Stack<SchedulingData> backtrack;
		private LastTransitionMapper mapper;

		public StatefulDynamicPOR(Stack<SchedulingData> stack) {
			backtrack = stack;
			mapper = new LastTransitionMapper();
		}

		/// Update L
		public void ThreadPicked(SchedulingData sd, int chosen) {
			mapper.Put(sd);
		}

		/// Merge the SII's 
		public void Backtracked(Stack<SchedulingData> stack, SchedulingData fromSD) {

			if (fromSD.State.SII == null) {
				if (Config.UseDPORCollapser)
					fromSD.State.SII = new CollapsedSII(fromSD.SII as SimplifiedSII);
				else
					fromSD.State.SII = fromSD.SII;
			}

			SchedulingData toSD = stack.Peek();
			(toSD.SII as SimplifiedSII).Merge(fromSD.SII, toSD.LastAccess);

			mapper.Remove(toSD);

			/* for C3 proviso */
			fromSD.State.SchedulingData = null;
		}

		public void OnNewState(CollapsedState collapsed, SchedulingData sd) {

			/// Pick a transition as the current optimal persistent set
			if (sd.Working.Count > 0) {
				sd.Working = new Queue<int>();
				sd.Working.Enqueue(sd.Enabled.Peek());
			}

			/// Check all enabled transitions for dependency with 
			/// transitions on the DFS stack
			foreach (int threadId in sd.Enabled) {
				MemoryLocation ml = ActiveState.cur.NextAccess(threadId);
				ExpandSelectedSet(new MemoryAccess(ml, threadId));
			}

			sd.SII = new SimplifiedSII();
			collapsed.SII = sd.SII;
			collapsed.SchedulingData = sd; // for C3 proviso
		}

		public void OnSeenState(CollapsedState collapsed, SchedulingData sd) {

			sd.SII = collapsed.SII;

			/* C3 proviso */
			if (collapsed.OnStack) {
				foreach (int enabled in collapsed.SchedulingData.Enabled)
					collapsed.SchedulingData.Enqueue(enabled);
			}

			/// Check dependency with transitions in the SII
			foreach (MemoryAccess ma in sd.SII)
				ExpandSelectedSet(ma);
		}

		public void ExpandSelectedSet(MemoryAccess next) {

			if (next.IsThreadLocal)
				return;

			SchedulingData sd = mapper.Get(next.MemoryLocation);

			if (sd != null) {
				if (sd.Enabled.Contains(next.ThreadId)) {
					sd.Enqueue(next.ThreadId);
				} else {
					foreach (int enabledThread in sd.Enabled)
						sd.Enqueue(enabledThread);
				}
			}
		}
	}

	class ObjectEscapePOR {

		int m_dfscount = 0;

		/// Update the reachability of objects when
		/// 1. An objectreference is written to object o
		/// 2. o is threadshared, and the o' referenced by
		/// the objectreference is not
		public static void UpdateReachability(Boolean parentIsShared, IDataElement oldRef, IDataElement newRef) {

			if (!Config.UseObjectEscapePOR || !(oldRef is ObjectReference) || oldRef.Equals(newRef))
				return;

			// literally and shamelessly mimiced from JPF:

			if (parentIsShared) {
				if (!((ObjectReference)newRef).Equals(ObjectReference.Null)) {

					DynamicAllocation da = ActiveState.cur.DynamicArea.Allocations[(ObjectReference)newRef];

					if (!da.ThreadShared)
						MarkAndSweepGC.msgc.MarkSharedRecursive((ObjectReference)newRef);
				}
			}

			return;
		}

		public int GetPersistentThread() {

			/*
			 * Return the thread id which executes an independent action
			 * It either returns -1 (no set), or an integer > 0, because 
			 * we do it the quick and easy way: mutual dependent threads in a subset 
			 * of the enabled set is not possible to computer with the coarse grained
			 * object escape analysis */

			/* 
			 * Run a object escape analysis 
			 */
			if (Explorer.DoSharingAnalysis) {
				MarkAndSweepGC.msgc.Mark();
				Explorer.DoSharingAnalysis = false;
			}
			

			int oldCurrentThreadId = ActiveState.cur.ThreadPool.CurrentThreadId;
			int retval = -1;

			foreach (int threadId in ActiveState.cur.ThreadPool.RunnableThreads) {

				Instruction instr = ActiveState.cur.ThreadPool.Threads[threadId].CurrentMethod.ProgramCounter;
				InstructionExecBase instrExec = InstructionExecProvider.iep.GetExecFor(instr);

				ActiveState.cur.ThreadPool.CurrentThreadId = threadId;

				if (!instrExec.IsDependent() || instrExec.IsMultiThreadSafe()) {
					retval = threadId;
					break;
				}
			}

			ActiveState.cur.ThreadPool.CurrentThreadId = oldCurrentThreadId;

			return retval;
		}

		public void CheckStoreThreadSharingData(Stack<SchedulingData> stack, SchedulingData fromSD) {
			m_dfscount = stack.Count;
		}

		public void StoreThreadSharingData(CollapsedState collapsed, SchedulingData sd) {
			/* storing attributes, otherwise the GC messes up */
			/* clean this up, because this is an ugly hack */

			ISparseElement old = null;
			AllocationList alloc = ActiveState.cur.DynamicArea.Allocations;

			for (int i = 0; i < alloc.Length; i++) {
				DynamicAllocation da = ActiveState.cur.DynamicArea.Allocations[i];

				if (da != null && da.ThreadShared)
					old = new SparseElement(i, 0, old);
			}

			sd.AllocAtttributes = old;
		}

		public void RestoreThreadSharingData(Stack<SchedulingData> stack, SchedulingData sd) {
			if (m_dfscount > stack.Count) {
				AllocationList malloc = ActiveState.cur.DynamicArea.Allocations;
				for (ISparseElement d = sd.AllocAtttributes; d != null; d = d.Next)
					malloc[d.Index].HeapAttribute = AllocatedObject.SHARED;
			}
		}
	}
}
