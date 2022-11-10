using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    internal class Resolver2 : IResolver
    {
        private readonly IAssemblyResolver _assemblyResolver;
        private readonly IResolver _smartResolver;

        private readonly Dictionary<TypeRef, TypeDef> _typeCache = new Dictionary<TypeRef, TypeDef>(TypeEqualityComparer.Instance);

        public Resolver2(IAssemblyResolver assemblyResolver, IResolver resolver)
        {
            _smartResolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _assemblyResolver = assemblyResolver ?? throw new ArgumentNullException(nameof(assemblyResolver));
        }

        public TypeDef Resolve(TypeRef typeRef, ModuleDef sourceModule)
        {
            if (!_typeCache.TryGetValue(typeRef, out TypeDef result))
            {
                result = _smartResolver.Resolve(typeRef, sourceModule);
                if (result == null)
                {
                    result = DumpSearch(typeRef, sourceModule);
                }

                _typeCache[typeRef] = result;
            }

            return result;
        }

        private TypeDef DumpSearch(TypeRef typeRef, ModuleDef sourceModule)
        {
            string fullName = typeRef.FullName;
            AssemblyResolver ar = (AssemblyResolver)_assemblyResolver;

            foreach (var a in ar.GetCachedAssemblies())
            {
                TypeDef td = a.Find(typeRef);
                if (td != null)
                {
                    return td;
                }
            }

            return null;
        }

        public IMemberForwarded Resolve(MemberRef memberRef)
        {
            IMemberForwarded result = _smartResolver.Resolve(memberRef);
            if (result == null)
            {
                result = DumpSearch(memberRef);
            }
            return result;
        }

        private IMemberForwarded DumpSearch(MemberRef memberRef)
        {
            // let's assume that the owner type was not found
            if (memberRef.Class is TypeRef ownerRef)
            {
                return DumpSearch(ownerRef, memberRef.Module).Resolve(memberRef);
            }
            return null;
        }
    }
}
