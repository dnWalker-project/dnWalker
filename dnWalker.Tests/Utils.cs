using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public static class Utils
    {
        private static Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public static MethodInfo GetMethodInfo(string methodName)
        {
            var lastDot = methodName.LastIndexOf(".");
            var methodTypeName = methodName.Substring(0, lastDot);

            if (!_types.Any())
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(t => !Regex.IsMatch(t.FullName, @"^(system|xunit|microsoft)\.|^(system|mscorlib),", RegexOptions.IgnoreCase))
                    .Distinct();
                var types = assemblies.SelectMany(a => a.GetTypes())
                    .Distinct()
                    .Where(t => !t.FullName.StartsWith("System."))
                    .ToList();
                foreach (var t in types)
                {
                    if (_types.ContainsKey(t.FullName))
                    {
                        continue;
                    }
                    _types.Add(t.FullName, t);
                }
            }

            if (!_types.TryGetValue(methodTypeName, out var type))
            {
                throw new Exception("Type not found");
            }

            return type.GetMethod(methodName.Substring(lastDot + 1));
        }
    }
}
