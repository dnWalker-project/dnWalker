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
    using MMC.Data;
    using MMC.State;
    using dnlib.DotNet;
    using MMC.InstructionExec;

    public class StateSpaceSetup
    {
        // Okay, this is just great. These are the fields of a Thread
        // allocation when running Mono with the --debug flag:
        //
        // threadObject o:Thread flds: {lock_thread_id=0 system_thread_handle=0
        // cached_culture_info=O(0) unused0=0 threadpool_thread=0 name=0
        // name_len=0 state=0 abort_exc=O(0) abort_state=O(0) thread_id=0
        // start_notify=0 stack_ptr=0 static_data=0 jit_data=0 lock_data=0
        // current_appcontext=O(0) stack_size=0 start_obj=O(0) appdomain_refs=0
        // interruption_requested=0 suspend_event=0 suspended_event=0
        // resume_event=0 synch_lock=O(0) serialized_culture_info=0
        // serialized_culture_info_len=0 serialized_ui_culture_info=0
        // serialized_ui_culture_info_len=0 _ec=O(0) unused1=0 unused2=0
        // unused3=0 unused4=0 unused5=0 unused6=0 unused7=0 threadstart=O(0)
        // thread_name=O(0) _principal=O(0)
        //
        // And these are the fields when running it without that flag:
        //
        // m_Context=O(0) m_LogicalCallContext=O(0) m_IllogicalCallContext=O(0)
        // m_Name=O(0) m_ExceptionStateInfo=O(0) m_Delegate=O(0)
        // m_PrincipalSlot=O(0) m_ThreadStatics=O(0) m_ThreadStaticsBits=O(0)
        // m_CurrentCulture=O(0) m_CurrentUICulture=O(0) m_Priority=0
        // DONT_USE_InternalThread=0
        //
        // Nice.

        public ModuleDef LoadAssemblies(IConfig config)
        {
            // Load assembly, and initialize the definition lookup and active state.
            try
            {
                Logger.l.Notice("loading main assembly...");
                DefinitionProvider.LoadAssembly(config.AssemblyToCheckFileName);
                Logger.l.Notice("loaded {0}", config.AssemblyToCheckFileName);
            }
            catch (System.Exception e)
            {
                MonoModelChecker.Fatal("error loading assembly: " + config.AssemblyToCheckFileName + System.Environment.NewLine + e);
            }

            return DefinitionProvider.dp.AssemblyDefinition;
        }

        public ExplicitActiveState CreateInitialState(IConfig config, IInstructionExecProvider instructionExecProvider)
        {
            var asmDef = LoadAssemblies(config);
            var cur = new ExplicitActiveState(config, instructionExecProvider);

            //StorageFactory.UseRefCounting(_config.UseRefCounting);
            if (config.UseRefCounting)
            {
                Logger.l.Notice("using reference counting");
            }

            // Wrap run arguments in ConstantString objects, and create the method state for Main().
            ObjectReference runArgsRef = cur.DynamicArea.AllocateArray(
                cur.DynamicArea.DeterminePlacement(false, cur),
                DefinitionProvider.dp.GetTypeDefinition("System.String"),
                cur.Configuration.RunTimeParameters.Length);

            if (config.RunTimeParameters.Length > 0)
            {
                AllocatedArray runArgs = (AllocatedArray)cur.DynamicArea.Allocations[runArgsRef];
                for (int i = 0; i < config.RunTimeParameters.Length; ++i)
                {
                    runArgs.Fields[i] = new ConstantString(config.RunTimeParameters[i]);
                }
            }
            MethodState mainState = new MethodState(
                asmDef.EntryPoint,
                cur.StorageFactory.CreateSingleton(runArgsRef),
                cur);

            // Initialize main thread.
            cur.ThreadPool.CurrentThreadId = cur.ThreadPool.NewThread(cur, mainState, CreateMainThreadObject(cur, asmDef.EntryPoint));

            cur.CurrentThread.State = (int)System.Threading.ThreadState.Running;

            return cur;
        }

        private ObjectReference CreateMainThreadObject(ExplicitActiveState cur, MethodDef mainDefinition)
        {
            // A thread is created as follows:
            // 1. A threadstart delegate is created: newobj ThreadStart::.ctor(Object, IntPtr),
            //    the first argument being null, the second being a function pointer to the
            //    entry method.
            // 2. A thread object is created: newobj Thread::.ctor(ThreadStart),
            //    the first argument being the ThreadStart just created. In the constructor
            //    the following field are initialized:
            //    a. The private field 'state' is set to 8 (ThreadState.Unstarted; see MSDN or
            //       Mono Sources: mcs/class/corlib/System.Threading/ThreadState.cs).
            //       This step is skipped, since the state of the main thread is set to
            //       Running immediately after this step.
            //    b. The private field 'sync_lock' is set to a new System.Object.
            //    c. The private field 'threadstart' is set to argument 1 (0 being 'this').
            //    And some base constructor calls are performed, which can be safely ignored.
            // 3 (added). Check if the 'state' field is accessable. When testing the application
            //   on Windows, using Mono 1.1.15, and when running without debug mode, the field
            //   names are different for some reason.

            // 1
            MethodPointer mainMethodPtr = new MethodPointer(mainDefinition);
            ObjectReference mainMethodDelegate = cur.DynamicArea.AllocateDelegate(
                cur.DynamicArea.DeterminePlacement(false, cur),
                ObjectReference.Null,
                mainMethodPtr);

            // 2
            ObjectReference threadObjectRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(false, cur),
                DefinitionProvider.dp.GetTypeDefinition("System.Threading.Thread"));

            AllocatedObject threadObject =
                cur.DynamicArea.Allocations[threadObjectRef] as AllocatedObject;

            // 2b
            // Note from corlib Thread.cs sources:
            // /* Don't lock on synch_lock in managed code, since it can result in deadlocks */
            // What? Oh well, we'll just do what Mono does.
            var synch_lockField = DefinitionProvider.dp.GetFieldDefinition("System.Threading.Thread", "synch_lock");

            if (synch_lockField != null)
            {
                // Simply skip if not found.
                ObjectReference newObjectRef = cur.DynamicArea.AllocateObject(
                    cur.DynamicArea.DeterminePlacement(false, cur),
                    DefinitionProvider.dp.GetTypeDefinition("System.Object"));
                threadObject.Fields[(int)synch_lockField.FieldOffset] = newObjectRef;
                // TODO: HV for maintaining the parents references in the incremental heap visitor
                //cur.DynamicArea.Allocations[newObjectRef].Parents.Add(threadObjectRef);
                cur.ParentWatcher.AddParentToChild(threadObjectRef, newObjectRef, cur.Configuration.MemoisedGC);
            }
            else
            {
                Logger.l.Warning("No thread local synchronisation object field found!");
            }

            // 2c
            // In Microsoft's .NET, the delegate is stored in m_Delegate
            var threadstartField = DefinitionProvider.dp.GetFieldDefinition("System.Threading.Thread", "threadstart");
            if (threadstartField != null)
            {
                threadObject.Fields[(int)threadstartField.FieldOffset] = mainMethodDelegate;
                // TODO: HV 
                //cur.DynamicArea.Allocations[mainMethodDelegate].Parents.Add(threadObjectRef);
                cur.ParentWatcher.AddParentToChild(threadObjectRef, mainMethodDelegate, cur.Configuration.MemoisedGC);
            }
            else
                Logger.l.Warning("No thread field found for storing Main delegate!");

            // 3
            var stateField = DefinitionProvider.dp.GetFieldDefinition("System.Threading.Thread", "state");

            if (stateField == null)
            {
                Logger.l.Warning("No field 'state' found in System.Threading.Thread object.");
                Logger.l.Warning("This probably means your class corlib is other than the");
                Logger.l.Warning("current SVN version for Linux we use. Try running the");
                Logger.l.Warning("application using: mono --debug mmc.exe ...");
                Logger.l.Warning("The state of thread will not be written back into the");
                Logger.l.Warning("System.Threading.Thread object, but a field in MMC");
                Logger.l.Warning("itself is used.");
                Logger.l.Warning("Behaviour might differ from normal execution!");
            }
            else
                ThreadState.state_field_offset = (int)stateField.FieldOffset;

            return threadObjectRef;
        }
    }
}
