using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IMethodResolver
    {
        IReadOnlyDictionary<MethodSignature, ParameterRef[]> GetMethodResults();

        bool TryGetMethodResult(MethodSignature methodSignature, int invocation, out ParameterRef resultRef);
        
        void SetMethodResult(MethodSignature methodSignature, int invocation, ParameterRef resultRef);

        void ClearMethodResult(MethodSignature methodSignature, int invocation);

        void MoveTo(IMethodResolver other)
        {
            // make copy of the results because we might edit the returned dictionary
            List<KeyValuePair<MethodSignature,ParameterRef[]>> results = GetMethodResults().ToList();
            foreach (var kvp in results)
            {
                for (int i = 0; i < kvp.Value.Length; ++i)
                {
                    if (kvp.Value[i] == ParameterRef.Empty) continue;

                    other.SetMethodResult(kvp.Key, i, kvp.Value[i]);
                    ClearMethodResult(kvp.Key, i);
                }
            }
        }
    }

    public interface IMethodResolverParameter : IMethodResolver, IParameter
    {
    }

    public static class MethodResolverParameterExtensions
    {
        public static bool TryGetMethodResult(this IMethodResolverParameter methodResolver, MethodSignature methodSignature, int invocation, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (methodResolver.TryGetMethodResult(methodSignature, invocation, out ParameterRef reference) &&
                reference.TryResolve(methodResolver.Context, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }
        public static bool TryGetMethodResult<TParameter>(this IMethodResolverParameter methodResolver, MethodSignature methodSignature, int invocation, [NotNullWhen(true)] out TParameter? parameter)
            where TParameter : class, IParameter
        {
            if (methodResolver.TryGetMethodResult(methodSignature, invocation, out ParameterRef reference) &&
                reference.TryResolve(methodResolver.Context, out parameter))
            {
                return true;
            }

            parameter = null;
            return false;
        }

        public static void SetMethodResult(this IMethodResolverParameter methodResolver, MethodSignature methodSignature, int invocation, IParameter methodResult)
        {
            methodResolver.SetMethodResult(methodSignature, invocation, methodResult.Reference);
        }

        public static IReadOnlyDictionary<MethodSignature, IParameter?[]> GetMethodResults(this IMethodResolverParameter methodResolver)
        {
            IReadOnlyDictionary<MethodSignature, ParameterRef[]> refs = methodResolver.GetMethodResults();

            return new Dictionary<MethodSignature, IParameter?[]>(refs.Select(p => KeyValuePair.Create(p.Key, p.Value.Select(r => r.Resolve(methodResolver.Context)).ToArray())));
        }
    }
}
