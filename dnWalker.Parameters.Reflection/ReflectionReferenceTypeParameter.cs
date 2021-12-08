using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Reflection
{
    public class ReflectionReferenceTypeParameter : ReflectionParameter, IReferenceTypeParameter
    {
        public ReflectionReferenceTypeParameter(Type type) : base(type)
        {
        }

        public ReflectionReferenceTypeParameter(Type type, int id) : base(type, id)
        {
        }

        public bool IsNull
        {
            get;
            set;
        }
    }
}
