using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        private readonly Stack<IConfigurationProvider> _providers = new Stack<IConfigurationProvider>();
        private readonly InMemoryConfigurationProvider _inMemoryProvider;

        public ConfigurationBuilder()
        {
            _inMemoryProvider = new InMemoryConfigurationProvider();
            _providers.Push(_inMemoryProvider);
        }

        public ConfigurationBuilder(IEnumerable<KeyValuePair<string, object>> values)
        {
            _inMemoryProvider = new InMemoryConfigurationProvider(values);
            _providers.Push(_inMemoryProvider);
        }
        

        public IConfiguration Build()
        {
            return new Configuration(_providers);
        }

        public void SetValue(string key, object? value)
        {
            _inMemoryProvider.SetValue(key, value);
        }

        public void AddProvider(IConfigurationProvider provider)
        {
            _providers.Push(provider);
        }

        public bool TryGetValue(string key, Type type, [NotNullWhen(true)]out object? value)
        {
            foreach (IConfigurationProvider p in _providers)
            {
                if (p.TryGetValue(key, type, out value))
                {
                    return true;
                }
            }

            value = null;
            return false;
        }

        public bool HasValue(string key)
        {
            return _providers.Any(p => p.HasValue(key));
        }
    }
}
