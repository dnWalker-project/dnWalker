using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public class InterfaceParameter : ReferenceTypeParameter
    {
        public InterfaceParameter(string typeName) : base(typeName)
        {
        }

        public InterfaceParameter(string typeName, string name) : base(typeName, name)
        {
        }

        private readonly Dictionary<string, Dictionary<int, Parameter>> _methodResults = new Dictionary<string, Dictionary<int, Parameter>>();

        public bool TryGetMethodResult(string methodName, int callIndex, out Parameter result)
        {
            if (_methodResults.TryGetValue(methodName, out var results))
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

        public void SetMethodResult(string methodName, int callIndex, Parameter parameter)
        {
            if (!_methodResults.TryGetValue(methodName, out var results))
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

        protected override void OnNameChanged(string newName)
        {
            base.OnNameChanged(newName);

            foreach (var results in _methodResults)
            {
                foreach (var result in results.Value)
                {
                    result.Value.Name = ParameterName.ConstructMethod(newName, results.Key, result.Key);
                }
            }
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            var dynamicArea = cur.DynamicArea;

            if (!IsNull.HasValue || IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                var nullReference = new ObjectReference(0);
                nullReference.SetParameter(this, cur);
                return nullReference;
            }

            var typeDef = cur.DefinitionProvider.GetTypeDefinition(TypeName);

            var location = dynamicArea.DeterminePlacement(false);
            var interfaceReference = dynamicArea.AllocateObject(location, typeDef);
            var allocatedInterface = (AllocatedObject)dynamicArea.Allocations[interfaceReference];
            allocatedInterface.ClearFields(cur);

            // TODO: somehow create structure for resolving the method and using the callindex

            //MethodResolver resolver = new MethodResolver();
            //foreach(MethodResultTrait methodResult in Traits.OfType<MethodResultTrait>())
            //{
            //    resolver[methodResult.MethodName] = new MethodResultProvider(s => methodResult.Value.AsDataElement(s));
            //}

            interfaceReference.SetParameter(this, cur);
            return interfaceReference;
        }

        public override IEnumerable<ParameterExpression> GetParameterExpressions()
        {
            return base.GetParameterExpressions()
                .Concat(_methodResults.Values.SelectMany(call2Result => call2Result.Values.SelectMany(result => result.GetParameterExpressions())));
        }

        public override bool TryGetChildParameter(string name, out Parameter childParameter)
        {
            if (base.TryGetChildParameter(name, out childParameter)) return true;

            var accessor = ParameterName.GetAccessor(Name, name);
            if (ParameterName.TryParseMethodName(accessor, out var methodName, out var callIndex) && TryGetMethodResult(methodName, callIndex, out childParameter))
            {
                return true;
            }
            else
            {
                childParameter = null;
                return false;
            }
        }
    }
}
