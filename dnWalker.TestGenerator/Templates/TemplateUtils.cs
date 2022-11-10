using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public static class TemplateUtils
    {
        public static readonly string Semicolon = ";";
        public static readonly string Coma = ",";
        public static readonly string ComaSpace = ", ";
        public static readonly string Dot = ".";
        public static readonly string Null = "null";
        public static readonly string Zero = "0";
        public static readonly string Default = "default";
        public static readonly string True = bool.TrueString;
        public static readonly string False = bool.FalseString;
        public static readonly string WhiteSpace = " ";
        public static readonly string Indent = "    ";
        
        public static readonly string AssignmentOperator = " = ";

        public static bool HasAlias(TypeSig type)
        {
            return type.ElementType switch
            {
                ElementType.Void => true,
                ElementType.Boolean => true,
                ElementType.Char => true,
                ElementType.I1 => true,
                ElementType.U1 => true,
                ElementType.I2 => true,
                ElementType.U2 => true,
                ElementType.I4 => true,
                ElementType.U4 => true,
                ElementType.I8 => true,
                ElementType.U8 => true,
                ElementType.R4 => true,
                ElementType.R8 => true,
                ElementType.String => true,
                _ => false
            };
        }

        public static string GetTypeNameOrAlias(TypeSig type)
        {
            return type.ElementType switch
            {
                ElementType.Void => "void",
                ElementType.Boolean => "bool",
                ElementType.Char => "char",
                ElementType.I1 => "sbyte",
                ElementType.U1 => "byte",
                ElementType.I2 => "short",
                ElementType.U2 => "ushort",
                ElementType.I4 => "int",
                ElementType.U4 => "uint",
                ElementType.I8 => "long",
                ElementType.U8 => "ulong",
                ElementType.R4 => "float",
                ElementType.R8 => "double",
                ElementType.String => "string",
                ElementType.Object => "object",
                _ => ((IType)type).Name
            };


            //if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.SByte))
            //{
            //    return "sbyte";
            //}
            //if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int16))
            //{
            //    return "short";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int32))
            //{
            //    return "int";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int64))
            //{
            //    return "long";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Byte))
            //{
            //    return "byte";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt16))
            //{
            //    return "ushort";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt32))
            //{
            //    return "uint";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt64))
            //{
            //    return "ulong";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Single))
            //{
            //    return "float";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Double))
            //{
            //    return "double";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Char))
            //{
            //    return "char";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Boolean))
            //{
            //    return "bool";
            //}
            //else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.String))
            //{
            //    return "string";
            //}
            //else
            //{
            //    return type.TypeName;
            //}
        }
    }
}
