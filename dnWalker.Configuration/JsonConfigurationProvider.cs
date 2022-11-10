using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public class JsonConfigurationProvider : IConfigurationProvider
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions();
        private readonly JsonDocument _document;

        public JsonConfigurationProvider(JsonDocument document)
        {
            _document = document ?? throw new ArgumentNullException(nameof(document));
        }

        public JsonConfigurationProvider(string json) : this(JsonDocument.Parse(json))
        {
        }

        public bool TryGetValue(string key, Type type, [NotNullWhen(true)] out object? value)
        {
            if (_document.RootElement.TryGetProperty(key, out JsonElement jsonValue))
            {   
                value = JsonSerializer.Deserialize(jsonValue, type);

                return value != null;
            }
            value = null;
            return false;
        }

        public bool HasValue(string key)
        {
            return _document.RootElement.TryGetProperty(key, out _);
        }
    }
}
