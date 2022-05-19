using dnlib.DotNet;

using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        Constraint Precondition { get; }

        IReadOnlyModel Clone();

        //IValue GetValue(IVariable variable);
        bool TryGetValue(IRootVariable variable, [NotNullWhen(true)] out IValue? value);

        IReadOnlyHeapInfo HeapInfo { get; }

        IReadOnlyCollection<IRootVariable> Variables { get; }
    }
}
