using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Symbolic
{
    public static class ModelExtensions
    {
        public static IEnumerable<string> GetNamespaces(this IReadOnlyModel model)
        {
            HashSet<string> namespaces = new HashSet<string>();
            // get namespaces of each variable
            foreach (IRootVariable rv in model.Variables)
            { 
                namespaces.Add(rv.Type.Namespace);
            }

            // get namespaces of each heap node
            foreach (IReadOnlyHeapNode node in model.HeapInfo.Nodes)
            {
                TypeSig type = node.Type;
                type = type.GetNext() ?? type;
                namespaces.Add(type.Namespace);
            }

            return namespaces;
        }
    }
}
