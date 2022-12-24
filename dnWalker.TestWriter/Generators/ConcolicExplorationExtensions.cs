using dnlib.DotNet;
using dnWalker.Explorations;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public static class ConcolicExplorationExtensions
    {
        private static IEnumerable<string> GetNamespaces(TypeSig ts)
        {
            // type may be an array => get namespaces of the "next"
            if (ts.IsArray || ts.IsSZArray)
            {
                return GetNamespaces(ts.Next);
            }

            // type may be a generic instance => get namespacec of all of the arguments
            if (ts.IsGenericInstanceType)
            {
                GenericInstSig genInstSig = ts.ToGenericInstSig();

                // create list of the namespaces & add the generic type namespace
                List<string> ns = new List<string>() { genInstSig.ToTypeDefOrRef().ResolveTypeDefThrow().GetNamespace() };

                foreach (TypeSig genParam in genInstSig.GetGenericParameters())
                {
                    ns.AddRange(GetNamespaces(genParam));
                }

                return ns;
            }

            // type is just "normal type"
            return new[] { ts.ToTypeDefOrRef().ResolveTypeDefThrow().GetNamespace() };
        }


        public static IEnumerable<string> GetNamespaces(this ConcolicExploration concolicExploration)
        {
            return concolicExploration.Iterations.SelectMany(it => GetNamespaces(it)).Concat(GetNamespaces(concolicExploration.MethodUnderTest.DeclaringType.ToTypeSig()));
        }

        public static IEnumerable<string> GetNamespaces(this ConcolicExplorationIteration it)
        {
            List<string> ns = new List<string>();

            foreach (IReadOnlyHeapNode n in it.InputModel.HeapInfo.Nodes)
            {
                TypeSig t = n is IReadOnlyArrayHeapNode ? n.Type.Next : n.Type;
                ns.AddRange(GetNamespaces(t));
            }

            foreach (IReadOnlyHeapNode n in it.OutputModel.HeapInfo.Nodes)
            {
                TypeSig t = n is IReadOnlyArrayHeapNode ? n.Type.Next : n.Type;
                ns.AddRange(GetNamespaces(t));
            }
            return ns;
        }
    }
}
