using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMC;
using MMC.Collections;
using MMC.State;

namespace dnWalker.ChoiceGenerators
{
    public interface IScheduler { }

    public class SchedulingChoiceGenerator : IChoiceGenerator, IScheduler
    {
        private readonly ExplicitActiveState cur;
        private Collapser m_stateConvertor;
        private readonly Stack<SchedulingData> m_dfs;
        private readonly BacktrackEventHandler backtrackStart;
        private readonly BacktrackEventHandler backtracked;
        private readonly BacktrackEventHandler backtrackStop;
        private readonly PickThreadEventHandle threadPicked;
        private readonly StateEventHandler stateConstructed;
        private readonly StateEventHandler _stateRevisited;
        private readonly Queue<int> m_emptyQueue = new Queue<int>(0);
        private readonly IStatistics _statistics;
        private readonly FastHashtable<CollapsedState, int> m_stateStorage;

        public bool HasMoreChoices => throw new NotImplementedException();

        internal SchedulingChoiceGenerator(
            Explorer explorer,
            IStatistics statistics,
            FastHashtable<CollapsedState, int> stateStorage,
            Collapser stateConvertor,
            StateEventHandler stateRevisited,
            BacktrackEventHandler backtrackStart,
            BacktrackEventHandler backtracked,
            BacktrackEventHandler backtrackStop,
            PickThreadEventHandle threadPicked,
            StateEventHandler stateConstructed)
        {
            this.backtrackStart = backtrackStart;
            this.backtracked = backtracked;
            this.backtrackStop = backtrackStop;
            this.threadPicked = threadPicked;
            this.stateConstructed = stateConstructed;
            _stateRevisited = stateRevisited;
            cur = explorer.cur;
            m_stateStorage = stateStorage;
            m_stateConvertor = stateConvertor;
            _statistics = statistics;

            // DFS stack
            m_dfs = new Stack<SchedulingData>();            
        }

        public object GetNextChoice()
        {
            /* Do a state matching, store if unmatched,
			 * if matched, a backtracking is initiated by
			 * returning an empty working queue 
			 */
            SchedulingData sd = UpdateHashtable(cur);

            backtrackStart(m_dfs, sd, cur);

            while (sd.Working.Count == 0 && m_dfs.Count > 0)
            {
                // apply the reverse delta
                m_stateConvertor.DecollapseByDelta(sd.Delta);
                backtracked(m_dfs, sd, cur);
                sd = m_dfs.Pop();
            }
            
            backtrackStop(m_dfs, sd, cur);

            m_stateConvertor.Reset(sd.State);
            cur.Clean();

            var threadId = -1;

            /*
		     * either the recently explored sd can be pushed on the stack, 
			 * or the last popped sd from the stack is not fully explored */
            if (sd.Working.Count > 0)
            {
                m_dfs.Push(sd);
                threadId = SelectRunnableThread(sd); // for the next round		

                // update last access information 
                // (used by dynamic POR + tracing explorer) 
                MemoryLocation ml = cur.NextAccess(threadId);
                sd.LastAccess = new MemoryAccess(ml, threadId);

                threadPicked(sd, threadId);
            }

            _statistics.BacktrackStackDepth(m_dfs.Count);

            return threadId;
        }

        protected virtual int SelectRunnableThread(SchedulingData sd)
        {
            return sd.Dequeue();
        }

        private SchedulingData UpdateHashtable(ExplicitActiveState cur)
        {
            var collapsedCurrent = m_stateConvertor.CollapseCurrentState();

            var sd = new SchedulingData
            {
                Delta = collapsedCurrent.GetDelta()
            };

            int id = m_stateStorage.Count + 1;
            bool seenState = m_stateStorage.FindOrAdd(ref collapsedCurrent, ref id);

            /*
			 * Note: the collapsedCurrent stored in the hashtable can be
			 * different from the current collapsedCurrent, although they
			 * represent the same collapsedState. This is due to the 
			 * changingintvector, which also contains information about
			 * the reversed delta, which may be different for two 
			 * representative collapsed states */
            sd.ID = id;
            sd.State = collapsedCurrent;

            if (seenState)
            {
                // state is not new
                sd.Working = m_emptyQueue;
                _stateRevisited(collapsedCurrent, sd, cur);
            }
            else
            {
                // state is new
                collapsedCurrent.ClearDelta(); // delta's do not need to be stored				
                sd.Enabled = cur.ThreadPool.RunnableThreads;

                if (sd.Enabled.Count > 0)
                {
                    sd.Working = sd.Enabled;
                    sd.Done = new Queue<int>();
                }
                else
                {
                    sd.Working = m_emptyQueue;
                    sd.Done = m_emptyQueue;
                }

                stateConstructed(collapsedCurrent, sd, cur);
            }

            return sd;
        }

        /// <summary>
        /// The error trace consists of thread id's on the current DFS stack. Used by the TracingExplorer
        /// </summary>
        public Stack<int> GetErrorTrace()
        {
            Stack<int> retval = new Stack<int>();
            foreach (SchedulingData sd in m_dfs)
            {
                retval.Push(sd.LastAccess.ThreadId);
            }

            return retval;
        }
    }
}
