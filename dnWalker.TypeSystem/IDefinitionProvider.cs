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


        [Obsolete("The results of this method should be somehow cached in the definition provider. AND distinct static vs instance fields.")]
        public static int GetFieldOffset(IField field)
        {
            FieldDef fd = field.ResolveFieldDefThrow();
            if (!fd.FieldOffset.HasValue)
            {
                IList<FieldDef> fields = fd.DeclaringType.Fields;
                for (var i = 0; i < fields.Count; i++)
                {
                    if (fields[i] == fd)
                    {
                        fd.FieldOffset = (uint)i;
                        break;
                    }
                }
            }


            int typeOffset = 0;
            bool matched = false;
            int retval = 0;

            foreach (TypeDef typeDef in fd.DeclaringType.InheritanceEnumerator())
            {
                /*
				 * We start searching for the right field from the declaring type,
				 * it is possible that the declaring type does not define field, therefore
				 * it might be possible that we have to search further for field in
				 * the inheritance tree, (hence matched), and this continues until
				 * a field is found which has the same offset and the same name 
				 */
                if (TypeEqualityComparer.Instance.Equals(typeDef, fd.DeclaringType) || matched)
                {
                    if (fd.FieldOffset < typeDef.Fields.Count
                        && typeDef.Fields[(int)fd.FieldOffset].Name.Equals(fd.Name))
                    {
                        retval = (int)fd.FieldOffset;
                        break;
                    }

                    matched = true;
                }

                if (TypeEqualityComparer.Instance.Equals(typeDef.BaseType, typeDef.Module.CorLibTypes.Object)) 
                    // if base type is System.Object, stop
                {
                    typeOffset += Math.Max(0, typeDef.Fields.Count - 1);
                }
            }

            return typeOffset + retval;
        }
    }
}
