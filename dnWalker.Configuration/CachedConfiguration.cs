using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    internal class CachedConfiguration : IConfiguration
    {
        private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();
        private readonly IConfigurationProvider _valueProvider;

        public CachedConfiguration(IConfigurationProvider valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public CachedConfiguration()
        {
            _valueProvider = new InMemoryConfigurationProvider();
        }

        public TValue GetValue<TValue>(string key)
        {
            if (!_cache.TryGetValue(key, out object value))
            {
                if (!_valueProvider.TryGetValue(key, typeof(TValue), out value))
                {
                    value = default(TValue);
                }
            }
            return (TValue)value;
        }

        public void SetValue<TValue>(string key, TValue value)
        {
            _cache[key] = value;
        }
    }
}
