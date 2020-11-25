using dnWalker.Traversal;
using MMC;
using System;
using System.Linq;

namespace dnWalker.Concolic
{
    public delegate void PathExploredHandler(dnWalker.Traversal.Path path);

    public class Explorer
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;
        private PathStore _pathStore;

        public Explorer(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _config = config;
            _logger = logger;
            _solver = solver;
            _definitionProvider = definitionProvider;            
        }

        public event PathExploredHandler OnPathExplored;

        public PathStore PathStore => _pathStore;

        public void Run(string methodName, params IArg[] args)
        {
            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException($"Method {methodName} not found");

            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);
            
            var pathStore = new Traversal.PathStore();
            _pathStore = pathStore;

            //var edges = cfg.GetEdges().ToDictionary(e => e.Origin.)
            //PathStore.

            /*TextWriter writer = new StringWriter();
            var dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
            dotWriter.Write(cfg);*/

            try
            {
                var cur = stateSpaceSetup.CreateInitialState(entryPoint, args);
                cur.CurrentThread.InstructionExecuted += pathStore.OnInstructionExecuted;

                var statistics = new SimpleStatistics();

                var explorer = new MMC.Explorer(cur, statistics, _logger, _config, _pathStore);
                explorer.InstructionExecuted += pathStore.OnInstructionExecuted;

                explorer.Run();

                var path = _pathStore.CurrentPath;
                var expressionToSolve = explorer.PathStore.GetNextPathConstraint(path);
                if (expressionToSolve == null)
                {
                    return;
                }

                OnPathExplored?.Invoke(path);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
