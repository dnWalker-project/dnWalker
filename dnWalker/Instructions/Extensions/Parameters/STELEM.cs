using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Parameters;
using dnWalker.TypeSystem;

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
    public class STELEM : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Stelem };

        public IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
        {
            // gather the operands
            IDataElement elementToStore = cur.EvalStack.Peek();
            int index = ((INumericElement)cur.EvalStack.Peek(1)).ToInt4(false).Value;
            ObjectReference arrayReference = (ObjectReference)cur.EvalStack.Peek(2);

            IArrayParameter arrayParameter = null;
            IParameter elementParameter = null;

            // no parameter system is available or both the array and the value to store is not parametrized
            if (!cur.TryGetParameterStore(out ParameterStore store) ||
                (!arrayReference.TryGetParameter(cur, out arrayParameter) &&
                 !elementToStore.TryGetParameter(cur, out elementParameter)))
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
            AllocatedArray allocatedArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayReference];

            if (index < 0 || index >= allocatedArray.Fields.Length)
            {
                // index out of range
                return next(instruction, cur);
            }

            // no exception will be raised
            // either the array is parametrized OR the elementToStore is parametrized
            // either way, ensure both are...
            TypeSignature elementType = new TypeSignature(allocatedArray.Type);
            if (arrayParameter == null)
            {
                arrayParameter = (IArrayParameter)arrayReference.GetOrCreateParameter(cur, elementType.CreateArray());
            }
            if (elementParameter == null)
            {
                // TODO: refactor the ObjectReference.Null usages...
                if (elementToStore is ObjectReference or && or.HashCode == ObjectReference.Null.HashCode)
                {
                    // the val is the GLOBAL null (ObjectReference.Null) => the GetOrCreate would actually associate the global NULL with one concrete parameter instance
                    // we need to avoid this => pop it and push there a new, unique NULL value
                    elementToStore = new ObjectReference(0);
                    cur.EvalStack.Pop();
                    cur.EvalStack.Push(elementToStore);
                }

                elementParameter = elementToStore.GetOrCreateParameter(cur, elementType);
            }

            // now we can run the actual execution
            IIEReturnValue returnValue = next(instruction, cur);

            // now we can execute the instruction on the parameter level
            arrayParameter.SetItem(index, elementParameter);

            return returnValue;
        }
    }
}
