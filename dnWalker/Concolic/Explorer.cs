﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Concolic.Traversal;
using dnWalker.DataElements;
using dnWalker.Instructions.Extensions;
using dnWalker.NativePeers;
using dnWalker.Parameters;

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
    public class Explorer : IDisposable, IExplorer
    {
        private readonly Config _config;
        private readonly Logger _logger;
        private readonly ISolver _solver;
        private readonly DefinitionProvider _definitionProvider;

        private int _currentIteration;
        private PathStore _pathStore;
        private ParameterStore _parameterStore;

        private readonly IExplorationExtension _explorationExporter;

        private readonly List<IExplorationExtension> _extensions = new List<IExplorationExtension>();

        public Explorer(DefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
        {
            _definitionProvider = definitionProvider;
            _config = config;
            _logger = logger;
            _solver = solver;
        }



        public void AddExtension(IExplorationExtension extension)
        {
            extension.Register(this);
            _extensions.Add(extension);
        }

        public void RemoveExtension(IExplorationExtension extension)
        {
            if (_extensions.Remove(extension))
            {
                extension.Unregister(this);
            }
        }

        public PathStore PathStore
        {
            get { return _pathStore; }
        }
        public ParameterStore ParameterStore
        {
            get { return _parameterStore; }
        }
        public int IterationCount
        {
            get { return _currentIteration; }
        }

        // setting event to an empty delegate will optimize invocation - no need to check for null...
        public event Action<dnWalker.Traversal.Path> PathExplored = delegate { };
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

        /// <summary>
        /// Runs the explorer with specified starting arguments. Only primitive value arguments can be used.
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="data"></param>
        /// <exception cref="MaxIterationsExceededException"></exception>
        /// <exception cref="NullReferenceException"></exception>
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
            var entryPoint = _definitionProvider.GetMethodDefinition(methodName) ?? throw new NullReferenceException($"Method {methodName} not found");

            // setup iteration global objects
            var stateSpaceSetup = new StateSpaceSetup(_definitionProvider, _config, _logger);

            _pathStore = new PathStore(entryPoint);

            var f = new dnWalker.Instructions.ExtendableInstructionFactory();
            f.AddSymbolicExecution();
            f.AddPathConstraintProducers();
            f.AddParameterHandlers();

            var instructionExecProvider = InstructionExecProvider.Get(_config, f);

            if (data == null)
            {
                data = new Dictionary<string, object>();
            }

            _parameterStore = new ParameterStore();

            foreach (KeyValuePair<string, object> kvp in data)
            {
                TypeSig type = entryPoint.Parameters.FirstOrDefault(p => p.Name == kvp.Key)?.Type;

                if (type == null)
                {
                    _logger.Warning($"Parameter with name {kvp.Key} is not defined for the explored method.");
                }
                else
                {
                    IParameter p = ParameterFactory.CreateParameter(type);
                    if (p is IPrimitiveValueParameter primitiveValueParameter)
                    {
                        primitiveValueParameter.Value = kvp.Value;
                        _parameterStore.AddRootParameter(kvp.Key, p);
                    }
                }
            }

            _parameterStore.EnsureMethodParameters(entryPoint);

            OnExplorationStarted(new ExplorationStartedEventArgs(_config.AssemblyToCheckFileName, entryPoint, _solver?.GetType()));

            // run iteration
            while (true)
            {
                SystemConsole.OutTextWriterRef = ObjectReference.Null;

                try
                {
                    NextIterationOrThrow();

                    // setup initial state
                    ExplicitActiveState cur = new ExplicitActiveState(_config, instructionExecProvider, _definitionProvider, _logger);
                    cur.PathStore = _pathStore;

                    DataElementList arguments = _parameterStore.CreateMethodArguments(entryPoint, cur);

                    MethodState mainState = new MethodState(entryPoint, arguments, cur);

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

                    OnIterationStarted(new IterationStartedEventArgs(_currentIteration, _parameterStore));

                    explorer.InstructionExecuted += _pathStore.OnInstructionExecuted;
                    explorer.Run();
                    explorer.InstructionExecuted -= _pathStore.OnInstructionExecuted;

                    var path = _pathStore.CurrentPath;
                    PathExplored?.Invoke(path);

                    if (entryPoint.HasReturnType)
                    {
                        ReturnValue retValue = (ReturnValue)cur.CurrentThread.RetValue;
                        _parameterStore.SetReturnValue(retValue, cur);
                    }

                    OnIterationFinished(new IterationFinishedEventArgs(_currentIteration, null, path));



                    _logger.Log(LogPriority.Message, "Explored path: {0}", path.PathConstraintString);
                    IList<ParameterExpression> usedExpressions =  cur.GetExpressionLookup().Values.ToList();
                    data = PathStore.GetNextInputValues(_solver, usedExpressions);

                    if (data == null)
                    {
                        break;
                    }

                    PathStore.ResetPath();

                    _parameterStore.NextGeneration(data);
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
            _extensions.ForEach(e => e.Unregister(this));
        }

        public IConfig GetConfiguration()
        {
            return _config;
        }
    }
}
