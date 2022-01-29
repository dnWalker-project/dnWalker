using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    /// <summary>
    /// Represents a base set of parameters. Provides execution context creation.
    /// </summary>
    public interface IBaseParameterSet : IParameterSet
    {
        /// <summary>
        /// Creates a new execution context from this base context.
        /// </summary>
        /// <returns></returns>
        IExecutionParameterSet CreateExecutionSet();
    }
}
