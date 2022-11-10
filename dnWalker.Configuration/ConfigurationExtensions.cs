using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Configuration;

public static class ConfigurationExtensions
{
    public static bool TryGetValue<TValue>(this IConfiguration configuration, string key, [NotNullWhen(true)] out TValue? value)
    {
        if (configuration.TryGetValue(key, typeof(TValue), out object? v))
        {
            value = (TValue)v;
            return true;
        }
        value = default(TValue);
        return false;
    }

    public static TValue? GetValueOrDefault<TValue>(this IConfiguration configuration, string key)
    {
        if (!TryGetValue<TValue>(configuration, key, out TValue? value))
        {
            value = default(TValue);
        }

        return value;
    }

    [return: NotNullIfNotNull("fallbackValue")]
    public static TValue? GetValueOrDefault<TValue>(this IConfiguration configuration, string key, TValue? fallbackValue)
    {
        if (!TryGetValue<TValue>(configuration, key, out TValue? value))
        {
            value = fallbackValue;
        }

        return value;
    }

    public static TValue? GetValueOrDefault<TValue>(this IConfiguration configuration, string key, Func<TValue?> fallbackFactory)
    {
        if (!TryGetValue<TValue>(configuration, key, out TValue? value))
        {
            value = fallbackFactory();
        }

        return value;
    }

    public static TValue? GetValueOrDefault<TValue>(this IConfiguration configuration, string key, Func<IConfiguration, TValue?> fallbackFactory)
    {
        if (!TryGetValue<TValue>(configuration, key, out TValue? value))
        {
            value = fallbackFactory(configuration);
        }

        return value;
    }

    public static TValue? GetValueOrDefault<TValue>(this IConfiguration configuration, string key, Func<IConfiguration, string, TValue?> fallbackFactory)
    {
        if (!TryGetValue<TValue>(configuration, key, out TValue? value))
        {
            value = fallbackFactory(configuration, key);
        }

        return value;
    }
}