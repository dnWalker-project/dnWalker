using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    /// <summary>
    /// Provides valuation for the variables.
    /// </summary>
    public interface IReadOnlyModel
    {
        IReadOnlyModel Clone();

        IValue GetValue(IVariable variable);

        IReadOnlyHeapInfo HeapInfo { get; }

        IReadOnlyList<IVariable> Variables { get; }
    }
}
