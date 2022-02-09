
using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;

namespace dnWalker.TestGenerator.Templates
{

    public partial class TemplateHelpers
    {
        public static readonly string Semicolon = ";";
        public static readonly string Coma = ", ";
        public static readonly string Dot = ".";
        public static readonly string Null = "null";
        public static readonly string Zero = "0";
        public static readonly string Default = "default";
        public static readonly string True = bool.TrueString;
        public static readonly string False = bool.FalseString;
        public static readonly string WhiteSpace = " ";
        public static readonly string Indent = "    ";

        public static string GetTypeNameOrAlias(TypeSignature typeSignature)
        {
            if (typeSignature.IsSByte)
            {
                return "sbyte";
            }
            if (typeSignature.IsInt16)
            {
                return "short";
            }
            else if (typeSignature.IsInt32)
            {
                return "int";
            }
            else if (typeSignature.IsInt64)
            {
                return "long";
            }
            else if (typeSignature.IsByte)
            {
                return "byte";
            }
            else if (typeSignature.IsUInt16)
            {
                return "ushort";
            }
            else if (typeSignature.IsUInt32)
            {
                return "uint";
            }
            else if (typeSignature.IsUInt64)
            {
                return "ulong";
            }
            else if (typeSignature.IsSingle)
            {
                return "float";
            }
            else if (typeSignature.IsDouble)
            {
                return "double";
            }
            else if (typeSignature.IsChar)
            {
                return "char";
            }
            else if (typeSignature.IsBoolean)
            {
                return "bool";
            }
            else if (typeSignature.IsString)
            {
                return "string";
            }
            else
            {
                return typeSignature.Name;
            }
        }

        public static string WithoutGenerics(string nameWithGenerics)
        {
            int index = nameWithGenerics.IndexOf('`');
            return index == -1 ? nameWithGenerics : nameWithGenerics.Substring(0, index);
        }



        public static string GetDefaultLiteral(TypeSignature typeSignature)
        {
            if (!typeSignature.IsValueType || typeSignature.IsArray || typeSignature.IsSZArray || typeSignature.IsInterface)
            {
                return Null;
            }
            else if (typeSignature.IsSByte)
            {
                return Zero;
            }
            if (typeSignature.IsInt16)
            {
                return Zero;
            }
            else if (typeSignature.IsInt32)
            {
                return Zero;
            }
            else if (typeSignature.IsInt64)
            {
                return Zero;
            }
            else if (typeSignature.IsByte)
            {
                return Zero;
            }
            else if (typeSignature.IsUInt16)
            {
                return Zero;
            }
            else if (typeSignature.IsUInt32)
            {
                return Zero;
            }
            else if (typeSignature.IsUInt64)
            {
                return Zero;
            }
            else if (typeSignature.IsSingle)
            {
                return Zero;
            }
            else if (typeSignature.IsDouble)
            {
                return Zero;
            }
            else if (typeSignature.IsChar)
            {
                return @"'\0'";
            }
            else if (typeSignature.IsBoolean)
            {
                return False;
            }
            else if (typeSignature.IsString)
            {
                return Null;
            }
            else
            {
                return Default;
            }

        }
    }
}