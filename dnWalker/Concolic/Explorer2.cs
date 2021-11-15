using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Concolic.Parameters;
using dnWalker.Concolic.Traversal;
using dnWalker.NativePeers;

using Echo.ControlFlow.Serialization.Dot;
using Echo.Platforms.Dnlib;

using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class Explorer2 : IDisposable
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;

        private int _currentIteration;
        private PathStore _pathStore;
        private ParameterStore _inputParameters;

        private readonly IExplorationExporter _explorationExporter;

        public Explorer2(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _definitionProvider = definitionProvider;
            _config = config;
            _logger = logger;
            _solver = solver;

            if (config.ExportIterationInfo)
            {
                _explorationExporter = new XmlExplorationExporter(config.ExplorationInfoOutputFile ?? "exploration_info.xml");
                _explorationExporter.HookUp(this);
            }
        }


        private static void WriteFlowGraph(dnlib.DotNet.MethodDef method, string filename = @"c:\temp\dot.dot")
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                var dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
                dotWriter.SubGraphAdorner = new ExceptionHandlerAdorner<Instruction>();
                dotWriter.NodeAdorner = new ControlFlowNodeAdorner<Instruction>();
                dotWriter.EdgeAdorner = new ControlFlowEdgeAdorner<Instruction>();
                dotWriter.Write(method.ConstructStaticFlowGraph());
            }
        }


        public PathStore PathStore
        {
            get { return _pathStore; }
        }
        public ParameterStore InputParameters
        {
            get { return _inputParameters; }
        }
        public int IterationCount
        {
            get { return _currentIteration; }
        }

        // setting event to an empty delegate will optimize invocation - no need to check for null...
        public event PathExploredHandler PathExplored = delegate { };
        public event EventHandler<ExplorationStartedEventArgs> ExplorationStarted = delegate { };
        public event EventHandler<ExplorationFinishedEventArgs> ExplorationFinished = delegate { };
        public event EventHandler<ExplorationFailedEventArgs> ExplorationFailed = delegate { };
        public event EventHandler<IterationStartedEventArgs> IterationStarted = delegate { };
        public event EventHandler<IterationFinishedEventArgs> IterationFinished = delegate { };

        protected virtual void OnPathExplored(dnWalker.Traversal.Path path)
        {
            PathExplored(path);
        }

        protected virtual void OnExplorationStarted(ExplorationStartedEventArgs e)
        {
            ExplorationStarted(this, e);
        }
        protected virtual void OnExplorationFailed(ExplorationFailedEventArgs e)
        {
            ExplorationFailed(this, e);
        }

        protected virtual void OnIterationStarted(IterationStartedEventArgs e)
        {
            IterationStarted(this, e);
        }

        protected virtual void OnIterationFinished(IterationFinishedEventArgs e)
        {
            IterationFinished(this, e);
        }

        public void Run(string methodName, IDictionary<string, object> data = null)
        {
            _currentIteration = 0;
            var maxIterations = _config.MaxIterations;

            void NextIterationOrThrow()
            {
                if (maxIterations > 0 && _currentIteration >= maxIterations)
                {
                    throw new MaxIterationsExceededException(_currentIteration);
                }
                ++_currentIteration;
            }

            // get the tested method
            var  entryPoint = _definitionProvider.GetMethodDefinition(methodName) ?? throw new NullReferenceException($"Method {methodName} not found");

            WriteFlowGraph(entryPoint, _config.FlowGraphFile);

            // setup iteration global objects
            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            _pathStore = new PathStore(entryPoint);

            var instructionExecProvider = InstructionExecProvider.Get(_config, new Symbolic.Instructions.InstructionFactory());
                
            if (data == null)
            {
                data = new Dictionary<string, object>();
            }

            OnExplorationStarted(new ExplorationStartedEventArgs(_config.AssemblyToCheckFileName, entryPoint.Module.Assembly.FullName, entryPoint.FullName, entryPoint.IsStatic, _solver?.GetType().FullName));

            // run iteration
            while (true)
            {
                SystemConsole.OutTextWriterRef = ObjectReference.Null;

                try
                {
                    NextIterationOrThrow();

                    // setup initial state
                    var cur = new ExplicitActiveState(_config, instructionExecProvider, _definitionProvider, _logger);
                    cur.PathStore = _pathStore;

                    // 1. clear parameterStore
                    _inputParameters = new ParameterStore();

                    // 2. setup default values for the arguments
                    _inputParameters.InitializeDefaultMethodParameters(entryPoint);

                    // 3. set traits using the 'data' dictionary - either passed as argument or as solver output
                    _inputParameters.SetTraits(cur.DefinitionProvider, data);

                    // 4. construct the arguments DataElementList
                    var arguments = _inputParameters.GetMethodParematers(cur, entryPoint);

                    var mainState = new MethodState(entryPoint, arguments, cur);

                    // Initialize main thread.
                    cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, StateSpaceSetup.CreateMainThreadObject(cur, entryPoint, _logger));
                    // -------------

                    //var cur = stateSpaceSetup.CreateInitialState(entryPoint, args);
                    cur.CurrentThread.InstructionExecuted += _pathStore.OnInstructionExecuted;

                    var statistics = new SimpleStatistics();

                    var explorer = new MMC.Explorer(cur, statistics, _logger, _config, PathStore);

                    //_logger.Log(LogPriority.Message, "Starting exploration, parameters: {0}", parameterStore.ToString());
                    _logger.Log(LogPriority.Message, "Starting exploration, data: ");
                    foreach (var p in data) _logger.Log(LogPriority.Message, "{0} = {1}", p.Key, p.Value);

                    OnIterationStarted(new IterationStartedEventArgs(_currentIteration, _inputParameters));

                    explorer.InstructionExecuted += _pathStore.OnInstructionExecuted;
                    explorer.Run();

                    explorer.InstructionExecuted -= _pathStore.OnInstructionExecuted;

                    var path = _pathStore.CurrentPath;
                    PathExplored?.Invoke(path);

                    _logger.Log(LogPriority.Message, "Explored path: {0}", path.PathConstraintString);


                    var exprs = _inputParameters.GetParametersAsExpressions();
                    data = PathStore.GetNextInputValues(_solver, exprs);

                    OnIterationFinished(new IterationFinishedEventArgs(_currentIteration, null, path));

                    if (data == null)
                    {
                        break;
                    }

                    PathStore.ResetPath();
                }
                catch (Exception e)
                {
                    _logger.Log(LogPriority.Fatal, e.Message);
                    OnExplorationFailed(new ExplorationFailedEventArgs(e));
                    throw;
                }

            }
        }

        public void Dispose()
        {

        }
    }
}
