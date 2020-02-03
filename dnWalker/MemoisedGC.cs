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
using MMC.Data;
using C5;

namespace MMC.State
{
    /// <summary>
    /// Holds a bag of parent references, i.e., counting set
    /// </summary>
    internal class ObjectReferenceBag
    {
        SGC.LinkedList<ObjectReference> values = new SGC.LinkedList<ObjectReference>();
        int[] m_referenceCounters = new int[256];
        int flipped = 0;

        public bool IsEmpty
        {
            get { return values.Count == 0; }
        }

        public void ClearDirty()
        {
            flipped = 0;
        }

        public bool IsDirty
        {
            get { return flipped != 0; }
        }

        public SGC.IEnumerator<ObjectReference> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public void Increment(ObjectReference o)
        {
            if (o.Equals(ObjectReference.Null))
                return;

            int index = (int)o.Location + 1;

            if (index >= m_referenceCounters.Length)
                m_referenceCounters = IntArray.GrowArray(m_referenceCounters, index * 2);

            int val = m_referenceCounters[index];

            if (val == 0)
            {
                flipped++;
                values.AddFirst(o);
            }

            m_referenceCounters[index] = val + 1;
        }

        public void Decrement(ObjectReference o)
        {
            if (o.Equals(ObjectReference.Null))
                return;

            int index = (int)o.Location + 1;
            int val = m_referenceCounters[index];

            if (val == 1)
            {
                flipped--;
                values.Remove(o);
            }

            m_referenceCounters[index] = val - 1;
        }
    }


    class ObjectReferenceDepthComparer : SGC.IComparer<ObjectReference>
    {
        private readonly ExplicitActiveState cur;

        public ObjectReferenceDepthComparer(ExplicitActiveState cur)
        {
            this.cur = cur;
        }

        public int Compare(ObjectReference a, ObjectReference b)
        {
            DynamicAllocation left = cur.DynamicArea.Allocations[a];
            DynamicAllocation right = cur.DynamicArea.Allocations[b];

            return left.Key - right.Key;
        }
    }

    class IncrementalHeapVisitor : BaseStorageVisitor, IStorageVisitor, IGarbageCollector
    {

        /// My own priority queue based on IntervalHeap from C5
        private class MyHeap
        {
            IPriorityQueue<ObjectReference> m_pqueue;
            IDictionary<ObjectReference, IPriorityQueueHandle<ObjectReference>> m_handleDict;

            public MyHeap(ExplicitActiveState cur)
            {
                m_pqueue = new IntervalHeap<ObjectReference>(new ObjectReferenceDepthComparer(cur));
                m_handleDict = new HashDictionary<ObjectReference, IPriorityQueueHandle<ObjectReference>>();
            }

            public int Count
            {
                get { return m_pqueue.Count; }
            }

            public void Insert(ObjectReference ida)
            {
                IPriorityQueueHandle<ObjectReference> h = null;
                m_pqueue.Add(ref h, ida);
                m_handleDict[ida] = h;
            }

            public ObjectReference FindAndDeleteMin()
            {
                ObjectReference ida = m_pqueue.DeleteMin();
                m_handleDict.Remove(ida);
                return ida;
            }

            public void Adjust(ObjectReference ida)
            {
                IPriorityQueueHandle<ObjectReference> h = null;

                if (m_handleDict.Find(ref ida, out h))
                {
                    m_pqueue.Replace(h, ida);
                }
                else
                {
                    Insert(ida);
                }
            }

            public void Remove(ObjectReference ida)
            {
                IPriorityQueueHandle<ObjectReference> h;
                if (m_handleDict.Remove(ida, out h))
                    m_pqueue.Delete(h);

                // if not in heap, then do nothing
            }

            public bool IsEmpty()
            {
                return m_handleDict.IsEmpty;
            }
        }

        //public static IncrementalHeapVisitor ihv = new IncrementalHeapVisitor();
        MyHeap heap;

        public IncrementalHeapVisitor(ExplicitActiveState cur)
        {
            heap = new MyHeap(cur);
        }

        public void Run(ExplicitActiveState cur)
        {
            RunMark(cur);

            AllocationList m_alloc = cur.DynamicArea.Allocations;

            /// Sweep phase
            for (int i = 0; i < m_alloc.Length; i++)
            {
                DynamicAllocation alloc = m_alloc[i];
                if (alloc != null && alloc.Depth == int.MaxValue && !alloc.Pinned)
                {
                    ObjectReference toDelete = new ObjectReference(i + 1);
                    cur.ParentWatcher.RemoveParentFromAllChilds(toDelete, cur);
                    cur.ParentWatcher[i + 1].ClearDirty();
                    cur.DynamicArea.DisposeLocation(i);
                }
            }
        }

        /* line 1 */
        public void RunMark(ExplicitActiveState cur)
        {
            AllocationList m_alloc = cur.DynamicArea.Allocations;
            for (int i = 0; i < m_alloc.Length; i++)
            {
                DynamicAllocation alloc = m_alloc[i];

                if (alloc != null)
                {
                    ObjectReferenceBag parents = cur.ParentWatcher[i + 1];
                    if (parents.IsDirty || parents.IsEmpty)
                    {
                        AddModified(i, cur);
                        parents.ClearDirty();
                    }
                }
            }

            /* lines 8 - 31 */
            while (!heap.IsEmpty())
            {
                // line 9
                ObjectReference reference = heap.FindAndDeleteMin();
                DynamicAllocation u = cur.DynamicArea.Allocations[reference];

                // line 10
                if (u.Rhs < u.Depth)
                {
                    // line 11
                    u.Depth = u.Rhs;
                    u.Accept(this, cur); // processing children
                }
                else
                {
                    u.Depth = int.MaxValue;
                    ProcessChild(reference, cur);
                    u.Accept(this, cur); // processing children
                }
            }
        }


        private void AddModified(int i, ExplicitActiveState cur)
        {
            DynamicAllocation ida = cur.DynamicArea.Allocations[i];

            ida.Rhs = DetermineDepth(i + 1, cur);    // line 3
            if (ida.Rhs != ida.Depth)           // line 4
            {
                heap.Insert(new ObjectReference(i + 1));            // line 5
            }
        }

        public override void VisitAllocatedArray(AllocatedArray aa, ExplicitActiveState cur)
        {
            for (int i = 0; i < aa.Fields.Length; i++)
            {
                IDataElement ide = aa.Fields[i];
                if (ide is ObjectReference)
                    ProcessChild((ObjectReference)ide, cur);
            }
        }

        public override void VisitAllocatedDelegate(AllocatedDelegate ad, ExplicitActiveState cur)
        {
            ProcessChild(ad.Object, cur);
        }

        public override void VisitAllocatedObject(AllocatedObject ao, ExplicitActiveState cur)
        {
            for (int i = 0; i < ao.Fields.Length; i++)
            {
                IDataElement ide = ao.Fields[i];
                if (ide is ObjectReference)
                    ProcessChild((ObjectReference)ide, cur);
            }
        }

        private void ProcessChild(ObjectReference reference, ExplicitActiveState cur)
        {
            if (ObjectReference.Null.Equals(reference))
            {
                return;
            }

            DynamicAllocation ida = cur.DynamicArea.Allocations[reference];

            // line 14-18 && 24-28
            ida.Rhs = DetermineDepth((int)reference.Location, cur);
            if (ida.Rhs != ida.Depth)
                heap.Adjust(reference);
            else
                heap.Remove(reference);

        }

        private int DetermineDepth(int i, ExplicitActiveState cur)
        {
            int retval = int.MaxValue - 1; // ensure no overflow

            foreach (ObjectReference parent in cur.ParentWatcher[i])
            {
                retval = Math.Min(cur.ParentWatcher.GetDynamicAllocation(parent, cur).Depth, retval);
            }

            retval++;

            return retval;
        }
    }

    /// We made a seperate watcher for monitoring object references on the callstack
    /// because initially we wanted to created a generalised heap analysis framework
    ///
    /// However, later, we found out that only garbage collection could be done 
    /// incrementally, therefore, this class is a relic from it. 
    ///
    /// All calls to this class are passed to ParentObjectWatcher
    class ThreadObjectWatcher
    {
        public static void DecrementAll(int threadId, IDataElementContainer ids, ExplicitActiveState cur)
        {
            if (cur.Configuration.MemoisedGC)
            {
                for (int i = 0; i < ids.Length; i++)
                    Decrement(threadId, ids[i], cur);
            }
        }

        public static void IncrementAll(int threadId, IDataElementContainer ids, ExplicitActiveState cur)
        {
            if (cur.Configuration.MemoisedGC)
            {
                for (int i = 0; i < ids.Length; i++)
                    Increment(threadId, ids[i], cur);
            }
        }

        public static void UpdateDifference(int threadId, IDataElementContainer oldList, IDataElementContainer newList, ExplicitActiveState cur)
        {
            if (cur.Configuration.MemoisedGC)
            {
                /* precondition: oldList.Length is equal to newList.Length */

                for (int i = 0; i < oldList.Length && i < newList.Length; i++)
                {
                    IDataElement oldElem = oldList[i];
                    IDataElement newElem = newList[i];
                    cur.ParentWatcher.UpdateParentOfDifferentChild(cur.ParentWatcher.RootObjectReference, oldElem, newElem, cur);
                }
            }
        }

        public static void IncrementAll(IDataElementContainer ids, ExplicitActiveState cur)
        {
            IncrementAll(cur.ThreadPool.CurrentThreadId, ids, cur);
        }

        public static void DecrementAll(IDataElementContainer ids, ExplicitActiveState cur)
        {
            DecrementAll(cur.ThreadPool.CurrentThreadId, ids, cur);
        }

        public static void DecrementAll(ThreadPool threadPool, IDataElementContainer ids, ExplicitActiveState cur)
        {
            DecrementAll(threadPool.CurrentThreadId, ids, cur);
        }

        public static void Increment(int threadId, IDataElement o, ExplicitActiveState cur)
        {
            cur.ParentWatcher.AddParentToChild(cur.ParentWatcher.RootObjectReference, o, cur);
        }

        public static void Decrement(int threadId, IDataElement o, ExplicitActiveState cur)
        {
            cur.ParentWatcher.RemoveParentFromChild(cur.ParentWatcher.RootObjectReference, o, cur);
        }

        /*public static void Increment(IDataElement o)
        {
            Increment(ActiveState.cur.ThreadPool.CurrentThreadId, o);
        }

        public static void Decrement(IDataElement o)
        {
            Decrement(ActiveState.cur.ThreadPool.CurrentThreadId, o);
        }*/

    }

    /// <summary>
    /// This monitors changes to the heap, and maintains the list of parents for each object
    /// </summary>
	internal class ParentWatcher : BaseStorageVisitor, IStorageVisitor
    {
        private readonly SGC.IDictionary<int, ObjectReferenceBag> parents;

        public ParentWatcher(IConfig config, DefinitionProvider definitionProvider)
        {
            parents = new SGC.Dictionary<int, ObjectReferenceBag>();

            RootAllocatedObject = new AllocatedObject(
                definitionProvider.GetTypeDefinition("System.Object"),
                config)
            {
                Depth = 0
            };

            adder = new UpdateSingle(AddParentToChild);
            remover = new UpdateSingle(RemoveParentFromChild);
        }

        /// <summary>
        /// Returns the parent list for a given location
        /// Automatically resizes the array when the location is
        /// larger than the current array
        /// </summary>
        public ObjectReferenceBag this[int loc]
        {
            get
            {
                if (parents.TryGetValue(loc, out var objectReferenceBag))
                {
                    return objectReferenceBag;
                }

                objectReferenceBag = new ObjectReferenceBag();
                parents.Add(loc, objectReferenceBag);
                return objectReferenceBag;
            }
        }

        public delegate void UpdateSingle(ObjectReference parent, IDataElement child, ExplicitActiveState cur);

        private /*static*/ UpdateSingle adder;// = new UpdateSingle(AddParentToChild);
        private /*static*/ UpdateSingle remover;// = new UpdateSingle(RemoveParentFromChild);

        /// <summary>
        /// Fictive root
        /// </summary>
        public /*static*/ AllocatedObject RootAllocatedObject { get; }/*= new AllocatedObject(
            cur.DefinitionProvider.GetTypeDefinition("System.Object"),
            Config.Instance)
        {
            Depth = 0
        };*/

        public /*static*/ ObjectReference RootObjectReference = new ObjectReference(uint.MaxValue);

        /*static ParentWatcher()
        {
            RootAllocatedObject.Depth = 0;
        }*/

        public /*static*/ DynamicAllocation GetDynamicAllocation(int loc)
        {
            if (loc == -1)
            {
                return RootAllocatedObject;
            }
            else
            {
                //return ActiveState.cur.DynamicArea.Allocations[loc];
                throw new NotImplementedException();
            }
        }

        public /*static*/ DynamicAllocation GetDynamicAllocation(ObjectReference or, ExplicitActiveState cur)
        {
            //if (RootObjectReference.Equals(or))
            if (or.Location == uint.MaxValue)
            {
                return RootAllocatedObject;
            }
            return cur.DynamicArea.Allocations[or];
        }

        public /*static*/ void RemoveParentFromChild(ObjectReference parentRef, IDataElement childRef, ExplicitActiveState cur)
        {
            var memoisedGC = cur.Configuration.MemoisedGC;
            if (memoisedGC && childRef is ObjectReference && !childRef.Equals(ObjectReference.Null))
            {
                ObjectReference reference = (ObjectReference)childRef;
                this[(int)reference.Location].Decrement(parentRef);
            }
        }

        public /*static*/ void RemoveParentFromChild(ObjectReference parentRef, IDataElement childRef, bool memoisedGC)
        {
            if (memoisedGC && childRef is ObjectReference && !childRef.Equals(ObjectReference.Null))
            {
                ObjectReference reference = (ObjectReference)childRef;
                this[(int)reference.Location].Decrement(parentRef);
            }
        }

        public /*static*/ void AddParentToChild(ObjectReference parentRef, IDataElement childRef, ExplicitActiveState cur)
        {
            var memoisedGC = cur.Configuration.MemoisedGC;
            if (memoisedGC && childRef is ObjectReference objRef && !objRef.Equals(ObjectReference.Null))
            {
                this[(int)objRef.Location].Increment(parentRef);
            }
        }

        public /*static*/ void AddParentToChild(ObjectReference parentRef, IDataElement childRef, bool memoisedGC)
        {
            if (memoisedGC && childRef is ObjectReference objRef && !objRef.Equals(ObjectReference.Null))
            {
                this[(int)objRef.Location].Increment(parentRef);
            }
        }

        public /*static*/ void UpdateParentOfDifferentChild(ObjectReference parentRef, IDataElement oldChildRef, IDataElement newChildRef, ExplicitActiveState cur)
        {
            if (cur.Configuration.MemoisedGC && oldChildRef != newChildRef)
            {
                RemoveParentFromChild(parentRef, oldChildRef, cur);
                AddParentToChild(parentRef, newChildRef, cur);
            }
        }

        private /*static*/ void UpdateParentFromAllChilds(ObjectReference parentRef, UpdateSingle updater, ExplicitActiveState cur)
        {
            if (cur.Configuration.MemoisedGC)
            {
                DynamicAllocation alloc = cur.DynamicArea.Allocations[parentRef];
                if (alloc == null)
                {
                    return;
                }

                switch (alloc.AllocationType)
                {
                    case AllocationType.Object:
                    case AllocationType.Array:
                        AllocatedObject parent = alloc as AllocatedObject;
                        DataElementList childs = parent.Fields;
                        for (int i = 0; i < childs.Length; i++)
                            updater(parentRef, childs[i], cur);
                        break;
                    case AllocationType.Delegate:
                        AllocatedDelegate allocDelegate = alloc as AllocatedDelegate;
                        updater(parentRef, allocDelegate.Object, cur);
                        break;
                }
            }
        }

        public /*static*/ void UpdateParentFromAllChilds(ObjectReference parentRef, UpdateSingle updater, DataElementList list, ExplicitActiveState cur)
        {
            var memoisedGC = cur.Configuration.MemoisedGC;
            if (memoisedGC && list != null)
            {
                for (int i = 0; i < list.Length; i++)
                {
                    updater(parentRef, list[i], cur);
                }
            }
        }

        public /*static*/ void RemoveParentFromAllChilds(ObjectReference parentRef, DataElementList list, ExplicitActiveState cur)
        {
            UpdateParentFromAllChilds(parentRef, remover, list, cur);
        }

        public /*static*/ void AddParentToAllChilds(ObjectReference parentRef, DataElementList list, ExplicitActiveState cur)
        {
            UpdateParentFromAllChilds(parentRef, adder, list, cur);
        }

        public /*static*/ void RemoveParentFromAllChilds(ObjectReference parentRef, ExplicitActiveState cur)
        {
            UpdateParentFromAllChilds(parentRef, remover, cur);
        }

        public /*static*/ void AddParentToAllChilds(ObjectReference parentRef, ExplicitActiveState cur)
        {
            UpdateParentFromAllChilds(parentRef, adder, cur);
        }

    }
}