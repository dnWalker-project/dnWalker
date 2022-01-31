using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    internal class MethodResolverImplementation : IMethodResolver
    {
        private readonly ParameterRef _ownerRef;
        private readonly IParameterSet _context;

        private readonly Dictionary<MethodSignature, ParameterRef[]> _results = new Dictionary<MethodSignature, ParameterRef[]>();

        public MethodResolverImplementation(ParameterRef ownerRef, IParameterSet context)
        {
            _ownerRef = ownerRef;
            _context = context;
        }

        public IReadOnlyDictionary<MethodSignature, ParameterRef[]> GetMethodResults()
        {
            return _results;
        }

        private bool EnsureSize(ref ParameterRef[] results, int minimalLength)
        {
            if (results.Length >= minimalLength) return false;

            ParameterRef[] newResults = new ParameterRef[minimalLength];
            results.CopyTo(newResults, 0);
            results = newResults;
            return true;
        }

        public bool TryGetMethodResult(MethodSignature methodSignature, int invocation, out ParameterRef resultRef)
        {
            if (_results.TryGetValue(methodSignature, out ParameterRef[]? resultRefs) &&
                (invocation >= 0 && invocation < resultRefs.Length))
            {
                resultRef = resultRefs[invocation];
                return resultRef != ParameterRef.Empty;
            }

            resultRef = ParameterRef.Empty;
            return false;
        }

        public void SetMethodResult(MethodSignature methodSignature, int invocation, ParameterRef resultRef)
        {
            if (invocation < 0) return;


            if (!_results.TryGetValue(methodSignature, out ParameterRef[]? resultRefs))
            {
                resultRefs = new ParameterRef[invocation + 1];
                _results[methodSignature] = resultRefs;
            }
            else
            {
                if (EnsureSize(ref resultRefs, invocation + 1))
                {
                    _results[methodSignature] = resultRefs;
                }
            }

            ClearMethodResult(methodSignature, invocation);

            resultRefs[invocation] = resultRef;

            if (resultRef.TryResolve(_context, out IParameter? p))
            {
                //p.Accessor = new MethodResultParameterAccessor(methodSignature, invocation, _ownerRef);
                p.Accessors.Add(new MethodResultParameterAccessor(methodSignature, invocation, _ownerRef));
            }
            else
            {
                throw new Exception("Trying to set method result with an unknown parameter!");
            }
        }

        public void ClearMethodResult(MethodSignature methodSignature, int invocation)
        {
            if (invocation >= 0 &&
                _results.TryGetValue(methodSignature, out ParameterRef[]? resultRefs) &&
                resultRefs.Length > invocation &&
                resultRefs[invocation] != ParameterRef.Empty)
            {
                if (resultRefs[invocation].TryResolve(_context, out IParameter? resultParameter))
                {
                    //resultParameter.Accessor = null;
                    resultParameter.Accessors.RemoveAt(resultParameter.Accessors.IndexOf(pa => pa is MethodResultParameterAccessor mr && mr.ParentRef == _ownerRef && mr.Invocation == invocation && mr.MethodSignature == methodSignature));
                }

                resultRefs[invocation] = ParameterRef.Empty;
            }
        }

        public void CopyTo(MethodResolverImplementation other)
        {
            foreach (var p in _results)
            {
                other._results.Add(p.Key, (ParameterRef[])p.Value.Clone());
            }
        }
    }
}
