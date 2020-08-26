using MMC;
using MMC.State;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.ChoiceGenerators
{
    public class DefaultChoiceStrategy : IChoiceStrategy
    {
        private IChoiceGenerator _choiceGenerator, _nextChoiceGenerator;
        private readonly ExplicitActiveState _activeState;
        private readonly BacktrackEventHandler backtrackStart;
        private readonly BacktrackEventHandler backtracked;
        private readonly BacktrackEventHandler backtrackStop;

        public IChoiceGenerator ChoiceGenerator => _choiceGenerator;

        public DefaultChoiceStrategy(
            ExplicitActiveState activeState, 
            BacktrackEventHandler backtrackStart, 
            BacktrackEventHandler backtracked,
            BacktrackEventHandler backtrackStop) 
        {
            _activeState = activeState;
            this.backtrackStart = backtrackStart;
            this.backtracked = backtracked;
            this.backtrackStop = backtrackStop;
        }

        public bool HasOptions => _nextChoiceGenerator != null
                || ChoiceGenerator?.Previous != null
                || ChoiceGenerator?.HasMoreChoices == true;

        public IChoiceGenerator Back()
        {
            _choiceGenerator = ChoiceGenerator.Previous;
            return _choiceGenerator;
        }

        public void Next(IChoiceGenerator choiceGenerator)
        {
            choiceGenerator.SetContext(_activeState);

            //_activeState.ChoiceGeneratorCreated?.Invoke(choiceGenerator);

            _nextChoiceGenerator = choiceGenerator;
        }

        public void DoBacktrack(out bool @continue)
        {
            @continue = true;
            if (_choiceGenerator is IScheduler)
            {
                return;
            }

            var cur = _activeState;
            var m_dfs = new Stack<SchedulingData>();
            var hasMoreChoices = _choiceGenerator.HasMoreChoices;

            if (!hasMoreChoices)
            {
                m_dfs.Push(_choiceGenerator.GetBacktrackData());
                _choiceGenerator = Back();// _choiceGenerator.Previous;
                if (!_choiceGenerator.HasMoreChoices)
                {
                    _choiceGenerator = Back(); //TODO
                }
            }
            else
            {
                Next(); // NOTE what if I switch to prev. CG and _next is not null

                /*if (cur.ChoiceGenerator == null)
                {
                    return;
                }
                */
                if (cur.CurrentMethod != null)
                {
                    return;
                }
            }

            var sd = _choiceGenerator.GetBacktrackData();
            if (sd == null)
            {
                @continue = false;
                return;
            }

            System.Diagnostics.Debug.WriteLine("Backtracking ...");

            m_dfs.Push(sd);

            var m_stateConvertor = cur.StateCollapser;

            // Run garbage collection
            cur.GarbageCollector.Run(cur);

            var l = m_dfs.Reverse().ToList();
            backtrackStart?.Invoke(m_dfs, sd, cur);

            while (l.Count > 0)
            {
                sd = l.First();// m_dfs.Last();
                // apply the reverse delta
                System.Diagnostics.Debug.WriteLine("... to " + sd.ID);
                m_stateConvertor.DecollapseByDelta(cur, sd.Delta);
                backtracked?.Invoke(m_dfs, sd, cur);
                //sd = m_dfs.Pop();
                l.RemoveAt(0);
            }

            backtrackStop?.Invoke(m_dfs, sd, cur);

            m_stateConvertor.Reset(sd.State);
            cur.Clean();

            var threadId = -1;
            /*
		     * either the recently explored sd can be pushed on the stack, 
			 * or the last popped sd from the stack is not fully explored */
            if (sd.Working.Count > 0)
            {
                m_dfs.Push(sd);
                threadId = sd.Dequeue(); // for the next round		

                // update last access information 
                // (used by dynamic POR + tracing explorer) 
                //var ml = cur.NextAccess(threadId);
                //sd.LastAccess = new MemoryAccess(ml, threadId);

                //threadPicked(sd, threadId);
            }

            //_statistics.BacktrackStackDepth(m_dfs.Count);
        }

        public void Next()
        {
            if (_nextChoiceGenerator == null)
            {
                return;
            }

            _choiceGenerator = _nextChoiceGenerator;
            _nextChoiceGenerator = null;
        }

        public bool CanBreak()
        {
            return _nextChoiceGenerator != null;
        }

        private IList<IChoiceGenerator> _choiceGenerators = new List<IChoiceGenerator>();

        public void RegisterChoiceGenerator(IChoiceGenerator choiceGenerator)
        {
            _choiceGenerators.Add(choiceGenerator);

            choiceGenerator.SetContext(_activeState);
            choiceGenerator.Previous = _choiceGenerator;
            _choiceGenerator = choiceGenerator;
        }

        public ThreadState GetExecutingThread()
        {
            if (_choiceGenerator is IScheduler)
            {
                if (!_choiceGenerators.Any(c => c.HasMoreChoices))
                {
                    return null;
                }

                var threadId = (int)_choiceGenerator.GetNextChoice();
                if (threadId < 0)
                {
                    return null;
                }

                return _activeState.ThreadPool.Threads[threadId];
            }
            else
            {
                return _activeState.CurrentThread; // no thread switching
            }
        }
    }
}
