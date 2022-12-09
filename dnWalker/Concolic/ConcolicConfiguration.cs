using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Concolic
{
    public static class ConcolicIConfigurationuration
    {
        public static (string assemblyName, string typeName) Strategy(this IConfiguration configuration)
        {
            return ExtensibilityPointHelper.FromTypeIdentifier(configuration.GetValueOrDefault<string>("Strategy"));
        }

        public static IConfiguration SetStrategy(this IConfigurationBuilder configuration, string assemblyName, string typeName)
        {
            configuration.SetValue("Strategy", ExtensibilityPointHelper.ToTypeIdentifier(assemblyName, typeName));
            return configuration;
        }
        public static IConfiguration SetStrategy(this IConfigurationBuilder configuration, Type type)
        {
            configuration.SetValue("Strategy", ExtensibilityPointHelper.ToTypeIdentifier(type));
            return configuration;
        }

        public static IConfiguration SetStrategy<TStrategy>(this IConfigurationBuilder configuration)
            where TStrategy : IExplorationStrategy
        {
            configuration.SetValue("Strategy", ExtensibilityPointHelper.ToTypeIdentifier(typeof(TStrategy)));
            return configuration;
        }

        public static IExplorationStrategy CreateStrategy(this IConfiguration configuration)
        {
            (string assemblyName, string typeName) = configuration.Strategy();
            return ExtensibilityPointHelper.Create<IExplorationStrategy>(assemblyName, typeName);
        }
    }
}
