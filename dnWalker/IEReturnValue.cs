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

namespace MMC.InstructionExec {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;
	using Mono.Cecil;
	using Mono.Cecil.Cil;
	using MMC.Data;
	using MMC.State;

	interface IIEReturnValue {

		bool ContinueExploration(MethodState current);
		Instruction GetNextInstruction(MethodState current);
	}

	class JumpReturnValue : IIEReturnValue {

		Instruction m_target;

		public bool ContinueExploration(MethodState current) {
			return true;
		}

		public JumpReturnValue(Instruction target) {
			m_target = target;
		}

		public override string ToString() {
			return string.Format("jump to target: {0}", m_target.ToString());
		}

		public Instruction GetNextInstruction(MethodState current) {
			// In theory, this supports long-jumps (no check is made whether
			// the target is actually in the method scope, but the compiler
			// will never allow this, so we're safe even if we skip the check.
			return m_target;
		}
	}

	class NextReturnValue : IIEReturnValue {

		public bool ContinueExploration(MethodState current) {
			return true;
		}

		public override string ToString() {
			return string.Format("next instruction");
		}

		public Instruction GetNextInstruction(MethodState current) {
			return current.ProgramCounter.Next;
		}
	}


	class NoIncrementReturnValue : IIEReturnValue {

		public bool ContinueExploration(MethodState current) {
			return true;
		}

		public override string ToString() {
			return string.Format("no update");
		}

		public Instruction GetNextInstruction(MethodState current) {
			return current.ProgramCounter;
		}
	}


	class AssertionViolatedReturnValue : IIEReturnValue {

		public bool ContinueExploration(MethodState current) {
			return false;
		}

		public override string ToString() {
			return string.Format("assertion violated");
		}

		public Instruction GetNextInstruction(MethodState current) {
			return current.ProgramCounter.Next;
		}
	}

	class ExceptionHandlerLookupReturnValue : IIEReturnValue {

		public bool ContinueExploration(MethodState current) {
			return true;
		}

		public override string ToString() {
			return "exception caught, traversing callstack to find handler";
		}
		
		/// See also the part on SEH exception handling in my thesis
		public Instruction GetNextInstruction(MethodState current) {
			ObjectReference exceptionRef = ActiveState.cur.CurrentThread.ExceptionReference;
			AllocatedObject exceptionObj = ActiveState.cur.DynamicArea.Allocations[exceptionRef] as AllocatedObject;

			Instruction retval = null;

			foreach (MethodState method in ActiveState.cur.CallStack) {
				method.EvalStack.PopAll();
				ExceptionHandler eh = method.NextFilterOrCatchHandler(method.ProgramCounter, exceptionObj.Type);

				if (eh != null) {
					if (eh.Type == ExceptionHandlerType.Catch) {
						/*
						 * Note that we change the program counter here, instead by the explorer.
						 * The explorer might not reach the handler yet because of finalisers, yet, in this method,
						 * the explorer has to jump to this handler
						 */
						method.ProgramCounter = eh.HandlerStart;
						method.EvalStack.Push(exceptionRef);
						retval = InstructionExecBase.finallyLookupRetval.GetNextInstruction(method);
						break;
					} else if (eh.Type == ExceptionHandlerType.Filter) {
						method.ProgramCounter = eh.HandlerStart;
						method.EvalStack.Push(exceptionRef);
						retval = eh.HandlerStart; // because in the explorer, the ProgramCounter is set again on current

						/*
						 * We use this unorthodox approach to prevent garbage collection 
						 * and collapsing problems. In a sense, the fact that filter 
						 * handling is active is encoded this by MMC.
						 */
						MethodState clone = method.DeepCopy();
						clone.ProgramCounter = eh.FilterStart;
						ActiveState.cur.CallStack.Push(clone);
						break;
					}
				}

			}

			if (retval == null)
				ActiveState.cur.CallStack.StackPointer = 0;

			/*
			 * if no suitable exception handler found, null is returned and the thread is terminated
			 */
			return retval;
		}
	}

	class ExceptionFinallyReturnValue : IIEReturnValue {

		public bool ContinueExploration(MethodState current) {
			return true;
		}

		public override string ToString() {
			return "exceptionhandler found, now executing all finally blocks above it ";
		}

		/// See the part of SEH exception handling in my thesis
		public Instruction GetNextInstruction(MethodState current) {

			ExceptionHandler eh = null;


			/*
			 * this is set to current.ProgramCounter, in case not finaliser is found, we wish
			 * to immediately jump to the exception handler
			 */
			Instruction retval = current.ProgramCounter;

			/*
			 * An empty evalstack indicates that the exception could not
			 * be handled by that method, and therefore, we must find
			 * all its finally handlers
			 */
			while (!ActiveState.cur.CallStack.IsEmpty() && ActiveState.cur.EvalStack.IsEmpty() && eh == null) {
				MethodState method = ActiveState.cur.CallStack.Pop();

				eh = method.NextFinallyOrFaultHandler(method.ProgramCounter);

				if (eh != null) {
					/*
					 * The retval is not interested here anyway, as we change the
					 * programcounter here ourselves, and not by the explorer
					 */
					retval = eh.HandlerStart;
					method.ProgramCounter = retval;
					ActiveState.cur.CallStack.Push(method);
				}
			}

			return retval;
		}
	}
}
