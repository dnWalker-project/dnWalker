using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public static class ConcolicIConfigurationuration
    {
        public static (string assemblyName, string typeName) Strategy(this IConfiguration configuration)
        {
            return ExtensibilityPointHelper.FromTypeIdentifier(configuration.GetValue<string>("Strategy"));
        }

        public static IConfiguration SetStrategy(this IConfiguration configuration, string assemblyName, string typeName)
        {
            configuration.SetValue("Strategy", ExtensibilityPointHelper.ToTypeIdentifier(assemblyName, typeName));
            return configuration;
        }
        public static IConfiguration SetStrategy(this IConfiguration configuration, Type type)
        {
            configuration.SetValue("Strategy", ExtensibilityPointHelper.ToTypeIdentifier(type));
            return configuration;
        }

        public static IConfiguration SetStrategy<TStrategy>(this IConfiguration configuration)
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
