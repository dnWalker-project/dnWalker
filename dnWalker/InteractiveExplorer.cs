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
	
	using MMC.State;
	using MMC.Data;
	using System.Text;

	/// Constants for message passing. No enum because of casting fuss and no constant folding.
	struct InteractionMessages {

		public const int IllegalInput    = -1;
		public const int LegalInput      = -2;
		public const int FirstRunnable   = -10;
		public const int ListThreads     = -11;
	}

	/// <summary>Explorer which allows the user to interactively guide the exploration.</summary>
	///
	/// The interactive explorer is split up into an abstract base class
	/// (InteractiveExplorer) and a InteractiveExplorerCLI class. The class
	/// contains relatively clean stuff, like handlers for the user commands.
	/// The latter deals with user input parsing. This is done for two reasons:
	///
	/// <ol>
	///     <li> Parsing without the proper tools is messy code, and we'd like to
	///          keep things reabale without using code and such. </li>
	///     <li> Separation of concerns. One might (who knows!) derive another
	///          class from the InteractiveExplorer class, and implement another
	///          user interface. </li>
	/// </ol>
	
	
	//abstract class InteractiveExplorer : NewExplorer {
	
	//    // TODO: sanitize the working print methods 
	//    protected IBreakPointHandler m_bph;
	//    protected bool m_breakPointsOnly;

	//    protected abstract void InteractiveOuput(string str);
	//    protected abstract int GetInput();

		
	//    protected void PrintRoots() {
	//        //InteractiveOuput(ThreadObjectWatcher.AsString());
	//    }

	//    protected void PrintParents() {
	//        /* TODO
	//        for (int i = 0; i < ActiveState.cur.DynamicArea.Allocations.Length; i++) {
	//            IDynamicAllocation child = ActiveState.cur.DynamicArea.Allocations[i];
	//            if (child != null) {
	//                foreach (ObjectReference parent in child.Parents) {
	//                    this.InteractiveOuput(parent.ToString() + " -> Alloc(" + (i+1) + ")"); 
	//                }
	//            }
	//        } */
	//    }

	//    protected void DoIncrementalHeapAnalysis() {
	//        /*
	//        IncrementalHeapVisitor.ihv.Run();

	//        StringBuilder sb = new StringBuilder();
	//        for (int i = 0; i < ActiveState.cur.DynamicArea.Allocations.Length; i++) {
	//            IDynamicAllocation child = ActiveState.cur.DynamicArea.Allocations[i];
	//            sb.AppendFormat("Alloc({0}).Depth = {1}\n", i+1, child.Depth);
	//        }

	//        InteractiveOuput(sb.ToString());

	//        ThreadObjectWatcher.Clear();
	//         */
	//    }
		
	//    protected void PrintState() {

	//        InteractiveOuput( ActiveState.cur.ToString() );
	//    }

	//    protected void PrintBacktrack() {

	//    }

	//    protected void BacktrackNow() {

	//    }
		
	//    protected void PrintObject(int offset) {
	//        if ((offset < 0) ||  (offset > ActiveState.cur.DynamicArea.Allocations.Length))
	//            this.InteractiveOuput("Heap offset is outside heap range");
	//        else {
	//            IDynamicAllocation o = ActiveState.cur.DynamicArea.Allocations[offset];
	//            this.InteractiveOuput(o.ToString());				
	//        }				
	//    }

	//    protected void PrintInstructions() {

	//        System.Text.StringBuilder sb; 

	//        sb = new System.Text.StringBuilder();
	//        for (int i=0; i < ActiveState.cur.ThreadPool.Threads.Length; ++i) {
	//            IThreadState t = ActiveState.cur.ThreadPool.Threads[i];
	//            if (t != null) {
	//                IMethodState m = ActiveState.cur.ThreadPool.Threads[i].CurrentMethod;
	//                if (m == null)
	//                    sb.AppendFormat("Thread {0}: empty call stack.\n", i);
	//                else
	//                    sb.AppendFormat("Thread {0}: {1} in {2}.{3} at offset {4}\n", i,
	//                            (m.ProgramCounter == null ?
	//                             "NULL" : m.ProgramCounter.OpCode.Name),
	//                            m.Definition.DeclaringType.Name.ToString(),
	//                            m.Definition.Name.ToString(),
	//                            (m.ProgramCounter == null ?
	//                             "NULL" : m.ProgramCounter.Offset.ToString("D4")));
	//            }
	//        }
	//        InteractiveOuput( sb.ToString() );
	//    }

	//    protected void PrintCurrentMethodCode() {

	//        MethodDefinition methDef;
	//        System.Text.StringBuilder sb; 
	//        string operand;

	//        sb = new System.Text.StringBuilder();
	//        if (ActiveState.cur.CurrentMethod == null)
	//            sb.Append("Empty call stack.");
	//        else {
	//            methDef = ActiveState.cur.CurrentMethod.Definition;
	//            sb.AppendFormat("CIL code for {0}::{1}:\n",
	//                    methDef.DeclaringType.Name,
	//                    methDef.Name);
	//            foreach (Instruction instr in methDef.Body.Instructions) {
	//                operand = (instr.Operand != null ? instr.Operand.ToString() : "");
	//                if (instr.Operand is Instruction)
	//                    operand = ((Instruction)instr.Operand).Offset.ToString("D4");
	//                sb.AppendFormat("   {0} {1}: {2} {3}\n",
	//                        (instr == ActiveState.cur.CurrentMethod.ProgramCounter ? ">" : " "),
	//                        instr.Offset.ToString("D4"),
	//                        instr.OpCode.Name,
	//                        operand);
	//            }
	//        }
	//        InteractiveOuput( sb.ToString() );
	//    }

	//    protected void ListThreads(SchedulingData sd) {

	//        System.Text.StringBuilder sb; 

	//        sb = new System.Text.StringBuilder();
	//        sb.Append("Runnable threads:");
	//        object[] q = sd.ThreadsToRun.ToArray();
	//        for (int i = 0; i < q.Length; ++i) {
	//            sb.Append(" ");
	//            sb.Append(q[i]);
	//        }
	//        InteractiveOuput( sb.ToString() );
	//    }

	//    protected override int SelectRunnableThread(SchedulingData sd) {

	//        int input;
	//        int retval;

	//        if (m_breakPointsOnly && !m_bph.IsBreakPoint())
	//            retval = (int)sd.ThreadsToRun.Dequeue();
	//        else {
	//            InteractiveOuput("Exploration suspended. Choose action or thread (default is "+
	//                    (int)sd.ThreadsToRun.Peek()+").");
	//            retval = -2;
	//            while (retval == -2) {
	//                input = GetInput();
	//                switch (input) {
	//                case InteractionMessages.FirstRunnable:
	//                    retval = (int)sd.ThreadsToRun.Dequeue();
	//                    break;
	//                case InteractionMessages.ListThreads:
	//                    ListThreads(sd);
	//                    break;
	//                default:
	//                    retval = input;
	//                    break;
	//                }
	//            }
	//            InteractiveOuput("Resuming with thread " + retval + ".");
	//        }
	//        return retval;
	//    }

	//    protected override void InitComponents() {

	//        base.InitComponents();
	//        m_bph = new BreakPointHandler();
	//        m_breakPointsOnly = false;
	//    }

	//    public InteractiveExplorer() : base() {}
	//}
}
