using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Parameters
{
    public class LDELEM : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] 
        {
            OpCodes.Ldelem,

            OpCodes.Ldelem_I,
            OpCodes.Ldelem_I1,
            OpCodes.Ldelem_I2,
            OpCodes.Ldelem_I4,
            OpCodes.Ldelem_I8,

            OpCodes.Ldelem_U1,
            OpCodes.Ldelem_U2,
            OpCodes.Ldelem_U4,
            
            OpCodes.Ldelem_R4,
            OpCodes.Ldelem_R8,

            OpCodes.Ldelem_Ref,
        };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
        {
            // gather the operands
            INumericElement indexElement = (INumericElement)cur.EvalStack.Peek(0);
            ObjectReference arrayReference = (ObjectReference)cur.EvalStack.Peek(1);

            // no parameter system is available or the array is not parametrized
            if (!cur.TryGetParameterStore(out ParameterStore store) ||
                !arrayReference.TryGetParameter(cur, out IArrayParameter arrayParameter))
            {
                return next(instruction, cur);
            }

            // check for exceptions
            // - null reference
            if (arrayReference.IsNull())
            {
                // null reference
                return next(instruction, cur);
            }


            // - index out of range
            int index = indexElement.ToInt4(false).Value;
            AllocatedArray allocatedArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayReference];

            if (index < 0 || index >= allocatedArray.Fields.Length)
            {
                // index out of range
                return next(instruction, cur);
            }

            // the execution will not throw any exception & we have a parametrized array
            // - ensure that the AllocatedArray actually has the proper element initialized

            IDataElement itemElement = allocatedArray.Fields[index];

            if (!itemElement.TryGetParameter(cur, out IParameter itemParameter))
            {
                // the item at the specified index is not yet associated with any parameter
                // 2 options:
                // 1) no parameter is yet defined for the location => create a default one
                // 2) a parameter is already defined do nothing
                if (!arrayParameter.TryGetItem(index, out itemParameter))
                {
                    // 1st option, we need to create it
                    // 2 options:
                    // 1) the array is an input => we need to create the item parameter within the BASE set & copy into the EXEC set
                    // 2) the array is a fresh object => we create the item parameter within the EXEC set and the BASE set is completely ignored
                    if (arrayParameter.Reference.TryResolve(store.BaseSet, out IArrayParameter baseArrayParameter))
                    {
                        IParameter baseItemParameter = store.BaseSet.CreateParameter(allocatedArray.Type.ToTypeSig());
                        baseArrayParameter.SetItem(index, baseItemParameter);

                        itemParameter = baseItemParameter.CloneData(store.ExecutionSet);
                    }
                    else
                    {
                        itemParameter = store.ExecutionSet.CreateParameter(allocatedArray.Type.ToTypeSig());
                    }

                    // make the connection of the item parameter and the array parameter
                    arrayParameter.SetItem(index, itemParameter);
                }

                // set the lazily initialized data element into the allocated array
                allocatedArray.Fields[index] = itemParameter.AsDataElement(cur);
            }

            // do the actual execution
            IIEReturnValue retValue = next(instruction, cur);

            return retValue;
        }
    }
}
