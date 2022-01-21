using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public readonly struct TypeName : ICodeExpression
    {
        public TypeName(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public Type Type { get; }

        public void WriteTo(StringBuilder output)
        {
            Write(output, Type);
        }

        private static void Write(StringBuilder output, Type type)
        {
            if (type.IsArray)
            {
                type = type.GetElementType()!;
                Write(output, type);
                output.Append("[]");
            }
            else if (type.IsGenericType)
            {
                // we have to handle the generics
                output.Append(TemplateHelpers.WithoutGenerics(TemplateHelpers.GetTypeNameOrAlias(type)));
                output.Append("<");

                Type[] genericArgs = type.GetGenericArguments();
                if (genericArgs.Length >= 1)
                {
                    Write(output, genericArgs[0]);

                    for (int i = 1; i < genericArgs.Length; ++i)
                    {
                        output.Append(TemplateHelpers.Coma);
                        Write(output, genericArgs[i]);
                    }
                }

                output.Append(">");
            }
            else
            {
                output.Append(TemplateHelpers.GetTypeNameOrAlias(type));
            }
        }
    }
}
