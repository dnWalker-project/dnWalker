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


    /// \brief This is a straith-forward implementation of IDefinitionProvider.
    ///
    /// Hashing is used to speed up the lookup process.
    ///
    /// The methods in this class use the Logger singleton to output warnings,
    /// and debug messages under priority Lookup.
    class DefinitionProvider
    {

        ModuleDef m_asmDef;
        ModuleDef[] m_referencedAssemblies;

        IDictionary<string, TypeDef> m_typeDefinitions;
        IDictionary m_virtualMethodDefinitions;
        IDictionary m_methodDefinitionsByReference;
        IDictionary m_methodDefinitionsByString;
        IDictionary<string, FieldDefinition> m_fieldDefinitions;
        IDictionary m_typeSizes;

        static DefinitionProvider instance;

        /// Get the global definition provider.
        public static DefinitionProvider dp
        {

            get { return instance; }
        }

        /// Load an assembly, and set up a definition provider for it.
        ///
        /// This is the place where the actual definition provider is created,
        /// so subtitute your own type here if needed.
        ///
        /// \param asmDef The assembly definition to use as the main assembly.
        public static void LoadAssembly(string location)
        {

            if (instance != null)
                Logger.l.Warning("unloading already loaded assembly " +
                        instance.AssemblyDefinition.Name);
            instance = Create(location);
        }

        /// \brief The main assembly we're working on (ro).
        public ModuleDef AssemblyDefinition
        {

            get
            {
                return m_asmDef;
            }
        }

        // ----------------------------------------------------------------------------------------------

        public int SizeOf(string type)
        {
            return (int)m_typeSizes[type];
        }

        /// Returns the inheritance chain of a type
        public IEnumerable<ITypeDefOrRef> InheritanceEnumerator(ITypeDefOrRef m_typeDef)
        {
            ITypeDefOrRef currentType = m_typeDef;
            do
            {
                var currentTypeDef = DefinitionProvider.dp.GetTypeDefinition(currentType);
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
        public TypeDef SearchType(string name, ModuleDef asm)
        {
            TypeDef retval = null;

            if (m_typeDefinitions.TryGetValue(name, out retval))
            {
                return retval;
            }

            if (!asm.TypeExists(name, false))
            {
                //return null;
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

            Logger.l.Lookup("SearchType: type {0} {1}found in assembly {2}", name, retval == null ? "not " : "", asm.Name.String);

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
        public TypeDef GetTypeDefinition(TypeRef typeRef)
        {
            //return GetTypeDefinition(typeRef.FullName);
            return typeRef.ResolveTypeDef();
        }

        public TypeDef GetTypeDefinition(TypeSig typeSig)
        {
            var typeDef = typeSig.ToTypeDefOrRef().ResolveTypeDef();
            if (typeDef.HasGenericParameters)
            {
            //    return typeDef;
            }

            //Logger.l.Lookup("looking up definition for type {0} => {1}", typeSig.FullName, typeDef);
            return typeDef;

            /*var typeDef = GetTypeDefinition(typeSig.FullName);
            if (typeDef != null)
            {
                return typeDef;
            }

            var typeDefOrRef = typeSig.ToTypeDefOrRef();
            if (typeDefOrRef.NumberOfGenericParameters > 0)
            {
                typeDef = GetTypeDefinition(typeSig);
            }

            return typeDefOrRef.ResolveTypeDef();*/
        }

        // TODO TypeDefFinder
        public TypeDef GetTypeDefinition(ITypeDefOrRef typeRef)
        {
            return typeRef.ResolveTypeDefThrow();
        }

        public TypeDef GetTypeDefinition(TypeDef typeDef)
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
            var retval = SearchType(name, m_asmDef);
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
                //var a = m_referencedAssemblies.FirstOrDefault(m => m.Name == "System.dll");
                //a
                //var sa = a.Types.Where(t => t.FullName.StartsWith("System.Threa")).Select(t => t.FullName).OrderBy(s => s).ToArray();

                throw new NullReferenceException($"Type {name} not found.");
            }

            Logger.l.Lookup("looking up definition for type {0} => {1}", name, retval);

            return retval;
        }

        // ----------------------------------------------------------------------------------------------

        public MethodDefinition GetMethodDefinition(IMethod methodRef)
        {
            //throw new NotSupportedException("GetMethodDefinition " + methodRef.FullName + " " + methodRef.GetType().FullName);
            return GetMethodDefinition(methodRef.ResolveMethodDef());
        }

        /// \brief Look up a method definition by reference, in its defining
        /// type.
        ///
        /// This simply calls GetMethodDefinition with its defining type, once
        /// it has been looked up.
        ///
        /// \param methRef Reference to the method to look up.
        /// \return A definition for the method to look for, or null if none
        /// was found.
        public MethodDefinition GetMethodDefinition(MethodReference methRef)
        {

            if (methRef is MethodDefinition)
                return methRef as MethodDefinition;

            MethodDefinition retval = m_methodDefinitionsByReference[methRef.ToString()] as MethodDefinition;

            if (retval == null)
            {
                TypeDef typeDef = GetTypeDefinition(methRef.DeclaringType);
                // Look in either the constructor or method definition collection.
                IEnumerator definitions = null;
                if (methRef.IsConstructor)
                    //|| methRef.Name == MethodDefinition.Cctor)
                    definitions = typeDef.FindInstanceConstructors().GetEnumerator();
                else
                    definitions = typeDef.Methods.GetEnumerator();
                // Search in all method definitions.
                while (retval == null && definitions.MoveNext())
                {
                    MethodDefinition curr = definitions.Current as MethodDefinition;
                    if (curr.ToString().Equals(methRef.ToString()))
                        retval = curr;
                }

                // Store in cache.
                if (retval != null)
                    m_methodDefinitionsByReference[methRef.ToString()] = retval;

                Logger.l.Lookup("SearchMethod(methref...): method {0} {1}found in type {2}",
                        methRef.Name, (retval == null ? "not " : ""), typeDef.Name);
            }
            else
            {
                Logger.l.Lookup("SearchMethod(methref...): method {0} found in cache", methRef.Name);
            }

            return retval;
        }

        public MethodDefinition SearchVirtualMethod(MethodReference methRef, ObjectReference objRef, ExplicitActiveState cur)
        {
            AllocatedObject ao = cur.DynamicArea.Allocations[objRef] as AllocatedObject;
            var superType = ao.Type;
            VirtualMethodDefinition vmdef = new VirtualMethodDefinition(methRef, objRef);
            MethodDefinition retval = m_virtualMethodDefinitions[vmdef] as MethodDefinition;

            if (retval != null)
                return retval;

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

            m_virtualMethodDefinitions[vmdef] = retval;

            return retval;
        }

        /// \brief Search for a method definition by name in a referenced type.
        ///
        /// As explained in the IDefinitionProvider interface, this search is
        /// always <b>approximate</b>, since we cannot compare the formal
        /// parameter list. This means things may go wrong if someone defines
        /// two method with the same name, which is very common in OO
        /// programming. You have been warned.
        ///
        /// \param name The name of the method to look up.
        /// \param typeDef Definition of the type to search in.
        /// \return A definition for the method to look for, or null if none
        /// was found.
        public MethodDefinition SearchMethod(string name, TypeDef typeDef)
        {

            string key = typeDef + "::" + name;
            MethodDefinition retval = m_methodDefinitionsByString[key] as MethodDefinition;
            if (retval == null)
            {
                // Look in either the constructor or method definition collection.
                IEnumerator definitions = null;
                if (name == ".ctor")
                    definitions = typeDef.FindConstructors().GetEnumerator();
                else if (name == ".cctor")
                    definitions = typeDef.FindConstructors().GetEnumerator();
                else
                    definitions = typeDef.Methods.GetEnumerator();
                // Search in all method definitions.
                while (retval == null && definitions.MoveNext())
                {
                    MethodDefinition curr = definitions.Current as MethodDefinition;
                    if (curr.Name == name)
                        retval = curr;
                }
                // Store in cache.
                if (retval != null)
                    m_methodDefinitionsByString[key] = retval;

                Logger.l.Lookup("SearchMethod(string...): method {0} {1}found in type {2}",
                        name, (retval == null ? "not " : ""), typeDef.Name);
            }
            else
            {
                Logger.l.Lookup("SearchMethod(string...): method {0} found in cache", name);
            }

            return retval;
        }

        // ----------------------------------------------------------------------------------------------

        /// \brief Look up a field definition by reference.
        ///
        /// This simply calls GetFieldDefinition with the full name of the
        /// declaring type, and the name of the field. Both searches (by
        /// string) are safe. Multiple fields with the same name are illegal.
        ///
        /// \param fieldRef A reference to the field to look up.
        /// \return Definition of the field to look for, or null if none was found.
        /*public FieldDefinition GetFieldDefinition(FieldReference fieldRef) {

			return GetFieldDefinition(fieldRef.DeclaringType.FullName, fieldRef.Name);
		}*/

        public FieldDefinition GetFieldDefinition(IField fieldRef)
        {
            //if (m_fieldDefinitions.TryGetValue()
            //return GetFieldDefinition(fieldRef.DeclaringType.FullName, fieldRef.Name);
            return fieldRef.ResolveFieldDef();
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
                    Logger.l.Warning("declaring type not found");
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

            return retval;
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
                if (!fld.IsStatic)
                    ++count;
            return count;
        }

        // ----------------------------------------------------------------------------------------------
        public IDataElement GetNullValue(TypeSig typeSig)
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

        /// \brief default(typeRef) of a type typeRef
        /// i.e., the default type for representation in the state of a given type

        public IDataElement GetNullValue(ITypeDefOrRef typeRef)
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

            throw new NotImplementedException("GetNullValue ITypeDefOrRef " + typeRef.FullName);
            /*switch (typeRef.FullName) {
				case Constants.Boolean:
				case Constants.Char:
				case Constants.Int16:
				case Constants.Int32:
				case Constants.SByte:
				case Constants.Byte:
					return Int4.Zero;

				case Constants.Single:
					return Float4.Zero;

				case Constants.Double:
					return Float8.Zero;

				case Constants.UInt16:
				case Constants.UInt32:
					return UnsignedInt4.Zero;

				case Constants.Int64:
					return Int8.Zero;

				case Constants.UInt64:
					return UnsignedInt8.Zero;
            }

			if (typeRef.IsValueType && !(typeRef is ArrayType))
				return Int4.Zero;
			else
				return ObjectReference.Null;*/
        }

        public static DefinitionProvider Create(string assemblyLocation)
        {
            return new DefinitionProvider(assemblyLocation);
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
        private ModuleContext _moduleContext;
        private DefinitionProvider(string assemblyLocation)
        {
            m_typeDefinitions = new Dictionary<string, TypeDef>();
            m_methodDefinitionsByReference = new Hashtable();
            m_methodDefinitionsByString = new Hashtable();
            m_fieldDefinitions = new Dictionary<string, FieldDefinition>();
            m_virtualMethodDefinitions = new Hashtable();

            /*
			 * We need to know the sizes in order to perform
			 * managed pointer arithmetica
			 */
            m_typeSizes = new Hashtable();

            m_typeSizes["System.UInt16"] = 2;
            m_typeSizes["System.UInt32"] = 4;
            m_typeSizes["System.UInt64"] = 8;
            m_typeSizes["System.Int16"] = 2;
            m_typeSizes["System.Int32"] = 4;
            m_typeSizes["System.Int64"] = 8;
            m_typeSizes["System.SByte"] = 1;
            m_typeSizes["System.Byte"] = 1;
            m_typeSizes["System.Boolean"] = 1;
            m_typeSizes["System.Char"] = 1;
            m_typeSizes["System.Double"] = 8;
            m_typeSizes["System.Decimal"] = 16;

            Logger.l.Notice("loading referenced assemblies...");

            _moduleContext = ModuleDef.CreateModuleContext();
            AssemblyResolver asmResolver = (AssemblyResolver)_moduleContext.AssemblyResolver;
            asmResolver.EnableTypeDefCache = true;

            ModuleDefMD module = ModuleDefMD.Load(assemblyLocation, _moduleContext);
            module.Context = _moduleContext;

            ((AssemblyResolver)module.Context.AssemblyResolver).AddToCache(module);

            AssemblyDef asm = module.Assembly;

            m_asmDef = module;
            //Assembly mainAsm = AssemblyFactory.CreateReflectionAssembly((AssemblyDefinition)m_asmDef); // run-time type is AD
            //AssemblyName[] refAsms = asmDef.re.GetReferencedAssemblies();
            
            //m_referencedAssemblies = (from item in module.GetModuleRefs() select item.).ToArray();
            
            //System.Console.Out.WriteLine(string.Join("; ", assembly.GetAssemblyRefs().Select(ar => ar.FullName)));

            m_referencedAssemblies =  module.GetAssemblyRefs()
                .Select(ar => _moduleContext.AssemblyResolver.Resolve(ar.Name, module))
                .SelectMany(a => a.Modules)
                .ToArray();
            //_moduleContext.AssemblyResolver.Resolve("mscorlib", module)
              //              asmDef.GetAssemblyRefs().Select(ar => ar.mod)))
            //m_referencedAssemblies = new ModuleDef[] { };


            // check whether we are in a Mono runtime 
            /*bool inMono = Type.GetType("Mono.Runtime", false) != null;

            // if we are not running in a Mono runtime, we have to retrieve the MONO_HOME environment var
            string monoHome = "";
            if (!inMono)
            {
                Logger.l.Notice("detected a non-Mono runtime");
                monoHome = System.Environment.GetEnvironmentVariable("MONO_HOME");
                if (monoHome == null)
                    MonoModelChecker.Fatal("the MONO_HOME variable was unset");
            }
            monoHome += @"\lib\mono\2.0\";*/

            /*
			 * There are two scenario's, either we are in a Mono runtime or in another.
			 * If we are in another, we have to use our custom assembly loader to ensure 
			 * that Mono's mscorlib is loaded, otherwise the internal calls do not match 
			 */
            /*for (int i = 0; i < refAsms.Length; ++i)
            {
                string fileName = "";

                if (!inMono && File.Exists(monoHome + refAsms[i].Name + ".dll"))
                {
                    fileName = monoHome + refAsms[i].Name + ".dll"; // <-- this is our custom assembly loader
                }
                else if (File.Exists(refAsms[i].Name + ".dll"))
                {
                    fileName = refAsms[i].Name + ".dll";
                }
                else
                {
                    fileName = Assembly.Load(refAsms[i]).Location;
                }

                try
                {
                    m_referencedAssemblies[i] = ModuleDefMD.Load(fileName);
                    Logger.l.Notice("loaded assembly " + fileName);
                }
                catch (System.Exception e)
                {
                    MonoModelChecker.Fatal("error loading referenced assembly " + fileName + System.Environment.NewLine + e);
                }
            }*/
        }
    }
}
