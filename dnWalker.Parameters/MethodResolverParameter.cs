using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class MethodResolverParameter : ReferenceTypeParameter, IMethodResolverParameter
    {
        protected MethodResolverParameter(string typeName) : base(typeName)
        {
        }

        protected MethodResolverParameter(string typeName, int id) : base(typeName, id)
        {
        }



        private readonly Dictionary<MethodSignature, List<IParameter?>> _methodResults = new Dictionary<MethodSignature, List<IParameter?>>();

        private static void Resize(List<IParameter?> list, int minLength)
        {
            while (list.Count < minLength)
            {
                list.Add(null);
            }
        }

        private static IParameter?[] CreateArrayWithoutTrailingNulls(List<IParameter?> list)
        {
            int length = list.FindLastIndex(p => p != null) + 1;
            if (length == 0)
            {
                return Array.Empty<IParameter?>();
            }

            IParameter?[] array = new IParameter?[length];
            list.CopyTo(0, array, 0, length);

            return array;
        }

        public IEnumerable<KeyValuePair<MethodSignature, IParameter?[]>> GetMethodResults()
        {
            return _methodResults.Select(p => KeyValuePair.Create(p.Key, CreateArrayWithoutTrailingNulls(p.Value)));
        }

        public bool TryGetMethodResult(MethodSignature methodSignature, int invocation, [NotNullWhen(true)] out IParameter? parameter)
        {
            if (_methodResults.TryGetValue(methodSignature, out List<IParameter?>? results))
            {
                if (results.Count > invocation)
                {
                    parameter = results[invocation];
                    return parameter != null;
                }
            }

            parameter = null;
            return false;
        }

        public void SetMethodResult(MethodSignature methodSignature, int invocation, IParameter? parameter)
        {
            ClearMethodResult(methodSignature, invocation);

            if (parameter != null)
            {
                if (!_methodResults.TryGetValue(methodSignature, out List<IParameter?>? results))
                {
                    results = new List<IParameter?>();
                    _methodResults[methodSignature] = results;
                }

                Resize(results, invocation + 1);
                results[invocation] = parameter;
                parameter.Accessor = new MethodResultParameterAccessor(methodSignature, invocation, this);
            }
        }

        public void ClearMethodResult(MethodSignature methodSignature, int invocation)
        {
            if (_methodResults.TryGetValue(methodSignature, out List<IParameter?>? results))
            {
                if (results.Count > invocation)
                {
                    IParameter? cleared = results[invocation];
                    if (cleared != null)
                    {
                        results[invocation] = null;
                        cleared.Accessor = null;
                    }
                }
            }
        }
    }
}
