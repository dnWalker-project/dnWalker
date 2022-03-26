using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters.Serialization
{
    /// <summary>
    /// Represents information about a <see cref="IParameterSet"/>.
    /// Can be used to construct <see cref="IReadOnlyParameterSet"/> when provided a <see cref="IParameterContext"/>.
    /// </summary>
    public interface IParameterSetInfo
    {
        IReadOnlyParameterSet Construct(IParameterContext context);
    }
}
