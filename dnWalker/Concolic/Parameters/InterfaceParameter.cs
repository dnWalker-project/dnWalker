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
    public class InterfaceParameter : NullableParameter
    {
        public TypeSig Type
        {
            get;
        }

        public InterfaceParameter(TypeSig type) : base(type.FullName)
        {
            Type = type;
        }

        public InterfaceParameter(TypeSig type, IEnumerable<ParameterTrait> traits) : base(type.FullName, traits)
        {
            Type = type;
        }

        public Parameter GetMethod(String methodName, Int32 callIndex)
        {
            if (TryGetTrait<MethodResultTrait>(t => t.MethodName == methodName && t.CallIndex == callIndex, out MethodResultTrait method))
            {
                return method.Value;
            }
            return null;
        }

        public void SetMethod(String methodName, Int32 callIndex, Parameter parameter)
        {
            if (TryGetTrait<MethodResultTrait>(t => t.MethodName == methodName && t.CallIndex == callIndex, out MethodResultTrait method))
            {
                method.Value = parameter;
            }
            else
            {
                method = new MethodResultTrait(methodName, callIndex, parameter);
            }
            parameter.Name = ParameterName.ConstructField(Name, methodName);
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

            TypeDef typeDef = Type.ToTypeDefOrRef().ResolveTypeDefThrow();

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
