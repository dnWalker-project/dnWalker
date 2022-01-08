using dnlib.DotNet;

using dnWalker.Parameters;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static MMC.InstructionExec.InstructionExecBase;

namespace dnWalker.Instructions.Extensions
{
    public class LDLEN_ParameterHandler : ITryExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[] 
        {
            typeof(LDLEN) 
        };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            IDataElement dataElement = cur.EvalStack.Peek();

            if (dataElement.TryGetParameter(cur, out IArrayParameter array))
            {
                // if (array.GetIsNull()) throw NullReferenceException();

                cur.EvalStack.Pop();
                UnsignedInt4 lengthDE = cur.GetLengthDataElement(array);
                cur.EvalStack.Push(lengthDE);

                retValue = nextRetval;
                return true;
            }

            retValue = null;
            return false;
        }
    }

    public class LDELEM_ParameterHandler : IPreExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[]
        {
            typeof(LDELEM)
        };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            INumericElement indexElement = (INumericElement) cur.EvalStack.Peek(0);
            ObjectReference arrayReference = (ObjectReference) cur.EvalStack.Peek(1);

            if (arrayReference.Location == ObjectReference.Null.Location)
            {
                // we have null reference => do nothing and let default behavior handle null reference exception
                return;
            }

            if (arrayReference.TryGetParameter(cur, out IArrayParameter arrayParameter))
            {
                int index = indexElement.ToInt4(false).Value;
                AllocatedArray allocatedArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayReference];
                if (index < 0 || index >= allocatedArray.Fields.Length)
                {
                    // outside of the bounds => do nothing and let default behavior handle index out of bounds
                    return;
                }

                IDataElement itemElement = allocatedArray.Fields[index];

                if (itemElement.TryGetParameter(cur, out IParameter itemParameter))
                {
                    // the item has already been initialized => do nothing
                    return;
                }

                cur.TryGetParameterStore(out ParameterStore store);

                if (!arrayParameter.TryGetItem(index, out itemParameter))
                {
                    // array parameter has no information about the item => create it with default value
                    // create the parameter with execution context
                    itemParameter = store.ExecutionContext.CreateParameter(allocatedArray.Type.ToTypeSig());
                    arrayParameter.SetItem(index, itemParameter);

                    // add it to the base context as well => we are lazily initializing start state...
                    IParameter baseItemParameter = itemParameter.Clone(store.BaseContext);
                    store.BaseContext.Parameters.Add(baseItemParameter.Reference, baseItemParameter);
                    arrayParameter.Reference.Resolve<IArrayParameter>(store.ExecutionContext).SetItem(index, baseItemParameter);
                }

                allocatedArray.Fields[index] = itemParameter.AsDataElement(cur);
            }

            return;
        }
    }
}
