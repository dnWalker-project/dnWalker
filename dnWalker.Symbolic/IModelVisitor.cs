using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public interface IModelVisitor
    {
        void Visit(IModelVisitable visitable);

        void VisitModel(IReadOnlyModel model);

        void VisitVariable(IVariable variable);


        void VisitHeapNode(IReadOnlyHeapNode heapNode);
    }
}
