using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public class ConfigurationBuilder
    {
        private readonly List<IConfigurationProvider> _providers = new List<IConfigurationProvider>();



        public IConfiguration Build()
        {
            IConfigurationProvider provider;
            if (_providers.Count == 0)
            {
                provider = new InMemoryConfigurationProvider();
            }
            else if (_providers.Count == 1)
            {
                provider = _providers[0];
            }
            else
            {
                provider = new MergedConfigurationProvider(_providers);
            }
            return new CachedConfiguration(provider);
        }

        public void AddProvider(IConfigurationProvider provider)
        {
            _providers.Add(provider);
        }

        public void AddJsonFile(string fileName)
        {
            AddProvider(new JsonConfigurationProvider(File.ReadAllText(fileName)));
        }

        public void AddValues(IEnumerable<KeyValuePair<string, object>> values)
        {
            AddProvider(new InMemoryConfigurationProvider(values));
        }
    }
}
