using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    internal class MergedConfigurationProvider : IConfigurationProvider
    {
        private readonly List<IConfigurationProvider> _providers;

        public MergedConfigurationProvider()
        {
            _providers = new List<IConfigurationProvider>();
        }

        public MergedConfigurationProvider(params IConfigurationProvider[] providers)
        {
            _providers = new List<IConfigurationProvider>(providers);
        }

        public MergedConfigurationProvider(IEnumerable<IConfigurationProvider> providers)
        {
            _providers = new List<IConfigurationProvider>(providers);
        }

        public void Add(IConfigurationProvider provider)
        {
            _providers.Add(provider);
        }

        public bool Remove(IConfigurationProvider provider)
        {
            return _providers.Remove(provider);
        }


        public bool TryGetValue(string key, Type type, out object value)
        {
            for (int i = 0; i < _providers.Count; ++i)
            {
                if (_providers[i].TryGetValue(key, type, out value)) return true;
            }

            value = null;
            return false;
        }
    }
}
