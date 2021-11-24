﻿using System;
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
        protected const string AssemblyFile = @"..\..\..\..\Examples\bin\debug\Examples.exe";

        protected readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;
        private readonly Logger _logger;

        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(AssemblyFile)));

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
            Action<dnWalker.Concolic.Explorer> finished,
            params IArg[] args)
        {
            before?.Invoke(_config);

            var explorer = new dnWalker.Concolic.Explorer(_definitionProvider, _config, _logger, new Z3.Solver());
            explorer.Run(methodName);//, args);

            finished(explorer);
        }
    }

    public abstract class SymbolicExamplesTestBase2 : AbstractTestBase
    {
        protected const string AssemblyFile = @"..\..\..\..\Examples\bin\debug\Examples.exe";

        protected readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;
        private readonly Logger _logger;
        
        protected static Lazy<DefinitionProvider> Lazy =
            new Lazy<DefinitionProvider>(() => DefinitionProvider.Create(TestBase.GetAssemblyLoader(AssemblyFile)));

        protected SymbolicExamplesTestBase2(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            _config = new Config();
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            _definitionProvider = Lazy.Value;            
        }

        protected void Explore(
            string methodName,
            Action<Config> before,
            Action<dnWalker.Concolic.Explorer> finished,
            params IArg[] args)
        {
            _config.FlowGraphFile = this.GetType().Namespace + "." + this.GetType().Name + "FlowGraph" + ".dot";
            before?.Invoke(_config);

            var explorer = new dnWalker.Concolic.Explorer(_definitionProvider, _config, _logger, new Z3.Solver());
            explorer.Run(methodName);//, args);

            finished(explorer);
        }
    }
}
