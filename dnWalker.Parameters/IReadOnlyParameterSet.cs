using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IReadOnlyParameterSet
    {
        IParameterContext Context { get; }

        IReadOnlyDictionary<ParameterRef, IParameter> Parameters { get; }
        IReadOnlyDictionary<string, ParameterRef> Roots { get; }
    }

    public static class ReadOnlyParameterSetExtensions
    {
        public static IParameter? GetRoot(this IReadOnlyParameterSet set, string expression)
        {
            return set.Roots[expression].Resolve(set);
        }

        public static bool TryGetRoot(this IReadOnlyParameterSet set, string expression,[NotNullWhen(true)] out IParameter? rootParameter)
        {
            if (set.Roots.TryGetValue(expression, out ParameterRef parameterRef))
            {
                return parameterRef.TryResolve(set, out rootParameter);
            }
            rootParameter = null;
            return false;
        }

        public static TParameter? GetRoot<TParameter>(this IReadOnlyParameterSet set, string expression)
            where TParameter : class, IParameter
        {
            return set.Roots[expression].Resolve<TParameter>(set);
        }

        public static bool TryGetRoot<TParameter>(this IReadOnlyParameterSet set, string expression, [NotNullWhen(true)] out TParameter? rootParameter)
            where TParameter : class, IParameter
        {
            if (set.Roots.TryGetValue(expression, out ParameterRef parameterRef))
            {
                return parameterRef.TryResolve<TParameter>(set, out rootParameter);
            }
            rootParameter = null;
            return false;
        }

        public static IParameter? GetThis(this IReadOnlyParameterSet set)
        {
            return set.GetRoot(ParameterAccessor.ThisName);
        }

        public static bool TryGetThis(this IReadOnlyParameterSet set, [NotNullWhen(true)] out IParameter? thisParameter)
        {
            return TryGetRoot(set, ParameterAccessor.ThisName, out thisParameter);
        }

        public static TParameter? GetThis<TParameter>(this IReadOnlyParameterSet set)
            where TParameter : class, IParameter
        {
            return set.GetRoot<TParameter>(ParameterAccessor.ThisName);
        }

        public static bool TryGetThis<TParameter>(this IReadOnlyParameterSet set, [NotNullWhen(true)] out TParameter? thisParameter)
            where TParameter : class, IParameter
        {
            return TryGetRoot(set, ParameterAccessor.ThisName, out thisParameter);
        }


        public static IParameter? GetReturnValue(this IReadOnlyParameterSet set)
        {
            return set.GetRoot(ParameterAccessor.ReturnValueName);
        }

        public static bool TryGetReturnValue(this IReadOnlyParameterSet set, [NotNullWhen(true)] out IParameter? returnValueParameter)
        {
            return TryGetRoot(set, ParameterAccessor.ReturnValueName, out returnValueParameter);
        }

        public static TParameter? GetReturnValue<TParameter>(this IReadOnlyParameterSet set)
            where TParameter : class, IParameter
        {
            return set.GetRoot<TParameter>(ParameterAccessor.ReturnValueName);
        }

        public static bool TryGetReturnValue<TParameter>(this IReadOnlyParameterSet set, [NotNullWhen(true)] out TParameter? returnValueParameter)
            where TParameter : class, IParameter
        {
            return TryGetRoot(set, ParameterAccessor.ReturnValueName, out returnValueParameter);
        }
    }
}
