using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class InterfaceParameter : MethodResolverParameter, IInterfaceParameter
    {
        public InterfaceParameter(string typeName) : base(typeName)
        {
        }

        public InterfaceParameter(string typeName, int id) : base(typeName, id)
        {
        }



        public override IEnumerable<IParameter> GetChildren()
        {
            return GetMethodResults().SelectMany(mr => mr.Value).Where(mr => mr != null).Select(mr => mr!);
        }

        public override IParameter ShallowCopy(ParameterStore store, int id)
        {
            InterfaceParameter interfaceParameter = new InterfaceParameter(TypeName, id);
            interfaceParameter.IsNull = IsNull;

            foreach (KeyValuePair<MethodSignature, IParameter?[]> methodResultInfo in GetMethodResults())
            {
                IParameter?[] results = methodResultInfo.Value;
                for (int i = 0; i < results.Length; ++i)
                {
                    if (results[i] is IReferenceTypeParameter refType)
                    {
                        interfaceParameter.SetMethodResult(methodResultInfo.Key, i, refType.CreateAlias(store));
                    }
                    else if (results[i] is IPrimitiveValueParameter valueType)
                    {
                        interfaceParameter.SetMethodResult(methodResultInfo.Key, i, valueType.ShallowCopy(store));
                    }
                }
            }

            return interfaceParameter;
        }
    }
}
