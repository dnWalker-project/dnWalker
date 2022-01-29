using dnWalker.TypeSystem;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode
{
    [Trait("Category", "Examples/Debug")]
    public abstract class DebugExamplesTestBase : ExamplesTestBase
    {
        protected static readonly string AssemblyFilePath = string.Format(ExamplesAssemblyFileFormat, "Debug");

        protected static Lazy<DefinitionProvider> LazyDefinitionProvider =
            new Lazy<DefinitionProvider>(() => new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilePath)));

        protected DebugExamplesTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, LazyDefinitionProvider.Value)
        {
            OverrideConcolicExplorerBuilderInitialization(builder => builder.SetAssemblyFileName(AssemblyFilePath));
        }
    }
}
