using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Traversal;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions
{
    public abstract class ExtendableInstructionExecBase : InstructionExecBase
    {
        private class ExecutionPipeline
        {
            private readonly ExtendableInstructionExecBase _instruction;
            private static readonly IInstructionExecutor[] _noExecutors = new IInstructionExecutor[0];

            public ExecutionPipeline(ExtendableInstructionExecBase instruction)
            {
                _instruction = instruction;
            }

            public IReadOnlyList<IInstructionExecutor> Executors
            {
                get
                {
                    return _instruction.Extensions ?? _noExecutors;
                }
            }

            private int _current = 0;

            public IIEReturnValue Execute(ExplicitActiveState cur)
            {
                _current = 0;
                return ExecuteNext(_instruction, cur);
            }

            private IIEReturnValue ExecuteNext(InstructionExecBase instruction, ExplicitActiveState cur)
            {
                IReadOnlyList<IInstructionExecutor> executors = Executors;
                if (_current >= executors.Count)
                {
                    // we have executed all of the extensions, execute the default behaviour now
                    return _instruction.ExecuteCore(cur);
                }

                return Executors[_current++].Execute(instruction, cur, ExecuteNext);
            }
        }


        public ExtendableInstructionExecBase(Instruction instr, object operand, InstructionExecAttributes atr) : base(instr, operand, atr)
        {
        }

        private IReadOnlyList<IInstructionExecutor> _extensions = null;

        /// <summary>
        /// An ordered collection of extensions to use.
        /// </summary>
        public IReadOnlyList<IInstructionExecutor> Extensions
        {
            get
            {
                return _extensions;
            }
            set
            {
                _extensions = value;
            }
        }

        /// <summary>
        /// This method, when overridden, will provide the default execution behavior.
        /// </summary>
        /// <param name="cur"></param>
        /// <returns></returns>
        protected abstract IIEReturnValue ExecuteCore(ExplicitActiveState cur);



        public sealed override IIEReturnValue Execute(ExplicitActiveState cur)
        {
            IIEReturnValue retValue;

            if (_extensions == null)
            {
                retValue = ExecuteCore(cur);
            }
            else
            {
                ExecutionPipeline pipeline = new ExecutionPipeline(this);
                retValue = pipeline.Execute(cur);
            }

            // track execution
            Path currentPath = cur.PathStore.CurrentPath;
            if (retValue == ehLookupRetval)
            {
                currentPath.OnExceptionThrown(cur.CurrentLocation, GetCurrentExceptionType(cur));
            }
            else
            {
                currentPath.OnInstructionExecuted(cur.CurrentLocation);
            }

            return retValue;
        }

        private static TypeDef GetCurrentExceptionType(ExplicitActiveState cur)
        {
            ObjectReference er = cur.CurrentThread.ExceptionReference;
            AllocatedObject allocatedException = (AllocatedObject)cur.DynamicArea.Allocations[er];
            return allocatedException.Type.ResolveTypeDefThrow();
        }
    }
}
