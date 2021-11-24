using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using dnWalker.Traversal;
using MMC;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests
{
    public abstract class ExamplesTestBase : TestBase
    {
        protected const string ExamplesAssemblyFileFormat = @"..\..\..\..\Examples\bin\{0}\net5.0\Examples.dll";

        protected ExamplesTestBase(ITestOutputHelper testOutputHelper, DefinitionProvider definitionProvider) : base(testOutputHelper, definitionProvider)
        {

        }

        protected void Explore(string methodName, Action<IConfig> initializeConfig = null, Action<dnWalker.Concolic.Explorer> starting = null, Action<dnWalker.Concolic.Explorer> finished = null, IDictionary<string, object> data = null)
        {
            initializeConfig?.Invoke(_config);

            var explorer = new dnWalker.Concolic.Explorer(_definitionProvider, _config, _logger, new Z3.Solver());

            starting?.Invoke(explorer);
            explorer.Run(methodName);//, args);

            finished?.Invoke(explorer);
        }

        public static string GetTestMethodName([CallerMemberName]string methodName = null)
        {
            return methodName ?? string.Empty;
        }
    }
}
