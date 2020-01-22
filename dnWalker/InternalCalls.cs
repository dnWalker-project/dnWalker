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

namespace MMC.ICall {

	using System.Collections;
	using System.Collections.Specialized;
	
	using MMC.State;
	using MMC.Data;

    using MethodDefinition = dnlib.DotNet.MethodDef;

    delegate void ICH(MethodDefinition methDef, DataElementList args);
	delegate MemoryLocation ICH_Access(MethodDefinition methDef, DataElementList args, int threadId);
	delegate bool ICH_Dependent(MethodDefinition methDef, DataElementList args);


	/// Singleton access class.
	static class IntCallManager {

		public static readonly HashedICM icm = new HashedICM();
	}

	/// Simple implementation of a name to handler (ICH) mapping.
	///
	/// This should be a mapping of a method definition to its handler, so use
	/// the definition provider to look up methods here. However, then we still
	/// have the problem of parameters that are not matched. A solution to
	/// this is to define our own key struct, and include the names of the
	/// parameter types there too.
	class HashedICM {

		IDictionary m_ichDependent;
		IDictionary m_ichAccessed;
		IDictionary m_ich;
		StringCollection m_threadUnsafe;

		/// Initialize the mapping of method name to handler.
		void SetupDictionary() {
			/* Below are Mono's VM internal call handlers */
			// TODO, do some if then else and some virtual machine detection

			//m_ich.Add("FastCopy", new ICH(ArrayHandlers.FastCopy));
			//m_ichDependent.Add("FastCopy", new ICH_Dependent(ArrayHandlers.FastCopy_IsDependent));

			/*m_ich.Add("Sin", new ICH(MathHandlers.Sin));
			m_ich.Add("Cos", new ICH(MathHandlers.Cos));
			m_ich.Add("Tan", new ICH(MathHandlers.Tan));
			m_ich.Add("Sinh", new ICH(MathHandlers.Sinh));
			m_ich.Add("Cosh", new ICH(MathHandlers.Cosh));
			m_ich.Add("Tanh", new ICH(MathHandlers.Tanh));
			m_ich.Add("Acos", new ICH(MathHandlers.Acos));
			m_ich.Add("Asin", new ICH(MathHandlers.Asin));
			m_ich.Add("Atan", new ICH(MathHandlers.Atan));
			m_ich.Add("Exp", new ICH(MathHandlers.Exp));
			m_ich.Add("Log", new ICH(MathHandlers.Log));
			m_ich.Add("Log10", new ICH(MathHandlers.Log10));
			m_ich.Add("Pow", new ICH(MathHandlers.Pow));
			m_ich.Add("Sqrt", new ICH(MathHandlers.Sqrt));*/

			// System.Environment
			/*m_ich.Add("get_Platform", new ICH(EnvironmentHandlers.Get_Platform));

			// System.Security.SecurityManager
			m_ich.Add("get_SecurityEnabled", new ICH(SecurityManagerHandlers.Get_SecurityEnabled));

			// System.Text.Encoding
			m_ich.Add("InternalCodePage", new ICH(EncodingHandlers.InternalCodePage));

			// System.Threading.Thread
			//m_ich.Add("CurrentThread_internal", new ICH(ThreadHandlers.CurrentThread_internal));
			//m_ichDependent.Add("CurrentThread_internal", new ICH_Dependent(ThreadHandlers.CurrentThread_internal_IsDependent));

			m_ich.Add("Sleep_internal", new ICH(ThreadHandlers.Sleep_internal));
			m_threadUnsafe.Add("Sleep_internal"); // not really unsafe, but forces reschedule

			m_ich.Add("Thread_internal", new ICH(ThreadHandlers.Thread_internal));
			m_ichDependent.Add("Thread_internal", new ICH_Dependent(ThreadHandlers.Thread_internal_IsDependent));
			m_threadUnsafe.Add("Thread_internal");

			//m_ich.Add("Join_internal", new ICH(ThreadHandlers.Join_internal));

			// System.Threading.Monitor
			ICH_Dependent monitor_ICH_dependent = new ICH_Dependent(MonitorHandlers.Monitor_IsDependent);
			m_ich.Add("Monitor_exit", new ICH(MonitorHandlers.Monitor_exit));
			m_threadUnsafe.Add("Monitor_exit");
			m_ichDependent.Add("Monitor_exit", monitor_ICH_dependent);

			m_ich.Add("Monitor_pulse", new ICH(MonitorHandlers.Monitor_pulse));
			m_threadUnsafe.Add("Monitor_pulse");
			m_ichDependent.Add("Monitor_pulse", monitor_ICH_dependent);

			m_ich.Add("Monitor_pulse_all", new ICH(MonitorHandlers.Monitor_pulse_all));
			m_threadUnsafe.Add("Monitor_pulse_all");
			m_ichDependent.Add("Monitor_pulse_all", monitor_ICH_dependent);

			m_ich.Add("Monitor_test_synchronised", new ICH(MonitorHandlers.Monitor_test_synchronised));
			m_threadUnsafe.Add("Monitor_test_synchronised");
			m_ichDependent.Add("Monitor_test_synchronised", monitor_ICH_dependent);

			m_ich.Add("Monitor_try_enter", new ICH(MonitorHandlers.Monitor_try_enter));
			//m_threadUnsafe.Add("Monitor_enter");
			m_threadUnsafe.Add("Monitor_try_enter");
			m_ichAccessed.Add("Monitor_try_enter", new ICH_Access(MonitorHandlers.Monitor_try_enter_Accessed));
			m_ichDependent.Add("Monitor_try_enter", monitor_ICH_dependent);

			m_ich.Add("Monitor_wait", new ICH(MonitorHandlers.Monitor_wait));
			m_threadUnsafe.Add("Monitor_wait");
			m_ichDependent.Add("Monitor_wait", monitor_ICH_dependent);*/


			/*  Below are Microsoft's VM internal call handlers */
			/*
			// System.Threading.Thread from Microsoft's .NET CLI
			m_ich.Add("StartupSetApartmentStateInternal", new ICH(ThreadHandlers.StartupSetApartmentStateInternal));
			m_ich.Add("GetFastCurrentThreadNative", new ICH(ThreadHandlers.CurrentThread_internal));
			m_ich.Add("GetCurrentThreadNative", new ICH(ThreadHandlers.CurrentThread_internal));
			m_ich.Add("StartInternal", new ICH(ThreadHandlers.StartInternal));
			m_ich.Add("SetStart", new ICH(ThreadHandlers.SetStart));
			*/
		}

		/// Call the ICH associated with the specified method.
		public bool HandleICall(MethodDefinition methDef, DataElementList args) {

			ICH handler = m_ich[methDef.Name] as ICH;
			if (handler != null)
				handler(methDef, args);
			return handler != null;
		}

		public MemoryLocation HandleICallAccessed(MethodDefinition methDef, DataElementList args, int threadId) {
			ICH_Access handler = m_ichAccessed[methDef.Name] as ICH_Access;
			if (handler != null)
				return handler(methDef, args, threadId);
			else
				return MemoryLocation.Null;
		}

		/// Check if an internal call is multi-thread safe.
		///
		/// \return True iff it is safe.
		public bool IsMultiThreadSafe(string methodName) {

			return !m_threadUnsafe.Contains(methodName); // default policy is true
		}

		public bool IsDependent(MethodDefinition methDef, DataElementList args) {
			ICH_Dependent handler = m_ichDependent[methDef.Name] as ICH_Dependent;
			if (handler != null)
				return handler(methDef, args);
			else
				return false;			
		}

		public HashedICM() {
			m_ichDependent = new Hashtable();
			m_ichAccessed = new Hashtable();
			m_ich = new Hashtable();			
			m_threadUnsafe = new StringCollection();
			SetupDictionary();
		}
	}

	/// ICall handlers for System.Array.
	class ArrayHandlers {

		public static void FastCopy(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
            ObjectReference source = (ObjectReference)args[0];
			int destIdx = ((INumericElement)args[1]).ToInt4(false).Value;
			ObjectReference dest = (ObjectReference)args[2];
			int sourceIdx = ((INumericElement)args[3]).ToInt4(false).Value;
			int length = ((INumericElement)args[4]).ToInt4(false).Value;

			AllocatedArray sourceArr = cur.DynamicArea.Allocations[source] as AllocatedArray;
			AllocatedArray destArr = cur.DynamicArea.Allocations[dest] as AllocatedArray;

            cur.ParentWatcher.RemoveParentFromAllChilds(dest, cur);

            for (int i = 0; i < length; i++, destIdx++, sourceIdx++)
            {
                destArr.Fields[destIdx] = sourceArr.Fields[sourceIdx];
            }

            cur.ParentWatcher.AddParentToAllChilds(dest, cur);

			cur.EvalStack.Push(new Int4(1));
		}

		public static bool FastCopy_IsDependent(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur) {
            ObjectReference source = (ObjectReference)args[0];
			ObjectReference dest = (ObjectReference)args[2];

			AllocatedArray sourceArr = cur.DynamicArea.Allocations[source] as AllocatedArray;
			AllocatedArray destArr = cur.DynamicArea.Allocations[dest] as AllocatedArray;

			return sourceArr.ThreadShared || destArr.ThreadShared;
		}
	}

	/// ICall handlers for System.Environment.
	class EnvironmentHandlers {

		public static void Get_Platform(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			cur.EvalStack.Push(new Int4((int)System.Environment.OSVersion.Platform));
		}
	}

	/// ICall handlers for System.Security.SecurityManager.
	class SecurityManagerHandlers {

		public static void Get_SecurityEnabled(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			cur.EvalStack.Push(MMC.VMDefaults.SecurityEnabled);
		}
	}

	/// ICall handlers for System.Math
	class MathHandlers {

		/*public static void Sin(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Sin(x.Value)));
		}


		public static void Cos(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Cos(x.Value)));
		}

		public static void Tan(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Tan(x.Value)));
		}

		public static void Sinh(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Sinh(x.Value)));
		}

		public static void Cosh(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Cosh(x.Value)));
		}

		public static void Tanh(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Tanh(x.Value)));
		}

		public static void Acos(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Acos(x.Value)));
		}

		public static void Asin(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Asin(x.Value)));
		}

		public static void Atan(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Atan(x.Value)));
		}

		public static void Exp(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Exp(x.Value)));
		}

		public static void Log(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Log(x.Value)));
		}

		public static void Log10(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Log10(x.Value)));
		}

		public static void Pow(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			Float8 y = (Float8)args[1];
			cur.EvalStack.Push(new Float8(System.Math.Pow(x.Value, y.Value)));
		}

		public static void Sqrt(MethodDefinition methDef, DataElementList args) {
			Float8 x = (Float8)args[0];
			cur.EvalStack.Push(new Float8(System.Math.Sqrt(x.Value)));
		}*/
	}


	/// ICall handlers for System.Text.Encoding.
	class EncodingHandlers {

		/// <summary>
		/// ICH for System.Text.Encoding.InternalCodePage. "Forward" to .NET.
		/// </summary>
		public static void InternalCodePage(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			LocalVariablePointer code_page = (LocalVariablePointer)args[0];
			code_page.Value = new Int4(System.Text.Encoding.Default.CodePage);
			cur.EvalStack.Push(new ConstantString(System.Text.Encoding.Default.EncodingName));
		}
	}

	/// ICall handlers for System.Threading.Thread.
	class ThreadHandlers {

		/// ICH for System.Threading.Thread.Sleep_internal.
		public static void Sleep_internal(
				MethodDefinition methDef, DataElementList args) {

			Logger.l.Message("thread sleeping (but not really)");
		}

		/// ICH for System.Threading.Thread.CurrentThread_internal.
		public static void CurrentThread_internal(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			cur.EvalStack.Push(cur.ThreadPool.CurrentThread.ThreadObject);
		}

		public static bool CurrentThread_internal_IsDependent(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			return cur.DynamicArea.Allocations[cur.ThreadPool.CurrentThread.ThreadObject].ThreadShared;
		}

        /// <summary>
        /// ICH for System.Threading.Thread.Thread_internal.
        /// This spawns a new thread.
        /// </summary>
        /// <param name="methDef"></param>
        /// <param name="args"></param>
        public static void Thread_internal(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
            ObjectReference threadObjectRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(false, cur),
                methDef.DeclaringType);
            AllocatedObject threadObject =
                cur.DynamicArea.Allocations[threadObjectRef] as AllocatedObject;

            // First, create a new thread. The first argument is the this pointer
            // to the thread object. The second argument is a reference to the
            // threadstart delegate, but we don't need to call it.
            AllocatedDelegate del = (AllocatedDelegate)cur.DynamicArea.Allocations[(ObjectReference)args[1]];

			// TODO: This does not deal with delegates that take additional
			// parameters (such as a state). It can probably be popped off the
			// stack, but not certain, so skipping for now.
			DataElementList delPars = cur.StorageFactory.CreateList(0);
			if (del.Method.Value.HasThis)
            {
				delPars = cur.StorageFactory.CreateList(1);
				delPars[0] = del.Object;
			}

            MethodState newThreadState = new MethodState(del.Method.Value, delPars, cur);

            int newThreadId = cur.ThreadPool.NewThread(cur, newThreadState, threadObjectRef);// (ObjectReference)args[0]);

            // Push ID on stack as an IntPointer.
            cur.EvalStack.Push(threadObjectRef);// new IntPointer(newThreadId));
		}

		public static bool Thread_internal_IsDependent(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur) {
			AllocatedDelegate del = (AllocatedDelegate)cur.DynamicArea.Allocations[(ObjectReference)args[1]];
			AllocatedObject ao = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)args[0]];

			return del.ThreadShared || ao.ThreadShared;
		}

        /// <summary>
        /// ICH for System.Threading.Thread.Join_internal.
        /// </summary>
        public static void Join_internal(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
            // private extern bool Join_internal(int ms, IntPtr handle);

            // Ignore the timeout (int ms), but issue warning if it's not equal to
            // Timeout.Infinite.
            if (((Int4)args[1]).Value != System.Threading.Timeout.Infinite)
            {
                Logger.l.Warning("call to Thread::Join_internal(int ms, IntPtr IntPtr), with");
                Logger.l.Warning("ms != Timeout.Infinite, but threated as such!");
            }

            // TODO: find out what the handle is for (see relevant Mono C code) and
            // simulate that behaviour.

            // To allow for easy checking of deadlocks etc, let the threadpool handle this.
            cur.ThreadPool.JoinThreads(
                cur.ThreadPool.CurrentThreadId,
                cur.ThreadPool.FindOwningThread(args[0]));

            // Return true if the thread has terminated, false otherwise. Since we do
            // not deal with timing issues for now, we can always return true.
            cur.EvalStack.Push(new Int4(1));
        }

		public static bool Join_internal_IsDependent(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			ThreadState tstate = cur.ThreadPool.Threads[cur.ThreadPool.FindOwningThread(args[0])];
			AllocatedObject ao = (AllocatedObject)cur.DynamicArea.Allocations[tstate.ThreadObject];

			return ao.ThreadShared;
		}

		/*
		/// ICH for System.Threading.Thread::StartupSetApartmentStateInternal()
		/// (From Microsoft's Thread class)
		/// 
		/// TODO: I have no clue what this does, but its result is popped by Thread anyway...
		public static void StartupSetApartmentStateInternal(
				MethodDefinition methDef, IDataElementList args) {

			cur.EvalStack.Push(new Int4(0));
		}

		// ICH for Microsoft's Thread::StartInternal
		// starts a thread
		public static void StartInternal(MethodDefinition methDef, IDataElementList args) {
		}

		// ICH for Microsoft's void Thread::SetStart
		// Sets up a threading object and its delegate called ``start''
		public static void SetStart(MethodDefinition methDef, IDataElementList args) {

			// First, create a new thread. The first argument is the this pointer
			// to the thread object. The second argument is a reference to the
			// threadstart delegate
			ObjectReference threadStartDelegateRef = (ObjectReference)args[1];
			AllocatedObject threadObject = (AllocatedObject)cur.DynamicArea.Allocations[(ObjectReference)args[0]];

			FieldDefinition fd = DefinitionProvider.dp.GetFieldDefinition("System.Threading.Thread", "m_Delegate");
			threadObject.Fields[(int)fd.Offset] = threadStartDelegateRef;			
		}*/
	}

	class MonitorHandlers {

        // private extern static void Monitor_exit(object obj)
        public static void Monitor_exit(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
            LockManager lm = cur.DynamicArea.LockManager;
            ObjectReference obj = (ObjectReference)args[0];

            // This thread must own the lock on the object.
            if (cur.me != lm.GetLock(obj, cur).Owner)
            {
                Logger.l.Warning("thread {0} attemts to exit monitor on {1} but isn't the owner", cur.me, obj.ToString());
            }
            else
            {
                lm.Release(obj, cur);
                // If the lock was acquired by a new thread, make sure that
                // thread is runnable.
                if (lm.IsLocked(obj, cur))
                    cur.ThreadPool.Threads[lm.GetLock(obj, cur).Owner].Awaken();
            }
        }

		public static bool Monitor_IsDependent(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			ObjectReference obj = (ObjectReference)args[0];
			return cur.DynamicArea.Allocations[obj].ThreadShared;
		}

		// private extern static void Monitor_pulse(object obj);
		public static void Monitor_pulse(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			LockManager lm = cur.DynamicArea.LockManager;
			ObjectReference obj = (ObjectReference)args[0];

			if (cur.me != lm.GetLock(obj, cur).Owner)
				Logger.l.Warning(
						"thread {0} attemts to pulse first waiting thread on {1} but isn't the owner",
						cur.me, obj.ToString());
			else
				lm.Pulse(obj, false, cur);
		}

		// private extern static void Monitor_pulse_all(object obj);
		public static void Monitor_pulse_all(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			LockManager lm = cur.DynamicArea.LockManager;
			ObjectReference obj = (ObjectReference)args[0];

			if (cur.me != lm.GetLock(obj, cur).Owner)
				Logger.l.Warning(
						"thread {0} attemts to pulse all waiting threads on {1} but isn't the owner",
						cur.me, obj.ToString());
			else
				lm.Pulse(obj, true, cur);
		}

		// private extern static bool Monitor_test_synchronised(object obj);
		public static void Monitor_test_synchronised(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			LockManager lm = cur.DynamicArea.LockManager;
			bool locked = lm.IsLocked((ObjectReference)args[0], cur);
			cur.EvalStack.Push((locked ? new Int4(1) : new Int4(0)));
		}

		// private extern static bool Monitor_try_enter(object obj, int ms);
		// obj = the object to lock on
		// ms  = timeout
		public static void Monitor_try_enter(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur)
        {
			LockManager lm = cur.DynamicArea.LockManager;
			ObjectReference obj = (ObjectReference)args[0];
			// IDataElement ms = args[1]; // todo: don't block forever if ms > 0 && ms < \infty

			bool acquired = lm.Acquire(obj, cur.me, cur);
			if (!acquired) {
				// Not acquired, but ms is non-zero, which we will interpret as infinity,
				// so add this thread to the wait queue.
				Lock lock_we_want = lm.GetLock(obj, cur);
				lock_we_want.ReadyQueue.Enqueue(cur.me);
				//				cur.CurrentThread.WaitFor(lock_we_want.Owner);
				cur.CurrentThread.WaitFor(LockManager.NoThread);

				cur.ThreadPool.CheckDeadlockFrom(cur.me);
			}

			// Return if we were able to get the lock immediately. This value is discarded
			// if the call was made from Monitor.Enter(object).
			cur.EvalStack.Push((acquired ? new Int4(1) : new Int4(0)));
		}

        public static MemoryLocation Monitor_try_enter_Accessed(MethodDefinition methDef, DataElementList args, int threadId, ExplicitActiveState cur)
        {
            LockManager lm = cur.DynamicArea.LockManager;
            ObjectReference obj = (ObjectReference)args[0];

            bool acquirable = lm.IsAcquireable(obj, threadId, cur);

            if (acquirable)
            {
                return new MemoryLocation(obj, cur);
            }
            else
            {
                return MemoryLocation.Null;
            }
        }

		// private extern static bool Monitor_wait(object obj, int ms);
		// For explaination of parameters, see Enter.
		// Again, we ignore the timing aspects completely, even though an ms != infitity
		// can mean the difference between a deadlock and a 'normal' timeout.
		public static void Monitor_wait(MethodDefinition methDef, DataElementList args, ExplicitActiveState cur) {

			LockManager lm = cur.DynamicArea.LockManager;
			ObjectReference obj = (ObjectReference)args[0];

			if (cur.me != lm.GetLock(obj, cur).Owner)
				Logger.l.Warning(
						"thread {0} attemts to wait on monitor for {1} but isn't the owner",
						cur.me, obj.ToString());
			else {
				lm.Release(obj, cur);
				lm.GetLock(obj, cur).WaitQueue.Enqueue(cur.me);
				// We're not specifically waiting for an other thread, just to
				// re-acquire this lock.
				cur.CurrentThread.WaitFor(LockManager.NoThread);

				if (lm.IsLocked(obj, cur))
					cur.ThreadPool.Threads[lm.GetLock(obj, cur).Owner].Awaken();
			}

			// If this thread is rescheduled, it has apperently reacquired the lock,
			// so make it find a true on its eval stack.
			cur.EvalStack.Push(new Int4(1));
		}
	}
}
