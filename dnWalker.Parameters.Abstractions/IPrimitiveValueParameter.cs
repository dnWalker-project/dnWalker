using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IPrimitiveValueParameter
    {
        object? GetValue();
    }

    public interface IPrimitiveValueParameter<T> : IPrimitiveValueParameter
        where T : struct
    {
        T Value { get; set; }
    }
}
