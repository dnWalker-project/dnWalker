using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker
{
    public static class ExtensibilityPointHelper
    {
        private static Type FindType(string assemblyName, string typeName)
        {
            AppDomain domain = AppDomain.CurrentDomain;

            Assembly assembly = string.IsNullOrEmpty(assemblyName) ? typeof(ExtensibilityPointHelper).Assembly :
                AppDomain.CurrentDomain
                .GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName)
                ?? domain.Load(assemblyName)
                ?? throw new Exception($"Could not find or load the assembly: '{assemblyName}'.");

            Type type = assembly
                .GetType(typeName)
                ?? throw new Exception($"Could not find the type '{typeName}'.");

            return type;
        }

        public static T Create<T>(string assemblyName, string typeName)
            where T : class
        {
            Type type = FindType(assemblyName, typeName);
            return (T)Activator.CreateInstance(type);
        }

        public static T Create<T>(string assemblyName, string typeName, params object?[] args)
            where T : class
        {
            Type type = FindType(assemblyName, typeName);
            return (T)Activator.CreateInstance(type, args);
        }

        public static (string assemblyName, string typeName) FromTypeIdentifier(string identifier)
        {
            string[] parts = identifier.Split(';');

            if (parts.Length == 1) return (null, parts[0]);
            return (parts[0], parts[1]);
        }
        public static string ToTypeIdentifier(string assemblyName, string typeName)
        {
            return $"{assemblyName};{typeName}";
        }
        public static string ToTypeIdentifier(Type type)
        {
            return ToTypeIdentifier(type.Assembly.GetName().Name, type.FullName);
        }
    }
}
