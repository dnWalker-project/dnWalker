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
        private readonly Stack<SchedulingData> m_dfs;
        private readonly BacktrackEventHandler backtrackStart;
        private readonly BacktrackEventHandler backtracked;
        private readonly BacktrackEventHandler backtrackStop;
        private readonly PickThreadEventHandle threadPicked;
        private readonly Explorer explorer;
        private readonly IStatistics _statistics;
        private readonly StateStorage m_stateStorage;
        private readonly ObjectEscapePOR m_spor;

        public bool HasMoreChoices => throw new NotImplementedException();

        internal SchedulingChoiceGenerator(
            Explorer explorer,
            IStatistics statistics,
            StateStorage stateStorage,
            ObjectEscapePOR m_spor,
            BacktrackEventHandler backtrackStart,
            BacktrackEventHandler backtracked,
            BacktrackEventHandler backtrackStop,
            PickThreadEventHandle threadPicked)
        {
            this.backtrackStart = backtrackStart;
            this.backtracked = backtracked;
            this.backtrackStop = backtrackStop;
            this.threadPicked = threadPicked;
            cur = explorer.cur;
            this.explorer = explorer;
            _statistics = statistics;
            m_stateStorage = stateStorage;
            this.m_spor = m_spor;

            // DFS stack
            m_dfs = new Stack<SchedulingData>();            
        }

        public object GetNextChoice()
        {
            var threadId = m_spor.GetPersistentThread(explorer);
            if (threadId >= 0)
            {
                return threadId;
            }

            var m_stateConvertor = cur.StateCollapser;

            // Run garbage collection
            cur.GarbageCollector.Run(cur);

            /* Do a state matching, store if unmatched,
			 * if matched, a backtracking is initiated by
			 * returning an empty working queue 
			 */
            SchedulingData sd = cur.Collapse(m_stateStorage);

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
