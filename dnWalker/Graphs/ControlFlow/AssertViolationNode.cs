using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public class AssertViolationNode : ControlFlowNode
    {
        public override string ToString()
        {
            return $"AssertViolation";
        }
    }
}
