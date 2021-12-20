using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IReferenceTypeParameter : IParameter
    {

        bool? IsNull { get; set; }
    }

    public static class ReferenceTypeParameterExtensions
    {
        public static bool GetIsNull(this IReferenceTypeParameter referenceTypeParameter)
        {
            return referenceTypeParameter.IsNull ?? true;
        }
    }
}
