/*
 *   Copyright 2007 University of Twente, Formal Methods and Tools group
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *       http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 */

namespace MMC
{

    using System.Collections.Generic;
    using System.Diagnostics;
    using MMC.InstructionExec;
    using MMC.State;

    public class ExplorationLogger
    {
        const int newstate_progress_delta = 25000; // states
        const int revisit_progress_delta = 25000; // times
        const int progressIndicatorDelta = 250;

        /// ID of the last visited (or popped) state: for the graph.
        int m_lastState = -1; // some illegal value.
        int m_lastRunThread = 0; // first thread (main) will have ID 0.

        IStatistics Statistics { get; }

        Stack<SchedulingData> m_stack;

        // --------------------------------- Exploration --------------------------------

        public void LogNewState(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            m_explorer.Logger.Debug("found new state: {0}", sd.ID);
            Statistics.NewState();
        }

        public void LogRevisitState(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            m_explorer.Logger.Debug("re-visit of state: {0}", sd.ID);
            Statistics.RevisitState();
        }

        // --------------------------------- Scheduling ---------------------------------

        public void LogDeadlock(SchedulingData sd)
        {
            m_explorer.Logger.Log(LogPriority.Severe, "DEADLOCK!");
        }

        public void LogPickedThread(SchedulingData sd, int chosen)
        {
            m_explorer.Logger.Debug("to be run next: {0}", chosen);
            m_lastRunThread = chosen;
        }

        // -------------------------------- Backtracking --------------------------------

        public void LogBacktrackDummy(Stack<SchedulingData> stack, SchedulingData fromSd, ExplicitActiveState cur)
        {
        }

        public void LogBacktrack(Stack<SchedulingData> stack, SchedulingData fromSd, ExplicitActiveState cur)
        {
            m_explorer.Logger.Debug("backtracked.");
            Statistics.Backtrack();
        }

        // -------------------------------- Exploration Halt ----------------------------

        public void ExplorationHalted(IIEReturnValue ier)
        {
            m_explorer.Logger.Log(LogPriority.Severe, "exploraton halted: {0}", ier.ToString());
        }

        // -------------------------------- Graph Writing -------------------------------

        public void GraphNewState(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            DotWriter.NewNode(sd.ID, sd.Enabled.ToArray());
            DotWriter.NewEdge(m_lastState, sd.ID, m_lastRunThread);
            m_lastState = sd.ID;
        }

        public void GraphRevisitState(CollapsedState collapsed, SchedulingData sd, ExplicitActiveState cur)
        {
            DotWriter.NewEdge(m_lastState, sd.ID, m_lastRunThread);
            m_lastState = sd.ID;
        }

        public void GraphBacktrack(Stack<SchedulingData> stack, SchedulingData fromSd, ExplicitActiveState cur)
        {
            DotWriter.NewEdge(m_lastState, stack.Peek().ID, DotGraph.BacktrackEdge);
            m_lastState = stack.Peek().ID;
        }

        private readonly System.Timers.Timer timer;

        private readonly Explorer m_explorer;

        public ExplorationLogger(IStatistics statistics, Explorer ex)
        {
            Statistics = statistics;

            if (!ex.Config.Quiet)
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimedEvent;
                timer.Interval = 5 * 1000;
                timer.Enabled = true;
            }

            m_explorer = ex;
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            m_explorer.Logger.Message("NewStates={0}, Revisits={1}, CurrDFSCount={2}", Statistics.StateCount, Statistics.RevisitCount, m_explorer.GetDFSStackSize());
        }
    }
}
