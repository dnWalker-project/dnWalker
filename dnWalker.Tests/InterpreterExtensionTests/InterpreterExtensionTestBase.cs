using dnWalker.TypeSystem;

using MMC;
using MMC.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterExtensionTests
{
    [Trait("Category", "Interpreter Extensions")]
    public class InterpreterExtensionTestBase : TestBase
    {
        private const string AssemblyFilename = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";
        protected static Lazy<DefinitionProvider> Lazy = new Lazy<DefinitionProvider>(() => new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilename)));
        public InterpreterExtensionTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, Lazy.Value)
        {
        }

    }
}
