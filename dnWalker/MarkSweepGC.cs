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
    using System.Diagnostics;
    using System.Collections.Generic;
    using MMC.Data;

    class MarkAndSweepGC : BaseStorageVisitor, IStorageVisitor, IGarbageCollector
    {
        Queue<int> todo = new Queue<int>();

        public void Run(ExplicitActiveState cur)
        {
            Mark(cur);
            Sweep(cur);
        }

        public void Mark(ExplicitActiveState cur)
        {
            todo.Clear();
            // reset the flags
            AllocationList m_alloc = cur.DynamicArea.Allocations;
            for (int i = 0; i < m_alloc.Length; ++i)
            {
                DynamicAllocation ida = m_alloc[i];
                if (ida != null)
                    ida.HeapAttribute = AllocatedObject.UNMARKED;
            }

            // Mark all pinned down allocations.
            foreach (ObjectReference pinnedObjRef in cur.DynamicArea.PinnedAllocations)
            {
                Mark(cur, pinnedObjRef, 0);
            }

            // Mark all thread objects and referenced allocations in all method states.
            ThreadState t_cur;
            MethodState m_cur;
            for (int t_id = 0; t_id < cur.ThreadPool.Threads.Length; ++t_id)
            {
                t_cur = cur.ThreadPool.Threads[t_id];
                // The thread object is no longer marked, since it's pinned down.
                if (t_cur != null && t_cur.IsAlive)
                {
                    for (int m_id = 0; m_id < t_cur.CallStack.StackPointer; ++m_id)
                    {
                        m_cur = t_cur.CallStack[m_id];
                        MarkContainer(cur, m_cur.Arguments, m_cur.Arguments.Length, t_id);
                        MarkContainer(cur, m_cur.Locals, m_cur.Locals.Length, t_id);
                        MarkContainer(cur, m_cur.EvalStack, m_cur.EvalStack.StackPointer, t_id);
                    }
                }
            }

            // Mark referenced allocations in the static area.
            foreach (int lcls in cur.StaticArea.LoadedClasses)
            {
                AllocatedClass ac = cur.StaticArea.Classes[lcls];
                for (int f_id = 0; f_id < ac.Fields.Length; ++f_id)
                {
                    if (ac.Fields[f_id] is ObjectReference)
                        Mark(cur, (ObjectReference)ac.Fields[f_id], AllocatedObject.SHARED);
                }
            }

            MarkRecursive(cur);
        }

        public void MarkSharedRecursive(ExplicitActiveState cur, ObjectReference objRef)
        {
            Mark(cur, objRef, AllocatedObject.SHARED);
            MarkRecursive(cur);
        }

        public void MarkRecursive(ExplicitActiveState cur)
        {
            // Mark recursive
            DynamicAllocation to_visit;
            int loc;
            // Perform BFS, pushing new references to visit on the m_todo stack.
            while (todo.Count > 0)
            {
                loc = todo.Dequeue();
                to_visit = cur.DynamicArea.Allocations[loc];
                Debug.Assert(to_visit != null, "Cannot visit a null allocation.");


                if (to_visit is AllocatedDelegate)
                {
                    AllocatedDelegate ad = to_visit as AllocatedDelegate;
                    Mark(cur, ad.Object, ad.HeapAttribute);
                }
                else if (to_visit is AllocatedObject)
                {
                    AllocatedObject ao = to_visit as AllocatedObject;
                    MarkContainer(cur, ao.Fields, ao.Fields.Length, ao.HeapAttribute);
                }
            }
        }

        void Mark(ExplicitActiveState cur, ObjectReference o, int tid)
        {
            int loc = (int)o.Location - 1;
            // Watch out for null references, and references to unallocated space.
            if (loc >= 0)
            {
                DynamicAllocation alloc = cur.DynamicArea.Allocations[loc];
                // If not yet seen, we need to examine this one further when marking
                // allocations recursively. 
                if (alloc != null)
                {
                    if (alloc.HeapAttribute == AllocatedObject.UNMARKED)
                    {
                        alloc.HeapAttribute = tid;
                        todo.Enqueue(loc);
                    }
                    else if (alloc.HeapAttribute != tid && alloc.HeapAttribute != AllocatedObject.SHARED)
                    {
                        alloc.HeapAttribute = AllocatedObject.SHARED;
                        todo.Enqueue(loc);
                    }
                }
            }
        }

        void MarkContainer(ExplicitActiveState cur, IDataElementContainer dec, int length, int tid)
        {
            for (int i = 0; i < length; ++i)
                if (dec[i] is ObjectReference)
                    Mark(cur, (ObjectReference)dec[i], tid);
        }

        public int Sweep(ExplicitActiveState cur)
        {
            int retval = 0;
            // var cur = cur;
            AllocationList m_alloc = cur.DynamicArea.Allocations;
            for (int i = 0; i < m_alloc.Length; ++i)
            {
                DynamicAllocation ida = m_alloc[i];
                if (ida != null)
                {
                    // If marked, unmark (for next run), else delete.
                    if (ida.HeapAttribute == AllocatedObject.UNMARKED && !ida.Pinned)
                    {
                        cur.ParentWatcher.RemoveParentFromAllChilds(new ObjectReference(i + 1), cur);
                        cur.DynamicArea.DisposeLocation(i);
                        ++retval;
                    }
                }
            }
            return retval;
        }

        /*
		public static readonly MarkAndSweepGC msgc = new MarkAndSweepGC();

	
		Stack<int> m_todoReachable;
		Stack<int> m_todoThreadShared;

		int m_count;

		public MarkAndSweepGC() {
			m_todoReachable = new Stack<int>();
			m_todoThreadShared = new Stack<int>();
		}
		
		public void Run() {
			RunMark(false);
			this.RunSweep();
		}
		
		public int Count {

			get { return m_count; }
		}

		public void RunMark(bool porOnly) {
			m_todoReachable.Clear();
			m_todoThreadShared.Clear();

			m_count = 0;
			
			// reset the flags
			AllocationList m_alloc = cur.DynamicArea.Allocations;
			for (int i = 0; i < m_alloc.Length; ++i) {
				DynamicAllocation ida = m_alloc[i];
				if (ida != null) {
					ida.Marked = false;
					ida.ThreadShared = false;
				}
			}			

			MarkRoots();
			MarkSharedRecursive();
			if (!porOnly) {
				MarkReachableRecursive();
			}
		}

		public int RunSweep() {
			int retval = 0;
			AllocationList m_alloc = cur.DynamicArea.Allocations;
			for (int i = 0; i < m_alloc.Length; ++i) {
				DynamicAllocation ida = m_alloc[i];
				if (ida != null) {
					// If marked, unmark (for next run), else delete.
					if (!ida.Marked && !ida.Pinned) {
						ParentWatcher.RemoveParentFromAllChilds(new ObjectReference(i + 1));
						cur.DynamicArea.DisposeLocation(i);
						++retval;
					}
				}
			}
			return retval;
		}

		void MarkRoots() {

			// Mark all pinned down allocations.
			foreach (ObjectReference pinnedObjRef in cur.DynamicArea.PinnedAllocations)
				MarkReachable(pinnedObjRef);

			// Mark all thread objects and referenced allocations in all method states.
			ThreadState t_cur;
			MethodState m_cur;
			for (int t_id = 0; t_id < cur.ThreadPool.Threads.Length; ++t_id) {
				t_cur = cur.ThreadPool.Threads[t_id];
				// The thread object is no longer marked, since it's pinned down.
				if (t_cur != null && t_cur.IsAlive) {
					for (int m_id = 0; m_id < t_cur.CallStack.StackPointer; ++m_id) {
						m_cur = t_cur.CallStack[m_id];
						MarkThreadInList(m_cur.Arguments, t_id);
						MarkThreadInList(m_cur.Locals, t_id);
						MarkThreadInStack(m_cur.EvalStack, t_id);
					}
				}
			}

			// Mark referenced allocations in the static area.
			foreach (int lcls in cur.StaticArea.LoadedClasses) {
				AllocatedClass ac = cur.StaticArea.Classes[lcls];
				for (int f_id = 0; f_id < ac.Fields.Length; ++f_id) {
//					if (ac.Type.Fields[f_id].IsStatic && ac.Fields[f_id] is ObjectReference)
					if (ac.Fields[f_id] is ObjectReference)
						MarkShared((ObjectReference)ac.Fields[f_id]);
				}
			}
		}

		void MarkThreadInList(DataElementList list, int thread) {

			for (int i = 0; i < list.Length; ++i)
				if (list[i] is ObjectReference)
					MarkThread((ObjectReference)list[i], thread);
		}

		void MarkThreadInStack(DataElementStack stack, int thread) {

			for (int i = 0; i < stack.StackPointer; ++i)
				if (stack[i] is ObjectReference)
					MarkThread((ObjectReference)stack[i], thread);
		}

		void MarkSharedInList(DataElementList list) {

			for (int i = 0; i < list.Length; ++i)
				if (list[i] is ObjectReference)
					MarkShared((ObjectReference)list[i]);
		}

		void MarkReachableInList(DataElementList list) {

			for (int i = 0; i < list.Length; ++i)
				if (list[i] is ObjectReference)
					MarkReachable((ObjectReference)list[i]);
		}

		void MarkReachable(ObjectReference o) {
			int loc = (int)o.Location - 1;
			// Watch out for null references, and references to unallocated space.
			if (loc >= 0) {
				DynamicAllocation alloc = cur.DynamicArea.Allocations[loc];
				// If not yet seen, we need to examine this one further when marking
				// allocations recursively. 
				if (alloc != null && !alloc.Marked) {
					m_todoReachable.Push(loc);
					alloc.Marked = true;
					m_count++;
				}
			}
		}

		void MarkThread(ObjectReference o, int threadId) {
			int loc = (int)o.Location - 1;
			// Watch out for null references, and references to unallocated space.
			if (loc >= 0) {
				DynamicAllocation alloc = cur.DynamicArea.Allocations[loc];
				// If not yet seen, we need to examine this one further when marking
				// allocations recursively. 
				if (alloc != null && !alloc.Marked) {
					alloc.FirstThread = threadId;
					alloc.Marked = true;
					m_todoReachable.Push(loc);
					m_count++;
				} else {
					if (threadId != alloc.FirstThread) {
						alloc.ThreadShared = true;
						m_todoThreadShared.Push(loc);
					}
				}
			}
		}

		void MarkShared(ObjectReference o) {
			int loc = (int)o.Location - 1;
			// Watch out for null references, and references to unallocated space.
			if (loc >= 0) {
				DynamicAllocation alloc = cur.DynamicArea.Allocations[loc];
				// If not yet seen, we need to examine this one further when marking
				// allocations recursively. 
				if (alloc != null && !alloc.ThreadShared) {
					alloc.Marked = true;
					alloc.ThreadShared = true;
					m_count++;
					m_todoThreadShared.Push(loc);
				}
			}
		}


		void MarkSharedRecursive() {

			DynamicAllocation to_visit;
			int loc;
			// Perform BFS, pushing new references to visit on the m_todo stack.
			while (m_todoThreadShared.Count > 0) {
				loc = m_todoThreadShared.Pop();
				to_visit = cur.DynamicArea.Allocations[loc];
				Debug.Assert(to_visit != null, "Cannot visit a null allocation.");
				// Use a partial visitor implementation to call the correct method for
				// each allocation type.

				if (to_visit is AllocatedDelegate) {
					AllocatedDelegate ad = to_visit as AllocatedDelegate;
					MarkShared(ad.Object);
				} else if (to_visit is AllocatedObject) {
					AllocatedObject ao = to_visit as AllocatedObject;
					MarkSharedInList(ao.Fields);
				}
			}
		}

		public static int TEL = 0; // TODO WEGHALEN

		void MarkReachableRecursive() {

			DynamicAllocation to_visit;
			int loc;
			// Perform BFS, pushing new references to visit on the m_todo stack.
			while (m_todoReachable.Count > 0) {
				TEL++;
				loc = m_todoReachable.Pop();
				to_visit = cur.DynamicArea.Allocations[loc];
				Debug.Assert(to_visit != null, "Cannot visit a null allocation.");
				// Use a partial visitor implementation to call the correct method for
				// each allocation type.

				if (to_visit is AllocatedDelegate) {
					AllocatedDelegate ad = to_visit as AllocatedDelegate;
					MarkReachable(ad.Object);
				} else if (to_visit is AllocatedObject || to_visit is AllocatedArray) {
					AllocatedObject ao = to_visit as AllocatedObject;
					MarkReachableInList(ao.Fields);
				}
			}
		}*/


        /*
		public override void VisitAllocatedObject(AllocatedObject ao) {

			MarkAllInList(ao.Fields, ao.Depth + 1);
		}

		public override void VisitAllocatedArray(AllocatedArray aa) {

			MarkAllInList(aa.Elements, aa.Depth + 1);
		}

		public override void VisitAllocatedDelegate(AllocatedDelegate ad) {

			MarkReference(ad.Object, ad.Depth + 1);
		}*/
    }
}