using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IDefinitionProvider
    {
        IDomain Context { get; }

        TypeDef GetTypeDefinition(string fullTypeName);
        MethodDef GetMethodDefinition(string fullMethodName);

        int SizeOf(TypeSig type);

        IBaseTypes BaseTypes { get; }
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

    }
}
