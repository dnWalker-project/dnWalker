using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public abstract class CacheBase<TKey, TValue> : ICache<TKey, TValue>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();

        public TValue Get(TKey key)
        {
            if (!_items.TryGetValue(key, out TValue value))
            {
                value = CreateValue(key);
                _items.Add(key, value);
            }
            return value;
        }

        protected abstract TValue CreateValue(TKey key);

        public virtual void Clear()
        {
            _items.Clear();
        }
    }

    public class DelegateCache<TKey, TValue> : ICache<TKey, TValue>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _items = new Dictionary<TKey, TValue>();
        private readonly Func<TKey, TValue> _factory;

        public DelegateCache(Func<TKey, TValue> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public virtual TValue Get(TKey key)
        {
            if (!_items.TryGetValue(key, out TValue value))
            {
                value = _factory(key);
                _items.Add(key, value);
            }
            return value;
        }

        public virtual void Clear()
        {
            _items.Clear();
        }
    }
}
