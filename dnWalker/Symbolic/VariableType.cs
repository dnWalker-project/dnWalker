using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    /// <summary>
    /// Specifies a type of variable.
    /// </summary>
    public enum VariableType
    {
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Int8,
        Int16,
        Int32,
        Int64,
        Boolean,
        Char,
        Single,
        Double,

        String,

        Object,
        Array
    }


    public static class VariableTypeExtensions
    {
        public static bool IsLogical(this VariableType type)
        {
            switch (type)
            {
                case VariableType.Boolean:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsInteger(this VariableType type)
        {
            switch (type)
            {
                case VariableType.UInt8:
                case VariableType.UInt16:
                case VariableType.UInt32:
                case VariableType.UInt64:
                case VariableType.Int8:
                case VariableType.Int16:
                case VariableType.Int32:
                case VariableType.Int64:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsReal(this VariableType type)
        {
            switch (type)
            {
                case VariableType.Single:
                case VariableType.Double:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsReference(this VariableType type)
        {
            switch (type)
            {
                case VariableType.String:
                case VariableType.Object:
                case VariableType.Array:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsPrimitive(this VariableType type)
        {
            switch (type)
            {
                case VariableType.UInt8:
                case VariableType.UInt16:
                case VariableType.UInt32:
                case VariableType.UInt64:
                case VariableType.Int8:
                case VariableType.Int16:
                case VariableType.Int32:
                case VariableType.Int64:

                case VariableType.Single:
                case VariableType.Double:
                case VariableType.Boolean:
                case VariableType.Char:
                    return true;

                default:
                    return false;

            }
        }

        public static VariableType ToVariableType(TypeSig type)
        {
            TypeEqualityComparer tec = TypeEqualityComparer.Instance;
            ICorLibTypes corTypes = type.Module.CorLibTypes;
            if (tec.Equals(type, corTypes.Boolean)) return VariableType.Boolean;
            if (tec.Equals(type, corTypes.Char)) return VariableType.Char;
            if (tec.Equals(type, corTypes.Byte)) return VariableType.UInt8;
            if (tec.Equals(type, corTypes.SByte)) return VariableType.Int8;
            if (tec.Equals(type, corTypes.UInt16)) return VariableType.UInt16;
            if (tec.Equals(type, corTypes.Int16)) return VariableType.Int16;
            if (tec.Equals(type, corTypes.UInt32)) return VariableType.UInt32;
            if (tec.Equals(type, corTypes.Int32)) return VariableType.Int32;
            if (tec.Equals(type, corTypes.UInt64)) return VariableType.UInt64;
            if (tec.Equals(type, corTypes.Int64)) return VariableType.Int64;
            if (tec.Equals(type, corTypes.Single)) return VariableType.Single;
            if (tec.Equals(type, corTypes.Double)) return VariableType.Double;
            if (tec.Equals(type, corTypes.String)) return VariableType.String;

            if (type.IsSZArray || type.IsArray) return VariableType.Array;
            
            return VariableType.Object;
        }
    }
}
