using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public abstract class ControlFlowNode
    {
        private bool _isCovered;

        public void MarkCovered()
        {
            _isCovered = true;
        }

        public bool IsCovered => _isCovered;
    }
}
