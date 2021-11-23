using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Reflection
{
    public static class ReflectionExtensions
    {
        private static readonly Dictionary<string, Type> _cachedTypesLookup = new Dictionary<string, Type>();

        public static Type? GetType(this AppDomain appDomain, string typename)
        {
            if (_cachedTypesLookup.TryGetValue(typename, out Type? type))
            {
                return type;
            }

            foreach (Assembly a in appDomain.GetAssemblies())
            {
                Type? t = a.GetType(typename);
                if (t != null)
                {
                    _cachedTypesLookup[typename] = t;
                    return t;
                }
            }

            return null;
        }

        public static string ToSignatureString(this MethodInfo methodInfo)
        {
            return new MethodSignature(methodInfo).ToString();
        }
    }
}
