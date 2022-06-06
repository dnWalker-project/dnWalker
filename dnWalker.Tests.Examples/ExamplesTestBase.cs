using dnWalker.Concolic;
using dnWalker.Configuration;
using dnWalker.Symbolic;
using dnWalker.TypeSystem;
using dnWalker.Z3;

using MMC;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Examples
{
    public abstract class ExamplesTestBase
    {
        private static string GetExamplesAssembly(string buildConfiguraiton, string buildTarget)
        {
            return $"../../../../Examples/bin/{buildConfiguraiton}/{buildTarget}/Examples.dll";
        }

        private static readonly ConcurrentDictionary<string, IDefinitionProvider> _definitionProviders = new ConcurrentDictionary<string, IDefinitionProvider>();

        private static IDefinitionProvider GetDefinitionProvider(string buildConfiguration, string buildTarget)
        {
            string assembly = GetExamplesAssembly(buildConfiguration, buildTarget);
            if (!_definitionProviders.TryGetValue(assembly, out IDefinitionProvider? definitionProvider))
            {
                System.Diagnostics.Debug.WriteLine($"Building definition provider for: '{assembly}'");

                definitionProvider = new DefinitionProvider(Domain.LoadFromFile(assembly));
                _definitionProviders.TryAdd(assembly, definitionProvider);
            }
            return definitionProvider;
        }


        private IDefinitionProvider? _definitionProvider;
        private readonly ITestOutputHelper _output;

        protected ExamplesTestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        protected IDefinitionProvider DefinitionProvider => _definitionProvider ?? throw new InvalidDataException("TEST IS NOT INITIALIZED");
        protected ITestOutputHelper Output => _output;

        protected virtual IExplorer CreateExplorer(BuildInfo buildInfo, Action<IConfiguration>? configure = null, Action<Logger>? configureLogging = null, Func<ISolver>? buildSolver = null)
        {
            _definitionProvider = GetDefinitionProvider(buildInfo.Configuration, buildInfo.Target);

            IConfiguration config = new ConfigurationBuilder()
                .Build()
                .InitializeDefaults();
            SetupConfiguration(config);
            configure?.Invoke(config);

            Logger logger = new Logger();
            SetupLogging(logger);
            configureLogging?.Invoke(logger);

            ISolver solver = buildSolver?.Invoke() ?? new Z3Solver();

            return new ConcolicExplorer(_definitionProvider, config, logger, solver);
        }

        protected virtual void SetupLogging(Logger logger) { }
        protected virtual void SetupConfiguration(IConfiguration configuration) 
        {
            configuration.SetStrategy(typeof(Concolic.Traversal.AllPathsCoverage));
        }
    }
}
