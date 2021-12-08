using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection
{
    public class ReflectionInterfaceParameter : ReflectionReferenceTypeParameter, IInterfaceParameter
    {
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
            int length = list.FindLastIndex(p => p!= null);
            if (length == -1) length = list.Count;

            IParameter?[] array = new IParameter?[length];
            list.CopyTo(0, array, 0, length);

            return array;
        }

        public ReflectionInterfaceParameter(Type type) : base(type)
        {
            if (!type.IsInterface) throw new ArgumentException("Expected a type which is an interface.");
        }

        public ReflectionInterfaceParameter(Type type, int id) : base(type, id)
        {
            if (!type.IsInterface) throw new ArgumentException("Expected a type which is an interface.");
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
            if (parameter == null)
            {
                ClearMethodResult(methodSignature, invocation);
            }

            if (!_methodResults.TryGetValue(methodSignature, out List<IParameter?>? results))
            {
                results = new List<IParameter?>();
            }

            Resize(results, invocation + 1);
            results[invocation] = parameter;
        }

        public void ClearMethodResult(MethodSignature methodSignature, int invocation)
        {
            throw new NotImplementedException();
        }

        public void ClearMetodResult(MethodSignature methodSignature)
        {
            throw new NotImplementedException();
        }
    }
}
