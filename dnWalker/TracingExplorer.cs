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
    using System.Text;
    using System.Collections.Generic;
    using System.IO;
    using MMC.State;
    using MMC.Util;
    using dnlib.DotNet.Emit;
    using dnWalker.Traversal;
    using dnWalker.Configuration;
    using dnWalker;

    /// <summary>
    /// This is the error tracer
    /// </summary>
    public class TracingExplorer : Explorer
    {
        private readonly Stack<int> m_tracingQueue;
        private string prevMethod = "";
        private readonly TextWriter tw;

        public TracingExplorer(ExplicitActiveState cur, IStatistics statistics, Stack<int> tracingQueue, TextWriter tw, Logger logger, IConfiguration config, PathStore pathStore)
            : base(cur, statistics, logger, config, pathStore)
        {
            m_tracingQueue = tracingQueue;
            // disable stats
            config.SetShowStatistics(false);
            this.tw = tw;
        }

        /*protected override int SelectRunnableThread(SchedulingData sd)
        {
            return m_tracingQueue.Pop();
        }*/

        /*
		 * This method is not really pretty, but it works...
		 */
        public override void PrintTransition()
        {
            var currentThread = cur.ThreadPool.CurrentThreadId;
            var currentMethod = cur.CurrentMethod;
            var instr = currentMethod.ProgramCounter;
            var isRet = instr.OpCode.Code == Code.Ret;
            var operandString = instr.Operand == null ?
                "" :
                CILElementPrinter.FormatCILElement(instr.Operand);

            var callstackSize = cur.CallStack.StackPointer - 1;

            var sameMethod = currentMethod.Definition.ToString().Equals(prevMethod);
            prevMethod = currentMethod.Definition.ToString();

            var sb = new StringBuilder();
            sb.AppendFormat("- thread: {0} ", currentThread);
            sb.Append(' ', callstackSize * 4);

            var preString = sb.ToString();

            if (!sameMethod && instr.Offset > 0)
            {
                sb.Append("|_ ");
                sb.AppendFormat(prevMethod);
                tw.WriteLine(sb.ToString());
            }

            sb = new StringBuilder();
            sb.Append(preString);

            if (instr.Offset == 0)
                sb.Append("|_ ");
            else
                sb.Append("   ");

            sb.AppendFormat("{0:D4} {1} {2} on stack {3}",
                            instr.Offset,
                            instr.OpCode.Name,
                            operandString,
                            currentMethod.EvalStack.ToString()
                );

            tw.WriteLine(sb.ToString());

            if (isRet && callstackSize > 0)
            {
                sb = new StringBuilder();
                sb.AppendFormat("- thread: {0} ", currentThread);
                sb.Append(' ', (callstackSize - 1) * 4);
                sb.Append(" ____________|");
                tw.WriteLine(sb.ToString());
            }
        }
    }
}
