using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public class DefinitionProvider : IDefinitionProvider
    {
        private class TypeSigEqualityComparer : IEqualityComparer<TypeSig>
        {
            private readonly SigComparer _sigComparer = new SigComparer();

            public bool Equals(TypeSig x, TypeSig y)
            {
                return _sigComparer.Equals(x, y);
            }

            public int GetHashCode([DisallowNull] TypeSig obj)
            {
                return obj.GetHashCode();
            }
        }

        private class BaseTypesInternal : IBaseTypes
        {
            private readonly DefinitionProvider _definitionProvider;
            private readonly ICorLibTypes _corLibTypes;

            public BaseTypesInternal(DefinitionProvider definitionProvider, ICorLibTypes corLibTypes)
            {
                _definitionProvider = definitionProvider ?? throw new ArgumentNullException(nameof(definitionProvider));
                _corLibTypes = corLibTypes ?? throw new ArgumentNullException(nameof(corLibTypes));

                Thread = definitionProvider.GetTypeDefinition("System.Threading.Thread").ToTypeSig().ToTypeDefOrRefSig();
                Delegate = definitionProvider.GetTypeDefinition("System.Delegate").ToTypeSig();
                Exception = definitionProvider.GetTypeDefinition("System.Exception").ToTypeSig().ToTypeDefOrRefSig();
            }

            public TypeRef GetTypeRef(string @namespace, string name)
            {
                return _corLibTypes.GetTypeRef(@namespace, name);
            }

            public CorLibTypeSig Void
            {
                get
                {
                    return _corLibTypes.Void;
                }
            }

            public CorLibTypeSig Boolean
            {
                get
                {
                    return _corLibTypes.Boolean;
                }
            }

            public CorLibTypeSig Char
            {
                get
                {
                    return _corLibTypes.Char;
                }
            }

            public CorLibTypeSig SByte
            {
                get
                {
                    return _corLibTypes.SByte;
                }
            }

            public CorLibTypeSig Byte
            {
                get
                {
                    return _corLibTypes.Byte;
                }
            }

            public CorLibTypeSig Int16
            {
                get
                {
                    return _corLibTypes.Int16;
                }
            }

            public CorLibTypeSig UInt16
            {
                get
                {
                    return _corLibTypes.UInt16;
                }
            }

            public CorLibTypeSig Int32
            {
                get
                {
                    return _corLibTypes.Int32;
                }
            }

            public CorLibTypeSig UInt32
            {
                get
                {
                    return _corLibTypes.UInt32;
                }
            }

            public CorLibTypeSig Int64
            {
                get
                {
                    return _corLibTypes.Int64;
                }
            }

            public CorLibTypeSig UInt64
            {
                get
                {
                    return _corLibTypes.UInt64;
                }
            }

            public CorLibTypeSig Single
            {
                get
                {
                    return _corLibTypes.Single;
                }
            }

            public CorLibTypeSig Double
            {
                get
                {
                    return _corLibTypes.Double;
                }
            }

            public CorLibTypeSig String
            {
                get
                {
                    return _corLibTypes.String;
                }
            }

            public CorLibTypeSig TypedReference
            {
                get
                {
                    return _corLibTypes.TypedReference;
                }
            }

            public CorLibTypeSig IntPtr
            {
                get
                {
                    return _corLibTypes.IntPtr;
                }
            }

            public CorLibTypeSig UIntPtr
            {
                get
                {
                    return _corLibTypes.UIntPtr;
                }
            }

            public CorLibTypeSig Object
            {
                get
                {
                    return _corLibTypes.Object;
                }
            }

            public TypeDefOrRefSig Thread
            {
                get;
            }
            public TypeSig Delegate { get; }
            public TypeDefOrRefSig Exception { get; }

            public AssemblyRef AssemblyRef
            {
                get
                {
                    return _corLibTypes.AssemblyRef;
                }
            }
        }


        private readonly IDefinitionContext _definitionContext;
        
        private readonly Dictionary<string, TypeDef> _typeCache;
        private readonly Dictionary<string, int> _sizeCache;
        //private readonly Dictionary<TypeSig, List<ITypeDefOrRef>> _inheritenceCache = new Dictionary<TypeSig, List<ITypeDefOrRef>>();
        private readonly Dictionary<string, MethodDef> _methodCache = new Dictionary<string, MethodDef>();
        private readonly BaseTypesInternal _baseTypes;

        public DefinitionProvider(IDefinitionContext definitionContext)
        {
            _definitionContext = definitionContext;
            _typeCache = BuildTypeCache(definitionContext.MainModule);
            _sizeCache = BuildSizeCache(definitionContext.MainModule);

            _baseTypes = new BaseTypesInternal(this, definitionContext.MainModule.CorLibTypes);

            // dirty, dirty trick
            AllocatedDelegate.DelegateTypeDef = GetTypeDefinition("System.Delegate");
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

        private static Dictionary<string, int> BuildSizeCache(ModuleDef module)
        {
            Dictionary<string, int> sizeCache = new Dictionary<string, int>();

            ICorLibTypes corLibTypes = module.CorLibTypes;

            sizeCache[corLibTypes.Boolean.FullName] = sizeof(bool);
            sizeCache[corLibTypes.Char.FullName] = sizeof(char);
            sizeCache[corLibTypes.SByte.FullName] = sizeof(sbyte);
            sizeCache[corLibTypes.Byte.FullName] = sizeof(byte);
            sizeCache[corLibTypes.Int16.FullName] = sizeof(short);
            sizeCache[corLibTypes.UInt16.FullName] = sizeof(ushort);
            sizeCache[corLibTypes.Int32.FullName] = sizeof(int);
            sizeCache[corLibTypes.UInt32.FullName] = sizeof(uint);
            sizeCache[corLibTypes.Int64.FullName] = sizeof(long);
            sizeCache[corLibTypes.UInt64.FullName] = sizeof(ulong);
            sizeCache[corLibTypes.Single.FullName] = sizeof(float);
            sizeCache[corLibTypes.Double.FullName] = sizeof(double);
            sizeCache[corLibTypes.IntPtr.FullName] = IntPtr.Size;
            sizeCache[corLibTypes.UIntPtr.FullName] = UIntPtr.Size;

            return sizeCache;
        }


        public TypeDef GetTypeDefinition(string fullTypeName)
        {
            if (_typeCache.TryGetValue(fullTypeName, out var type))
            {
                return type;
            }

            // naive, very naive implementation
            foreach (TypeDef t in _definitionContext.Modules.SelectMany(m => m.Types))
            {
                if (t.FullName == fullTypeName)
                {
                    _typeCache[t.FullName] = t;
                    return t;
                }
            }

            throw new TypeNotFoundException(fullTypeName);
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
            if (_sizeCache.TryGetValue(type.FullName, out var size))
            {
                return size;
            }

            throw new TypeNotSupportForSize(type.FullName);
        }

        //public IEnumerable<ITypeDefOrRef> InheritanceEnumerator(ITypeDefOrRef type)
        //{
        //    static List<ITypeDefOrRef> BuildInheritenceChain(ITypeDefOrRef type)
        //    {
        //        List<ITypeDefOrRef> chain = new List<ITypeDefOrRef>();

        //        var currentType = type;
        //        do
        //        {
        //            var currentTypeDef = currentType.ResolveTypeDefThrow();
        //            if (currentTypeDef == null)
        //            {
        //                break;
        //            }
        //            chain.Add(currentTypeDef);
        //            currentType = currentTypeDef.BaseType;
        //        } while (currentType != null);

        //        return chain;
        //    }

        //    TypeSig typeSig = type.ToTypeSig();
        //    if (_inheritenceCache.TryGetValue(typeSig, out var inheritenceChain))
        //    {
        //        return inheritenceChain;
        //    }

        //    inheritenceChain = BuildInheritenceChain(type);

        //    _inheritenceCache.Add(typeSig, inheritenceChain);
        //    return inheritenceChain;
        //}

        public IDefinitionContext Context
        {
            get
            {
                return _definitionContext;
            }
        }
    }
}
