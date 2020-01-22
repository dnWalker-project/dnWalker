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
	
	using MMC.Collections;
	using MMC.Data;
	using MMC.Util;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using TypeDefinition = dnlib.DotNet.TypeDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;

    public interface IStaticArea : ICleanable, IStorageVisitable {

		/// List containing all classes.
		SparseReferenceList<AllocatedClass> Classes { get; }
		/// List containing all dirty classes.
		DirtyList DirtyClasses { get; }
		/// Iterator provider for all loaded classes.
		IEnumerable LoadedClasses { get; }

		/// Check if a class for a given type is loaded.
		bool ClassLoaded(TypeDefinition typeDef);
		/// Get the location at which a class for the given type is loaded.
		int GetClassLocation(TypeDefinition typeDef);
		/// Get the class for a given type.
		AllocatedClass GetClass(TypeDefinition typeDef);
		/// Get the class at a given location.
		AllocatedClass GetClass(int location);
		/// Delete the class at a given location.
		void DeleteClassAtLocation(int location);
	}

	class StaticArea : IStaticArea {

		/// \brief Mapping of type definitions to the location of the allocated
		/// class.
		/// This mapping does not change during the exploration. In other
		/// words: the first loading of a class defines its place forever. 
		IDictionary m_typeToLocation;

		/// \brief The allocated (not necessarily loaded) classes.
		SparseReferenceList<AllocatedClass> m_classes;

		/// \brief An object that provides IEnumerator objects to iterate over
		/// the loaded classes.
		LoadedClassEnumeratorProvider m_enumeratorProvider;
        private readonly ExplicitActiveState _cur;

        /// List containing all classes.
        public SparseReferenceList<AllocatedClass> Classes { get { return m_classes; } }
		/// List containing all dirty classes.
		public IEnumerable LoadedClasses { get { return m_enumeratorProvider; } }

		/// \brief Check if a class for a given type is loaded.
		///
		/// \param typeDef Type for which to check this.
		/// \return True iff the class exists and is loaded.
		public bool ClassLoaded(TypeDefinition typeDef) {

			object loc = m_typeToLocation[typeDef];
			return (loc != null && m_classes[(int)loc].Loaded);
		}

		/// \brief Get the location at which a class for the given type is loaded.
		///
		/// This queries the mapping m_typeToLocation, documented above.
		///
		/// \param typeDef The type.
		/// \return The location.
		public int GetClassLocation(TypeDefinition typeDef) {

			object retval = m_typeToLocation[typeDef];
			if (retval == null) {
				retval = m_typeToLocation.Count;
				m_typeToLocation[typeDef] = retval;
				AllocatedClass new_class = new AllocatedClass(typeDef, _cur);
				m_classes[(int)retval] = new_class;
			}
			return (int)retval;
		}
		
		/// \brief Get the class for a given type.
		///
		/// This calls GetClass with the location found with GetClassLocation.
		/// Convenience method.
		///
		/// \param typeDef The type.
		/// \return The class.
		public AllocatedClass GetClass(TypeDefinition typeDef) {

			return GetClass(GetClassLocation(typeDef));
		}

		/// \brief Get the class at a given location.
		/// 
		/// \param location The location in the class store.
		/// \return The class.
		public AllocatedClass GetClass(int location) {

			AllocatedClass retval = m_classes[location];
			if (!retval.Loaded) {
				Logger.l.Debug("loading class {0} at location {1}", 
						retval.Type.Name, location);
				retval.Loaded = true;
			}
			return retval;
		}

		/// \brief Delete the class at a given location.
		///
		/// Actually, this only unloads the class. The structure is not deleted.
		///
		/// \param location The location.
		public void DeleteClassAtLocation(int location) {

			m_classes[location].Loaded = false;
		}

		public void Accept(IStorageVisitor visitor, ExplicitActiveState cur) {

			visitor.VisitStaticArea(this);
		}

		public override string ToString() {

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (int lcls in LoadedClasses)
				sb.Append(m_classes[lcls].ToString()+"\n");
			return sb.ToString();
		}

		public bool IsDirty() {

			bool retval = false;
			for (int i=0; !retval && i < m_classes.Length; ++i)
				retval = m_classes[i] != null &&
				   m_classes[i].Loaded && m_classes[i].IsDirty();
			return retval;
		}

		/// List containing all dirty classes.
		public DirtyList DirtyClasses {

			get {
				DirtyList retval = new DirtyList();
				foreach (int lcls in LoadedClasses) {
					if (m_classes[lcls].IsDirty())
						retval.SetDirty(lcls);
				}
				return retval;
			}
		}

		public void Clean() {

			foreach (int lcls in LoadedClasses)
				m_classes[lcls].Clean();
		}

		public StaticArea(ExplicitActiveState cur)
        {
			m_typeToLocation = new Hashtable();
			m_classes = new SparseReferenceList<AllocatedClass>(); // TODO need to fix this default size in an option
			m_enumeratorProvider = new LoadedClassEnumeratorProvider(this);
            _cur = cur;
        }

		private class LoadedClassEnumeratorProvider : IEnumerable {

			IStaticArea m_sa;

			public IEnumerator GetEnumerator() {

				return new LoadedClassEnumerator(m_sa);
			}

			public LoadedClassEnumeratorProvider(IStaticArea sa) {

				m_sa = sa;
			}

			private class LoadedClassEnumerator : IEnumerator {

				int m_cur;
				IStaticArea m_sa;

				public object Current {

					get { return m_cur; }
				}

				public bool MoveNext() {

					bool retval = false;
					for (++m_cur; !retval && m_cur < m_sa.Classes.Length; ++m_cur)
						retval = m_sa.Classes[m_cur] != null && m_sa.Classes[m_cur].Loaded;
					if (retval)
						--m_cur;
					return retval;
				}

				public void Reset() {

					m_cur = -1;
				}

				public LoadedClassEnumerator(IStaticArea sa) {

					m_sa = sa;
					Reset();
				}
			}
		}
	}

}
