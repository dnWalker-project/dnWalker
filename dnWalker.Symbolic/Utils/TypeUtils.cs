using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Utils
{
    public static class TypeUtils
    {
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
    }
}
