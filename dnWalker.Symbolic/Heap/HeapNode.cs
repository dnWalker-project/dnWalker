using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Heap
{
    public abstract class HeapNode : IHeapNode
    {
        protected static IValue GetValueOrDefault<TKey>(IDictionary<TKey, IValue> src, TKey key, TypeSig type)
        {
            if (!src.TryGetValue(key, out IValue? value))
            {
                value = ValueFactory.GetDefault(type);
                src[key] = value;
            }
            return value;
        }

        protected HeapNode(Location location, TypeSig type)
        {
            Location = location;
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        IHeapNode IHeapNode.Clone() => Clone();
        IReadOnlyHeapNode IReadOnlyHeapNode.Clone() => Clone();

        public abstract HeapNode Clone();

        public Location Location { get; }

        public TypeSig Type { get; }

    }
}
