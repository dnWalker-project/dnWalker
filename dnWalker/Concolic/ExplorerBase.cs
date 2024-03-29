﻿using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Input;
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

        public ExplorerBase(IDefinitionProvider definitionProvider, IConfiguration config, Logger logger, ISolver solver)
        {
            _definitionProvider = definitionProvider;
            _config = config;
            _logger = logger;
            _solver = solver;

            ExtendableInstructionFactory f = new ExtendableInstructionFactory().AddExtensionsFrom(config);
            _instructionExecProvider = InstructionExecProvider.Get(config, f);
        }


        public ExplorationResult Run(MethodDef entryPoint, IExplorationStrategy strategy, IEnumerable<Constraint> constraints = null)
        {
            ControlFlowGraphProvider _cfgProvider = new ControlFlowGraphProvider();
            PathStore pathStore = new PathStore(entryPoint, _cfgProvider);

            try
            {
                ExplicitActiveState cur = new ExplicitActiveState(_config, _instructionExecProvider, _definitionProvider, _logger) 
                {
                    PathStore = pathStore 
                };
                cur.Services.RegisterService(_solver);

                strategy.Initialize(cur, entryPoint, _config);

                OnExplorationStarted(new ExplorationStartedEventArgs(_config.AssemblyToCheckFileName(), entryPoint, _solver?.GetType()));
                ExplorationResult result = RunCore(entryPoint, pathStore, cur, strategy, constraints);
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

        protected abstract ExplorationResult RunCore(MethodDef entryPoint, PathStore pathStore, ExplicitActiveState cur, IExplorationStrategy strategy, IEnumerable<Constraint> constraints = null);

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

        public IConfiguration Configuration => _config;


        #region IDisposable
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {

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
