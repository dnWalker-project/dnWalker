using dnlib.DotNet;

using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        protected void WriteTypeName(TypeSignature type)
        {
            if (type.IsNested) // i.a. is nested
            {
                WriteTypeName(type.DeclaringType);
                Write(TemplateHelpers.Dot);
            }

            if (type.IsSZArray || type.IsArray)
            {
                type = type.ElementType;
                WriteTypeName(type);
                Write("[]");
            }
            else if (type.IsGenericInstance)
            {
                Write(TemplateHelpers.WithoutGenerics(TemplateHelpers.GetTypeNameOrAlias(type)));
                Write("<");

                TypeSignature[] genericParams = type.GetGenericParameters();
                if (genericParams.Length >= 1)
                {
                    WriteTypeName(genericParams[0]);

                    for (int i = 1; i < genericParams.Length; ++i)
                    {
                        Write(TemplateHelpers.Coma);
                        WriteTypeName(genericParams[i]);
                    }
                }

                Write(">");
            }
            else
            {
                Write(TemplateHelpers.GetTypeNameOrAlias(type));
            }
        }

        protected void WriteMockTypeName(TypeSignature type)
        {
            Write("Mock<");
            WriteTypeName(type);
            Write(">");
        }
    }
}
