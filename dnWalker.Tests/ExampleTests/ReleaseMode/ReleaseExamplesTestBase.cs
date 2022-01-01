using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    [Trait("Category", "Examples/Release")]
    public abstract class ReleaseExamplesTestBase : ExamplesTestBase
    {
        protected static readonly string AssemblyFilePath = string.Format(ExamplesAssemblyFileFormat, "Release");

        protected static Lazy<DefinitionProvider> LazyDefinitionProvider =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(AssemblyFilePath)));


        protected ReleaseExamplesTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, LazyDefinitionProvider.Value)
        {
            OverrideConcolicExplorerBuilderInitialization(builder => builder.SetAssemblyFileName(AssemblyFilePath));
        }
    }
}
