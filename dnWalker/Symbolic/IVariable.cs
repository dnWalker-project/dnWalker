using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    /// <summary>
    /// Represents a symbolic variable.
    /// </summary>
    public interface IVariable : IEquatable<IVariable>
    {
        VariableType VariableType { get; }
    }
}
