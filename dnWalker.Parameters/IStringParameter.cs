using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IStringParameter : IReferenceTypeParameter
    {
        public string? Value { get; set; }
    }

}
