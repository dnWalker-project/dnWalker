using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface ITypeTranslator
    {
        TypeSignature FromString(string text);
        string GetString(TypeSignature signature);
    }

    public static class TypeTranslatorExtensions
    {
        public static string GetString(this ITypeTranslator typeTranslator, TypeSig type)
        {
            return typeTranslator.GetString(new TypeSignature(type.ToTypeDefOrRef()));
        }

        public static string GetString(this ITypeTranslator typeTranslator, ITypeDefOrRef type)
        {
            return typeTranslator.GetString(new TypeSignature(type));
        }
    }
}
