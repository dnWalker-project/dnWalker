using dnWalker.Parameters;

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

        //public static string ToSignatureString(this MethodInfo methodInfo)
        //{
        //    return new MethodSignature(methodInfo).ToString();
        //}

        //public static MethodSignature GetMethodSignature(this Type type, dnWalker.Parameters.MethodSignature methodSignature)
        //{
        //    Type[] argTypes = methodSignature.ArgumentTypeFullNames.Length == 0 ? 
        //        Type.EmptyTypes : 
        //        methodSignature.ArgumentTypeFullNames
        //            .Select(t => AppDomain.CurrentDomain.GetType(t) ?? throw new Exception($"Could not find type '{t}'"))
        //            .ToArray();

        //    return new MethodSignature(type.GetMethod(methodSignature.MethodName, argTypes) ?? throw new Exception($"Could not find method with signature '{methodSignature}'"));
        //}

        public static MethodInfo? GetMethodFromSignature(this Type type, MethodSignature methodSignature)
        {
            Type[] argTypes = methodSignature.ArgumentTypeFullNames.Length == 0 ?
                Type.EmptyTypes :
                methodSignature.ArgumentTypeFullNames
                    .Select(t => AppDomain.CurrentDomain.GetType(t) ?? throw new Exception($"Could not find type '{t}'"))
                    .ToArray();

            return type.GetMethod(methodSignature.MethodName, argTypes);
        }
        //public static MethodInfo? GetMethodFromSignature(this Type type, MethodSignature methodSignature, BindingFlags bindingFlags)
        //{
        //    Type[] argTypes = methodSignature.ArgumentTypeFullNames.Length == 0 ?
        //        Type.EmptyTypes :
        //        methodSignature.ArgumentTypeFullNames
        //            .Select(t => AppDomain.CurrentDomain.GetType(t) ?? throw new Exception($"Could not find type '{t}'"))
        //            .ToArray();

        //    return type.GetMethod(methodSignature.MethodName, argTypes);
        //}
    }
}
