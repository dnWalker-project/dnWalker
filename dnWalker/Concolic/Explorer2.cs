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
    public class Explorer2
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;

        private int _currentIteration;
        private PathStore _pathStore;

        public Explorer2(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _definitionProvider = definitionProvider;
            _config = config;
            _logger = logger;
            _solver = solver;
        }
        public static Explorer2 ForAssembly(string assemblyFilename, ISolver solver, Action<Config> setup = null, TextWriter loggerOutput = null)
        {
            var assemblyLoader = new AssemblyLoader();

            var data = File.ReadAllBytes(assemblyFilename);

            _ = assemblyLoader.GetModuleDef(data);

            System.Reflection.Assembly.LoadFrom(assemblyFilename);
            var definitionProvider = DefinitionProvider.Create(assemblyLoader);


            var config = new Config();
            var logger = new Logger(Logger.Default); // | LogPriority.Trace);

            logger.AddOutput(new TextLoggerOutput(loggerOutput ?? Console.Out));

            return new Explorer2(definitionProvider, config, logger, solver);
        }


        public event PathExploredHandler OnPathExplored;

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
        public int IterationCount
        {
            get { return _currentIteration; }
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

            WriteFlowGraph(entryPoint);

            // setup iteration global objects
            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            _pathStore = new PathStore(entryPoint);

            var instructionExecProvider = InstructionExecProvider.Get(_config, new Symbolic.Instructions.InstructionFactory());
                
            var parameterStore = new ParameterStore();
            if (data == null)
            {
                data = new Dictionary<string, object>();
            }

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
                    parameterStore.Clear();

                    // 2. setup default values for the arguments
                    parameterStore.InitializeDefaultMethodParameters(entryPoint);

                    // 3. set traits using the 'data' dictionary - either passed as argument or as solver output
                    parameterStore.SetTraits(cur.DefinitionProvider, data);

                    // 4. construct the arguments DataElementList
                    var arguments = parameterStore.GetMethodParematers(cur, entryPoint);

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
                    
                    explorer.InstructionExecuted += _pathStore.OnInstructionExecuted;

                    explorer.Run();

                    explorer.InstructionExecuted -= _pathStore.OnInstructionExecuted;

                    var path = _pathStore.CurrentPath;
                    OnPathExplored?.Invoke(path);

                    _logger.Log(LogPriority.Message, "Explored path: {0}", path.PathConstraintString);


                    var exprs = parameterStore.GetParametersAsExpressions();
                    data = PathStore.GetNextInputValues(_solver, exprs);

                    if (data == null)
                    {
                        break;
                    }

                    PathStore.ResetPath();
                }
                catch (Exception e)
                {
                    _logger.Log(LogPriority.Fatal, e.Message);
                    throw;
                }

            }
        }
    }
}
