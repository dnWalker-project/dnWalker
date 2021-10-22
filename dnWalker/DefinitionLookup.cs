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
    using System.Collections.Generic;
    using MMC.Data;
    using MMC.State;
    using MethodDefinition = dnlib.DotNet.MethodDef;
    using MethodReference = dnlib.DotNet.MethodDef;
    using FieldDefinition = dnlib.DotNet.FieldDef;
    using dnlib.DotNet;
    using System.Linq;
    using dnWalker;
    using System.IO;
    using dnWalker.NativePeers;
    using dnWalker.DataElements;

    /// <summary>
    /// This definition is used for quick storage of methodreferences.
    /// </summary>
    /// <remarks>A virtual method is associated with an object, that is why
    /// the objectreference is also included in the virtualmethdef</remarks>
    internal struct VirtualMethodDefinition
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
            var other = (VirtualMethodDefinition)obj;

            var equals = other.Method.Name.Equals(Method.Name) &&
                other.Reference.Equals(Reference) &&
                other.Method.Parameters.Count == Method.Parameters.Count;

            for (var i = 0; equals && i < other.Method.Parameters.Count; ++i)
            {
                equals = other.Method.Parameters[i].Type.TypeName ==
                   Method.Parameters[i].Type.TypeName;
            }

            return equals;
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode() ^ Reference.GetHashCode();
        }
    }

    public interface IDefinitionProvider
    {
        TypeDef GetTypeDefinition(string typeName);
    }


    /// <summary>
    /// This is a straightforward implementation of IDefinitionProvider.
    /// </summary>
    /// <remarks>
    /// Hashing is used to speed up the lookup process.
    /// </remarks>
    public sealed class DefinitionProvider : IDefinitionProvider
    {
        private readonly object _lock = new object();
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
            if (m_typeSizes.TryGetValue(type, out var result))
            {
                return result;
            }

            throw new KeyNotFoundException(type);
        }

        /// <summary>
        /// Returns the inheritance chain of a type
        /// </summary>
        public static IEnumerable<ITypeDefOrRef> InheritanceEnumerator(ITypeDefOrRef m_typeDef)
        {
            var currentType = m_typeDef;
            do
            {
                var currentTypeDef = GetTypeDefinition(currentType);
                if (currentTypeDef == null)
                {
                    break;
                }
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

        /// <summary>Look up a type definition by name in a given assembly.</summary>
        /// <remarks>
        /// This uses its <b>full</b> name, so including namespaces. As a
        /// result, this search is <b>not</b> approximate.
        /// </remarks>
        /// <param name="name">The full name of the type to look up.</param>
        /// <param name="asm">The assembly to look in (all modules are searched).</param>
        /// <returns>A definition of the found type, or null is nothing was found.</returns>
        internal TypeDef SearchType(string name, ModuleDef asm)
        {
            lock (_lock)
            {
                if (m_typeDefinitions.TryGetValue(name, out var retval))
                {
                    return retval;
                }

                if (name == "System.IO.TextWriter")
                {
                    retval = GetOwnTypeDefinition(typeof(SystemIOTextWriterImpl).FullName);
                }

                if (retval == null)
                {
                    retval = asm.Types.FirstOrDefault(t => t.ReflectionFullName == name);
                }

                if (retval != null)
                {
                    m_typeDefinitions.Add(name, retval);
                }

                return retval;
            }
        }

        private TypeDef GetOwnTypeDefinition(string name)
        {
            //var assemblyLoader = new AssemblyLoader();

            //var data = File.ReadAllBytes(GetType().Assembly.Modules);
            return GetType().Assembly.Modules.Select(m =>
            {
                var moduleDef = ModuleDefMD.Load(m);
                return moduleDef.Types.FirstOrDefault(t => t.ReflectionFullName == name);
            }).FirstOrDefault();
        }

        /// <summary>
        /// Look up a type definition by reference in the main assembly
        /// and its reference assemblies.
        /// </summary>
        /// <remarks>
        /// This simply calls GetTypeDefinition(typeRef.FullName), which is okay
        /// as long as the search by name is not approximate.
        /// </remarks>
        /// <param name="typeRef">reference to find the definition of.</param>
        /// <returns>The definition, or null if none was found.</returns>
        /// <seealso cref="GetTypeDefinition(string)"/> 
        internal TypeDef GetTypeDefinition(TypeRef typeRef)
        {
            return typeRef.ResolveTypeDef();
        }

        internal TypeDef GetTypeDefinition(TypeSig typeSig)
        {
            var typeDef = typeSig.ToTypeDefOrRef().ResolveTypeDef();
            if (typeDef == null)
            {
                typeDef = GetTypeDefinition(typeSig.FullName);
            }
            if (typeDef != null && typeDef.HasGenericParameters)
            {
                //    return typeDef;
            }

            return typeDef;
        }

        // TODO TypeDefFinder
        internal static TypeDef GetTypeDefinition(ITypeDefOrRef typeRef)
        {
            try
            {
                return typeRef.ResolveTypeDefThrow();
            }
            catch
            {
                if (typeRef.FullName == "System.Object")
                {
                    return null;
                }
                throw;
            }
        }

        internal TypeDef GetTypeDefinition(TypeDef typeDef)
        {
            return typeDef;
        }

        /// <summary>
        /// Look up a type definition by name in the main assembly
        /// and its reference assemblies.
        /// </summary>
        /// <remarks>
        /// This calls GetTypeDefinition(name, ...) for the main assembly, and
        /// then all its referenced assemblies until a definition is found.
        /// </remarks>
        /// <param name="name">of the type to find a definition for.</param>
        /// <returns>The definition, or null if none was found.</returns>
        /// <seealso cref="GetTypeDefinition(string, AssemblyDefinition)"/>
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

            var typeDef = GetTypeDefinition(methodTypeName);
            retval = typeDef.FindMethod(new UTF8String(methodName.Substring(lastDot + 1)));

            return retval;
        }

        public MethodDefinition SearchVirtualMethod(MethodReference methRef, IDataElement dataElement, ExplicitActiveState cur)
        {
            dataElement = dataElement is IManagedPointer ptr ?
                ptr.Value :
                dataElement;

            if (!(dataElement is ObjectReference objRef))
            {
                throw new NotSupportedException($"ObjectReference expected, {dataElement?.GetType().FullName} found.");
            }

            var ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;
            var superType = ao.Type;
            var vmdef = new VirtualMethodDefinition(methRef, objRef);

            if (m_virtualMethodDefinitions.TryGetValue(vmdef, out var retval))
            {
                return retval;
            }

            foreach (var typeRef in InheritanceEnumerator(superType))
            {
                var typeDef = GetTypeDefinition(typeRef);

                foreach (var curr in typeDef.Methods)
                {
                    if (curr.Body != null && curr.Body.Instructions.Count > 0)
                    {
                        var vmdefCurr = new VirtualMethodDefinition(curr, objRef);

                        if (vmdefCurr.Equals(vmdef))
                        {
                            retval = curr;
                            break;
                        }
                    }
                }

                if (retval != null)
                {
                    break;
                }
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
            var methodName = typeDef + "::" + name;

            if (m_methodDefinitionsByReference.TryGetValue(methodName, out var retval))
            {
                return retval;
            }

            if (name == ".cctor")
            {
                var cctor = typeDef.FindStaticConstructor();
                if (cctor != null)
                {
                    m_methodDefinitionsByReference.Add(methodName, cctor);
                    return cctor;
                }
            }

            throw new NotSupportedException("SearchMethod " + methodName);
        }

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

            return fieldDefinition;
        }

        /// <summary>Look up a field definition by name, in a type given by name.</summary>
        /// <remarks>
        /// The type name should be fully specified, i.e. with namespace.
        /// </remarks>
        /// <param name="declTypeName">Name of the type to look in.</param>
        /// <param name="fieldName">Name of the field to look for.</param>
        /// <returns>Definition of the field to look for, or null if none was found.</returns>
        public FieldDefinition GetFieldDefinition(string declTypeName, string fieldName)
        {
            var key = declTypeName + "::" + fieldName;
            if (!m_fieldDefinitions.TryGetValue(key, out var retval))
            {
                var declType = GetTypeDefinition(declTypeName);
                if (declType == null)
                {
                    throw new System.Exception($"Declaring type {declTypeName} not found");
                }
                else
                {
                    var equal = false;
                    var i = 0;
                    for (; !equal && i < declType.Fields.Count; ++i)
                    {
                        equal = declType.Fields[i].Name == fieldName;
                    }
                    if (equal)
                    {
                        retval = declType.Fields[i - 1];
                        //retval.Offset = (uint)i - 1;
                    }
                }
                if (retval != null)
                {
                    m_fieldDefinitions[key] = retval;
                }
            }

            return GetFieldDefinition(retval);
        }

        /// <summary>Get the number of static fields in a type definition.</summary>
        /// <param name="typeDef">The definition of the type of which to count the static fields.</param>
        /// <returns>The number of static fields.</returns>
        public int GetStaticFieldCount(TypeDef typeDef)
        {
            return typeDef.Fields.Count - GetNonStaticFieldCount(typeDef);
        }

        /// <summary>Get the number of non-static fields in a type definition.</summary>
        /// <param name="typeDef">The definition of the type of which to count the non-static fields.</param>
        /// <returns>The number of non-static fields.</returns>
        public int GetNonStaticFieldCount(TypeDef typeDef)
        {
            var count = 0;
            foreach (var fld in typeDef.Fields)
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

            throw new NotSupportedException("GetNullValue for " + typeRef.FullName);
        }

        public static DefinitionProvider Create(AssemblyLoader assemblyLoader)
        {
            return new DefinitionProvider(assemblyLoader);
        }

        public bool TryGetTypeHandle(ITypeDefOrRef typeRef, out RuntimeTypeHandle typeHandle)
        {
            var corLibType = AssemblyDefinition.CorLibTypes.GetCorLibTypeSig(typeRef);
            if (corLibType != null)
            {
                var typeSig = typeRef.ToTypeSig();
                if (corLibType == AssemblyDefinition.CorLibTypes.String)
                {
                    typeHandle = typeof(string).TypeHandle;
                    return true;
                }
            }

            typeHandle = default(RuntimeTypeHandle);
            return false;
        }

        // ----------------------------------------------------------------------------------------------

        /// <summary>Initialize a new HashedDefinitionProvider.</summary>
        /// <remarks>
        /// This initialized the hash tables, and load the referenced
        /// assemblies. This loading is done in a terribly inefficient way. In
        /// order to find the file name of a references assembly, we use Mono
        /// reflection classes. These classes provide only constructor methods
        /// that immediately load the whole thing into memory. All that just
        /// for a file name. This is the reason MMC starts so slowly.
        /// </remarks>
        /// <param name="assemblyLoader">Assembly loader</param>
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
                ["System.UInt16"] = sizeof(ushort),
                ["System.UInt32"] = sizeof(uint),
                ["System.UInt64"] = sizeof(ulong),
                ["System.Int16"] = sizeof(short),
                ["System.Int32"] = sizeof(int),
                ["System.Int64"] = sizeof(long),
                ["System.SByte"] = sizeof(sbyte),
                ["System.Byte"] = sizeof(byte),
                ["System.Boolean"] = sizeof(bool),
                ["System.Char"] = sizeof(char),
                ["System.Double"] = sizeof(double),
                ["System.Decimal"] = sizeof(decimal),
                ["System.Single"] = sizeof(float),
                ["System.IntPtr"] = IntPtr.Size,
                ["System.UIntPtr"] = UIntPtr.Size
            };

                AssemblyDefinition = assemblyLoader.GetModule();

            m_referencedAssemblies = assemblyLoader.GetReferencedModules(AssemblyDefinition);

            AllocatedDelegate.DelegateTypeDef = GetTypeDefinition("System.Delegate");
        }

        internal DefinitionProvider(ModuleDef mainModule, ModuleDef[] referencedModules)
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
                ["System.UInt16"] = sizeof(ushort),
                ["System.UInt32"] = sizeof(uint),
                ["System.UInt64"] = sizeof(ulong),
                ["System.Int16"] = sizeof(short),
                ["System.Int32"] = sizeof(int),
                ["System.Int64"] = sizeof(long),
                ["System.SByte"] = sizeof(sbyte),
                ["System.Byte"] = sizeof(byte),
                ["System.Boolean"] = sizeof(bool),
                ["System.Char"] = sizeof(char),
                ["System.Double"] = sizeof(double),
                ["System.Decimal"] = sizeof(decimal),
                ["System.Single"] = sizeof(float),
                ["System.IntPtr"] = IntPtr.Size,
                ["System.UIntPtr"] = UIntPtr.Size
            };

            AssemblyDefinition = mainModule;

            m_referencedAssemblies = referencedModules;

            AllocatedDelegate.DelegateTypeDef = GetTypeDefinition("System.Delegate");
        }

        public IDataElement CreateDataElement(object value)
        {
            if (value is null)
            {
                return ObjectReference.Null;
            }

            var type = value.GetType();
            if (type.IsArray)
            {
                var array = value as Array;
                return new ArrayOf(array, GetTypeDefinition(type.GetElementType().FullName));
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean: return new Int4((Boolean)value ? 1 : 0);
                case TypeCode.Char: return new Int4((Char)value);
                case TypeCode.SByte: return new Int4((SByte)value);
                case TypeCode.Byte: return new Int4((Byte)value);
                case TypeCode.Int16: return new Int4((Int16)value);
                case TypeCode.UInt16: return new UnsignedInt4((UInt16)value);
                case TypeCode.Int32: return new Int4((Int32)value);
                case TypeCode.UInt32: return new UnsignedInt4((UInt32)value);
                case TypeCode.Int64: return new Int8((Int64)value);
                case TypeCode.UInt64: return new UnsignedInt8((UInt64)value);
                case TypeCode.Single: return new Float4((Single)value);
                case TypeCode.Double: return new Float8((Double)value);
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

                    // TODO: handle reference & complex types...
                    var typeName = type.FullName;

                    var typeDef = this.GetTypeDefinition(typeName);

                    if (typeDef.IsInterface)
                    {
                        return new InterfaceProxy(typeDef);
                    }

                    //throw new NotSupportedException("CreateDataElement for " + value.GetType());
                    return ObjectReference.Null;
            }
        }
    }
}
