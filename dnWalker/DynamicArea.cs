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

	using System.Collections;
	using System.Diagnostics;
	using MMC.Data;
	using MMC.Util;
    using dnlib.DotNet;
    using System.Collections.Generic;

    // TODO:
    // At least make a note if a destructor exists. If it's trivial to execute
    // it, do it.

    /// Class holding the heap of the VM.
    class DynamicArea :  ICleanable, IStorageVisitable {

		/// Dynamic allocations, i.e. the heap.
		AllocationList m_alloc;
		/// The mapping used for symmetry reduction. 
		IPlacementMapping m_placementMapping;
		/// Manager for (un)locking on allocations.
		LockManager m_lockManager;
		/// Pinned locations.
		HashSet<ObjectReference> m_pinned;
		/// First really free slot.
		int m_freeSlot;
        private readonly IConfig _config;

        public override string ToString() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			string fmt = "{1} Alloc({2} ({3}) -> {4}\n";
			// Leave out the "(refcount)" part if we don't use reference counting.
			/*if (!Config.UseRefCounting)
				fmt = "{1} Alloc({2}) -> {4}\n";*/

			for (int loc = 0; loc < m_alloc.Length; ++loc)
				if (m_alloc[loc] != null) {
					sb.AppendFormat(fmt,
							loc,
							(m_alloc[loc].Pinned ? "*" : " "),
							loc + 1,
							m_alloc[loc].RefCount,
							m_alloc[loc].ToString());
				}
			if (sb.Length == 0)
				sb.Append("Empty.\n");
			return sb.ToString();
		}

		public void Accept(IStorageVisitor visitor) {

			visitor.VisitDynamicArea(this);
		}

		/// \brief Check if the heap is dirty.
		///
		/// This checks if the list itself, or one of its allocations is dirty.
		///
		/// \return True iff the heap is dirty.
		public bool IsDirty() {

			bool retval = m_alloc.IsDirty();
			DynamicAllocation alloc;
			for (int i = 0; !retval && i < m_alloc.Length; ++i) {
				alloc = m_alloc[i];
				retval = alloc != null && alloc.IsDirty();
			}
			return retval;
		}

		/// \brief Set all heap elements as clean.
		///
		/// This cleans the list itself as well as all allocations.
		public void Clean() {

			// Clean up locations.
			m_alloc.Clean();
			
			/*
			 * We don't check for dirtyness, just clean is it
			 * faster anyways */
			for (int i = 0; i < m_alloc.Length; ++i) {
				DynamicAllocation alloc = m_alloc[i];
				if (alloc != null)
					alloc.Clean();
			}
		}

		/// \brief Get the list of dirty allocations.
		///
		/// This includes locations that have been dirtied (e.g. an allocation
		/// was deleted), as well as allocations that have been altered.
		/// The list contains offsets in the allocations list.
		///
		/// \return List of indeces of dirty allocations.
		public DirtyList DirtyAllocations {

			get {

				DirtyList retval = m_alloc.DirtyLocations;

				DynamicAllocation alloc;

				for (int i = 0; i < m_alloc.Length; ++i) {
					// Skip elements whose location is dirty.

					alloc = m_alloc[i];
					if (alloc != null && alloc.IsDirty())
						retval.SetDirty(i);
				}
				return retval;
			}
		}

		/// Get the list of all pinned allocations.
		public HashSet<ObjectReference> PinnedAllocations {

			get { return m_pinned; }
		}

		/// \brief Set an allocation to be pinned-down.
		///
		/// Pinned-down allocations are never deleted in GC runs. Typical use
		/// for this is thread objects of running threads. Pinning down
		/// null-references is okay but doesn't actually do anything.
		///
		/// Objects can be pinned multiple times. All pins need to be removed
		/// before an object is to be released by the GC if it's no longer
		/// referenced. Very much like reference counting.
		///
		/// \param objRef Reference to the allocation being pinned down.
		/// \param pin True iff the object is to be pinned-down.
		public void SetPinnedAllocation(ObjectReference objRef, bool pin)
        {
            if (objRef.Location > 0)
            {
                DynamicAllocation alloc = m_alloc[objRef];
                Debug.Assert(alloc != null, "(Un)pinning non-existent allocation " + objRef + ".");
                if (pin)
                {
                    Logger.l.Debug("pinning allocation " + objRef + ".");
                    alloc.Pinned = true;
                    m_pinned.Add(objRef);
                    ParentWatcher.AddParentToChild(ParentWatcher.RootObjectReference, objRef, _config.MemoisedGC);
                }
                else
                {
                    Debug.Assert(m_pinned.Contains(objRef), "Unpinning unpinned " + objRef + ". (chk 1)");
                    Debug.Assert(alloc.Pinned, "Unpinning unpinned " + objRef + ". (chk 2)");
                    Logger.l.Debug("unpinning allocation " + objRef + ".");
                    alloc.Pinned = false;
                    if (!alloc.Pinned)
                    {
                        m_pinned.Remove(objRef);
                        ParentWatcher.RemoveParentFromChild(ParentWatcher.RootObjectReference, objRef, _config.MemoisedGC);
                    }
                }
            }

			// TODO, all increments are done at the places there SetPinnedAllocation is called.., it should be different)
		}

        public DynamicArea(IConfig config)
        {
            m_lockManager = new LockManager();
            m_alloc = new AllocationList();
            m_pinned = new HashSet<ObjectReference>();
            m_freeSlot = 0;
            _config = config;

            if (config.SymmetryReduction)
            {
                m_placementMapping = new PlacementMapping();
            }
        }

		// ------------------------- Allocation Related -------------------------

		/// \brief Allocate a new object of the given type at the given place.
		///
		/// \param loc The location to put the object.				
		/// \param typeDef The type of the object to create.
		/// \return A reference to the newly created object.
		/// \sa DeterminePlacement
		public ObjectReference AllocateObject(int loc, ITypeDefOrRef typeDef, IConfig config) {

			AllocatedObject newObj = new AllocatedObject(typeDef, config);
			newObj.ClearFields();
			m_alloc[loc] = newObj;
			return new ObjectReference(loc + 1);
		}

		/// \brief Allocate a new array of the given type and length at the given place.
		///
		/// \param loc The location to put the array.
		/// \param typeDef The element type of the array.
		/// \param length The length of the array.
		/// \return A reference to the newly created array.
		/// \sa DeterminePlacement
		public ObjectReference AllocateArray(int loc, ITypeDefOrRef typeDef, int length, IConfig config)
        {
			AllocatedArray newArr = new AllocatedArray(typeDef, length, config);
			newArr.ClearFields();
			m_alloc[loc] = newArr;
			return new ObjectReference(loc + 1);
		}

		/// \brief Allocate a new delegate at the given place.
		///
		/// \param loc The location to put the delegate.
		/// \param obj The object to invoke the method on.
		/// \param ptr A pointer to the method to invoke.
		/// \return A reference to the newly created delegate.
		/// \sa DeterminePlacement
		public ObjectReference AllocateDelegate(int loc, ObjectReference obj, MethodPointer ptr, IConfig config)
        {
			m_alloc[loc] = new AllocatedDelegate(obj, ptr, config);
			ObjectReference newDelRef = new ObjectReference(loc + 1);
			ParentWatcher.AddParentToChild(newDelRef, obj, config.MemoisedGC);
			return newDelRef;
		}

        /// <summary>
        /// Increase the reference count of the referenced allocation.
        /// </summary>
        /// <param name="obj">A reference to the allocation.</param>
        public void IncRefCount(ObjectReference obj)
        {
            if (obj.Location != 0 && _config.UseRefCounting)
            {
                DynamicAllocation alloc = Allocations[obj];
                Debug.Assert(alloc != null, "allocation of to change ref.count is null");
                alloc.RefCount++;
            }
        }

        /// Decrease the reference count of the referenced allocation.
        ///
        /// If the reference count reaches zero, this deletes the allocation!
        ///
        /// \param obj A reference to the allocation.
        public void DecRefCount(ObjectReference obj)
        {
            if (obj.Location != 0 && _config.UseRefCounting)
            {
                DynamicAllocation alloc = Allocations[obj];
                Debug.Assert(alloc != null, "allocation of to change ref.count is null");
                alloc.RefCount--;
                if (alloc.RefCount == 0)
                    DisposeAllocation(obj);
            }
        }

		// ------------------------- Accessors -------------------------

		/// <summary>
		/// List of all allocation on this heap.
		/// </summary>
		public AllocationList Allocations
        {
			get { return m_alloc; }
		}

		/// <summary>
		/// Manager class for locking of the allocations.
		/// </summary>
		public LockManager LockManager
        {
			get { return m_lockManager; }
		}

		// ------------------------- Garbage collection -------------------------

		/// Dispose allocation on the given location.
		///
		/// \param loc The location (offset in the list) of the allocation to
		/// delete.
		public void DisposeLocation(int loc) {

			Debug.Assert(loc >= 0, "Deleting location at index < 0.");
			if (m_alloc[loc] != null)
				m_alloc[loc].Dispose();
			m_alloc[loc] = null;
		}

		/// Dispose allocation on the given location.
		///
		/// \param obj Reference to the object of the allocation to delete.
		public void DisposeAllocation(ObjectReference obj) {

			DisposeLocation(((int)obj.Location) - 1);
		}


		//		int DeleteZeroRefCount() {
		//
		//			int retval = 0;
		//			bool removed_something = true;
		//			while (removed_something) {
		//				removed_something = false;
		//				for (int i=0; i < m_alloc.Length; ++i)
		//					if (m_alloc[i] != null && m_alloc[i].RefCount == 0 && !m_alloc[i].Pinned) {
		//						DisposeLocation(i);
		//						removed_something = true;
		//						++retval;
		//					}
		//			}
		//			return retval;
		//		}

		// ------------------------- Symmetry Reduction Helpers -------------------------

		/// \brief Get a new free slot.
		///
		/// This location has never been used before. Backtracking etc. has no
		/// influence on this behavior.
		///
		/// \return A never before used location.
		public int FreeSlot() {

			return m_freeSlot++;
		}

		/// \brief Determine the placement for an allocation being allocated at
		/// the current instruction.
		///
		/// This calls DeterminePlacement(true).
		///
		/// \return The place to put the allocation.
		public int DeterminePlacement() {

			return DeterminePlacement(true);
		}

		/// \brief Determine the placement for an allocation.
		///
		/// If byCil is true, the current instruction is used to determine the
		/// placement. If byCil is false, or if Config.SymmetryReduction is
		/// false, we return a free slot.
		///
		/// We could also check for the absence of any CIL instruction to be
		/// executed, as this is (atm) only called from SetupMainState.
		///
		/// \param byCil Determine the place by the current instruction.
		/// \return The place to put the allocation.
		/// \sa PlacementMapping
		/// \sa FreeSlot
		public int DeterminePlacement(bool byCil)
        {
			int retval;
			if (!_config.SymmetryReduction || !byCil)
				retval = FreeSlot();
			else
				retval = m_placementMapping.GetLocation();
			Logger.l.Debug("new allocation will be placed at {0}", retval);
			return retval;
		}
	}
}
