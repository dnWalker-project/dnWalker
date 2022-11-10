using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    internal class InMemoryConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, object> _values;

        public InMemoryConfigurationProvider()
        {
            _values = new Dictionary<string, object>();
        }
        public InMemoryConfigurationProvider(IEnumerable<KeyValuePair<string, object>> values)
        {
            _values = new Dictionary<string, object>(values);
        }

        public bool TryGetValue(string key, Type type, out object? value)
        {
            return _values.TryGetValue(key, out value);
        }

        public void SetValue(string key, object value)
        {
            if (value == null)
            {
                _values.Remove(key);
            }
            else
            {
                _values[key] = value;
            }
        }
    }
}
