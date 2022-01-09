using dnlib.DotNet;

using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public class ReturnValueParameterHandler : IPreExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[] { typeof(RET) };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (cur.CallStack.StackPointer > 1)
            {
                // we are not returning from the entry point
                return;
            }

            MethodDef method = cur.CurrentMethod.Definition;
            if (method.HasReturnType)
            {
                TypeSig retType = method.ReturnType;

                IDataElement returnValue =  cur.EvalStack.Peek();
                if (!cur.TryGetParameterStore(out ParameterStore store))
                {
                    // parameter store is not setup => do nothing
                    return;
                }
                store.SetReturnValue(returnValue, cur, retType);
            }
        }
    }
}
