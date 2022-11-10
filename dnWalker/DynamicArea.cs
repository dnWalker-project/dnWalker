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
    using System;

    // TODO:
    // At least make a note if a destructor exists. If it's trivial to execute
    // it, do it.

    public interface IDynamicArea
	{
		ObjectReference AllocateObject(int location, ITypeDefOrRef type);
		ObjectReference AllocateArray(int location, ITypeDefOrRef elementType, int length);

        int DeterminePlacement(bool enableSymetryReduction = false);

		AllocationList Allocations { get; }

		void DisposeLocation(int location);
		void DisposeAllocation(ObjectReference objectReference);
	}

	/// Class holding the heap of the VM.
	public class DynamicArea : IDynamicArea,  ICleanable, IStorageVisitable {

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
        private readonly ExplicitActiveState _cur;

        public override string ToString() {

			var sb = new System.Text.StringBuilder();

			var fmt = "{1} Alloc({2} ({3}) -> {4}\n";
			// Leave out the "(refcount)" part if we don't use reference counting.
			/*if (!IConfiguration.UseRefCounting)
				fmt = "{1} Alloc({2}) -> {4}\n";*/

			for (var loc = 0; loc < m_alloc.Length; ++loc)
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

		public void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
			visitor.VisitDynamicArea(this, cur);
		}

		/// <summary>Check if the heap is dirty.</summary>
		///
		/// This checks if the list itself, or one of its allocations is dirty.
		///
		/// <returns>True iff the heap is dirty.</returns>
		public bool IsDirty() {

			var retval = m_alloc.IsDirty();
			DynamicAllocation alloc;
			for (var i = 0; !retval && i < m_alloc.Length; ++i) {
				alloc = m_alloc[i];
				retval = alloc != null && alloc.IsDirty();
			}
			return retval;
		}

		/// <summary>Set all heap elements as clean.</summary>
		///
		/// This cleans the list itself as well as all allocations.
		public void Clean() {

			// Clean up locations.
			m_alloc.Clean();
			
			/*
			 * We don't check for dirtyness, just clean is it
			 * faster anyways */
			for (var i = 0; i < m_alloc.Length; ++i) {
				var alloc = m_alloc[i];
				if (alloc != null)
					alloc.Clean();
			}
		}

		/// <summary>Get the list of dirty allocations.</summary>
		///
		/// This includes locations that have been dirtied (e.g. an allocation
		/// was deleted), as well as allocations that have been altered.
		/// The list contains offsets in the allocations list.
		///
		/// <returns>List of indeces of dirty allocations.</returns>
		public DirtyList DirtyAllocations {

			get {

				var retval = m_alloc.DirtyLocations;

				DynamicAllocation alloc;

				for (var i = 0; i < m_alloc.Length; ++i) {
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

		/// <summary>Set an allocation to be pinned-down.</summary>
		///
		/// Pinned-down allocations are never deleted in GC runs. Typical use
		/// for this is thread objects of running threads. Pinning down
		/// null-references is okay but doesn't actually do anything.
		///
		/// Objects can be pinned multiple times. All pins need to be removed
		/// before an object is to be released by the GC if it's no longer
		/// referenced. Very much like reference counting.
		///
		/// <param name="objRef">Reference to the allocation being pinned down.</param>
		/// <param name="pin">True iff the object is to be pinned-down.</param>
		public void SetPinnedAllocation(ObjectReference objRef, bool pin)
        {
            if (objRef.Location > 0)
            {
                var alloc = m_alloc[objRef];
                Debug.Assert(alloc != null, "(Un)pinning non-existent allocation " + objRef + ".");
                if (pin)
                {
                    _cur.Logger.Debug("pinning allocation " + objRef + ".");
                    alloc.Pinned = true;
                    m_pinned.Add(objRef);
                    _cur.ParentWatcher.AddParentToChild(_cur.ParentWatcher.RootObjectReference, objRef, _cur.Configuration.MemoisedGC());
                }
                else
                {
                    Debug.Assert(m_pinned.Contains(objRef), "Unpinning unpinned " + objRef + ". (chk 1)");
                    Debug.Assert(alloc.Pinned, "Unpinning unpinned " + objRef + ". (chk 2)");
                    _cur.Logger.Debug("unpinning allocation " + objRef + ".");
                    alloc.Pinned = false;
                    if (!alloc.Pinned)
                    {
                        m_pinned.Remove(objRef);
                        _cur.ParentWatcher.RemoveParentFromChild(_cur.ParentWatcher.RootObjectReference, objRef, _cur.Configuration.MemoisedGC());
                    }
                }
            }

			// TODO, all increments are done at the places there SetPinnedAllocation is called.., it should be different)
		}

        public DynamicArea(ExplicitActiveState cur)
        {
            m_lockManager = new LockManager(cur);
            m_alloc = new AllocationList();
            m_pinned = new HashSet<ObjectReference>();
            m_freeSlot = 0;
            _cur = cur;

            if (cur.Configuration.SymmetryReduction())
            {
                m_placementMapping = new PlacementMapping();
            }
        }

        // ------------------------- Allocation Related -------------------------

        /// <summary>
        /// Allocate a new object of the given type at the given place.
        /// </summary>
        /// <param name="loc">The location to put the object.</param>
        /// <param name="typeDef">The type of the object to create.</param>
        /// <seealso cref="DeterminePlacement"/>
        /// <returns>A reference to the newly created object.</returns>
        public ObjectReference AllocateObject(int loc, ITypeDefOrRef typeDef)
        {
			var newObj = new AllocatedObject(typeDef, _cur.Configuration);
			newObj.ClearFields(_cur);
			m_alloc[loc] = newObj;
			return new ObjectReference(loc + 1, typeDef.FullName);
		}

        /// <summary>
        /// Allocate a new object of the given type.
        /// </summary>
        /// <param name="typeDef">The type of the object to create.</param>
        /// <seealso cref="DeterminePlacement"/>
        /// <returns>A reference to the newly created object.</returns>
        public ObjectReference AllocateObject(ITypeDefOrRef typeDef) => AllocateObject(DeterminePlacement(), typeDef);

        /// <summary>
        /// Allocate a new array of the given type and length.
        /// </summary>
        /// <param name="typeDef">The element type of the array.</param>
        /// <param name="length">The length of the array.</param>
        /// <seealso cref="DeterminePlacement"/>
        /// <returns>A reference to the newly created array.</returns>
        public ObjectReference AllocateArray(ITypeDefOrRef typeDef, int length) => AllocateArray(DeterminePlacement(), typeDef, length);

        /// <summary>
        /// Allocate a new array of the given type and length at the given place.
        /// </summary>
        /// <param name="loc">The location to put the array.</param>
        /// <param name="typeDef">The element type of the array.</param>
        /// <param name="length">The length of the array.</param>
        /// <seealso cref="DeterminePlacement"/>
        /// <returns>A reference to the newly created array.</returns>
        public ObjectReference AllocateArray(int loc, ITypeDefOrRef typeDef, int length)
        {
			var newArr = new AllocatedArray(typeDef, length, _cur.Configuration);
			newArr.ClearFields(_cur);
			m_alloc[loc] = newArr;
			return new ObjectReference(loc + 1, typeDef.FullName);
		}

		/// <summary>Allocate a new delegate at the given place.</summary>
		///
		/// <param name="loc">The location to put the delegate.</param>
		/// <param name="obj">The object to invoke the method on.</param>
		/// <param name="ptr">A pointer to the method to invoke.</param>
        /// <returns>A reference to the newly created delegate.</returns>
		/// \sa DeterminePlacement
		public ObjectReference AllocateDelegate(int loc, ObjectReference obj, MethodPointer ptr)
        {
			m_alloc[loc] = new AllocatedDelegate(_cur.DefinitionProvider.BaseTypes.Delegate.ToTypeDefOrRef(), obj, ptr, _cur.Configuration);
			var newDelRef = new ObjectReference(loc + 1);
			_cur.ParentWatcher.AddParentToChild(newDelRef, obj, _cur.Configuration.MemoisedGC());
			return newDelRef;
		}

        /// <summary>
        /// Increase the reference count of the referenced allocation.
        /// </summary>
        /// <param name="obj">A reference to the allocation.</param>
        public void IncRefCount(ObjectReference obj)
        {
            if (obj.Location != 0 && _cur.Configuration.UseRefCounting())
            {
                var alloc = Allocations[obj];
                Debug.Assert(alloc != null, "allocation of to change ref.count is null");
                alloc.RefCount++;
            }
        }

        /// Decrease the reference count of the referenced allocation.
        ///
        /// If the reference count reaches zero, this deletes the allocation!
        ///
        /// <param name="obj">A reference to the allocation.</param>
        public void DecRefCount(ObjectReference obj)
        {
            if (obj.Location != 0 && _cur.Configuration.UseRefCounting())
            {
                var alloc = Allocations[obj];
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
		/// <param name="loc">The location (offset in the list) of the allocation to</param>
		/// delete.
		public void DisposeLocation(int loc) {

			Debug.Assert(loc >= 0, "Deleting location at index < 0.");
			if (m_alloc[loc] != null)
				m_alloc[loc].Dispose();
			m_alloc[loc] = null;
		}

		/// Dispose allocation on the given location.
		///
		/// <param name="obj">Reference to the object of the allocation to delete.</param>
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

		/// <summary>Get a new free slot.</summary>
		///
		/// This location has never been used before. Backtracking etc. has no
		/// influence on this behavior.
		///
		/// <returns>A never before used location.</returns>
		public int FreeSlot() {

			return m_freeSlot++;
		}

		/// <summary>Determine the placement for an allocation being allocated at</summary>
		/// the current instruction.
		///
		/// This calls DeterminePlacement(true).
		///
		/// <returns>The place to put the allocation.</returns>
		public int DeterminePlacement() {

			return DeterminePlacement(true);
		}

		/// <summary>Determine the placement for an allocation.</summary>
		///
		/// If byCil is true, the current instruction is used to determine the
		/// placement. If byCil is false, or if IConfiguration.SymmetryReduction is
		/// false, we return a free slot.
		///
		/// We could also check for the absence of any CIL instruction to be
		/// executed, as this is (atm) only called from SetupMainState.
		///
		/// <param name="byCil">Determine the place by the current instruction.</param>
		/// <returns>The place to put the allocation.</returns>
		/// \sa PlacementMapping
		/// \sa FreeSlot
		public int DeterminePlacement(bool byCil)
        {
			int retval;
			if (!_cur.Configuration.SymmetryReduction() || !byCil)
				retval = FreeSlot();
			else
				retval = m_placementMapping.GetLocation(_cur);
			_cur.Logger.Debug("new allocation will be placed at {0}", retval);
			return retval;
		}
	}
}
