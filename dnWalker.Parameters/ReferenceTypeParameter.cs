using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter, IReferenceTypeParameter
    {
        protected ReferenceTypeParameter(IParameterSet context, TypeSignature type) : base(context, type)
        {
        }

        protected ReferenceTypeParameter(IParameterSet context, TypeSignature type, ParameterRef reference) : base(context, type, reference)
        {
        }

        public bool? IsNull
        {
            get;
            set;
        }
    }
}
