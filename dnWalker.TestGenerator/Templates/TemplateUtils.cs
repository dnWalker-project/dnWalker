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

        public static string GetTypeNameOrAlias(TypeSig type)
        {
            if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.SByte))
            {
                return "sbyte";
            }
            if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int16))
            {
                return "short";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int32))
            {
                return "int";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Int64))
            {
                return "long";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Byte))
            {
                return "byte";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt16))
            {
                return "ushort";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt32))
            {
                return "uint";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.UInt64))
            {
                return "ulong";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Single))
            {
                return "float";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Double))
            {
                return "double";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Char))
            {
                return "char";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.Boolean))
            {
                return "bool";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, type.Module.CorLibTypes.String))
            {
                return "string";
            }
            else
            {
                return type.TypeName;
            }
        }
    }
}
