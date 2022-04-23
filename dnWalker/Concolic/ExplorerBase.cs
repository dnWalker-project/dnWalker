using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Parameters;
using dnWalker.TypeSystem;

using MMC;

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
        private readonly Config _config;
        private readonly List<IExplorationExtension> _extensions = new List<IExplorationExtension>();
        private readonly Logger _logger;
        private readonly ISolver _solver;

        private PathStore _pathStore;
        private int _currentIteration;

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



        public ExplorerBase(IDefinitionProvider definitionProvider, Config config, Logger logger, ISolver solver)
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

        public IReadOnlyCollection<IExplorationExtension> Extensions { get { return _extensions; } }

        public void Run(string methodName, IDictionary<string, object> data = null)
        {
            MethodDef entryPoint = _definitionProvider.GetMethodDefinition(methodName) ?? throw new NullReferenceException($"Method {methodName} not found");
            _pathStore = CreatePathStore(entryPoint);

            try
            {
                OnExplorationStarted(new ExplorationStartedEventArgs(_config.AssemblyToCheckFileName, entryPoint, _solver?.GetType()));
                RunCore(entryPoint, data);
                OnExplorationFinished(new ExplorationFinishedEventArgs());
            }
            catch (Exception e)
            {
                _logger.Log(LogPriority.Fatal, e.Message);
                OnExplorationFailed(new ExplorationFailedEventArgs(e));
            }
        }

        protected abstract void RunCore(MethodDef entryPoint, IDictionary<string, object> data = null)


        public PathStore PathStore
        {
         
            get { return _pathStore; }
        }
        protected virtual PathStore CreatePathStore(MethodDef entryPoint)
        {
            return new PathStore(entryPoint);
        }

        [Obsolete("Not using the parameter store anymore, switching to IModel & IHeapInfo")]
        public ParameterStore ParameterStore
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int IterationCount
        {
            get { return _currentIteration; }
        }
        public int MaxIterations { get; set; }

        protected void ResetIterations()
        {
            _currentIteration = 0;
        }
        protected void NextIterationOrThrow()
        {
            int maxIterations = MaxIterations;
            if (maxIterations > 0 && _currentIteration >= maxIterations)
            {
                throw new MaxIterationsExceededException(_currentIteration);
            }
            ++_currentIteration;
        }

        public IConfig GetConfiguration() => _config;


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
