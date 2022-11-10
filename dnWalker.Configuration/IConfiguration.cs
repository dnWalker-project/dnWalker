using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Configuration
{
    public interface IConfiguration
    {
        bool TryGetValue(string key, Type type, [NotNullWhen(true)] out object? value);
        bool HasValue(string key);
    }
}
