using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class DefinitionProvider : IDefinitionProvider
    {
        private readonly IDomain _definitionContext;

        private readonly Dictionary<string, TypeDef> _typeCache;
        private readonly Dictionary<TypeSig, int> _sizeCache;
        //private readonly Dictionary<TypeSig, List<ITypeDefOrRef>> _inheritenceCache = new Dictionary<TypeSig, List<ITypeDefOrRef>>();
        private readonly Dictionary<string, MethodDef> _methodCache = new Dictionary<string, MethodDef>();
        private readonly BaseTypes _baseTypes;

        public DefinitionProvider(IDomain definitionContext)
        {
            _definitionContext = definitionContext;
            _typeCache = BuildTypeCache(definitionContext.MainModule);
            _sizeCache = BuildSizeCache(definitionContext.MainModule);

            _baseTypes = new BaseTypes(this);
        }

        private static Dictionary<string, TypeDef> BuildTypeCache(ModuleDef module)
        {
            Dictionary<string, TypeDef> typeCache = new Dictionary<string, TypeDef>();

            foreach (var typeDef in module.Types)
            {
                CacheType(typeDef);
            }

            ICorLibTypes corLibTypes = module.CorLibTypes;
            CacheType(corLibTypes.Void.TypeDef);
            CacheType(corLibTypes.Boolean.TypeDef);
            CacheType(corLibTypes.Char.TypeDef);
            CacheType(corLibTypes.SByte.TypeDef);
            CacheType(corLibTypes.Byte.TypeDef);
            CacheType(corLibTypes.Int16.TypeDef);
            CacheType(corLibTypes.UInt16.TypeDef);
            CacheType(corLibTypes.Int32.TypeDef);
            CacheType(corLibTypes.UInt32.TypeDef);
            CacheType(corLibTypes.Int64.TypeDef);
            CacheType(corLibTypes.UInt64.TypeDef);
            CacheType(corLibTypes.Single.TypeDef);
            CacheType(corLibTypes.Double.TypeDef);
            CacheType(corLibTypes.String.TypeDef);
            CacheType(corLibTypes.TypedReference.TypeDef);
            CacheType(corLibTypes.IntPtr.TypeDef);
            CacheType(corLibTypes.UInt64.TypeDef);
            CacheType(corLibTypes.Object.TypeDef);

            void CacheType(TypeDef type)
            {
                if (type == null) return;

                typeCache[type.FullName] = type;
            }


            return typeCache;
        }

        private static Dictionary<TypeSig, int> BuildSizeCache(ModuleDef module)
        {
            Dictionary<TypeSig, int> sizeCache = new Dictionary<TypeSig, int>(TypeEqualityComparer.Instance);

            ICorLibTypes corLibTypes = module.CorLibTypes;

            sizeCache[corLibTypes.Boolean] = sizeof(bool);
            sizeCache[corLibTypes.Char] = sizeof(char);
            sizeCache[corLibTypes.SByte] = sizeof(sbyte);
            sizeCache[corLibTypes.Byte] = sizeof(byte);
            sizeCache[corLibTypes.Int16] = sizeof(short);
            sizeCache[corLibTypes.UInt16] = sizeof(ushort);
            sizeCache[corLibTypes.Int32] = sizeof(int);
            sizeCache[corLibTypes.UInt32] = sizeof(uint);
            sizeCache[corLibTypes.Int64] = sizeof(long);
            sizeCache[corLibTypes.UInt64] = sizeof(ulong);
            sizeCache[corLibTypes.Single] = sizeof(float);
            sizeCache[corLibTypes.Double] = sizeof(double);
            sizeCache[corLibTypes.IntPtr] = IntPtr.Size;
            sizeCache[corLibTypes.UIntPtr] = UIntPtr.Size;

            return sizeCache;
        }


        public TypeDef GetTypeDefinition(string fullTypeName)
        {
            if (_typeCache.TryGetValue(fullTypeName, out var type))
            {
                return type;
            }

            // check if it is a nested type
            int slashIndex = fullTypeName.IndexOf('/');
            if (slashIndex > 0)
            {
                // there is a nested type
                string outerTypeName = fullTypeName.Substring(0, slashIndex);
                TypeDef outerType = GetTypeDefinition(outerTypeName);

                foreach (TypeDef nestedType in outerType.NestedTypes)
                {
                    if (nestedType.FullName == fullTypeName)
                    {
                        _typeCache[fullTypeName] = nestedType;
                        return nestedType;
                    }
                }

                // we did not find the nested type within the outer type nested types

                throw new TypeNotFoundException(fullTypeName);
            }
            else
            {
                // it is not a nested type => should be within the ModuleDef.Types lists
                // of all modules within definition context

                // naive, very naive implementation
                foreach (TypeDef t in _definitionContext.Modules.SelectMany(m => m.Types))
                {
                    if (t.FullName == fullTypeName)
                    {
                        _typeCache[t.FullName] = t;
                        return t;
                    }
                }

                foreach (ExportedType et in _definitionContext.Modules.SelectMany(m => m.ExportedTypes))
                {
                    if (et.FullName == fullTypeName)
                    {
                        TypeDef td = et.Resolve();
                        _typeCache[et.FullName] = td;
                        return td;
                    }
                }

                throw new TypeNotFoundException(fullTypeName);
            }
        }
        public MethodDef GetMethodDefinition(string fullMethodName)
        {
            if (_methodCache.TryGetValue(fullMethodName, out var method))
            {
                return method;
            }

            int lastDot = fullMethodName.LastIndexOf(".");
            string methodTypeName = fullMethodName.Substring(0, lastDot);

            TypeDef typeDef = GetTypeDefinition(methodTypeName);

            string methodName = fullMethodName.Substring(lastDot + 1);
            method = typeDef.FindMethod(methodName);

            _methodCache[fullMethodName] = method;

            return method;
        }

        public IBaseTypes BaseTypes
        {
            get
            {
                return _baseTypes;
            }
        }

        public int SizeOf(TypeSig type)
        {
            if (_sizeCache.TryGetValue(type, out var size))
            {
                return size;
            }

            throw new TypeNotSupportForSize(type.FullName);
        }

        public IDomain Context
        {
            get
            {
                return _definitionContext;
            }
        }
    }
}
