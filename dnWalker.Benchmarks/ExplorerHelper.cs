using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Symbolic;
using dnWalker.TypeSystem;

using MMC;

namespace dnWalker.Benchmarks
{
    internal class ExplorerHelper
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly IConfiguration _configuration;

        public ExplorerHelper(IDefinitionProvider definitionProvider)
        {
            _configuration = new ConfigurationBuilder()
                .InitializeDefaults()
                .SetMaxIterationsWithoutNewEdge(10)
                .Build();

            _definitionProvider = definitionProvider;
        }


        private IExplorer CreateExplorer()
        {
            IConfiguration configuration = _configuration;
            Logger logger = new Logger();
            ISolver solver = new Z3.Z3Solver();
            IDefinitionProvider definitionProvider = _definitionProvider;

            return new ConcolicExplorer(definitionProvider, configuration, logger, solver);
        }

        public ExplorationResult Explore(string method)
        {
            MethodDef md = _definitionProvider.GetMethodDefinition(method);

            IExplorer explorer = CreateExplorer();

            return explorer.Run(md, new SmartAllPathsCoverage());
        }
    }
}