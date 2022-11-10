using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;

using MMC.Data;
using MMC.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Traversal
{
    public class PathStore
    {
        private Path _currentPath;
        private IList<Path> _paths = new List<Path>();

        private readonly ICache<MethodDef, MethodTracer> _methodTracers;
        private readonly MethodDef _entryPoint;
        private readonly ICache<MethodDef, ControlFlowGraph> _cfgProvider;

        public PathStore(MethodDef entryPoint, ICache<MethodDef, ControlFlowGraph> cfgProvider)
        {
            _entryPoint = entryPoint ?? throw new ArgumentNullException(nameof(entryPoint));
            _cfgProvider = cfgProvider;
            _methodTracers = new DelegateCache<MethodDef, MethodTracer>(method => new MethodTracer(method, _cfgProvider.Get(method)));
            _currentPath = CreatePath();
            _paths.Add(_currentPath);
        }

        public Path CurrentPath => _currentPath;

        protected virtual Path CreatePath()
        {
            return new Path(_methodTracers);
        }

        public void ResetPath()
        {
            _currentPath = CreatePath();
            _paths.Add(_currentPath);
        }

        public Path ResetPath(bool checkTermination)
        {
            if (!checkTermination || _currentPath.IsTerminated) ResetPath();
            return _currentPath;
        }

        public void BacktrackStart(Stack<SchedulingData> stack, SchedulingData fromSD, ExplicitActiveState cur)
        {
        }

        public void StateConstructed(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            _currentPath.Extend(sd.ID);
        }

        public void BacktrackStop(Stack<SchedulingData> stack, SchedulingData sd, ExplicitActiveState cur)
        {
            var path = _currentPath.BacktrackTo(sd.ID);
            if (path == null)
            {
                return;
            }

            _currentPath = path;
            _paths.Add(_currentPath);
        }

        public IReadOnlyList<Path> Paths => new ReadOnlyCollection<Path>(_paths);

        public ICache<MethodDef, ControlFlowGraph> ControlFlowGraphProvider => _cfgProvider;

        public ICache<MethodDef, MethodTracer> MethodTracerProvider => _methodTracers;

        public MethodDef EntryPoint
        {
            get
            {
                return _entryPoint;
            }
        }

        public Coverage GetCoverage() => _methodTracers.Get(_entryPoint).GetCoverage();

        public void NewThreadSpawned(ThreadState threadState)
        {
            threadState.ThreadStateChanged += ThreadState_ThreadStateChanged;
            threadState.CallStackEmptied += ThreadState_CallStackEmptied;
        }

        private void ThreadState_CallStackEmptied(ThreadState threadState)
        {
            TerminatePath(threadState);
        }

        protected virtual void TerminatePath(ThreadState threadState)
        {
            _currentPath.Terminate(threadState);
        }

        private void ThreadState_ThreadStateChanged(ThreadState threadState, System.Threading.ThreadState state)
        {            
        }
    }
}
