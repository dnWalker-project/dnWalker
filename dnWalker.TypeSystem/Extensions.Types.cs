using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
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



        public static TypeCode GetTypeCode(this TypeSig type)
        {
            if (type.IsObject()) return TypeCode.Object;
            else if (type.IsBoolean()) return TypeCode.Boolean;
            else if (type.IsByte()) return TypeCode.Byte;
            else if (type.IsSByte()) return TypeCode.SByte;
            else if (type.IsUInt16()) return TypeCode.UInt16;
            else if (type.IsInt16()) return TypeCode.Int16;
            else if (type.IsUInt32()) return TypeCode.UInt32;
            else if (type.IsInt32()) return TypeCode.Int32;
            else if (type.IsUInt64()) return TypeCode.UInt64;
            else if (type.IsInt64()) return TypeCode.Int64;
            else if (type.IsSingle()) return TypeCode.Single;
            else if (type.IsDouble()) return TypeCode.Double;
            else if (type.IsString()) return TypeCode.String;
            else if (type.IsChar()) return TypeCode.Char;

            return TypeCode.Empty;
        }

        public static bool IsBoolean(this TypeSig type)
        {
            return TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Boolean, type);
        }

        public static bool IsNumber(this TypeSig type)
        {
            return
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.SByte, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int16, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int32, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int64, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Byte, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt16, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt32, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt64, type) ||

                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.IntPtr, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UIntPtr, type) ||

                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Single, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Double, type);
        }

        public static bool IsInteger(this TypeSig type)
        {
            return
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.SByte, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int16, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int32, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int64, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Byte, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt16, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt32, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt64, type) ||

                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.IntPtr, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UIntPtr, type);
        }

        public static bool IsChar(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Char, type);
        public static bool IsByte(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Byte, type);
        public static bool IsUInt16(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt16, type);
        public static bool IsUInt32(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt32, type);
        public static bool IsUInt64(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.UInt64, type);
        public static bool IsSByte(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.SByte, type);
        public static bool IsInt16(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int16, type);
        public static bool IsInt32(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int32, type);
        public static bool IsInt64(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Int64, type);
        public static bool IsSingle(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Single, type);
        public static bool IsDouble(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Double, type);
        public static bool IsObject(this TypeSig type) => TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Object, type);

        public static bool IsReal(this TypeSig type)
        {
            return
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Single, type) ||
                TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.Double, type);
        }

        public static bool IsString(this TypeSig type)
        {
            return TypeEqualityComparer.Instance.Equals(type.Module.CorLibTypes.String, type);
        }

        public static bool IsAssignableTo(this ITypeDefOrRef inheritingType, ITypeDefOrRef baseType)
        {
            if (inheritingType == null) throw new ArgumentNullException(nameof(inheritingType));
            if (baseType == null) throw new ArgumentNullException(nameof(baseType));

            ITypeDefOrRef? type = inheritingType;

            while (type != null)
            {
                if (TypeEqualityComparer.Instance.Equals(type, baseType))
                {
                    return true;
                }
                type = type.GetBaseType();
            }
            return false;
        }

        public static bool IsAssignableFrom(this ITypeDefOrRef baseType, ITypeDefOrRef inheritingType)
        {
            return IsAssignableFrom(inheritingType, baseType);
        }


        public static bool InitLayout(this TypeDef typeDef)
        {
            IList<FieldDef> fields = typeDef.Fields;

            if (fields.Count == 0 || fields[0].FieldOffset.HasValue) return false; // initialized

            InitLayoutRecursive(typeDef);

            return true;
        }

        private static uint InitLayoutRecursive(TypeDef typeDef)
        {
            if (typeDef.FullName == "System.Object") return 0;

            // does it actually work?
            uint? fOffset = typeDef.Fields.Count == 0 ? null : typeDef.Fields[0].FieldOffset;
            if (fOffset.HasValue)
            {
                // already initialized => skip it & reuse
                return (uint)(fOffset.Value + typeDef.Fields.Count);
            }

            uint offset = InitLayoutRecursive(typeDef.GetBaseTypeThrow().ResolveTypeDefThrow());

            IList<FieldDef> fields = typeDef.Fields;
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].FieldOffset = offset++;
            }
            return offset;
        }
    }
}
