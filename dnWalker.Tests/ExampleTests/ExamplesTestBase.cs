using System;
using System.Linq;
using dnWalker.Traversal;
using MMC;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests
{
    public abstract class ExamplesTestBase : TestBase
    {
        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(GetAssemblyLoader(@"..\..\..\..\Examples\bin\debug\Examples.exe")));//..\..\..\Examples\bin\debug\Examples.exe")));

        protected ExamplesTestBase(DefinitionProvider definitionProvider) : base(definitionProvider)
        {
            _config.StateStorageSize = 5;
        }
    }

    public abstract class SymbolicExamplesTestBase : AbstractTestBase
    {
        protected readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;
        private readonly Logger _logger;
        
        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(@"..\..\..\..\Examples\bin\debug\Examples.exe")));

        protected SymbolicExamplesTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _config = new Config();
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            _definitionProvider = Lazy.Value;            
        }

        protected void Explore(
            string methodName,
            Action<IConfig> before,
            Action<dnWalker.Concolic.Explorer2> finished,
            params IArg[] args)
        {
            before?.Invoke(_config);

            var explorer = new dnWalker.Concolic.Explorer2(_definitionProvider, _config, _logger, new Z3.Solver());
            explorer.Run(methodName);//, args);

            finished(explorer);
        }
    }
}
