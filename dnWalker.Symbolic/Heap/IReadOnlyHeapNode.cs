using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Represents a read only heap node.
    /// </summary>
    public interface IReadOnlyHeapNode
    {
        /// <summary>
        /// Specifies the location of the node.
        /// </summary>
        Location Location { get; }

        /// <summary>
        /// Gets the type associated with this heap node.
        /// </summary>
        TypeSig Type { get; }

        /// <summary>
        /// Clones the heap node.
        /// </summary>
        /// <returns></returns>
        IReadOnlyHeapNode Clone();
    }
}
