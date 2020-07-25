using MMC.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
