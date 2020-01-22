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

using MMC.Data;
using MMC.Util;
using MMC.Collections;
using TypeReference = dnlib.DotNet.TypeRef;
using MethodDefinition = dnlib.DotNet.MethodDef;
using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace MMC.State {
	class StateDecollapser
    {
		// Use the same constants as the ones used in the vectors.
		public const int not_set = ChangingIntVector.not_set;
		public const int deleted = ChangingIntVector.deleted;

		private readonly PoolData m_pool;
        private readonly ExplicitActiveState cur;

        public StateDecollapser(PoolData pool, ExplicitActiveState cur)
        {
            this.cur = cur;
			m_pool = pool;
		}

		public void RestoreState(CollapsedStateDelta delta) {

            // We restore the state in the following order:
            //   - Restore allocations.
            //   - Delete allocations.
            //   - Restore classes.
            //   - Restore call stacks.	
            //   - Reset thread states.
            for (ISparseElement d = delta.Allocations; d != null; d = d.Next) {
				int pool_index = d.DeltaVal;
				int i = d.Index;

				if (pool_index == deleted) {
                    
                    ParentWatcher.RemoveParentFromAllChilds(new ObjectReference(i + 1), cur);
					cur.DynamicArea.DisposeLocation(i);
				} else if (pool_index != not_set) {
					/// The removal of childs is done by the RestoreAllocation method
					///
					/// TODO: should be cleaned up, and moves here
					RestoreAllocation(i, pool_index);
					ParentWatcher.AddParentToAllChilds(new ObjectReference(i + 1), cur);
				}
			}

			for (ISparseElement d = delta.Classes; d != null; d = d.Next) {
				int pool_index = d.DeltaVal;
				int i = d.Index;

				// we don't delete class definitions, but reset its init data,
				// so a class may be "unloaded" here.
				if (pool_index != not_set && pool_index != deleted) {
					AllocatedClass ac = cur.StaticArea.GetClass(i);
					ThreadObjectWatcher.DecrementAll(ac.Fields, cur);
					RestoreClass(i, pool_index);
					ThreadObjectWatcher.IncrementAll(ac.Fields, cur);
				} else if (pool_index == deleted) {
					AllocatedClass ac = cur.StaticArea.GetClass(i);
					ThreadObjectWatcher.DecrementAll(ac.Fields, cur);
					cur.StaticArea.DeleteClassAtLocation(i);
				}
			}

			cur.ThreadPool.SetThreadUpperBound(delta.ThreadsUpperBound);

			for (ISparseElement d = delta.Threads; d != null; d = d.Next) {
				int pool_index = d.DeltaVal;
				int i = d.Index;

				if (pool_index == deleted)
					cur.ThreadPool.DeleteThread(i);
				else if (pool_index != not_set)
					RestoreThread(i, pool_index);
			}
		}

		/////////////////////////////////////////////////////////////////////
		// Heap
		/////////////////////////////////////////////////////////////////////

		void RestoreAllocation(int alloc_id, int pool_index) {

			WrappedIntArray pool_entry = m_pool.GetList(pool_index);
			int alloc_type = pool_entry[AllocationPartsOffsets.AllocationType];
			DynamicAllocation alloc = null;
			switch (alloc_type) {
				case (int)AllocationType.Object:
					alloc = RestoreObject(alloc_id, pool_entry);
					break;
				case (int)AllocationType.Array:
					alloc = RestoreArray(alloc_id, pool_entry);
					break;
				case (int)AllocationType.Delegate:
					alloc = RestoreDelegate(alloc_id, pool_entry);
					break;
				default:
					Logger.l.Warning("unknown allocation type: {0}", alloc_type);
					break;
			}
			if (alloc != null) {
				// Store back in the heap.
				cur.DynamicArea.Allocations[alloc_id] = alloc;
				// Restore the lock (if it has one).
				if (pool_entry[AllocationPartsOffsets.Lock] > CollectionConstants.NotSet)
					alloc.Lock = m_pool.GetLock(pool_entry[AllocationPartsOffsets.Lock]);
				else
					alloc.Lock = null;
			}
		}

		DynamicAllocation RestoreObject(int alloc_id, WrappedIntArray co)
        {
			// If the old allocation is still here, re-use it.
			var type = (ITypeDefOrRef)m_pool.GetObject(co[ObjectPartsOffsets.Definition]);
			DynamicAllocation alloc = cur.DynamicArea.Allocations[alloc_id];

			AllocatedObject obj;

			if (alloc == null) {
				obj = new AllocatedObject(type, cur.Configuration);
			} else {
				obj = (AllocatedObject)alloc;
				ParentWatcher.RemoveParentFromAllChilds(new ObjectReference(alloc_id + 1), obj.Fields);
			}

			DataElementList newFields = m_pool.GetDataElementList(co[ObjectPartsOffsets.Fields]);

			// Overwrite the fields.			
			obj.Fields = newFields;

			return obj;
		}

		DynamicAllocation RestoreArray(int alloc_id, WrappedIntArray ca)
        {
			// Again, check if there is some old value we can re-use.
			var type = (ITypeDefOrRef)m_pool.GetObject(ca[ArrayPartsOffsets.Definition]);

			//int array_length = ca.Length - ArrayPartsOffsets.Count;
			DynamicAllocation alloc = cur.DynamicArea.Allocations[alloc_id];

			DataElementList newFields = m_pool.GetDataElementList(ca[ArrayPartsOffsets.Elements]);
			int array_length = newFields.Length;
			AllocatedArray arr = (alloc == null) ? new AllocatedArray(type, array_length, cur.Configuration) : (AllocatedArray)alloc;

			// Overwrite the elements.
			ParentWatcher.RemoveParentFromAllChilds(new ObjectReference(alloc_id + 1), arr.Fields);
			arr.Fields = newFields;
			return arr;
		}

		DynamicAllocation RestoreDelegate(int alloc_id, WrappedIntArray cd)
        {
            ObjectReference obj_ref = (ObjectReference)m_pool.GetElement(cd[DelegatePartsOffsets.Object]);
			MethodPointer meth_ptr = (MethodPointer)m_pool.GetElement(cd[DelegatePartsOffsets.MethodPointer]);
			DynamicAllocation alloc = cur.DynamicArea.Allocations[alloc_id];

			if (alloc != null) {
				AllocatedDelegate ad = (AllocatedDelegate)alloc;
				ParentWatcher.RemoveParentFromChild(new ObjectReference(alloc_id + 1), ad.Object, cur.Configuration.MemoisedGC);
				ad.Method = meth_ptr;
				ad.Object = obj_ref;
			} else {
				alloc = new AllocatedDelegate(obj_ref, meth_ptr, cur.Configuration);
			}

			return alloc;
		}

		/////////////////////////////////////////////////////////////////////
		// Classes
		/////////////////////////////////////////////////////////////////////

		void RestoreClass(int class_id, int pool_index) {

			WrappedIntArray cc = m_pool.GetList(pool_index);

			AllocatedClass cls = cur.StaticArea.GetClass(class_id);

			//			ThreadObjectWatcher.DecrementAll(int.MaxValue, cls.Fields);

			cls.Fields = m_pool.GetDataElementList(cc[ClassPartsOffsets.Fields]);
			cls.InitData = m_pool.GetInitData(cc[ClassPartsOffsets.InitData]);
			if (cc[ClassPartsOffsets.Lock] >= 0)
				cls.Lock = m_pool.GetLock(cc[ClassPartsOffsets.Lock]);
			else
				cls.Lock = null;

			//			ThreadObjectWatcher.IncrementAll(int.MaxValue, cls.Fields);
		}

		/////////////////////////////////////////////////////////////////////
		// Threads
		/////////////////////////////////////////////////////////////////////

		void RestoreThread(int thread_id, int pool_index) {

			WrappedIntArray collapsed_trd = m_pool.GetList(pool_index);
			ThreadState trd = cur.ThreadPool.Threads[thread_id];
			if (trd.State != collapsed_trd[ThreadPartOffsets.State])
				trd.State = collapsed_trd[ThreadPartOffsets.State];
			trd.WaitingFor = collapsed_trd[ThreadPartOffsets.WaitingFor];

			if (collapsed_trd[ThreadPartOffsets.ExceptionReference] == 0) {
				trd.ExceptionReference = ObjectReference.Null;
			} else
				trd.ExceptionReference = new ObjectReference(collapsed_trd[ThreadPartOffsets.ExceptionReference]);

			if (collapsed_trd[ThreadPartOffsets.CallStack] != not_set)
				RestoreCallStack(thread_id, collapsed_trd[ThreadPartOffsets.CallStack]);
		}

        void RestoreCallStack(int thread_id, int pool_index)
        {
            WrappedIntArray collapsed_frames = m_pool.GetList(pool_index);

            /*
			 * This was originally done */
            /*			cur.ThreadPool.Threads[thread_id].CallStack.StackPointer =
							collapsed_frames.Length;*/

            /* 
			 * Compensate when the collapsed frame is smaller than the stackpointer. We need to decrements the root objects
			 * count here */
            int stackptr = cur.ThreadPool.Threads[thread_id].CallStack.StackPointer;
            if (collapsed_frames.Length < stackptr)
            {
                for (int i = collapsed_frames.Length; i < stackptr; i++)
                {
                    MethodState method = cur.ThreadPool.Threads[thread_id].CallStack[i];
                    ThreadObjectWatcher.DecrementAll(thread_id, method.Locals, cur.Configuration);
                    ThreadObjectWatcher.DecrementAll(thread_id, method.Arguments, cur.Configuration);
                    ThreadObjectWatcher.DecrementAll(thread_id, method.EvalStack, cur.Configuration);
                }
            }

            for (int i = 0; i < collapsed_frames.Length; ++i)
            {
                if (collapsed_frames[i] != not_set)
                {
                    RestoreMethod(thread_id, i, collapsed_frames[i]);
                }
            }
            cur.ThreadPool.Threads[thread_id].CallStack.StackPointer = collapsed_frames.Length;
        }

		void RestoreMethod(int thread_id, int method_id, int pool_index)
        {
            WrappedIntArray cm = m_pool.GetList(pool_index);

			// Check if we can re-use the value on the current stack. If we only
			// have a partially collapsed method here, this is mandatory, not just
			// an optimalization.
			MethodState method = cur.ThreadPool.Threads[thread_id].CallStack[method_id];

			MethodDefinition meth_def = (MethodDefinition)m_pool.GetObject(cm[MethodPartsOffsets.Definition]);
			/*
			 * It is well possible that the CallStack still contains an old methodstate instance
			 * while the stackpointer is below that instance. This is originally implemented as
			 * a caching feature. 
			 * 
			 * However, now we have to compensate that during counting object references
			 */
			bool stillOnStack = method_id < cur.ThreadPool.Threads[thread_id].CallStack.StackPointer;

            if (method == null)
            {
                method = new MethodState(null, null, null, null, cur);
            }

			bool sameMethod = meth_def == method.Definition;

			method.ProgramCounter = m_pool.GetObject(cm[MethodPartsOffsets.ProgramCounter]) as Instruction;
			method.IsExceptionSource = (cm[MethodPartsOffsets.IsExceptionSource] == 1) ? true : false;
			//method.EndFinallyTarget = m_pool.GetObject(cm[MethodPartsOffsets.EndFinallyTarget]) as Instruction;
			method.OnDispose = m_pool.GetObject(cm[MethodPartsOffsets.OnDispose]) as MethodStateCallback;
			method.Definition = meth_def;

			if (cm[MethodPartsOffsets.Arguments] != not_set)
            {
				DataElementList args = m_pool.GetDataElementList(cm[MethodPartsOffsets.Arguments]);

                /*
				 * When the lengths match, it is likely that the lists are similar 
				 * and therefore we can use our difference updater in this
				 * scenario, which is calls childs' parent-objectreferencebag less 
				 * often */
                if (args.Length == method.Arguments.Length && stillOnStack)
                {
                    ThreadObjectWatcher.UpdateDifference(thread_id, method.Arguments, args, cur.Configuration);
                }
                else if (stillOnStack)
                {
                    ThreadObjectWatcher.DecrementAll(thread_id, method.Arguments, cur.Configuration);
                    ThreadObjectWatcher.IncrementAll(thread_id, args, cur.Configuration);
                }
                else
                {
                    ThreadObjectWatcher.IncrementAll(thread_id, args, cur.Configuration);
                }

				method.Arguments = args;
            }

			if (cm[MethodPartsOffsets.Locals] != not_set)
            {
				DataElementList locals = m_pool.GetDataElementList(cm[MethodPartsOffsets.Locals]);

                /*
				 * When the lengths match, it is likely that the lists are similar 
				 * and therefore we can use our difference updater in this
				 * scenario, which is calls childs' parent-objectreferencebag less 
				 * often */
                if (locals.Length == method.Locals.Length && stillOnStack)
                {
                    ThreadObjectWatcher.UpdateDifference(thread_id, method.Locals, locals, cur.Configuration);
                }
                else if (stillOnStack)
                {
                    ThreadObjectWatcher.DecrementAll(thread_id, method.Locals, cur.Configuration);
                    ThreadObjectWatcher.IncrementAll(thread_id, locals, cur.Configuration);
                }
                else
                {
                    ThreadObjectWatcher.IncrementAll(thread_id, locals, cur.Configuration);
                }

				method.Locals = locals;
			}

			if (cm[MethodPartsOffsets.EvalStack] != not_set)
            {
				DataElementStack stack = m_pool.GetDataElementStack(cm[MethodPartsOffsets.EvalStack]);

                /*
				 * When the lengths match, it is likely that the lists are similar 
				 * and therefore we can use our difference updater in this
				 * scenario, with as effect that the childs' parent-objectreferencebag are 
				 * calle less often */
                if (stack.Length == method.EvalStack.Length && stillOnStack)
                {
                    ThreadObjectWatcher.UpdateDifference(thread_id, method.EvalStack, stack, cur.Configuration);
                }
                else if (stillOnStack)
                {
                    ThreadObjectWatcher.DecrementAll(thread_id, method.EvalStack, cur.Configuration);
                    ThreadObjectWatcher.IncrementAll(thread_id, stack, cur.Configuration);
                }
                else
                {
                    ThreadObjectWatcher.IncrementAll(thread_id, stack, cur.Configuration);
                }

				method.EvalStack = stack;
			}

			cur.ThreadPool.Threads[thread_id].CallStack[method_id] = method;
		}
	}
}
