using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Configuration;

public class TextConfigurationProvider : IConfigurationProvider
{
    private readonly Dictionary<string, string> _values;

    public TextConfigurationProvider()
    {
        _values = new Dictionary<string, string>();
    }

    public TextConfigurationProvider(IEnumerable<KeyValuePair<string, string>> values)
    {
        _values = new Dictionary<string, string>(values);
    }

    public bool TryGetValue(string key, Type type, [NotNullWhen(true)] out object? value)
    {
        if (type.IsAssignableTo(typeof(IConvertible)))
        {
            if (_values.TryGetValue(key, out string? valueString))
            {
                try
                {
                    value = Convert.ChangeType(valueString, type);
                    return true;
                }
                catch (Exception e)
                {
                    value = null;
                    return false;
                }
            }
        }

        value = null;
        return false;
    }

    public void SetValue<T>(string key, T? value)
        where T : IConvertible
    {
        string? stringValue = value?.ToString();
        if (stringValue == null)
        {
            _values.Remove(key);
        }
        else
        {
            _values[key] = stringValue;
        }
    }
    
    public bool HasValue(string key)
    {
        return _values.ContainsKey(key);
    }
}