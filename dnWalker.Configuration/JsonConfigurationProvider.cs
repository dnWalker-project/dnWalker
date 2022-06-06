using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    internal class JsonConfigurationProvider : IConfigurationProvider
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

        public bool TryGetValue(string key, Type type, out object? value)
        {
            if (_document.RootElement.TryGetProperty(key, out JsonElement jsonValue))
            {
                value = JsonSerializer.Deserialize(jsonValue, type);
            }
            value = null;
            return false;
        }
    }
}
