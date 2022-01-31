using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public static partial class Extensions
    {
        public static bool IsInterface(this TypeSig typeSig)
        {
            return typeSig.ToTypeDefOrRef().ResolveTypeDefThrow().IsInterface;
        }

        public static bool IsAbstract(this TypeSig typeSig)
        {
            return typeSig.ToTypeDefOrRef().ResolveTypeDefThrow().IsAbstract;
        }

        public static bool IsGeneric(this TypeSig typeSig)
        {
            return typeSig.IsGenericInstanceType;
        }

        public static IList<TypeSig> GetGenericParameters(this TypeSig typeSig)
        {
            return typeSig.ToGenericInstSig()?.GetGenericParameters() ?? Array.Empty<TypeSig>();
        }

        public static IEnumerable<ITypeDefOrRef> InheritanceEnumerator(this ITypeDefOrRef type)
        {
            var currentType = type;
            do
            {
                var currentTypeDef = currentType.ResolveTypeDefThrow();
                if (currentTypeDef == null)
                {
                    break;
                }
                yield return currentTypeDef;
                currentType = currentTypeDef.BaseType;
            } while (currentType != null);
        }

        public static bool TryGetTypeHandle(this ITypeDefOrRef typeRef, out RuntimeTypeHandle typeHandle)
        {
            TypeSig corLibType = typeRef.Module.CorLibTypes.GetCorLibTypeSig(typeRef);
            if (corLibType != null)
            {
                if (corLibType == typeRef.Module.CorLibTypes.String)
                {
                    typeHandle = typeof(string).TypeHandle;
                    return true;
                }
            }

            typeHandle = default(RuntimeTypeHandle);
            return false;
        }

        public static int GetFieldOffset(this FieldDef field)
        {
            if (field.FieldOffset.HasValue) return (int)field.FieldOffset.Value;

            IList<FieldDef> fields = field.DeclaringType.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                if (fields[i] == field)
                {
                    field.FieldOffset = (uint)i;
                    return i;
                }
            }

            throw new TypeSystemException($"Could not determine the field offset for field: {field}");
        }

        public static GenericInstSig CreateGenericTypeSig(this ClassOrValueTypeSig genericType, params TypeSig[] genericParameters)
        {
            return new GenericInstSig(genericType, genericParameters);
        }

        public static GenericInstSig CreateGenericTypeSig(this ClassOrValueTypeSig genericType, IEnumerable<TypeSig> genericParameters)
        {
            return new GenericInstSig(genericType, genericParameters.ToList());
        }
    }
}
