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
        public static void WriteFullName(this IWriter writer, TypeSig type)
        {
            if (type.IsSZArray || type.IsArray)
            {
                writer.WriteFullName(type.Next);
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
                    writer.WriteFullName(declType.ToTypeSig());
                    writer.Write(TemplateUtils.Dot);
                }
            }

            IType t = type;

            writer.Write(t.Namespace);
            writer.Write(TemplateUtils.Dot);
            writer.Write(t.Name);

            if (type.IsGenericInstanceType)
            {
                IList<TypeSig> genericArgs = ((GenericInstSig)type).GenericArguments;
                writer.Write("<");

                writer.WriteFullName(genericArgs[0]);

                for (int i = 1; i < genericArgs.Count; ++i)
                {
                    writer.Write(TemplateUtils.ComaSpace);
                    writer.WriteFullName(genericArgs[i]);
                }

                writer.Write(">");
            }
        }
    
        public static void WriteNameOrAlias(this IWriter writer, TypeSig type)
        {
            if (type.IsSZArray || type.IsArray)
            {
                writer.WriteNameOrAlias(type.Next);
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
                    writer.WriteNameOrAlias(declType.ToTypeSig());
                    writer.Write(TemplateUtils.Dot);
                }
            }

            writer.Write(TemplateUtils.GetTypeNameOrAlias(type));

            if (type.IsGenericInstanceType)
            {
                IList<TypeSig> genericArgs = ((GenericInstSig)type).GenericArguments;
                writer.Write("<");

                writer.WriteNameOrAlias(genericArgs[0]);

                for (int i = 1; i < genericArgs.Count; ++i)
                {
                    writer.Write(TemplateUtils.ComaSpace);
                    writer.WriteNameOrAlias(genericArgs[i]);
                }

                writer.Write(">");
            }
        }
    }
}
