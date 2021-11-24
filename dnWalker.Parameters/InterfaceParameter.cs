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

        public InterfaceParameter(string typeName, string localName, Parameter? owner) : base(typeName, localName, owner)
        {
        }

        private readonly Dictionary<string, Dictionary<int, Parameter>> _methodResults = new Dictionary<string, Dictionary<int, Parameter>>();

        public override IEnumerable<Parameter> GetOwnedParameters()
        {
            return _methodResults.Values.SelectMany(mr => mr.Values).Append(IsNullParameter);
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
            result.Owner = this;
        }

        public void ClearMethodResult(string methodName, int callNumber)
        {
            if (!_methodResults.TryGetValue(methodName, out Dictionary<int, Parameter>? results))
            {
                return;
            }

            if (results.TryGetValue(callNumber, out Parameter? result))
            {
                result.Owner = null;
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
                result.Owner = null;
            }

            _methodResults.Remove(methodName);
        }
    }
}
