using dnWalker.Explorations.Xml;
using dnWalker.TestGenerator;
using dnWalker.Tests.ExampleTests;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.TestGeneratorIntegration
{
    public class IntegrationTestBase : ExamplesTestBase
    {
        protected static readonly string AssemblyFilePath = string.Format(ExamplesAssemblyFileFormat, "Release");
        private readonly XmlExplorationSerializer _serializer;
        private readonly XmlExplorationDeserializer _deserializer;

        public IntegrationTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilePath)))
        {
            OverrideConcolicExplorerBuilderInitialization(b =>
            {
                b.SetAssemblyFileName(AssemblyFilePath);
            });


            IMethodTranslator methodTranslator = new MethodTranslator(DefinitionProvider);
            ITypeTranslator typeTranslator = new TypeTranslator(DefinitionProvider);
            _serializer = new XmlExplorationSerializer(new XmlModelSerializer(typeTranslator, methodTranslator));
            _deserializer = new XmlExplorationDeserializer(methodTranslator, new XmlModelDeserializer(typeTranslator, methodTranslator));
        }

        protected ITestGeneratorConfiguration GetConfiguration()
        {
            return new TestGeneratorConfiguration()
            {
                PreferLiteralsOverVariables = true,
            };
        }

        protected XmlExplorationSerializer Serializer => _serializer;
        protected XmlExplorationDeserializer Deserializer => _deserializer;
    }
}
