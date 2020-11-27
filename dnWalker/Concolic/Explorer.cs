using dnlib.DotNet.Emit;
using dnWalker.NativePeers;
using dnWalker.Symbolic;
using dnWalker.Traversal;
using Echo.ControlFlow.Serialization.Dot;
using Echo.Platforms.Dnlib;
using MMC;
using MMC.Data;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic
{
    public delegate void PathExploredHandler(dnWalker.Traversal.Path path);

    public class Explorer
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;

        public Explorer(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _config = config;
            _logger = logger;
            _solver = solver;
            _definitionProvider = definitionProvider;            
        }

        public event PathExploredHandler OnPathExplored;

        public Traversal.PathStore PathStore { get; private set; }

        public void Run(string methodName, params IArg[] arguments)
        {
            var entryPoint = _definitionProvider.GetMethodDefinition(methodName)
                ?? throw new NullReferenceException($"Method {methodName} not found");

            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);
            
            var pathStore = new Traversal.PathStore(entryPoint);
            PathStore = pathStore;

            using (TextWriter writer = File.CreateText(@"c:\temp\dot.dot"))
            {
                var dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
                dotWriter.SubGraphAdorner = new ExceptionHandlerAdorner<Instruction>();
                dotWriter.NodeAdorner = new ControlFlowNodeAdorner<Instruction>();
                dotWriter.EdgeAdorner = new ControlFlowEdgeAdorner<Instruction>();
                dotWriter.Write(entryPoint.ConstructStaticFlowGraph());
            }

            var args = arguments?.Select(a => a.AsDataElement(_definitionProvider)).ToArray() ?? new IDataElement[] { };

            while (true)
            {
                SystemConsole.OutTextWriterRef = ObjectReference.Null;

                try
                {
                    var cur = stateSpaceSetup.CreateInitialState(entryPoint, args);
                    cur.CurrentThread.InstructionExecuted += pathStore.OnInstructionExecuted;

                    var statistics = new SimpleStatistics();

                    var explorer = new MMC.Explorer(cur, statistics, _logger, _config, PathStore);
                    explorer.InstructionExecuted += pathStore.OnInstructionExecuted;

                    explorer.Run();

                    var path = PathStore.CurrentPath;

                    System.Diagnostics.Debug.WriteLine($"Path explored {path.PathConstraintString}, input {string.Join(", ", args.Select(a => a.ToString()))}");

                    var next = PathStore.GetNextInputValues(_solver);

                    OnPathExplored?.Invoke(path);

                    if (next == null)
                    {
                        break;
                    }

                    args = entryPoint.Parameters.Select(p => new { p.Name, Value = next[p.Name] })
                        .Select(v =>
                        DataElementFactory.CreateDataElement(v.Value.GetType(), v.Value, Expression.Parameter(v.Value.GetType(), v.Name)))
                        .ToArray();

                    PathStore.ResetPath();
                }
                catch (Exception e)
                {
                    _logger.Log(LogPriority.Fatal, e.Message);
                }
            }
        }
    }
}
