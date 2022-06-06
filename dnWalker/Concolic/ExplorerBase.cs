using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Instructions;
using dnWalker.Instructions.Extensions;
using dnWalker.Symbolic;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public abstract class ExplorerBase : IExplorer, IDisposable
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly IConfiguration _config;
        private readonly List<IExplorationExtension> _extensions = new List<IExplorationExtension>();
        private readonly Logger _logger;
        private readonly ISolver _solver;

        //private IMethod _entryPoint;

        private readonly IInstructionExecProvider _instructionExecProvider;

        //private PathStore _pathStore;
        //private int _currentIteration;

        //private ControlFlowGraphProvider _cfgProvider = new ControlFlowGraphProvider();

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
        protected virtual void OnExplorationFinished(ExplorationFinishedEventArgs e)
        {
            ExplorationFinished(this, e);
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

        protected IInstructionExecProvider ExecutorProvider => _instructionExecProvider;

        //protected ExplicitActiveState CreateActiveState()
        //{
        //    ExplicitActiveState state = new ExplicitActiveState(_config, _instructionExecProvider, _definitionProvider, _logger);
        //    state.PathStore = _pathStore;
        //    return state;
        //}

        public ExplorerBase(IDefinitionProvider definitionProvider, IConfiguration config, Logger logger, ISolver solver)
        {
            _definitionProvider = definitionProvider;
            _config = config;
            _logger = logger;
            _solver = solver;

            ExtendableInstructionFactory f = new ExtendableInstructionFactory().AddStandardExtensions();
            _instructionExecProvider = InstructionExecProvider.Get(config, f);
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

        public IReadOnlyCollection<IExplorationExtension> Extensions { get { return _extensions; } }


        //public IMethod EntryPoint => _entryPoint;

        public ExplorationResult Run(string methodName, IDictionary<string, object> data = null)
        {
            MethodDef entryPoint = _definitionProvider.GetMethodDefinition(methodName) ?? throw new NullReferenceException($"Method {methodName} not found");
            ControlFlowGraphProvider _cfgProvider = new ControlFlowGraphProvider();
            PathStore pathStore = new PathStore(entryPoint, _cfgProvider);

            try
            {
                ExplicitActiveState cur = new ExplicitActiveState(_config, _instructionExecProvider, _definitionProvider, _logger) 
                {
                    PathStore = pathStore 
                };

                OnExplorationStarted(new ExplorationStartedEventArgs(_config.AssemblyToCheckFileName(), entryPoint, _solver?.GetType()));
                ExplorationResult result = RunCore(entryPoint, pathStore, cur, data);
                OnExplorationFinished(new ExplorationFinishedEventArgs(result));
                return result;
            }
            catch (Exception e)
            {
                _logger.Log(LogPriority.Fatal, e.Message);
                OnExplorationFailed(new ExplorationFailedEventArgs(e));
                throw;
            }
        }

        protected abstract ExplorationResult RunCore(MethodDef entryPoint, PathStore pathStore, ExplicitActiveState cur, IDictionary<string, object> data = null);


        //public PathStore PathStore
        //{
         
        //    get { return _pathStore; }
        //}

        //public int IterationCount
        //{
        //    get { return _currentIteration; }
        //}
        //public int MaxIterations { get; set; }

        public ISolver Solver
        {
            get
            {
                return _solver;
            }
        }

        public Logger Logger
        {
            get
            {
                return _logger;
            }
        }

        public IDefinitionProvider DefinitionProvider
        {
            get
            {
                return _definitionProvider;
            }
        }

        //public ControlFlowGraphProvider ControlFlowGraphProvider
        //{
        //    get
        //    {
        //        return _cfgProvider;
        //    }
        //}

        //protected void ResetIterations()
        //{
        //    _currentIteration = 0;
        //}
        //protected void NextIterationOrThrow()
        //{
        //    int maxIterations = MaxIterations;
        //    if (maxIterations > 0 && _currentIteration >= maxIterations)
        //    {
        //        throw new MaxIterationsExceededException(_currentIteration);
        //    }
        //    ++_currentIteration;
        //}

        public IConfiguration Configuration() => _config;


        #region IDisposable
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (IExplorationExtension extension in Extensions)
                    {
                        extension.Unregister(this);
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable
    }
}
