using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteTypeName(Type type)
        {
            if (type.IsNested)
            {
                WriteTypeName(type.DeclaringType ?? throw new Exception("Could not access the nested element declaring type."));
                Write(TemplateHelpers.Dot);
            }

            if (type.IsArray)
            {
                type = type.GetElementType() ?? throw new Exception("Could not access the array element type.");
                WriteTypeName(type);
                Write("[]");
            }
            else if (type.IsGenericType)
            {
                Write(TemplateHelpers.WithoutGenerics(TemplateHelpers.GetTypeNameOrAlias(type)));
                Write("<");

                Type[] genericArgs = type.GetGenericArguments();
                if (genericArgs.Length >= 1)
                {
                    WriteTypeName(genericArgs[0]);

                    for (int i = 1; i < genericArgs.Length; ++i)
                    {
                        Write(TemplateHelpers.Coma);
                        WriteTypeName(genericArgs[i]);
                    }
                }

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.GetTypeNameOrAlias(type));
            }
        }
    }
}
