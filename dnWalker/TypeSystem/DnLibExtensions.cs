using dnlib.DotNet;

using System;
using System.Collections.Generic;

namespace dnWalker.TypeSystem
{
    public static class DnLibExtensions
    {
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
    }
}
