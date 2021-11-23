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

	using MMC.Data;
	using MMC.Exception;
    using dnlib.DotNet;
    using System.Collections.Generic;

    /// <summary>
    /// An object instances on the heap.
    /// </summary>
    public class AllocatedObject : DynamicAllocation
    {
        public override AllocationType AllocationType
        {
			get { return AllocationType.Object; }
		}

		public override int InnerSize
        {
			get { return Fields.Length; }
		}

        /// <summary>
        /// The fields of the object (including the static ones).
        /// </summary>
        /// <remarks>Note that the static fields never get assigned.</remarks>
        public DataElementList Fields { get; set; }

        /// <summary>
        /// The offset of the value field for wrapped types.
        /// </summary>
        public int ValueFieldOffset
        {
            get
            {
                var found = false;
                var i = 0;

                var typeDef = DefinitionProvider.GetTypeDefinition(Type);

                for (; !found && i < typeDef.Fields.Count; ++i)
                    found = typeDef.Fields[i].Name == VALUE_FIELD_NAME;

                if (!found)
                {
                    throw new FieldNotFoundException(this, Type.FullName + "." + VALUE_FIELD_NAME);
                }

                return i - 1;
            }
        }

        /// <summary>
        /// True iff the lock or the fields are dirty.
        /// </summary>
        public override bool IsDirty()
        {
            return Fields.IsDirty() || Lock.IsDirty();
        }

        /// <summary>
        /// Clean the fields and the lock.
        /// </summary>
        public override void Clean()
        {
            Fields.Clean();
            Lock.Clean();
        }

		/// <summary>
		/// Dispose of the fields.
		/// </summary>
		public override void Dispose()
        {
			Fields.Dispose();
		}

        /// <summary>
        /// Initialize / null all (inherited) fields.
        /// </summary>
        public virtual void ClearFields(ExplicitActiveState cur)
        {
            /* 
			 * determine the field length of this object
			 */
            var fields = new List<FieldDef>();

            foreach (var typeDefOrRef in DefinitionProvider.InheritanceEnumerator(Type))
            {
                fields.AddRange(typeDefOrRef.ResolveTypeDef().Fields);
            }

            if (Fields == null)
            {
                Fields = cur.StorageFactory.CreateList(fields.Count);
            }

            /*
			 * Initialize the fields with default values
			 */

            //int typeOffset = 0;

            //foreach (var typeDef in cur.DefinitionProvider.InheritanceEnumerator(m_typeDef)) {
            for (var i = 0; i < fields.Count; i++)
            {
                //int fieldsOffset = typeOffset + i;
                var type = cur.DefinitionProvider.GetTypeDefinition(fields[i].FieldType);
                if (type == null && !fields[i].FieldType.IsPrimitive)
                {
                    Fields[i] = ObjectReference.Null;
                    continue;
                }
                Fields[i] = DefinitionProvider.GetNullValue(type);
            }
            //typeOffset += typeDef.Fields.Count; 			}
        }

		public override void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
			visitor.VisitAllocatedObject(this, cur);
		}

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            if (Locked)
            {
                sb.AppendFormat("locked by: {0}, ", Lock.ToString());
            }

            var typeOffset = 0;
            foreach (var t in DefinitionProvider.InheritanceEnumerator(Type))
            {
                var typeDef = t.ResolveTypeDef();
                sb.AppendFormat("{0}:{{", typeDef.Name);
                for (var i = 0; i < typeDef.Fields.Count; ++i)
                {
                    if (!typeDef.Fields[i].IsStatic)
                    {
                        sb.AppendFormat("{1}={2}{0}",
                                i == Fields.Length ? "" : ", ",
                                typeDef.Fields[i].Name,
                                Fields[typeOffset + i].ToString());
                    }
                }

                typeOffset += typeDef.Fields.Count;
                sb.Append("} ");
            }

            return sb.ToString();
        }

		public AllocatedObject(ITypeDefOrRef typeDef, IConfig config) : this(typeDef, config.UseRefCounting, config.MemoisedGC) { }

        public AllocatedObject(ITypeDefOrRef typeDef, bool useRefCounting, bool memoisedGC) : base(typeDef, useRefCounting, memoisedGC) { }
	}

    /// VY thinks that eventually an array should not be a first-class citizen,
    /// It should be the same as any object
    public class AllocatedArray : AllocatedObject {

		public override AllocationType AllocationType {
			get { return AllocationType.Array; }
		}

        public override void ClearFields(ExplicitActiveState cur)
        {
            var nullVal = DefinitionProvider.GetNullValue(Type);
            for (var i = 0; i < Fields.Length; i++)
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
        public AllocatedArray(ITypeDefOrRef arrayType, int length, bool useRefCounting, bool memoisedGC) : base(arrayType, useRefCounting, memoisedGC)
        {
            this.Fields = new DataElementList(length);
        }
    }

    /// <summary>
    /// VY thinks that delegates should not be first class citizens
    /// They should be just an object of a particular delegate type
    /// </summary>
    public class AllocatedDelegate : DynamicAllocation
    {
        bool m_isDirty;

        public static ITypeDefOrRef DelegateTypeDef { get; set; }//= new Lazy<TypeDef>(() => DefinitionProvider.GetTypeDefinition("System.Delegate"));

		public override AllocationType AllocationType
        {
            get { return AllocationType.Delegate; }
        }

		public override int InnerSize
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// A reference to the object the method is to be invoked upon.
        /// </summary>
        public ObjectReference Object { get; set; }

        /// <summary>
        /// The method referenced by the delegate.
        /// </summary>
        public MethodPointer Method { get; set; }

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

		public override void Accept(IStorageVisitor visitor, ExplicitActiveState cur)
        {
			visitor.VisitAllocatedDelegate(this, cur);
		}

		public override string ToString()
        {
			return "delegate:" + Object.ToString() + "." + Method.Value.Name;
		}

		public AllocatedDelegate(ObjectReference obj, MethodPointer ptr, IConfig config) : base(DelegateTypeDef, config.UseRefCounting, config.MemoisedGC)
        {
			Object = obj;
			Method = ptr;
			m_isDirty = true;
        }
        public AllocatedDelegate(ObjectReference obj, MethodPointer ptr, bool useRefCounting, bool memoisedGC) : base(DelegateTypeDef, useRefCounting, memoisedGC)
        {
            Object = obj;
            Method = ptr;
            m_isDirty = true;
        }
    }
}
