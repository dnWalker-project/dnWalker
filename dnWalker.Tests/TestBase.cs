using MMC;
using MMC.Data;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace dnWalker.Tests
{
    public abstract class TestBase
    {
        private readonly AssemblyLoader _assemblyLoader;
        private readonly Config _config;
        private readonly DefinitionProvider _definitionProvider;

        protected TestBase(string assemblyFilename)
        {
            _assemblyLoader = new AssemblyLoader();
            _config = new Config();
            var data = File.ReadAllBytes(assemblyFilename);
            var moduleDef = _assemblyLoader.GetModuleDef(data);
            _logger = new Logger(Logger.Default | LogPriority.Trace);
            _logger.AddOutput(new TestLoggerOutput());
            //_appDomain = _assemblyLoader.CreateAppDomain(moduleDef, data);
            Assembly.LoadFrom(assemblyFilename);
            _definitionProvider = DefinitionProvider.Create(_assemblyLoader, _logger);
        }

        public Logger _logger { get; }

        protected virtual object Test(string methodName, params object[] args)
        {
            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException("Method not found");

            var state = stateSpaceSetup.CreateInitialState(
                entryPoint,
                args.Select(a => _definitionProvider.CreateDataElement(a)).ToArray());

            var statistics = new SimpleStatistics();

            var explorer = new Explorer(state, statistics, _logger, _config);
            explorer.Run();

            var ex = explorer.GetUnhandledException();
            if (ex != null)
            {
                throw ex;
            }

            return state.CurrentThread.RetValue;
        }
    }
}
