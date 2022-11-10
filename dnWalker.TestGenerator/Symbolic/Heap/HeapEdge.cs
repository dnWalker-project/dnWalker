using dnWalker.Symbolic;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Symbolic.Heap
{
    public enum HeapEdgeType
    {
        ArrayElement,
        Field,
        MethodResult
    }

    public class HeapEdge : Edge<Location>
    {
        private readonly HeapEdgeType _type;

        public HeapEdge([NotNull]Location source, [NotNull] Location target, HeapEdgeType type) : base(source, target)
        {
            _type = type;
        }

        public HeapEdgeType Type
        {
            get
            {
                return _type;
            }
        }
    }
}
