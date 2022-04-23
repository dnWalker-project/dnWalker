using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public static class ExplicitActiveStateExtensions
    {
        private const string BaseHeapModelAttribute = "base-heap-model";
        private const string ExecHeapModelAttribute = "exec-heap-model";

        public static IHeapInfo GetBaseHeapModel(this ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.TryGetPathAttribute(BaseHeapModelAttribute, out IHeapInfo heapModel);
            return heapModel;
        }
        public static IHeapInfo GetExecHeapModel(this ExplicitActiveState cur)
        {
            cur.PathStore.CurrentPath.TryGetPathAttribute(ExecHeapModelAttribute, out IHeapInfo heapModel);
            return heapModel;
        }
        public static void InitHeapModel(this ExplicitActiveState cur, IModel model)
        {
            IHeapInfo baseHeap = model.HeapInfo.Clone();
            IHeapInfo execHeap = model.HeapInfo.Clone();
            cur.PathStore.CurrentPath.SetPathAttribute(BaseHeapModelAttribute, baseHeap);
            cur.PathStore.CurrentPath.SetPathAttribute(ExecHeapModelAttribute, execHeap);
        }

    }
}
