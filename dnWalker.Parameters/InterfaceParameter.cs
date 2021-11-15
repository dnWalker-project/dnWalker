using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class InterfaceParameter : ReferenceTypeParameter, IEquatable<InterfaceParameter>
    {
        public InterfaceParameter(String typeName) : base(typeName)
        {
        }

        public InterfaceParameter(String typeName, String name) : base(typeName, name)
        {
        }

        private readonly Dictionary<String, Dictionary<Int32, Parameter>> _methodResults = new Dictionary<String, Dictionary<Int32, Parameter>>();

        public Boolean TryGetMethodResult(String methodName, Int32 callIndex, out Parameter result)
        {
            if (_methodResults.TryGetValue(methodName, out Dictionary<Int32, Parameter> results))
            {
                return results.TryGetValue(callIndex, out result);
            }
            result = null;
            return false;

            //if (TryGetTrait<MethodResultTrait>(t => t.MethodName == methodName && t.CallIndex == callIndex, out MethodResultTrait method))
            //{
            //    return method.Value;
            //}
            //return null;
        }

        public void SetMethodResult(String methodName, Int32 callIndex, Parameter parameter)
        {
            if (!_methodResults.TryGetValue(methodName, out Dictionary<Int32, Parameter> results))
            {
                results = new Dictionary<int, Parameter>();
                _methodResults[methodName] = results;
            }

            results[callIndex] = parameter;

            if (HasName()) parameter.Name = ParameterName.ConstructMethod(Name, methodName, callIndex);

            //if (TryGetTrait<MethodResultTrait>(t => t.MethodName == methodName && t.CallIndex == callIndex, out MethodResultTrait method))
            //{
            //    method.Value = parameter;
            //}
            //else
            //{
            //    method = new MethodResultTrait(methodName, callIndex, parameter);
            //}
            //parameter.Name = ParameterName.ConstructField(Name, methodName);
        }

        public IEnumerable<KeyValuePair<String, Dictionary<Int32, Parameter>>> GetKnownMethodResults()
        {
            return _methodResults;
        }

        protected override void OnNameChanged(String newName)
        {
            base.OnNameChanged(newName);

            if (_methodResults != null)
            {
                foreach (KeyValuePair<String, Dictionary<Int32, Parameter>> results in _methodResults)
                {
                    foreach (KeyValuePair<Int32, Parameter> result in results.Value)
                    {
                        result.Value.Name = ParameterName.ConstructMethod(newName, results.Key, result.Key);
                    }
                }
            }
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return base.GetParameterExpressions()
                .Concat(_methodResults.Values.SelectMany(call2Result => call2Result.Values.SelectMany(result => result.GetParameterExpressions())));
        }

        public override Boolean TryGetChildParameter(String name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;

            String accessor = ParameterName.GetAccessor(Name, name);
            if (ParameterName.TryParseMethodName(accessor, out String methodName, out Int32 callIndex) && TryGetMethodResult(methodName, callIndex, out childParameter))
            {
                return true;
            }
            else
            {
                childParameter = null;
                return false;
            }
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as InterfaceParameter);
        }

        public bool Equals(InterfaceParameter other)
        {
            bool isNull = IsNull.HasValue ? IsNull.Value : true;

            return other != null &&
                   Name == other.Name &&
                   IsNull == other.IsNull &&
                   TypeName == other.TypeName &&
                   (isNull || _methodResults.Count == other._methodResults.Count) &&
                   (isNull || _methodResults.All(p => other._methodResults.TryGetValue(p.Key, out Dictionary<int, Parameter> otherResults) && p.Value.Count == otherResults.Count && p.Value.All(r => otherResults.TryGetValue(r.Key, out Parameter otherResult) && Equals(otherResult, r.Value))));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, TypeName, IsNull, _methodResults);
        }

        public static bool operator ==(InterfaceParameter left, InterfaceParameter right)
        {
            return EqualityComparer<InterfaceParameter>.Default.Equals(left, right);
        }

        public static bool operator !=(InterfaceParameter left, InterfaceParameter right)
        {
            return !(left == right);
        }
    }
}
