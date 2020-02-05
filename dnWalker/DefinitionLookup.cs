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

namespace MMC
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using MMC.Data;
    using System.Security.Policy;
    using System.IO;
    using MMC.State;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using MethodReference = dnlib.DotNet.MethodDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;
    using dnlib.DotNet;
    using System.Linq;
    using dnWalker;

    /// This definition is used for quick storage of methodreferences.
    /// 
    /// A virtual method is associated with an object, that is why
    /// the objectreference is also included in the virtualmethdef
    struct VirtualMethodDefinition
    {
        public MethodReference Method;
        public ObjectReference Reference;

        public VirtualMethodDefinition(MethodReference methRef, ObjectReference objRef)
        {
            Method = methRef;
            Reference = objRef;
        }

        public override bool Equals(object obj)
        {
            VirtualMethodDefinition other = (VirtualMethodDefinition)obj;

            bool equals = other.Method.Name.Equals(this.Method.Name) &&
                other.Reference.Equals(this.Reference) &&
                other.Method.Parameters.Count == this.Method.Parameters.Count;
            for (int i = 0; equals && i < other.Method.Parameters.Count; ++i)
                equals = other.Method.Parameters[i].Type.TypeName ==
                    this.Method.Parameters[i].Type.TypeName;

            return equals;
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode() ^ Reference.GetHashCode();
        }
    }

    /// <summary>
    /// This is a straitforward implementation of IDefinitionProvider.
    /// </summary>
    /// <remarks>
    /// Hashing is used to speed up the lookup process.
    /// </remarks>
    public class DefinitionProvider
    {
        private readonly ModuleDef[] m_referencedAssemblies;
        private readonly IDictionary<string, TypeDef> m_typeDefinitions;
        private readonly Dictionary<VirtualMethodDefinition, MethodDefinition> m_virtualMethodDefinitions;
        private readonly IDictionary<string, MethodDefinition> m_methodDefinitionsByReference;
        private readonly IDictionary<string, FieldDefinition> m_fieldDefinitions;
        private readonly IDictionary<string, int> m_typeSizes;

        /// <summary>
        /// The main assembly we're working on (ro).
        /// </summary>
        public ModuleDef AssemblyDefinition { get; }

        // ----------------------------------------------------------------------------------------------

        public int SizeOf(string type)
        {
            return (int)m_typeSizes[type];
        }

        /// <summary>
        /// Returns the inheritance chain of a type
        /// </summary>
        public static IEnumerable<ITypeDefOrRef> InheritanceEnumerator(ITypeDefOrRef m_typeDef)
        {
            ITypeDefOrRef currentType = m_typeDef;
            do
            {
                var currentTypeDef = GetTypeDefinition(currentType);
                yield return currentTypeDef;
                currentType = currentTypeDef.BaseType;
            } while (currentType != null);
        }

        public bool IsSubtype(ITypeDefOrRef supertype, ITypeDefOrRef subtype)
        {
            foreach (var typeRef in InheritanceEnumerator(supertype))
            {
                if (typeRef.FullName.Equals(subtype.FullName))
                    return true;
            }

            return false;
        }

        /// \brief Look up a type definition by name in a given assembly.
        ///
        /// This uses its <b>full</b> name, so including namespaces. As a
        /// result, this search is <b>not</b> approximate.
        ///
        /// \param name The full name of the type to look up.
        /// \param asm The assembly to look in (all modules are searched).
        /// \return A definition of the found type, or null is nothing was found.
        internal TypeDef SearchType(string name, ModuleDef asm)
        {
            TypeDef retval = null;

            if (m_typeDefinitions.TryGetValue(name, out retval))
            {
                return retval;
            }

            retval = asm.Types.FirstOrDefault(t => t.ReflectionFullName == name);

            /*IEnumerator types = asm.Types.GetEnumerator();

            while (retval == null && types.MoveNext())
            {
                var curr = types.Current as TypeDef;
                if (curr.FullName == name)
                    retval = curr;
            }*/

            if (retval != null)
            {
                m_typeDefinitions.Add(name, retval);
            }

            return retval;
        }

        /// \brief Look up a type definition by reference in the main assembly
        /// and its reference assemblies.
        ///
        /// This simply calls GetTypeDefinition(typeRef.FullName), which is okay
        /// as long as the search by name is not approximate.
        ///
        /// \param Type reference to find the definition of.
        /// \return The definition, or null if none was found.
        /// \sa GetTypeDefinition(string)
        internal TypeDef GetTypeDefinition(TypeRef typeRef)
        {
            return typeRef.ResolveTypeDef();
        }

        internal TypeDef GetTypeDefinition(TypeSig typeSig)
        {
            var typeDef = typeSig.ToTypeDefOrRef().ResolveTypeDef();
            if (typeDef.HasGenericParameters)
            {
                //    return typeDef;
            }

            return typeDef;
        }

        // TODO TypeDefFinder
        internal static TypeDef GetTypeDefinition(ITypeDefOrRef typeRef)
        {
            return typeRef.ResolveTypeDefThrow();
        }

        internal TypeDef GetTypeDefinition(TypeDef typeDef)
        {
            return typeDef;
        }

        /// \brief Look up a type definition by name in the main assembly
        /// and its reference assemblies.
        ///
        /// This calls GetTypeDefinition(name, ...) for the main assembly, and
        /// then all its referenced assemblies until a definition is found.
        ///
        /// \param Name of the type to find a definition for.
        /// \return The definition, or null if none was found.
        /// \sa GetTypeDefinition(string, AssemblyDefinition)
        public TypeDef GetTypeDefinition(string name)
        {
            var retval = SearchType(name, AssemblyDefinition);
            foreach (var refA in m_referencedAssemblies)
            {
                retval = SearchType(name, refA);
                if (retval != null)
                {
                    break;
                }
            }

            if (retval == null)
            {
                throw new NullReferenceException($"Type {name} not found.");
            }

            return retval;
        }

        public MethodDefinition GetMethodDefinition(string methodName)
        {
            if (m_methodDefinitionsByReference.TryGetValue(methodName, out var retval))
            {
                return retval;
            }

            var lastDot = methodName.LastIndexOf(".");
            var methodTypeName = methodName.Substring(0, lastDot);

            TypeDef typeDef = GetTypeDefinition(methodTypeName);
            retval = typeDef.FindMethod(new UTF8String(methodName.Substring(lastDot + 1)));

            return retval;
        }

        public MethodDefinition SearchVirtualMethod(MethodReference methRef, ObjectReference objRef, ExplicitActiveState cur)
        {
            AllocatedObject ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;
            var superType = ao.Type;
            VirtualMethodDefinition vmdef = new VirtualMethodDefinition(methRef, objRef);

            if (m_virtualMethodDefinitions.TryGetValue(vmdef, out var retval))
            {
                return retval;
            }

            foreach (var typeRef in InheritanceEnumerator(superType))
            {
                var typeDef = GetTypeDefinition(typeRef);

                foreach (MethodDefinition curr in typeDef.Methods)
                {
                    if (curr.Body != null && curr.Body.Instructions.Count > 0)
                    {
                        VirtualMethodDefinition vmdefCurr = new VirtualMethodDefinition(curr, objRef);

                        if (vmdefCurr.Equals(vmdef))
                        {
                            retval = curr;
                            break;
                        }
                    }
                }

                if (retval != null)
                    break;
            }

            m_virtualMethodDefinitions.Add(vmdef, retval);

            return retval;
        }

        /// <summary>
        /// Search for a method definition by name in a referenced type.
        /// </summary>
        /// <remarks>
        /// As explained in the IDefinitionProvider interface, this search is
        /// always <b>approximate</b>, since we cannot compare the formal
        /// parameter list. This means things may go wrong if someone defines
        /// two method with the same name, which is very common in OO
        /// programming. You have been warned.
        /// </remarks>
        /// <param name="name">The name of the method to look up.</param>
        /// <param name="typeDef">Definition of the type to search in.</param>
        /// <returns>A definition for the method to look for, or null if none was found.</returns>
        public MethodDefinition SearchMethod(string name, TypeDef typeDef)
        {
            string methodName = typeDef + "::" + name;

            if (m_methodDefinitionsByReference.TryGetValue(methodName, out var retval))
            {
                return retval;
            }

            if (name == ".cctor" && typeDef.FindStaticConstructor() == null)
            {
                return null;
            }

            throw new NotImplementedException("SearchMethod " + methodName);
            /*
            if (retval == null)
            {
                // Look in either the constructor or method definition collection.
                IEnumerator definitions = null;
                if (name == ".ctor")
                {
                    definitions = typeDef.FindConstructors().GetEnumerator();
                }
                else if (name == ".cctor")
                {
                    definitions = typeDef.FindConstructors().GetEnumerator();
                }
                else
                {
                    definitions = typeDef.Methods.GetEnumerator();
                }
                // Search in all method definitions.
                while (retval == null && definitions.MoveNext())
                {
                    MethodDefinition curr = definitions.Current as MethodDefinition;
                    if (curr.Name == name)
                    {
                        retval = curr;
                    }
                }
                // Store in cache.
                if (retval != null)
                {
                    m_methodDefinitionsByReference.Add(methodName, retval);
                }
            }*/

            return retval;
        }

        // ----------------------------------------------------------------------------------------------

        public static FieldDefinition GetFieldDefinition(IField fieldRef)
        {
            if (fieldRef == null)
            {
                return null;
            }

            var fieldDefinition = fieldRef.ResolveFieldDef();
            if (!fieldDefinition.FieldOffset.HasValue)
            {
                var fields = fieldRef.DeclaringType.ResolveTypeDef().Fields;
                for (var i = 0; i < fields.Count; i++)
                {
                    if (fields[i] == fieldDefinition)
                    {
                        fieldDefinition.FieldOffset = (uint)i;
                        break;
                    }
                }
            }
            //if (m_fieldDefinitions.TryGetValue()
            //return GetFieldDefinition(fieldRef.DeclaringType.FullName, fieldRef.Name);
            return fieldDefinition;
        }

        /// \brief Look up a field definition by name, in a type given by name.
        ///
        /// The type name should be fully specified, i.e. with namespace.
        ///
        /// \param declTypeName Name of the type to look in.
        /// \param fieldName Name of the field to look for.
        /// \return Definition of the field to look for, or null if none was found.
        public FieldDefinition GetFieldDefinition(string declTypeName, string fieldName)
        {
            string key = declTypeName + "::" + fieldName;
            if (!m_fieldDefinitions.TryGetValue(key, out FieldDefinition retval))
            {
                TypeDef declType = GetTypeDefinition(declTypeName);
                if (declType == null)
                {
                    throw new System.Exception($"Declaring type {declTypeName} not found");
                }
                else
                {
                    bool equal = false;
                    int i = 0;
                    for (; !equal && i < declType.Fields.Count; ++i)
                        equal = declType.Fields[i].Name == fieldName;
                    if (equal)
                    {
                        retval = declType.Fields[i - 1];
                        //retval.Offset = (uint)i - 1;
                    }
                }
                if (retval != null)
                    m_fieldDefinitions[key] = retval;
            }

            return GetFieldDefinition(retval);
        }

        /// \brief Get the number of static fields in a type definition.
        ///
        /// \param typeDef The definition of the type of which to count the
        /// static fields.
        /// \return The number of static fields.
        public int GetStaticFieldCount(TypeDef typeDef)
        {
            return typeDef.Fields.Count - GetNonStaticFieldCount(typeDef);
        }

        /// \brief Get the number of non-static fields in a type definition.
        ///
        /// \param typeDef The definition of the type of which to count the
        /// non-static fields.
        /// \return The number of non-static fields.
        public int GetNonStaticFieldCount(TypeDef typeDef)
        {
            int count = 0;
            foreach (FieldDefinition fld in typeDef.Fields)
            {
                if (!fld.IsStatic)
                {
                    ++count;
                }
            }
            return count;
        }

        // ----------------------------------------------------------------------------------------------
        public static IDataElement GetNullValue(TypeSig typeSig)
        {
            return GetNullValue(typeSig.ToTypeDefOrRef());
        }

        public IDataElement GetParameterNullOrDefaultValue(Parameter parameter)
        {
            var methodDef = parameter.Method;
            if (parameter.IsHiddenThisParameter)
            {
                return ObjectReference.Null;
            }

            return GetNullValue(parameter.Type.ToTypeDefOrRef());
        }

        /// <summary>
        /// Get default(typeRef) of a type typeRef
        /// i.e., the default type for representation in the state of a given type
        /// </summary>
        public static IDataElement GetNullValue(ITypeDefOrRef typeRef)
        {
            var typeSig = typeRef.ToTypeSig();
            if (!typeSig.IsPrimitive)
            {
                return ObjectReference.Null;
            }

            if (typeRef.Module.CorLibTypes.IntPtr == typeSig
                || typeRef.Module.CorLibTypes.Boolean == typeSig
                || typeRef.Module.CorLibTypes.Char == typeSig
                || typeRef.Module.CorLibTypes.Int16 == typeSig
                || typeRef.Module.CorLibTypes.Int32 == typeSig
                || typeRef.Module.CorLibTypes.SByte == typeSig
                || typeRef.Module.CorLibTypes.Byte == typeSig)
            {
                return Int4.Zero;
            }

            if (typeRef.Module.CorLibTypes.Single == typeSig)
            {
                return Float4.Zero;
            }

            if (typeRef.Module.CorLibTypes.Double == typeSig)
            {
                return Float8.Zero;
            }

            if (typeRef.Module.CorLibTypes.UInt16 == typeSig
                || typeRef.Module.CorLibTypes.UInt32 == typeSig)
            {
                return UnsignedInt4.Zero;
            }

            if (typeRef.Module.CorLibTypes.Int64 == typeSig)
            {
                return Int8.Zero;
            }

            if (typeRef.Module.CorLibTypes.UInt64 == typeSig)
            {
                return UnsignedInt8.Zero;
            }

            if (typeRef.Module.CorLibTypes.UIntPtr == typeSig)
            {
                return UnsignedInt8.Zero;
            }

            throw new NotImplementedException("GetNullValue for " + typeRef.FullName);
        }

        public static DefinitionProvider Create(AssemblyLoader assemblyLoader)
        {
            return new DefinitionProvider(assemblyLoader);
        }

        // ----------------------------------------------------------------------------------------------

        /// \brief Initialize a new HashedDefinitionProvider.
        ///
        /// This initialized the hash tables, and load the referenced
        /// assemblies. This loading is done in a terribly inefficient way.  In
        /// order to find the file name of a references assembly, we use Mono
        /// reflection classes. These classes provide only constructor methods
        /// that immediately load the whole thing into memory. All that just
        /// for a file name. This is the reason MMC starts so slowly.
        ///
        /// \param asmDef Main assembly definition.
        private DefinitionProvider(AssemblyLoader assemblyLoader)
        {
            m_typeDefinitions = new Dictionary<string, TypeDef>();
            m_methodDefinitionsByReference = new Dictionary<string, MethodDefinition>();
            m_fieldDefinitions = new Dictionary<string, FieldDefinition>();
            m_virtualMethodDefinitions = new Dictionary<VirtualMethodDefinition, MethodDefinition>();

            /*
			 * We need to know the sizes in order to perform
			 * managed pointer arithmetica
			 */
            m_typeSizes = new Dictionary<string, int>
            {
                ["System.UInt16"] = 2,
                ["System.UInt32"] = 4,
                ["System.UInt64"] = 8,
                ["System.Int16"] = 2,
                ["System.Int32"] = 4,
                ["System.Int64"] = 8,
                ["System.SByte"] = 1,
                ["System.Byte"] = 1,
                ["System.Boolean"] = 1,
                ["System.Char"] = 1,
                ["System.Double"] = 8,
                ["System.Decimal"] = 16
            };

            AssemblyDefinition = assemblyLoader.GetModule();

            m_referencedAssemblies = assemblyLoader.GetReferencedModules(AssemblyDefinition);

            AllocatedDelegate.DelegateTypeDef = GetTypeDefinition("System.Delegate");
        }

        public IDataElement CreateDataElement(object value)
        {
            if (value is null)
            {
                return ObjectReference.Null;
            }

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean: return new Int4((bool)value ? 1 : 0);
                case TypeCode.Char: return new Int4((char)value);
                case TypeCode.SByte: return new Int4((sbyte)value);
                case TypeCode.Byte: return new Int4((byte)value);
                case TypeCode.Int16: return new Int4((short)value);
                case TypeCode.UInt16: return new UnsignedInt4((ushort)value);
                case TypeCode.Int32: return new Int4((int)value);
                case TypeCode.UInt32: return new UnsignedInt4((uint)value);
                case TypeCode.Int64: return new Int8((long)value);
                case TypeCode.UInt64: return new UnsignedInt8((ulong)value);
                case TypeCode.Single: return new Float4((float)value);
                case TypeCode.Double: return new Float8((double)value);
                case TypeCode.String: return new ConstantString(value.ToString());
                default:
                    if (value is IntPtr ip)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(ip.ToInt32()) : CreateDataElement(ip.ToInt64());
                    }
                    if (value is UIntPtr up)
                    {
                        return IntPtr.Size == 4 ? CreateDataElement(up.ToUInt32()) : CreateDataElement(up.ToUInt64());
                    }
                    throw new NotSupportedException("CreateDataElement for " + value.GetType());
            }
        }
    }
}
