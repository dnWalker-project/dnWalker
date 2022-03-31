using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.TypeSystem;

using MMC;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ConcolicExplorer = dnWalker.Concolic.Explorer;

namespace dnWalker.Tests
{
    public class ConcolicExplorerBuilder : ConfigBuilder<IConcolicExplorerBuilder>, IConcolicExplorerBuilder
    {
        private readonly List<IExplorationExtension> _extensions = new List<IExplorationExtension>();


        private Func<Logger> _provideLogger;
        private Func<IDefinitionProvider> _provideDefinitions;
        private Func<ISolver> _provideSolver;

        public ConcolicExplorerBuilder(Func<Logger> provideLogger, Func<IDefinitionProvider> provideDefinitions, Func<ISolver> provideSolver)
        {
            _provideLogger = provideLogger ?? throw new ArgumentNullException(nameof(provideLogger));
            _provideDefinitions = provideDefinitions ?? throw new ArgumentNullException(nameof(provideDefinitions));
            _provideSolver = provideSolver ?? throw new ArgumentNullException(nameof(provideSolver));
        }

        public IConcolicExplorerBuilder With(IExplorationExtension extension)
        {
            _extensions.Add(extension);
            return this; ;
        }

        public IConcolicExplorerBuilder OverrideSolver(Func<ISolver> solverFactory)
        {
            _provideSolver = solverFactory ?? throw new ArgumentNullException(nameof(solverFactory));
            return this;
        }

        public IConcolicExplorerBuilder OverrideLogger(Func<Logger> loggerFactory)
        {
            _provideLogger = loggerFactory ?? throw new ArgumentNullException(nameof(_provideLogger));
            return this;
        }

        public IConcolicExplorerBuilder OverrideDefinitionProvider(Func<IDefinitionProvider> definitionProviderFactory)
        {
            _provideDefinitions = definitionProviderFactory ?? throw new ArgumentNullException(nameof(definitionProviderFactory));
            return this;
        }

        public IExplorer Build()
        {
            if (_provideLogger == null)
            {
                throw new InvalidOperationException("Logger provider is not set!");
            }

            if (_provideDefinitions == null)
            {
                throw new InvalidOperationException("DefinitionsProvider provider is not set!");
            }

            if (_provideSolver == null)
            {
                throw new InvalidOperationException("Solver provider is not set!");
            }

            // curse of static data
            IDefinitionProvider definitionProvider = _provideDefinitions();
            AllocatedDelegate.DelegateTypeDef = definitionProvider.BaseTypes.Delegate.ToTypeDefOrRef();
            ConcolicExplorer explorer = new ConcolicExplorer(_provideDefinitions(), base.Config, _provideLogger(), _provideSolver());

            foreach (IExplorationExtension extension in _extensions)
            {
                explorer.AddExtension(extension);
            }

            return explorer;
        }

        protected override ConcolicExplorerBuilder GetOutterBuilder()
        {
            return this;
        }
    }
}
