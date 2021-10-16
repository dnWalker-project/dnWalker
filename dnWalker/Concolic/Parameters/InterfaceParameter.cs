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
    //using MethodResultProvider = Func<ExplicitActiveState, IDataElement>;
    //using MethodResolver =  Dictionary<String, Func<ExplicitActiveState, IDataElement>>;

    public class InterfaceParameter : NullableParameter
    {
        public ITypeDefOrRef Type
        {
            get;
        }

        public InterfaceParameter(ITypeDefOrRef type) : base(type.FullName)
        {
            Type = type;
        }

        public InterfaceParameter(ITypeDefOrRef type, IEnumerable<ParameterTrait> traits) : base(type.FullName, traits)
        {
            Type = type;
        }

        public override IDataElement AsDataElement(ExplicitActiveState cur)
        {
            DynamicArea dynamicArea = cur.DynamicArea;

            if (!IsNull.HasValue || IsNull.Value)
            {
                // dont care or explicit null => return NullReference
                ObjectReference nullReference = new ObjectReference(0);
                nullReference.SetParameter(this, cur);
                return nullReference;
            }

            Int32 location = dynamicArea.DeterminePlacement(false);
            ObjectReference interfaceReference = dynamicArea.AllocateObject(location, Type);
            AllocatedObject allocatedInterface = (AllocatedObject)dynamicArea.Allocations[interfaceReference];
            allocatedInterface.ClearFields(cur);


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
