using dnlib.DotNet;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public class ModelCheckerExplorerBuilder : ConfigBuilder<IModelCheckerExplorerBuilder>, IModelCheckerExplorerBuilder
    {
        private Func<Logger> _provideLogger;
        private Func<DefinitionProvider> _provideDefinitionProvider;
        private Func<IStatistics> _provideStatistics;

        public ModelCheckerExplorerBuilder(Func<Logger> provideLogger, Func<DefinitionProvider> provideDefinitionProvider, Func<IStatistics> provideStatistics, string methodName = null)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(provideDefinitionProvider));
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));

            MethodName = methodName;
        }

        public IModelCheckerExplorerBuilder OverrideLogger(Func<Logger> provideLogger)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideDefinitionProvider(Func<DefinitionProvider> provideDefinitionProvider)
        {
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(_provideDefinitionProvider));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideStatistics(Func<IStatistics> provideStatistics)
        {
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));
            return this;
        }

        public string MethodName
        {
            get; set;
        }

        public IDataElement[] Args
        {
            get; set;
        }

        public Explorer BuildAndRun()
        {
            if (MethodName == null)
            {
                throw new InvalidOperationException("Cannot initialize the ModelChecker without method name!");
            }

            DefinitionProvider definitionProvider = _provideDefinitionProvider();

            Logger logger = _provideLogger();

            StateSpaceSetup stateSpaaceSetup = new StateSpaceSetup(definitionProvider, Config, logger);
            MethodDef entryPoint = definitionProvider.GetMethodDefinition(MethodName) ?? throw new NullReferenceException($"Method {MethodName} not found");

            ExplicitActiveState cur = stateSpaaceSetup.CreateInitialState(entryPoint, Args ?? throw new NullReferenceException("Args is null!"));
            IStatistics statistics = _provideStatistics();
            Explorer explorer = new Explorer(cur, statistics, logger, Config);
            explorer.Run();
            return explorer;
        }

        protected override IModelCheckerExplorerBuilder GetOutterBuilder()
        {
            return this;
        }

        public IModelCheckerExplorerBuilder WithMethod(string methodName)
        {
            MethodName = methodName;
            return this;
        }

        public IModelCheckerExplorerBuilder WithArgs(IDataElement[] args)
        {
            Args = args;
            return this;
        }
    }
}
