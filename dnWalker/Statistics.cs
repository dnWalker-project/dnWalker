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
    public interface IReadOnlyStatistics
    {
        int StateCount { get; }
        int RevisitCount { get; }
        int Deadlocks { get; }
        int AssertionViolations { get; }
        int AssertionPasses { get; }
    }

    public interface IStatistics : IReadOnlyStatistics
    {

        void Start();
        void NewState();
        void RevisitState();
        void Backtrack();
        void AssertionViolation();
        void AssertionPass();
        void Deadlock();
        void MaxHashtableSize(int count);
        void MaxHeapArray(int length);
        void BacktrackStackDepth(int count);
        void MeasureMemory(long memUsed);
    }



    public class SimpleStatistics : IStatistics
    {
        System.DateTime m_start;
        System.TimeSpan m_elapsed = new System.TimeSpan(0, 0, 0, 0, 0);
        int m_hashtableCount;
        int m_backtrackCount;
        int m_maxBacktrackStack;
        long m_maxMemUsage = 0;        
        int m_assertionViolations;
        int m_assertionPasses;
        int m_maxAllocArray;

        public int Deadlocks { get; private set; }

        public void Deadlock()
        {
            Deadlocks++;
        }

        public void AssertionViolation()
        {
            m_assertionViolations++;
        }
        public void AssertionPass()
        {
            m_assertionPasses++;
        }

        public System.TimeSpan TotalElapsed
        {
            get { return m_elapsed; }
        }

        public int StateCount { get; private set; }

        public int RevisitCount { get; private set; }

        public int AssertionViolations
        {
            get
            {
                return m_assertionViolations;
            }
        }
        public int AssertionPasses
        {
            get
            {
                return m_assertionPasses;
            }
        }

        public void Start()
        {
            m_start = System.DateTime.Now;
        }

        public void Stop()
        {
            m_elapsed += System.DateTime.Now - m_start;
        }

        public void NewState()
        {
            StateCount++;
        }

        public void RevisitState()
        {
            RevisitCount++;
        }

        public void Backtrack()
        {
            m_backtrackCount++;
        }

        public void MaxHeapArray(int length)
        {
            m_maxAllocArray = System.Math.Max(m_maxAllocArray, length);
        }

        public void MaxHashtableSize(int count)
        {
            m_hashtableCount = System.Math.Max(count, m_hashtableCount);
        }

        public void MeasureMemory(long use)
        {
            m_maxMemUsage = System.Math.Max(m_maxMemUsage, use);
        }

        public void BacktrackStackDepth(int depth)
        {
            m_maxBacktrackStack = System.Math.Max(depth, m_maxBacktrackStack);
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            var usedAfterGC = System.GC.GetTotalMemory(true) / 1024; // in Kb

            sb.Append("\n--------------------------------------\n");
            sb.AppendFormat("Time                  : {0} sec\n", m_elapsed.TotalSeconds);
            sb.AppendFormat("States                : {0}\n", StateCount);
            sb.AppendFormat("Revisits              : {0}\n", RevisitCount);
            sb.AppendFormat("Backtracks            : {0}\n", m_backtrackCount);
            sb.AppendFormat("Max. DFS stack        : {0}\n", m_maxBacktrackStack);
            sb.AppendFormat("Max. heap array len.  : {0}\n", m_maxAllocArray);
            sb.AppendFormat("Max. stored states    : {0}\n", m_hashtableCount);
            sb.AppendFormat("Max. mem. used        : {0} Kb\n", m_maxMemUsage / 1024);
            sb.AppendFormat("Current. mem. use     : {0} Kb\n", usedAfterGC);
            sb.AppendFormat("Deadlocks             : {0}\n", Deadlocks);
            sb.AppendFormat("Assertion violations  : {0}\n", m_assertionViolations);
            sb.Append("--------------------------------------\n");

            return sb.ToString();
        }
    }
}