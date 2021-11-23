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

    using MMC.Data;
    using MMC.Util;
    using MMC.Collections;


    /// Implementation of a state collapser.
    ///
    /// This implementation collapses a state using the values of references,
    /// so the offset in the heap <b>is</b> relevant. In other words, garbage
    /// collection is mandatory to keep the heap canonical, and ensure equal
    /// states are seen as such.
    ///
    /// The data collapser (i.e. the one that substitutes the actual values of
    /// e.g. an object for its reference) is depcricated, which is not a great
    /// loss since it was way too optimistic an had serious issues to be dealt
    /// with (e.g. object graph isomorphism and multi-reference handling just
    /// to name two).
    ///
    /// Since there are no partial or friend classes in C# 1, this class is
    /// somewhat larger than I would like.
    ///
    /// This implementation was written with extensibility in mind, not speed.
    class StateCollapser
    {

        // Use the same constants as the ones used in the vectors.
        public const int not_set = ChangingIntVector.not_set;
        public const int deleted = ChangingIntVector.deleted;

        CollapsedState m_curstate;
        PoolData m_pool;

        public StateCollapser(PoolData pool)
        {
            m_pool = pool;
        }

        /// <summary>
        /// Reset the collapser after an interruption in the chain of states.
        /// </summary>
        /// This sets the specified state as the last visited (and collapsed)
        /// state. This method should be called after backtracking.
        /// <param name="s">The restored (collapsed) state.</param>
        public void Reset(CollapsedState s)
        {
            m_curstate = s;

            /*
			if (s is CollapsedState)
				m_curstate = (CollapsedState)s;
			else
				throw new System.ArgumentException("s");*/
        }

        public CollapsedState GetStorableState(ExplicitActiveState cur)
        {
            m_curstate = m_curstate == null ? new CollapsedState(cur) : m_curstate.Clone();

            CollapseThreads(cur);

            if (cur.DynamicArea.IsDirty())
            {
                CollapseDynamicArea(cur);
            }
            if (cur.StaticArea.IsDirty())
            {
                CollapseStaticArea(cur);
            }

            // MonoModelChecker.Message(m_pool.ToString());
            // cur.Clean();
            return m_curstate;
        }

        public static CollapsedState CollapseCurrent(ExplicitActiveState state)
        {
            return new StateCollapser(new PoolData()).GetStorableState(state);
        }

        private void CollapseDynamicArea(ExplicitActiveState cur)
        {
            foreach (var da in cur.DynamicArea.DirtyAllocations)
            {
                var alloc = cur.DynamicArea.Allocations[da];
                if (alloc != null)
                {
                    m_curstate.Allocations[da] = CollapseAllocation(alloc);
                }
                else if (da < m_curstate.Allocations.Length && m_curstate.Allocations[da] != not_set)
                {
                    /*
					 * In rare cases, the length of the state vector is smaller than the index of da,
					 * happens when the state space consists of only one state
					 */
                    m_curstate.Allocations[da] = not_set;
                }
            }
        }

        private int CollapseAllocation(DynamicAllocation alloc)
        {
            WrappedIntArray collapsed_alloc = null;
            // Bit dirty...
            switch (alloc.AllocationType)
            {
                case AllocationType.Object:
                    collapsed_alloc = CollapseObject((AllocatedObject)alloc);
                    break;
                case AllocationType.Array:
                    collapsed_alloc = CollapseObject((AllocatedArray)alloc);
                    collapsed_alloc[ArrayPartsOffsets.AllocationType] = (int)AllocationType.Array;
                    break;
                case AllocationType.Delegate:
                    collapsed_alloc = CollapseDelegate((AllocatedDelegate)alloc);
                    break;
                default:
                    break;
            }

            if (collapsed_alloc != null)
            {
                collapsed_alloc[AllocationPartsOffsets.Lock] = (alloc.Lock != null) ? m_pool.GetInt(alloc.Lock) : not_set;
            }

            return m_pool.GetInt(collapsed_alloc);
        }

        private WrappedIntArray CollapseObject(AllocatedObject ao)
        {
            var col = new WrappedIntArray(ObjectPartsOffsets.Count);
            col[ObjectPartsOffsets.AllocationType] = (int)AllocationType.Object;
            col[ObjectPartsOffsets.Definition] = m_pool.GetInt(ao.Type);
            col[ObjectPartsOffsets.Fields] = m_pool.GetInt(ao.Fields);
            return col;
        }

        private WrappedIntArray CollapseDelegate(AllocatedDelegate ad)
        {
            var col = new WrappedIntArray(DelegatePartsOffsets.Count);
            col[DelegatePartsOffsets.AllocationType] = (int)AllocationType.Delegate;
            col[DelegatePartsOffsets.Object] = m_pool.GetInt(ad.Object);
            col[DelegatePartsOffsets.MethodPointer] = m_pool.GetInt(ad.Method);
            return col;
        }

        private void CollapseStaticArea(ExplicitActiveState cur)
        {
            foreach (var dirty_class in cur.StaticArea.DirtyClasses)
            {
                var ac = cur.StaticArea.Classes[dirty_class];
                m_curstate.Classes[dirty_class] = CollapseClass(ac);
            }
        }

        private int CollapseClass(AllocatedClass ac)
        {

            var row = new WrappedIntArray(ClassPartsOffsets.Count);
            row[ClassPartsOffsets.InitData] = m_pool.GetInt(ac.InitData);
            row[ClassPartsOffsets.Fields] = m_pool.GetInt(ac.Fields);

            if (ac.Locked)
                row[ClassPartsOffsets.Lock] = m_pool.GetInt(ac.Lock);
            else
                row[ClassPartsOffsets.Lock] = not_set;

            return m_pool.GetInt(row);
        }

        private void CollapseThreads(ExplicitActiveState cur)
        {
            foreach (var thread_id in cur.ThreadPool.DirtyThreads)
            {
                // MonoModelChecker.Message("thread {0} is dirty.", thread_id);
                var trd = cur.ThreadPool.Threads[thread_id];
                if (trd != null)
                {
                    var collapsed_trd = (thread_id < m_curstate.Threads.Length ?
                            m_pool.GetList(m_curstate.Threads[thread_id]) : null);

                    collapsed_trd = (collapsed_trd == null) ?
                            new WrappedIntArray(ThreadPartOffsets.Count) : collapsed_trd.Clone();

                    collapsed_trd[ThreadPartOffsets.State] = (int)trd.State;
                    collapsed_trd[ThreadPartOffsets.WaitingFor] = trd.WaitingFor;
                    collapsed_trd[ThreadPartOffsets.ExceptionReference] = (int)trd.ExceptionReference.Location;

                    if (trd.CallStack.IsDirty())
                    {
                        collapsed_trd[ThreadPartOffsets.CallStack] =
                            CollapseCallStack(trd.CallStack,
                                    collapsed_trd[ThreadPartOffsets.CallStack]);
                    }

                    m_curstate.Threads[thread_id] = m_pool.GetInt(collapsed_trd);
                }
                else
                {
                    m_curstate.Threads[thread_id] = not_set;
                }
            }
        }

        private int CollapseCallStack(CallStack stack, int pool_index)
        {
            // MonoModelChecker.Message("called CollapseCallStacks()...");
            // If available, get the previous call stack from the pool.
            var cs = GetOldList(pool_index, stack.StackPointer);

            // Collapse dirty frames.
            foreach (var frame in stack.DirtyFrames)
            {
                //				MonoModelChecker.Message("frame {0} is dirty.", frame);
                cs[frame] = CollapseMethod(stack[frame], cs[frame]);
            }

            return m_pool.GetInt(cs);
        }

        private int CollapseMethod(MethodState method, int pool_index)
        {
            // First, attempt to get the old values for this method.
            var cm = m_pool.GetList(pool_index);
            var def = m_pool.GetInt(method.Definition);

            // Check if we found something sensible. Otherwise start anew.

            if (cm != null)
            {
                if (cm[MethodPartsOffsets.Definition] == def)
                    cm = cm.Clone();
                else
                    cm = null;
            }

            if (cm == null)
                cm = new WrappedIntArray(MethodPartsOffsets.Count);

            // Update the collapsed method array by collapsing the different parts.
            // Note that if cm[ part_X_offset ] == not_set, part X should be dirty.
            cm[MethodPartsOffsets.Definition] = def;
            cm[MethodPartsOffsets.ProgramCounter] = m_pool.GetInt(method.ProgramCounter);
            cm[MethodPartsOffsets.IsExceptionSource] = method.IsExceptionSource ? 1 : 0;
            //cm[MethodPartsOffsets.EndFinallyTarget] = m_pool.GetInt(method.EndFinallyTarget);
            cm[MethodPartsOffsets.OnDispose] = m_pool.GetInt(method.OnDispose);

            if (method.Arguments.IsDirty())
                cm[MethodPartsOffsets.Arguments] = m_pool.GetInt(method.Arguments);
            //else if (cm[MethodPartsOffsets.Arguments] == not_set)
            //	MonoModelChecker.Message("arguments is still not-set. not dirty either.");

            if (method.Locals.IsDirty())
                cm[MethodPartsOffsets.Locals] = m_pool.GetInt(method.Locals);
            //else if (cm[MethodPartsOffsets.Locals] == not_set)
            //	MonoModelChecker.Message("locals is still not-set. not dirty either.");

            if (method.EvalStack.IsDirty())
                cm[MethodPartsOffsets.EvalStack] = m_pool.GetInt(method.EvalStack);
            //else if (cm[MethodPartsOffsets.EvalStack] == not_set)
            //	MonoModelChecker.Message("evalstack is still not-set. not dirty either.");

            return m_pool.GetInt(cm);
        }

        private WrappedIntArray GetOldList(int pool_index, int intended_length)
        {
            var retval = m_pool.GetList(pool_index);

            if (retval != null)
            {
                retval = new WrappedIntArray(retval, intended_length);
            }
            else
            {
                retval = new WrappedIntArray(intended_length);
            }

            return retval;
        }
    }
}