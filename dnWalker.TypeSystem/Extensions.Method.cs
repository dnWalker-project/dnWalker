using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TypeSystem
{
    public static partial class Extensions
    {
        public static bool IsGenericMethod(this IMethod method)
        {
            return method.MethodSig.CallingConvention.HasFlag(CallingConvention.Generic);
        }

        public static IList<TypeSig> GetGenericParameters(this IMethod method)
        {
            if (method is MethodSpec methodSpec)
            {
                return methodSpec.GenericInstMethodSig?.GetGenericArguments() ?? Array.Empty<TypeSig>();
            }

            return Array.Empty<TypeSig>();
        }

        public static bool HasReturnValue(this IMethod method)
        {
            return !TypeEqualityComparer.Instance.Equals(method.MethodSig.RetType, method.Module.CorLibTypes.Void);
        }
    }
}
