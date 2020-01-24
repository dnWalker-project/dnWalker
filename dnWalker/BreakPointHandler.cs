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

namespace MMC {

	using System;
	using System.Collections;
    using dnlib.DotNet;
    using MMC.State;

	public interface IBreakPointHandler {

		int BreakPointCount { get; }
		bool SetBreakPoint();
		bool RemoveBreakPoint();
		bool RemoveBreakPoint(int n);
		bool IsBreakPoint();
	}

	public class BreakPointHandler : IBreakPointHandler {

		IList m_breakpoints;

		public int BreakPointCount {

			get { return m_breakpoints.Count; }
		}

		BreakPoint MakeBreakPoint()
        {
            throw new NotImplementedException();/*
            ExplicitActiveState cur = null;// ActiveState.cur;

            BreakPoint bp = new BreakPoint();
			bp.Thread = cur.ThreadPool.CurrentThreadId;
			if (cur.CurrentMethod != null) {
				bp.Offset = (int)cur.CurrentMethod.ProgramCounter.Offset;
				bp.MethodDefinition = cur.CurrentMethod.Definition;
			} else {
				bp.Offset = -1;
				bp.MethodDefinition = null;
			}
			return bp;*/
		}

        BreakPoint MakeBreakPoint(string threadId, string methodFullName, string offset)
        {
            BreakPoint bp = new BreakPoint();
            bp.MethodDefinition = null;
            try
            {
                ExplicitActiveState cur = null;
                bp.Thread = Int32.Parse(threadId);
                bp.Offset = Int32.Parse(offset);
                // Generate class and method names.
                int breakAt = methodFullName.LastIndexOf('.');
                string typeName = methodFullName.Substring(0, breakAt);
                string methodName = methodFullName.Substring(breakAt + 1);
                var declType = cur.DefinitionProvider.GetTypeDefinition(typeName);
                if (declType != null)
                {
                    bp.MethodDefinition = cur.DefinitionProvider.SearchMethod(methodName, declType);
                }
                else
                {
                    //Logger.l.Warning("type definition not found for {0}", typeName);
                    throw new NotImplementedException("Just a reminder");
                }
            }
            catch (FormatException) { }
            catch (OverflowException) { }
            catch (ArgumentOutOfRangeException) { } // illegal format for methodname.
            return bp;
        }

		internal bool SetBreakPoint(string threadId, string methodFullName, string offset) {

			BreakPoint bp = MakeBreakPoint(threadId, methodFullName, offset);
			return (bp.MethodDefinition != null) && AddBreakPoint(bp);
		}

		public bool SetBreakPoint() {

			return AddBreakPoint(MakeBreakPoint());
		}

		bool AddBreakPoint(BreakPoint bp) {

			bool found = false;
			for (int i = 0; !found && i < m_breakpoints.Count; ++i)
				found = bp.Equals(m_breakpoints[i]);
			if (!found)
				m_breakpoints.Add(bp);

			return !found;
		}

		internal bool RemoveBreakPoint(string threadId, string methodName, string offset) {

			BreakPoint bp = MakeBreakPoint(threadId, methodName, offset);
			return DeleteBreakPoint(bp);
		}

		public bool RemoveBreakPoint() {

			return DeleteBreakPoint(MakeBreakPoint());
		}

		public bool RemoveBreakPoint(int n) {

			bool retval = n >= 0 && n < m_breakpoints.Count;
			if (retval)
				m_breakpoints.RemoveAt(n);
			return retval;
		}

		bool DeleteBreakPoint(BreakPoint bp) {

			int i;
			bool found = false;
			for (i = 0; !found && i < m_breakpoints.Count; ++i)
				found = m_breakpoints[i].Equals(bp);
			if (found)
				m_breakpoints.RemoveAt(i - 1);

			return found;
		}

		public bool IsBreakPoint() {

			bool retval = false;
			BreakPoint bp;

			bp = MakeBreakPoint();
			for (int i = 0; !retval && i < m_breakpoints.Count; ++i)
				retval = bp.Equals(m_breakpoints[i]);

			return retval;
		}

		public override string ToString() {

			BreakPoint bp;

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (m_breakpoints.Count == 0)
				sb.Append("No breakpoints set.");
			else {
				sb.Append("Breakpoints:\n");
				for (int i = 0; i < m_breakpoints.Count; ++i) {
					bp = (BreakPoint)m_breakpoints[i];
					sb.AppendFormat("   {0}: {1}", i, bp.ToString());
				}
			}

			return sb.ToString();
		}

		public BreakPointHandler() {

			m_breakpoints = new ArrayList();
		}

		struct BreakPoint {

			public int Thread;
			public int Offset;
			public MethodDef MethodDefinition;

			public override string ToString() {

				return string.Format("thread {1} at {2}.{3}: {4}\n",
						Thread,
						MethodDefinition.DeclaringType.FullName,
						MethodDefinition.Name,
						Offset);
			}

			public override bool Equals(object other) {

				BreakPoint o = (BreakPoint)other;
				return o.Thread == Thread &&
				   o.Offset == Offset &&
				   o.MethodDefinition == MethodDefinition;
			}


			public override int GetHashCode() {

				return Thread * (Offset ^ 13) *
					(MethodDefinition.GetHashCode() ^ 101);
			}
		}
	}
}
