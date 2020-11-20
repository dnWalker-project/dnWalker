using dnWalker.Traversal;
using MMC;
using System;

namespace dnWalker.Concolic
{
    public delegate void PathExploredHandler(Path path);

    public class Explorer
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly DefinitionProvider _definitionProvider;
        
        public Explorer(DefinitionProvider definitionProvider, Config config, Logger logger, PathStore pathStore)
        {
            _config = config;
            _logger = logger;
            PathStore = pathStore;
            _definitionProvider = definitionProvider;
        }

        public event PathExploredHandler OnPathExplored;

        public PathStore PathStore { get; }

        public void Run(string methodName, params IArg[] args)
        {
            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException($"Method {methodName} not found");

            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);
            var state = stateSpaceSetup.CreateInitialState(entryPoint, args);

            var statistics = new SimpleStatistics();

            var explorer = new MMC.Explorer(state, statistics, _logger, _config, PathStore);
            explorer.Run();
        }
    }
}
