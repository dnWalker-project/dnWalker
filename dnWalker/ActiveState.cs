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

namespace MMC.State {

	using System.Collections;
	using Mono.Cecil.Cil;
	using MMC.Data;
	using MMC.Util;
	using MMC.ICall;
	using MMC.InstructionExec;

	/// Singleton accessor class.
	static class ActiveState {

		/// \brief The currect IActiveState. 
		///
		/// A simple public field suffices here, no need to define a property.
		public static readonly ExplicitActiveState cur = new ExplicitActiveState();
	}

	/// An implementation of the active state of the virtual machine.
	class ExplicitActiveState : IStorageVisitable, ICleanable {

		DynamicArea m_dyn;
		IStaticArea m_stat;
		ThreadPool m_tp;

		/// The allocation heap, i.e. the dynamic part of the state.
		public DynamicArea DynamicArea {

			get { return m_dyn; }
		}

		/// Loaded classes reside here, i.e. the static part.
		public IStaticArea StaticArea {

			get { return m_stat; }
		}

		/// Running sub-processes are kept in here.
		public ThreadPool ThreadPool {

			get { return m_tp; }
		}

		/// Currently running thread.
		public ThreadState CurrentThread {

			get { return m_tp.CurrentThread; }
		}

		/// Current location in the CIL image.
		public CILLocation CurrentLocation {

			get { return CurrentThread.CurrentLocation; }
		}

		/// ID of currently running thread.
		public int me {

			get { return m_tp.CurrentThreadId; }
		}

		/// Callstack of currently running thread.
		public CallStack CallStack {

			get { return m_tp.CurrentThread.CallStack; }
		}

		/// Top of callstack of currently running thread.
		public MethodState CurrentMethod {

			get { return CurrentThread.CurrentMethod; }
		}

		/// Evaluation stack of top of callstack of currently running thread.
		public DataElementStack EvalStack {

			get {
				DataElementStack retval = null;
				if (CurrentMethod != null)
					retval = CurrentMethod.EvalStack;
				return retval;
			}
		}

		/// \brief Determine if we are "running" in the assembly to be checked.
		///
		/// This means the current thread is currently executing code that is
		/// contained in the main assembly.
		///
		/// \return True iff code from the main assembly is being executed.
		public bool IsAssemblyLocalState() {

			return CurrentMethod != null &&
				(CurrentMethod.Definition.DeclaringType.Module.Assembly ==
				 DefinitionProvider.dp.AssemblyDefinition);
		}

		/// Callback method for visitors.
		///
		/// \param visitor A visitor visiting this state.
		public void Accept(IStorageVisitor visitor) {

			visitor.VisitActiveState(this);
		}

		/// Set all state-defining parts to be clean.
		public void Clean() {

			m_dyn.Clean();
			m_stat.Clean();
			m_tp.Clean();
		}

		/// Check if any of the four state-defining parts are dirty.
		public bool IsDirty() {

			return m_dyn.IsDirty() || m_stat.IsDirty() || m_tp.IsDirty();
		}
		
		public MemoryLocation NextAccess(int threadId) {
			MethodState method = ActiveState.cur.ThreadPool.Threads[threadId].CurrentMethod;
			Instruction instr = method.ProgramCounter;
			InstructionExecBase instrExec = InstructionExecProvider.iep.GetExecFor(instr);

			return instrExec.Accessed(threadId);
		}
		

		/// Convert the active state to a somewhat large string representation.
		///
		/// Do not over-use this method. It is quite slow to print each and every
		/// explored state for example. For small examples this is fine, but for
		/// >100.000 states it turns out ~80% of the time it's building strings.
		///
		/// \return A string representation of the active state.
		public override string ToString() {

			return string.Format(@"------------------------- DYNAMIC AREA -------------------------
{0}------------------------- STATIC  AREA -------------------------
{1}------------------------- THREAD  POOL -------------------------
{2}",
				m_dyn.ToString(), m_stat.ToString(), m_tp.ToString() );
		}

		public void Reset() {

			m_dyn = new DynamicArea();
			m_stat = new StaticArea();
			m_tp = new ThreadPool();
			
		}
		
		/// Constructor.
		public ExplicitActiveState() {
			Reset();
		}
	}
}
