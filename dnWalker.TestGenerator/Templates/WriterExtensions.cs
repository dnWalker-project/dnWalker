using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public static class WriterExtensions
    {
        public static void Write(this IWriter writer, TypeSig type)
        {
            if (type.IsSZArray || type.IsArray)
            {
                writer.Write(type.Next);
                writer.Write("[]");
                return;
            }

            if (type.IsTypeDefOrRef)
            {
                ITypeDefOrRef typeDefOrRef = ((TypeDefOrRefSig)type).TypeDefOrRef;
                ITypeDefOrRef declType = typeDefOrRef.DeclaringType;
                if (declType != null)
                {
                    // is nested
                    writer.Write(declType.ToTypeSig());
                    writer.Write(TemplateUtils.Dot);
                }
            }

            writer.Write(TemplateUtils.GetTypeNameOrAlias(type));

            if (type.IsGenericInstanceType)
            {
                IList<TypeSig> genericArgs = ((GenericInstSig)type).GenericArguments;
                writer.Write("<");

                writer.Write(genericArgs[0]);

                for (int i = 1; i < genericArgs.Count; ++i)
                {
                    writer.Write(TemplateUtils.ComaSpace);
                    writer.Write(genericArgs[i]);
                }

                writer.Write(">");
            }
        }
    }
}
