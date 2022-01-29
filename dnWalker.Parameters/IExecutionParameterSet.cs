using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    /// <summary>
    /// Represents an execution set of parameters.
    /// </summary>
    public interface IExecutionParameterSet : IParameterSet
    {
        /// <summary>
        /// Gets the base parameter context.
        /// </summary>
        IBaseParameterSet? BaseSet { get; }
    }
}
