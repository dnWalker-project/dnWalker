﻿using dnlib.DotNet;

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

        public static IEnumerable<IReadOnlyModel> GetInputModels(this IConfiguration configuration, MethodDef method, IDefinitionProvider definitionProvider)
        {
            List<IReadOnlyModel> models = new List<IReadOnlyModel>();


            TypeParser tp = new TypeParser(definitionProvider);
            XmlModelDeserializer deserializer = new XmlModelDeserializer(tp, new MethodParser(definitionProvider, tp));

            String inputModelsFile = configuration.GetValueOrDefault<string>("InputModelsFile");
            if (inputModelsFile != null)
            {
                if (!System.IO.File.Exists(inputModelsFile))
                {
                    throw new ExplorationException($"InputModelsFile specified, but not found: {inputModelsFile}");
                }

                XElement xml = XElement.Load(inputModelsFile);
                String fullMethodName = method.DeclaringType.FullName + "." + method.Name;
                models.AddRange(xml.Elements().Where(e => e.Name == "InputModel" && e.Attribute("Method").Value == fullMethodName).Select(x => deserializer.FromXml(x, method)));
            }

            return models;
        }

        public static IConfiguration SetInputModelsFile(this IConfigurationBuilder configuration, string filename)
        {
            configuration.SetValue("InputModelsFile", filename);
            return configuration;
        }
    }
}
