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
        private static Lazy<Dictionary<string, Type>> _lazyTypes = new Lazy<Dictionary<string, Type>>(Init);

        private static Dictionary<string, Type> Init()
        {
            var typesDict = new Dictionary<string, Type>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(t => !Regex.IsMatch(t.FullName, @"^(system|xunit|microsoft)\.|^(system|mscorlib),", RegexOptions.IgnoreCase))
                .Distinct();

            var types = assemblies.SelectMany(a => a.GetTypes())
                .Distinct()
                .Where(t => !t.FullName.StartsWith("System."))
                .ToList();

            foreach (var t in types)
            {
                if (typesDict.ContainsKey(t.FullName))
                {
                    continue;
                }
                typesDict.Add(t.FullName, t);
            }

            return typesDict;
        }

        public static MethodInfo GetMethodInfo(string methodName)
        {
            var lastDot = methodName.LastIndexOf(".");
            var methodTypeName = methodName.Substring(0, lastDot);

            if (!_lazyTypes.Value.TryGetValue(methodTypeName, out var type))
            {
                throw new Exception("Type not found");
            }

            return type.GetMethod(methodName.Substring(lastDot + 1));
        }
    }
}
