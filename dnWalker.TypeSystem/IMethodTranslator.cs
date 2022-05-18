using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public interface IMethodTranslator
    {
        MethodSignature FromString(string text);
        string GetString(MethodSignature signature);
    }

    public static class MethodTranslatorExtensions
    {
        public static string GetString(this IMethodTranslator methodTranslator, IMethod method)
        {
            return methodTranslator.GetString(new MethodSignature(method));
        }
    }
}
