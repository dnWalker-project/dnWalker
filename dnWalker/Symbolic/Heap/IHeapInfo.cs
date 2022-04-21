using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    /// <summary>
    /// Provides valuation of the heap.
    /// </summary>
    public interface IHeapInfo : IReadOnlyHeapInfo
    {
        new IHeapInfo Clone();

        IReadOnlyHeapInfo IReadOnlyHeapInfo.Clone()
        {
            return Clone();
        }

        new IHeapNode GetNode(Location location);

        IReadOnlyHeapNode IReadOnlyHeapInfo.GetNode(Location location)
        {
            return GetNode(location);
        }

        IObjectHeapNode InitializeObject(TypeSig type);
        IArrayHeapNode InitializeArray(TypeSig elementType, int length);
        // IStringHeapNode InitializeString(TypeSig stringType, string content);
    }
}
