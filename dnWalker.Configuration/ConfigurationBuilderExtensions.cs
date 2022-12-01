using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddJsonFile(this IConfigurationBuilder builder, string fileName)
    {
        try
        {
            string json = File.ReadAllText(fileName);
            JsonConfigurationProvider jsonConfig = new JsonConfigurationProvider(json);
            builder.AddProvider(jsonConfig);
            return builder;
        }
        catch (IOException e)
        {
            throw;
        }
    }

    public static IConfigurationBuilder AddIniFile(this IConfigurationBuilder builder, string fileName)
    {
        try
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                while (TryReadLine(reader, out string? line))
                {
                    int sepIndex = line.IndexOf('=');
                    if (sepIndex < 0) continue;

                    string key = line.Substring(sepIndex).Trim();
                    string value = line.Substring(sepIndex + 1, line.Length - sepIndex).Trim();
                    data.Add(KeyValuePair.Create(key, value));
                }
            }

            TextConfigurationProvider provider = new TextConfigurationProvider(data);
            builder.AddProvider(provider);
            return builder;
        }
        catch (IOException e)
        {
            throw;
        }

        static bool TryReadLine(StreamReader reader, [NotNullWhen(true)] out string? line)
        {
            line = reader.ReadLine();
            return line != null;
        }
    }
}