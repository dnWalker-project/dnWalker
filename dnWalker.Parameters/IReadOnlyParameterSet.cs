using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IReadOnlyParameterSet
    {
        IParameterContext Context { get; }

        IReadOnlyDictionary<ParameterRef, IParameter> Parameters { get; }
        IReadOnlyDictionary<string, ParameterRef> Roots { get; }
    }
}
