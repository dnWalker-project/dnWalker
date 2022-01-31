
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
            TypeSig type = typeSignature.ToTypeDefOrRef().ToTypeSig();
            ICorLibTypes types = type.Module.CorLibTypes;

            if (TypeEqualityComparer.Instance.Equals(type, types.SByte))
            {
                return "sbyte";
            }
            if (TypeEqualityComparer.Instance.Equals(type, types.Int16))
            {
                return "short";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Int32))
            {
                return "int";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Int64))
            {
                return "long";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Byte))
            {
                return "byte";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt16))
            {
                return "ushort";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt32))
            {
                return "uint";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt64))
            {
                return "ulong";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Single))
            {
                return "float";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Double))
            {
                return "double";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Char))
            {
                return "char";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Boolean))
            {
                return "bool";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.String))
            {
                return "string";
            }
            else
            {
                return type.TypeName;
            }
        }

        public static string WithoutGenerics(string nameWithGenerics)
        {
            int index = nameWithGenerics.IndexOf('`');
            return index == -1 ? nameWithGenerics : nameWithGenerics.Substring(0, index);
        }



        public static string GetDefaultLiteral(TypeSignature typeSignature)
        {
            TypeSig type = typeSignature.ToTypeDefOrRef().ToTypeSig();
            ICorLibTypes types = type.Module.CorLibTypes;

            if (type.IsClassSig || type.IsArray || type.IsSZArray)
            {
                return Null;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.SByte))
            {
                return Zero;
            }
            if (TypeEqualityComparer.Instance.Equals(type, types.Int16))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Int32))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Int64))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Byte))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt16))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt32))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.UInt64))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Single))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Double))
            {
                return Zero;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Char))
            {
                return @"'\0'";
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.Boolean))
            {
                return False;
            }
            else if (TypeEqualityComparer.Instance.Equals(type, types.String))
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