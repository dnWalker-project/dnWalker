using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class ReferenceTypeParameter : Parameter, IReferenceTypeParameter
    {
        protected ReferenceTypeParameter(IParameterContext context) : base(context)
        {
        }

        protected ReferenceTypeParameter(IParameterContext context, ParameterRef reference) : base(context, reference)
        {
        }

        public bool? IsNull
        {
            get;
            set;
        }
    }
}
