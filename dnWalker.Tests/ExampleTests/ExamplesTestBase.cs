using System;
using System.Linq;
using dnWalker.Traversal;
using MMC;

namespace dnWalker.Tests.ExampleTests
{
    public abstract class ExamplesTestBase : TestBase
    {
        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(GetAssemblyLoader(@"..\..\..\Examples\bin\debug\Examples.exe")));

        protected ExamplesTestBase(DefinitionProvider definitionProvider) : base(definitionProvider)
        {
            _config.StateStorageSize = 5;
        }
    }

    public abstract class SymbolicExamplesTestBase
    {
        protected readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;
        private readonly Logger _logger;
        private readonly PathStore _pathStore;

        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(@"..\..\..\..\Examples\bin\debug\Examples.exe")));

        protected SymbolicExamplesTestBase()
        {
            _config = new Config();
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            _definitionProvider = Lazy.Value;
            _pathStore = new PathStore();
        }

        protected void Explore(
            string methodName,
            Action<IConfig> before,
            Action<Concolic.Explorer> finished,
            params IArg[] args)
        {
            before?.Invoke(_config);

            var explorer = new Concolic.Explorer(_definitionProvider, _config, _logger, _pathStore);
            explorer.Run(methodName, args);

            finished(explorer);
        }
    }
}
