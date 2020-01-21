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
	using MMC.Data;
	using MMC.Exception;
	using MMC.Util;
    using dnlib.DotNet;
    using System.Collections.Generic;
    using System;

    /// An object instances on the heap.
    class AllocatedObject : DynamicAllocation {

		DataElementList m_fields;

		public override AllocationType AllocationType {
			get { return AllocationType.Object; }
		}

		public override int InnerSize {
			get { return m_fields.Length; }
		}

		/// The fields of the object (including the static ones).
		///
		/// Note that the static fields never get assigned.
		public DataElementList Fields {
			get { return m_fields; }
			set { m_fields = value; }
		}

		/// The offset of the value field for wrapped types.
		public int ValueFieldOffset {
			get {
				bool found = false;
				int i = 0;

				var typeDef = DefinitionProvider.dp.GetTypeDefinition(Type);

				for (; !found && i < typeDef.Fields.Count; ++i)
					found = typeDef.Fields[i].Name == VALUE_FIELD_NAME;

				if (!found)
					throw new FieldNotFoundException(this, VALUE_FIELD_NAME);

				return i - 1;
			}
		}

		/// True iff the lock or the fields are dirty.
		public override bool IsDirty() {
			return m_fields.IsDirty() || Lock.IsDirty();
		}

		/// Clean the fields and the lock.
		public override void Clean() {
			m_fields.Clean();
			Lock.Clean();
		}

		/// Dispose of the fields.
		public override void Dispose() {
			m_fields.Dispose();
		}

        /// Initialize / null all (inherited) fields.
        public virtual void ClearFields()
        {
            /* 
			 * determine the field length of this object
			 */
            var fields = new List<FieldDef>();

            foreach (var typeDefOrRef in DefinitionProvider.dp.InheritanceEnumerator(Type))
            {
                if (typeDefOrRef is TypeDef typeDef)
                {
                    fields.AddRange(typeDef.Fields);
                    continue;
                }

                if (typeDefOrRef is TypeRef typeRef)
                {
                    fields.AddRange(typeRef.Resolve().Fields);
                    continue;
                }
            }

            if (m_fields == null)
                m_fields = StorageFactory.sf.CreateList(fields.Count);

            /*
			 * Initialize the fields with default values
			 */

            //int typeOffset = 0;

            //foreach (var typeDef in DefinitionProvider.dp.InheritanceEnumerator(m_typeDef)) {
            for (int i = 0; i < fields.Count; i++)
            {
                //int fieldsOffset = typeOffset + i;
                var type = DefinitionProvider.dp.GetTypeDefinition(fields[i].FieldType);
                if (type == null && !fields[i].FieldType.IsPrimitive)
                {
                    m_fields[i] = ObjectReference.Null;
                    continue;
                }
                m_fields[i] = DefinitionProvider.dp.GetNullValue(type);
            }
            //typeOffset += typeDef.Fields.Count; 			}
        }

		public override void Accept(IStorageVisitor visitor)
        {
			visitor.VisitAllocatedObject(this);
		}

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            if (Locked)
            {
                sb.AppendFormat("locked by: {0}, ", Lock.ToString());
            }

            int typeOffset = 0;
            foreach (var t in DefinitionProvider.dp.InheritanceEnumerator(Type))
            {
                var typeDef = t.ResolveTypeDef();
                sb.AppendFormat("{0}:{{", typeDef.Name);
                for (int i = 0; i < typeDef.Fields.Count; ++i)
                {
                    if (!typeDef.Fields[i].IsStatic)
                    {
                        sb.AppendFormat("{1}={2}{0}",
                                i == m_fields.Length ? "" : ", ",
                                typeDef.Fields[i].Name,
                                m_fields[typeOffset + i].ToString());
                    }
                }

                typeOffset += typeDef.Fields.Count;
                sb.Append("} ");
            }

            return sb.ToString();
        }

		public AllocatedObject(ITypeDefOrRef typeDef, IConfig config) : base(typeDef, config) { }
	}
	
	/// VY thinks that eventually an array should not be a first-class citizen,
	/// It should be the same as any object
	class AllocatedArray : AllocatedObject {

		public override AllocationType AllocationType {
			get { return AllocationType.Array; }
		}

		public override void ClearFields() {
			IDataElement nullVal = DefinitionProvider.dp.GetNullValue(Type);
			for (int i = 0; i < Fields.Length; i++)
				Fields[i] = nullVal;
		}

		public override string ToString() {
			return string.Format("array:{0}[{1}] = [{2}]",
					Type.Name, Fields.Length, Fields.ToString());
		}

        /*
		public override void Accept(IStorageVisitor visitor) {

			visitor.VisitAllocatedObject(this);
		}*/

        public AllocatedArray(ITypeDefOrRef arrayType, int length, IConfig config)
            : base(arrayType, config)
        {
            this.Fields = new DataElementList(length);
        }
	}

	/// VY thinks that delegates should not be first class citizens
	/// They should be just an object of a particular delegate type
	class AllocatedDelegate : DynamicAllocation {

		ObjectReference m_obj;
		MethodPointer m_ptr;
		bool m_isDirty;

        private static Lazy<TypeDef> _delegateTypeLazy = new Lazy<TypeDef>(() => DefinitionProvider.dp.GetTypeDefinition("System.Delegate"));

		public override AllocationType AllocationType {

			get { return AllocationType.Delegate; }
		}

		public override int InnerSize {
			get {
				return 0;
			}
		}

		/// A reference to the object the method is to be invoked upon.
		public ObjectReference Object {

			get { return m_obj; }
			set { m_obj = value; }
		}

		/// The method referenced by the delegate.
		public MethodPointer Method
        {
			get { return m_ptr; }
			set { m_ptr = value; }
		}

		public override void Dispose() {}

		public override bool IsDirty()
        {
			return Lock.IsDirty() || m_isDirty;
		}

		public override void Clean()
        {
            m_isDirty = false;
			Lock.Clean();
		}

		public override void Accept(IStorageVisitor visitor)
        {
			visitor.VisitAllocatedDelegate(this);
		}

		public override string ToString()
        {
			return "delegate:" + m_obj.ToString() + "." + m_ptr.Value.Name;
		}

		public AllocatedDelegate(ObjectReference obj, MethodPointer ptr, IConfig config) : base(_delegateTypeLazy.Value, config)
        {
			m_obj = obj;
			m_ptr = ptr;
			m_isDirty = true;
		}
	}
}
