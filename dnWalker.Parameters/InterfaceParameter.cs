using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class InterfaceParameter : ReferenceTypeParameter
    {
        public InterfaceParameter(string typeName, string localName) : base(typeName, localName)
        {
        }

        public InterfaceParameter(string typeName, string localName, Parameter parent) : base(typeName, localName, parent)
        {
        }

        private readonly Dictionary<string, Dictionary<int, Parameter>> _methodResults = new Dictionary<string, Dictionary<int, Parameter>>();

        public override IEnumerable<Parameter> GetChildren()
        {
            return GetKnownMethodResults().Select(p => p.Value).Append(IsNullParameter);
        }

        public IEnumerable<KeyValuePair<(string, int), Parameter>> GetKnownMethodResults()
        {
            return IsNull ? Enumerable.Empty<KeyValuePair<(string, int), Parameter>>() :
                _methodResults.SelectMany(ps => ps.Value.Select(pi => KeyValuePair.Create((ps.Key, pi.Key), pi.Value)));
        }

        public bool TryGetMethodResult(string methodName, int callNumber, [NotNullWhen(true)]out Parameter? result)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            if (callNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(callNumber));
            }

            if (_methodResults.TryGetValue(methodName, out Dictionary<int, Parameter>? results))
            {
                return results.TryGetValue(callNumber, out result);
            }

            result = null;
            return false;
        }

        public void SetMethodResult(string methodName, int callNumber, Parameter? result)
        {
            if (String.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }
            
            if (callNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(callNumber));
            }

            if (result == null)
            {
                ClearMethodResult(methodName, callNumber);
                return;
            }

            if (!_methodResults.TryGetValue(methodName, out Dictionary<int, Parameter>? results))
            {
                results = new Dictionary<int, Parameter>();
                _methodResults[methodName] = results;
            }

            results[callNumber] = result;
            result.Parent = this;
        }

        public void ClearMethodResult(string methodName, int callNumber)
        {
            if (!_methodResults.TryGetValue(methodName, out Dictionary<int, Parameter>? results))
            {
                return;
            }

            if (results.TryGetValue(callNumber, out Parameter? result))
            {
                result.Parent = null;
                results.Remove(callNumber);
            }

        }
        public void ClearMethodResult(string methodName)
        {
            if (!_methodResults.TryGetValue(methodName, out Dictionary<int, Parameter>? results))
            {
                return;
            }

            foreach(Parameter result in results.Values)
            {
                result.Parent = null;
            }

            _methodResults.Remove(methodName);
        }

        public override bool TryGetChild(ParameterName parameterName, [NotNullWhen(true)] out Parameter? parameter)
        {
            if (base.TryGetChild(parameterName, out parameter))
            {
                return true;
            }

            if (parameterName.TryGetMethodResult(out string? methdoName, out int index))
            {
                return TryGetMethodResult(methdoName, index, out parameter);
            }
            return false;
        }
    }
}
