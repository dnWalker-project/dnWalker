using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IMethodResolverParameter : IParameter
    {
        IEnumerable<KeyValuePair<MethodSignature, IParameter?[]>> GetMethodResults();

        bool TryGetMethodResult(MethodSignature methodSignature, int invocation, [NotNullWhen(true)] out IParameter? parameter);
        void SetMethodResult(MethodSignature methodSignature, int invocation, IParameter? parameter);
        void ClearMethodResult(MethodSignature methodSignature, int invocation);
        void ClearMetodResult(MethodSignature methodSignature);
    }
}
