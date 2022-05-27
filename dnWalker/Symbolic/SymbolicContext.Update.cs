using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Utils;

using MMC.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public partial class SymbolicContext
    {
        public void Update(IVariable instance, IField field, IValue value)
        {

            if (!_outputModel.TryGetValue(instance, out IValue instanceValue))
            {
                Debug.Fail("The instance variable should have resolved...");
            }

            if (!_outputModel.HeapInfo.TryGetNode((Location)instanceValue, out IHeapNode node))
            {
                Debug.Fail("The instance location should have resolved...");
            }

            IObjectHeapNode objectNode = (IObjectHeapNode)node;
            objectNode.SetField(field, value);
        }
    }
}
