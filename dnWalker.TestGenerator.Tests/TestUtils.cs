using System;

using dnWalker.TypeSystem;
using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.Templates.FluentAssertions;
using dnWalker.TestGenerator.Templates.Moq;

namespace dnWalker.TestGenerator.Tests
{
    public static class TestUtils
    {
        private static readonly IDomain _domain;
        private static readonly IDefinitionProvider _definitionProvider;

        private static readonly ITemplateProvider _templates;

        public static IDomain Domain => _domain;
        public static IDefinitionProvider DefinitionProvider => _definitionProvider;

        public static ITemplateProvider Templates => _templates;

        static TestUtils()
        {
            _domain = TypeSystem.Domain.LoadFromAppDomain(typeof(TestUtils).Assembly);
            _definitionProvider = new DefinitionProvider(_domain);

            _templates = new TemplateProvider(new MoqTemplate(), new BasicActTemplate(), new FluentAssertionsTemplate());
        }
    }
}

