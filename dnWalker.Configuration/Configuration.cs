using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Configuration;

public class Configuration : IConfiguration
{
    private readonly IConfigurationProvider[] _providers;
    private readonly Dictionary<string, object> _cache = new Dictionary<string, object>();

    public Configuration(IEnumerable<IConfigurationProvider> providers)
    {
        _providers = providers.ToArray();
    }


    public bool TryGetValue(string key, Type type, [NotNullWhen(true)] out object? value)
    {
        if (_cache.TryGetValue(key, out value)) return true;
        
        foreach (IConfigurationProvider provider in _providers)
        {
            if (provider.TryGetValue(key, type, out value))
            {
                _cache[key] = value;
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