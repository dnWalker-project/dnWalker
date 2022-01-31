using dnlib.DotNet;

using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.Data;
using MMC.InstructionExec;
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
        private Func<IDefinitionProvider> _provideDefinitionProvider;
        private Func<IStatistics> _provideStatistics;

        private string _methodName;
        private Func<ExplicitActiveState, IDataElement[]> _provideArgs = (cur) => Array.Empty<IDataElement>();

        public ModelCheckerExplorerBuilder(Func<Logger> provideLogger, Func<IDefinitionProvider> provideDefinitionProvider, Func<IStatistics> provideStatistics, string methodName = null)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(provideDefinitionProvider));
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));

            _methodName = methodName;
        }

        public IModelCheckerExplorerBuilder OverrideLogger(Func<Logger> provideLogger)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideDefinitionProvider(Func<IDefinitionProvider> provideDefinitionProvider)
        {
            _provideDefinitionProvider = provideDefinitionProvider ?? throw new ArgumentNullException(nameof(_provideDefinitionProvider));
            return this;
        }

        public IModelCheckerExplorerBuilder OverrideStatistics(Func<IStatistics> provideStatistics)
        {
            _provideStatistics = provideStatistics ?? throw new ArgumentNullException(nameof(provideStatistics));
            return this;
        }

        public Explorer Build()
        {
            if (_methodName == null)
            {
                throw new InvalidOperationException("Cannot initialize the ModelChecker without method name!");
            }

            IDefinitionProvider definitionProvider = _provideDefinitionProvider();

            Logger logger = _provideLogger();

            StateSpaceSetup stateSpaaceSetup = new StateSpaceSetup(definitionProvider, Config, logger);
            MethodDef entryPoint = definitionProvider.GetMethodDefinition(_methodName) ?? throw new NullReferenceException($"Method {_methodName} not found");

            PathStore pathStore = new PathStore();

            ExplicitActiveState cur = stateSpaaceSetup.CreateInitialState(entryPoint, _provideArgs ?? throw new NullReferenceException("Args is null!"));
            cur.PathStore = pathStore;



            IStatistics statistics = _provideStatistics();
            Explorer explorer = new Explorer(cur, statistics, logger, Config, pathStore);
            //explorer.Run();
            return explorer;
        }

        protected override IModelCheckerExplorerBuilder GetOutterBuilder()
        {
            return this;
        }

        public IModelCheckerExplorerBuilder SetMethod(string methodName)
        {
            _methodName = methodName;
            return this;
        }

        public IModelCheckerExplorerBuilder SetArgs(IDataElement[] args)
        {
            _provideArgs = state => args;
            return this;
        }

        public IModelCheckerExplorerBuilder SetArgs(Func<ExplicitActiveState, IDataElement[]> args)
        {
            _provideArgs = args;
            return this;
        }
    }
}
