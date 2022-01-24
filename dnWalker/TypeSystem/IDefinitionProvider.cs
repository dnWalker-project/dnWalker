using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    /// <summary>
    /// Provides the type and method definitions.
    /// </summary>
    public interface IDefinitionProvider
    {
        IDefinitionContext Context { get; }

        TypeDef GetTypeDefinition(string fullTypeName);

        MethodDef GetMethodDefinition(string fullMethodName);

        int SizeOf(TypeSig type);

        IBaseTypes BaseTypes { get; }

        //IEnumerable<ITypeDefOrRef> InheritanceEnumerator(ITypeDefOrRef type);
    }

    public interface IBaseTypes : ICorLibTypes
    {
        TypeDefOrRefSig Thread { get; }
        TypeDefOrRefSig Exception { get; }
        TypeSig Delegate { get; }
    }

    public static class DefinitionProviderExtensions
    {
        public static bool IsSubtype(this IDefinitionProvider definitionProvider, ITypeDefOrRef subType, ITypeDefOrRef superType)
        {
            SigComparer sigComparer = new SigComparer();
            
            TypeSig superSig = superType.ToTypeSig();

            foreach (ITypeDefOrRef tr in subType.InheritanceEnumerator())
            {
                if (sigComparer.Equals(tr.ToTypeSig(), superSig))
                {
                    return true;
                }
            }

            return false;
        }

        public static MethodDef FindVirtualMethod(this ExplicitActiveState cur, MethodDef method, IDataElement dataElement)
        {
            const string VirtualMethodLookupKey = "virtual-methods";

            dataElement = dataElement is IManagedPointer ptr ? ptr.Value : dataElement;

            if (!(dataElement is ObjectReference reference))
            {
                throw new NotSupportedException($"ObjectReference expected, '{dataElement?.GetType().FullName}' found.");
            }
            
            (MethodSig sig, string name) key = (method.MethodSig, method.Name);

            if (!cur.PathStore.CurrentPath.TryGetObjectAttribute(dataElement, VirtualMethodLookupKey, out Dictionary<(MethodSig, string), MethodDef> lookup))
            {
                lookup = new Dictionary<(MethodSig, string), MethodDef>();
                cur.PathStore.CurrentPath.SetObjectAttribute(dataElement, VirtualMethodLookupKey, lookup);
            }

            if (!lookup.TryGetValue(key, out MethodDef result))
            {
                AllocatedObject ao = (AllocatedObject)cur.DynamicArea.Allocations[reference];
                ITypeDefOrRef type = ao.Type;

                foreach (ITypeDefOrRef superType in type.InheritanceEnumerator())
                {
                    TypeDef superTypeDef = superType.ResolveTypeDefThrow();
                    MethodDef candidate = superTypeDef.FindMethod(method.Name, key.sig);

                    if (candidate != null && 
                        candidate.Body != null && 
                        candidate.Body.Instructions.Count > 0)
                    {
                        result = candidate;
                        break;
                    }
                }

                if (result != null)
                {
                    lookup[key] = result;
                }
                else
                {
                    throw new MemberNotFoundException(type.FullName, key.name);
                }
            }

            return result;
        }
    }
}
