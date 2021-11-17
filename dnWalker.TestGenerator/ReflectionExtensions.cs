using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public static class ReflectionExtensions
    {
        public static Type? GetType(this AppDomain appDomain, string typename)
        {
            foreach (Assembly a in appDomain.GetAssemblies())
            {
                Type? t = a.GetType(typename);
                if (t != null) return t;
            }

            return null;
        }
    }
}
