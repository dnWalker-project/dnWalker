using dnlib.DotNet;

using dnWalker.Symbolic.Heap;

using MMC.State;
using System.Linq.Expressions;

namespace dnWalker.Symbolic
{
    public static class AllocationExtensions
    {
        private const string HeapNodeAttribute = "heap-node";

        public static void SetHeapNode(this Allocation allocation, IHeapNode heapNode, ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.SetAllocationAttribute(allocation, HeapNodeAttribute, heapNode);
        }

        public static bool TryGetHeapNode(this Allocation allocation, ExplicitActiveState cur, out IHeapNode heapNode)
        {
            return cur.PathStore.CurrentPath.TryGetAllocationAttribute(allocation, HeapNodeAttribute, out heapNode);
        }
    }
}
