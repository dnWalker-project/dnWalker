using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IAliasParameter : IReferenceTypeParameter
    {
        ParameterRef ReferencedParameter { get; }

        bool? IReferenceTypeParameter.IsNull
        {
            get 
            {
                return ReferencedParameter.Resolve<IReferenceTypeParameter>(Context)!.IsNull;
            }
            set
            {
                ReferencedParameter.Resolve<IReferenceTypeParameter>(Context)!.IsNull = value;
            }
        }
    }
}
