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

using MMC.Util;
using SGC = System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using MMC.Data;
using C5;
using MMC.Collections;
using System.IO;

namespace MMC.State {

	/// Holds a bag of parent references, i.e., counting set
	class ObjectReferenceBag {
		SGC.LinkedList<ObjectReference> values = new SGC.LinkedList<ObjectReference>();
		int[] m_referenceCounters = new int[1024];
		int flipped = 0;

		public bool IsEmpty {
			get { return values.Count == 0; }
		}

		public void ClearDirty() {
			flipped = 0;
		}

		public bool IsDirty {
			get { return flipped != 0; }
		}

		public SGC.IEnumerator<ObjectReference> GetEnumerator() {			
			return values.GetEnumerator();
		}

		public void Increment(ObjectReference o) {
			if (o.Equals(ObjectReference.Null))
				return;

			int index = (int)o.Location + 1;
			
			if (index >= m_referenceCounters.Length) 
				m_referenceCounters = IntArray.GrowArray(m_referenceCounters, index*2);
			
				
			int val = m_referenceCounters[index];

			if (val == 0) {
				flipped++;
				values.AddFirst(o);
			}

			m_referenceCounters[index] = val + 1;
		}

		public void Decrement(ObjectReference o) {
			if (o.Equals(ObjectReference.Null))
				return;

			int index = (int)o.Location + 1;
			int val = m_referenceCounters[index];

			if (val == 1) {
				flipped--;
				values.Remove(o);
			}

			m_referenceCounters[index] = val - 1;
		}
	}


	class ObjectReferenceDepthComparer : IComparer<ObjectReference> {

		public static readonly ObjectReferenceDepthComparer cmp = new ObjectReferenceDepthComparer();

		public int Compare(ObjectReference a, ObjectReference b) {

			DynamicAllocation left = ActiveState.cur.DynamicArea.Allocations[a];
			DynamicAllocation right = ActiveState.cur.DynamicArea.Allocations[b];

			return left.Key - right.Key;
		}
	}

	class IncrementalHeapVisitor : BaseStorageVisitor, IStorageVisitor, IGarbageCollector {

		/// My own priority queue based on IntervalHeap from C5
		private class MyHeap {

			IPriorityQueue<ObjectReference> m_pqueue;
			C5.IDictionary<ObjectReference, IPriorityQueueHandle<ObjectReference>> m_handleDict;

			public MyHeap() {
				m_pqueue = new IntervalHeap<ObjectReference>(ObjectReferenceDepthComparer.cmp);
				m_handleDict = new HashDictionary<ObjectReference, IPriorityQueueHandle<ObjectReference>>();
			}

			public int Count {
				get { return m_pqueue.Count; }
			}

			public void Insert(ObjectReference ida) {
				IPriorityQueueHandle<ObjectReference> h = null;
				m_pqueue.Add(ref h, ida);
				m_handleDict[ida] = h;
			}

			public ObjectReference FindAndDeleteMin() {
				ObjectReference ida = m_pqueue.DeleteMin();
				m_handleDict.Remove(ida);
				return ida;
			}

			public void Adjust(ObjectReference ida) {
				IPriorityQueueHandle<ObjectReference> h = null;

				if (m_handleDict.Find(ida, out h)) {
					m_pqueue.Replace(h, ida);
				} else {
					Insert(ida);
				}
			}
			
			public void Remove(ObjectReference ida) {
				IPriorityQueueHandle<ObjectReference> h;
				if (m_handleDict.Remove(ida, out h))
					m_pqueue.Delete(h);

				// if not in heap, then do nothing
			}

			public bool IsEmpty() {
				return m_handleDict.IsEmpty;
			}
		}

		public static IncrementalHeapVisitor ihv = new IncrementalHeapVisitor();
		MyHeap heap;

		private IncrementalHeapVisitor() {
			heap = new MyHeap();
		}


		public void Run() {
			IncrementalHeapVisitor.ihv.RunMark();

			AllocationList m_alloc = ActiveState.cur.DynamicArea.Allocations;
			
			/// Sweep phase
			for (int i = 0; i < m_alloc.Length; i++) {
				DynamicAllocation alloc = m_alloc[i];
				if (alloc != null && alloc.Depth == int.MaxValue && !alloc.Pinned) {
					ObjectReference toDelete = new ObjectReference(i + 1);
					ParentWatcher.RemoveParentFromAllChilds(toDelete);
					ParentWatcher.pw[i + 1].ClearDirty();
					ActiveState.cur.DynamicArea.DisposeLocation(i);
				}
			}
		}


		/* line 1 */
		public void RunMark() {

			AllocationList m_alloc = ActiveState.cur.DynamicArea.Allocations;
			for (int i = 0; i < m_alloc.Length; i++) {
				DynamicAllocation alloc = m_alloc[i];

				if (alloc != null) {
					ObjectReferenceBag parents = ParentWatcher.pw[i + 1];
					if (parents.IsDirty || parents.IsEmpty) {
						AddModified(i);
						parents.ClearDirty();
					} 
				}
			}


			/* lines 8 - 31 */
			while (!heap.IsEmpty()) {

				// line 9
				ObjectReference reference = heap.FindAndDeleteMin();
				DynamicAllocation u = ActiveState.cur.DynamicArea.Allocations[reference];

				// line 10
				if (u.Rhs < u.Depth) {
					// line 11
					u.Depth = u.Rhs;
					u.Accept(this); // processing children
				} else {
					u.Depth = int.MaxValue;
					ProcessChild(reference);
					u.Accept(this); // processing children
				}
			}
		}


		private void AddModified(int i) {
			DynamicAllocation ida = ActiveState.cur.DynamicArea.Allocations[i];

			ida.Rhs = DetermineDepth(i + 1);	// line 3
			if (ida.Rhs != ida.Depth) 			// line 4
				heap.Insert(new ObjectReference(i + 1));			// line 5
		}

		
		public override void VisitAllocatedArray(AllocatedArray aa) {
			for (int i = 0; i < aa.Fields.Length; i++) {
				IDataElement ide = aa.Fields[i];
				if (ide is ObjectReference)
					ProcessChild((ObjectReference)ide);
			}
		}

		public override void VisitAllocatedDelegate(AllocatedDelegate ad) {
			ProcessChild(ad.Object);
		}

		public override void VisitAllocatedObject(AllocatedObject ao) {
			for (int i = 0; i < ao.Fields.Length; i++) {
				IDataElement ide = ao.Fields[i];
				if (ide is ObjectReference)
					ProcessChild((ObjectReference)ide);
			}
		}

		private void ProcessChild(ObjectReference reference) {

			if (ObjectReference.Null.Equals(reference))
				return;

			DynamicAllocation ida = ActiveState.cur.DynamicArea.Allocations[reference];

			// line 14-18 && 24-28
			ida.Rhs = DetermineDepth((int)reference.Location);
			if (ida.Rhs != ida.Depth)
				heap.Adjust(reference);
			else
				heap.Remove(reference);

		}

		private int DetermineDepth(int i) {
			int retval = int.MaxValue - 1; // ensure no overflow

			foreach (ObjectReference parent in ParentWatcher.pw[i])
				retval = Math.Min(ParentWatcher.GetDynamicAllocation(parent).Depth, retval);

			retval++;

			return retval;
		}

	}

	/// We made a seperate watcher for monitoring objectreferences on the callstack
	/// because initially we wanted to created a generalised heap analysis framework
	///
	/// However, later, we found out that only garbage collection could be done 
	/// incrementally, therefore, this class is a relic from it. 
	///
	/// All calls to this class are passed to ParentObjectWatcher
	class ThreadObjectWatcher {

		public static void DecrementAll(int threadId, IDataElementContainer ids) {
			if (Config.MemoisedGC) {
				for (int i = 0; i < ids.Length; i++)
					Decrement(threadId, ids[i]);
			}
		}

		public static void IncrementAll(int threadId, IDataElementContainer ids) {
			if (Config.MemoisedGC) {
				for (int i = 0; i < ids.Length; i++)
					Increment(threadId, ids[i]);
			}
		}

		public static void UpdateDifference(int threadId, IDataElementContainer oldList, IDataElementContainer newList) {
			if (Config.MemoisedGC) {
				/* precondition: oldList.Length is equal to newList.Length */

				for (int i = 0; i < oldList.Length && i < newList.Length; i++) {
					IDataElement oldElem = oldList[i];
					IDataElement newElem = newList[i];
					ParentWatcher.UpdateParentOfDifferentChild(ParentWatcher.RootObjectReference, oldElem, newElem);
				}
			}
		}

		public static void IncrementAll(IDataElementContainer ids) {
			IncrementAll(ActiveState.cur.ThreadPool.CurrentThreadId, ids);
		}

		public static void DecrementAll(IDataElementContainer ids) {
			DecrementAll(ActiveState.cur.ThreadPool.CurrentThreadId, ids);
		}

		public static void Increment(int threadId, IDataElement o) {
			ParentWatcher.AddParentToChild(ParentWatcher.RootObjectReference, o);
		}

		public static void Decrement(int threadId, IDataElement o) {
			ParentWatcher.RemoveParentFromChild(ParentWatcher.RootObjectReference, o);
		}

		public static void Increment(IDataElement o) {
			Increment(ActiveState.cur.ThreadPool.CurrentThreadId, o);
		}

		public static void Decrement(IDataElement o) {
			Decrement(ActiveState.cur.ThreadPool.CurrentThreadId, o);
		}

	}

	/// This monitors changes to the heap, and maintains the list of parents for each
	/// object 
	class ParentWatcher : BaseStorageVisitor, IStorageVisitor {

		public ObjectReferenceBag[] parents;

		public ParentWatcher() {
			parents = new ObjectReferenceBag[1024];
			for (int i = 0; i < parents.Length; i++)
				parents[i] = new ObjectReferenceBag();
		}

		/// Returns the parent list for a given location
		/// Automatically resizes the array when the location is
		/// larger than the current array
		public ObjectReferenceBag this[int loc] {
			get {
				int oldLength = parents.Length;
				if (loc >= oldLength) {
					ObjectReferenceBag[] newParents = new ObjectReferenceBag[oldLength * 2];
					System.Array.Copy(parents, newParents, oldLength);
					for (int i = oldLength; i < oldLength * 2; i++)
						newParents[i] = new ObjectReferenceBag();
					parents = newParents;
				}
				return parents[loc];
			}
		}

		public static ParentWatcher pw = new ParentWatcher();
		
		public delegate void UpdateSingle(ObjectReference parent, IDataElement child);

		public static UpdateSingle adder = new UpdateSingle(AddParentToChild);
		public static UpdateSingle remover = new UpdateSingle(RemoveParentFromChild);
	
		/// Fictive root
		public static AllocatedObject RootAllocatedObject = new AllocatedObject(DefinitionProvider.dp.GetTypeDefinition("System.Object"));
		public static ObjectReference RootObjectReference = new ObjectReference(uint.MaxValue);

		static ParentWatcher() {
			RootAllocatedObject.Depth = 0;
		}

		public static DynamicAllocation GetDynamicAllocation(int loc) {
			if (loc == -1)
				return RootAllocatedObject;
			else
				return ActiveState.cur.DynamicArea.Allocations[loc];
		}

		public static DynamicAllocation GetDynamicAllocation(ObjectReference or) {
			//if (RootObjectReference.Equals(or))
			if (or.Location == uint.MaxValue)
				return RootAllocatedObject;
			else
				return ActiveState.cur.DynamicArea.Allocations[or];
		}

		public static void RemoveParentFromChild(ObjectReference parentRef, IDataElement childRef) {
			if (Config.MemoisedGC && childRef is ObjectReference && !childRef.Equals(ObjectReference.Null)) {

				ObjectReference reference = (ObjectReference)childRef;
				pw[(int)reference.Location].Decrement(parentRef);
			}
		}

		public static void AddParentToChild(ObjectReference parentRef, IDataElement childRef) {
			if (Config.MemoisedGC && childRef is ObjectReference && !childRef.Equals(ObjectReference.Null)) {
				ObjectReference reference = (ObjectReference)childRef;
				pw[(int)reference.Location].Increment(parentRef);
			}
		}


		public static void UpdateParentOfDifferentChild(ObjectReference parentRef, IDataElement oldChildRef, IDataElement newChildRef) {
			if (Config.MemoisedGC && oldChildRef != newChildRef) {
				RemoveParentFromChild(parentRef, oldChildRef);
				AddParentToChild(parentRef, newChildRef);
			}
		}

		private static void UpdateParentFromAllChilds(ObjectReference parentRef, UpdateSingle updater) {
			if (Config.MemoisedGC) {

				DynamicAllocation alloc = ActiveState.cur.DynamicArea.Allocations[parentRef];
				if (alloc == null)
					return;

				switch (alloc.AllocationType) {
					case AllocationType.Object:
					case AllocationType.Array:
						AllocatedObject parent = alloc as AllocatedObject;
						DataElementList childs = parent.Fields;
						for (int i = 0; i < childs.Length; i++)
							updater(parentRef, childs[i]);
						break;
					case AllocationType.Delegate:
						AllocatedDelegate allocDelegate = alloc as AllocatedDelegate;
						updater(parentRef, allocDelegate.Object);
						break;
				}
			}
		}

		public static void UpdateParentFromAllChilds(ObjectReference parentRef, UpdateSingle updater, DataElementList list) {
			if (Config.MemoisedGC && list != null) {
				for (int i = 0; i < list.Length; i++)
					updater(parentRef, list[i]);
			}
		}

		public static void RemoveParentFromAllChilds(ObjectReference parentRef, DataElementList list) {
			UpdateParentFromAllChilds(parentRef, remover, list);
		}

		public static void AddParentToAllChilds(ObjectReference parentRef, DataElementList list) {
			UpdateParentFromAllChilds(parentRef, adder, list);
		}

		public static void RemoveParentFromAllChilds(ObjectReference parentRef) {
			UpdateParentFromAllChilds(parentRef, remover);
		}

		public static void AddParentToAllChilds(ObjectReference parentRef) {
			UpdateParentFromAllChilds(parentRef, adder);
		}

	}
}