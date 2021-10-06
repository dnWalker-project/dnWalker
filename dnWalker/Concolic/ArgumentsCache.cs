using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ArgumentsCache
    {
        private class ArgumentsCacheLayer
        {
            private readonly Dictionary<IArg, ArgumentsCacheLayer> _items = new Dictionary<IArg, ArgumentsCacheLayer>();
            private readonly int _depth;

            public ArgumentsCacheLayer(Int32 depth)
            {
                _depth = depth;
            }

            public bool TryAdd(IArg[] args, int index)
            {
                IArg currentArg = args[index];
                if (args.Length - 1 == index && _items.ContainsKey(currentArg))
                {
                    // we are trying to add the last one and it is already in the layer => return false
                    return false;
                }

                if (!_items.TryGetValue(currentArg, out ArgumentsCacheLayer nextLayer))
                {
                    nextLayer = new ArgumentsCacheLayer(_depth + 1);
                    _items[currentArg] = nextLayer;
                }

                return index == args.Length - 1 || nextLayer.TryAdd(args, index + 1);
            }

            public bool Remove(IArg[] args, int index)
            {
                if (args.Length - 1 == index)
                {
                    // last item
                    return _items.Remove(args[index]);
                }

                if (_items.TryGetValue(args[index], out ArgumentsCacheLayer nextLayer))
                {
                    return nextLayer.Remove(args, index + 1);
                }

                return false;
            }

            public bool Contains(IArg[] args, int index)
            {
                if (args.Length - 1 == index)
                {
                    // last item
                    return _items.ContainsKey(args[index]);
                }

                if (_items.TryGetValue(args[index], out ArgumentsCacheLayer nextLayer))
                {
                    return nextLayer.Contains(args, index + 1);
                }
                return false;
            }

            public void Clear()
            {
                _items.Clear();
            }
        }

        private readonly int _depth;
        private readonly ArgumentsCacheLayer _baseLayer = new ArgumentsCacheLayer(0);

        public Int32 Depth
        {
            get
            {
                return _depth;
            }
        }

        public ArgumentsCache(Int32 depth)
        {
            _depth = depth;
        }

        public void Clear()
        {
            _baseLayer.Clear();
        }

        public bool TryAdd(IArg[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length != _depth)
            {
                throw new ArgumentException("Args array has wrong length", nameof(args));
            }

            return _baseLayer.TryAdd(args, 0);
        }

        public bool Remove(IArg[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length != _depth)
            {
                throw new ArgumentException("Args array has wrong length", nameof(args));
            }

            return _baseLayer.Remove(args, 0);
        }

        public bool Contains(IArg[] args)
        {
            if (args == null || args.Length != _depth)
            {
                return false;
            }

            return _baseLayer.Contains(args, 0);
        }
    }
}
