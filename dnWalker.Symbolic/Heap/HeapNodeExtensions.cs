using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public static class HeapNodeExtensions
    {
        public static IValue GetField(this IReadOnlyObjectHeapNode node, string fieldName)
        {
            IField field = node.Fields.First(fld => fld.Name == fieldName);
            return node.GetFieldOrDefault(field);
        }

        public static IValue GetMethodResult(this IReadOnlyObjectHeapNode node, string methodName, int invocation)
        {
            IMethod method = node.MethodInvocations.First(((IMethod m, int inv) t) => t.inv == invocation && t.m.Name == methodName).Item1;

            return node.GetMethodResult(method, invocation);
        }
    }
}
