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

        public IntegrationTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilePath)))
        {
            OverrideConcolicExplorerBuilderInitialization(b =>
            {
                b.SetAssemblyFileName(AssemblyFilePath);
            });
        }



    }
}
