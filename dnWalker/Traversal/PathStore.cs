using dnlib.DotNet.Emit;
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

        public PathStore()
        {
            _currentPath = new Path();
            _paths.Add(_currentPath);
        }

        public Path CurrentPath => _currentPath;

        public void BacktrackStart(Stack<SchedulingData> stack, SchedulingData fromSD, ExplicitActiveState cur)
        {
        }

        public void StateConstructed(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            _currentPath.Extend(sd.ID);
        }

        public virtual void AddPathConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            _currentPath.AddPathConstraint(expression, next, cur);
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

        public virtual Expression GetNextPathConstraint(Path path)
        {
            var pc = path.PathConstraints.Select(p => p.Expression).Take(path.PathConstraints.Count - 1).ToList();
            pc.Add(Expression.Not(path.PathConstraints.Last().Expression));
            return pc.Aggregate((a, b) => Expression.And(a, b));
        }

        public IReadOnlyList<Path> Paths => new ReadOnlyCollection<Path>(_paths);

        public void NewThreadSpawned(ThreadState threadState)
        {
            threadState.ThreadStateChanged += ThreadState_ThreadStateChanged;
            threadState.CallStackEmptied += ThreadState_CallStackEmptied;
        }

        private void ThreadState_CallStackEmptied(ThreadState threadState)
        {
            _currentPath.Terminate(threadState);
        }

        private void ThreadState_ThreadStateChanged(ThreadState threadState, System.Threading.ThreadState state)
        {            
        }
    }
}
