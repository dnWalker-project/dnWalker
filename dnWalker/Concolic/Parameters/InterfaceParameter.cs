using dnlib.DotNet;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public class InterfaceParameter : ReferenceTypeParameter
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

        protected override void OnNameChanged(String newName)
        {
            base.OnNameChanged(newName);

            foreach (KeyValuePair<String, Dictionary<Int32, Parameter>> results in _methodResults)
            {
                foreach (KeyValuePair<Int32, Parameter> result in results.Value)
                {
                    result.Value.Name = ParameterName.ConstructMethod(newName, results.Key, result.Key);
                }
            }
        }

        public override IDataElement CreateDataElement(ExplicitActiveState cur)
        {
            DynamicArea dynamicArea = cur.DynamicArea;

            if (!IsNull.HasValue || IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(this, cur);
                return nullReference;
            }

            TypeDef typeDef = cur.DefinitionProvider.GetTypeDefinition(TypeName);

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference interfaceReference = dynamicArea.AllocateObject(location, typeDef);
            AllocatedObject allocatedInterface = (AllocatedObject)dynamicArea.Allocations[interfaceReference];
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
    }
}
