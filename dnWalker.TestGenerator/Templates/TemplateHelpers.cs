using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    internal static class TemplateHelpers
    {
        public const string Coma = ", ";

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
        public static void JoinAndWriteExpressions<TExpr>(StringBuilder output, string separator, IEnumerable<TExpr> expressions)
            where TExpr : ICodeExpression
        {
            JoinAndWriteExpressions(output, separator, expressions.ToArray());
        }

        public static void JoinAndWriteExpressions<TExpr>(StringBuilder output, string separator, params TExpr[] expressions)
            where TExpr : ICodeExpression
        {
            if (expressions.Length == 0) return;
            expressions[0].WriteTo(output);
            for (int i = 1; i < expressions.Length; ++i)
            {
                output.Append(separator);
                expressions[i].WriteTo(output);
            }
        }
    }
}
