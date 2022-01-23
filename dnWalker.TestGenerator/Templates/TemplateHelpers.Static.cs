
using System;

namespace dnWalker.TestGenerator.Templates
{

    public partial class TemplateHelpers
    {
        public static readonly string Coma = ", ";
        public static readonly string Dot = ".";
        public static readonly string Null = "null";
        public static readonly string Zero = "0";
        public static readonly string Default = "default";
        public static readonly string True = bool.TrueString;
        public static readonly string False = bool.FalseString;
        public static readonly string WhiteSpace = " ";
        public static readonly string Indent = "    ";

        public static string GetTypeNameOrAlias(Type t)
        {
            if (t == typeof(sbyte))
            {
                return "sbyte";
            }
            else if (t == typeof(short))
            {
                return "short";
            }
            else if (t == typeof(int))
            {
                return "int";
            }
            else if (t == typeof(long))
            {
                return "long";
            }
            else if (t == typeof(byte))
            {
                return "byte";
            }
            else if (t == typeof(ushort))
            {
                return "ushort";
            }
            else if (t == typeof(uint))
            {
                return "uint";
            }
            else if (t == typeof(ulong))
            {
                return "ulong";
            }
            else if (t == typeof(float))
            {
                return "float";
            }
            else if (t == typeof(double))
            {
                return "double";
            }
            else if (t == typeof(char))
            {
                return "char";
            }
            else if (t == typeof(bool))
            {
                return "bool";
            }
            else if (t == typeof(string))
            {
                return "string";
            }
            else
            {
                return t.Name;
            }
        }

        public static string WithoutGenerics(string nameWithGenerics)
        {
            int index = nameWithGenerics.IndexOf('`');
            return index == -1 ? nameWithGenerics : nameWithGenerics.Substring(0, index);
        }



        public static string GetDefaultLiteral(Type type)
        {
            if (type.IsClass || type.IsInterface)
            {
                return Null;
            }
            else if (type == typeof(sbyte))
            {
                return Zero;
            }
            else if (type == typeof(short))
            {
                return Zero;
            }
            else if (type == typeof(int))
            {
                return Zero;
            }
            else if (type == typeof(long))
            {
                return Zero;
            }
            else if (type == typeof(byte))
            {
                return Zero;
            }
            else if (type == typeof(ushort))
            {
                return Zero;
            }
            else if (type == typeof(uint))
            {
                return Zero;
            }
            else if (type == typeof(ulong))
            {
                return Zero;
            }
            else if (type == typeof(float))
            {
                return Zero;
            }
            else if (type == typeof(double))
            {
                return Zero;
            }
            else if (type == typeof(char))
            {
                return "'\0'";
            }
            else if (type == typeof(bool))
            {
                return False;
            }

            else
            {
                return Default;
            }
        }
    }
}