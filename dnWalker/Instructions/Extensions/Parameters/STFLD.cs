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
    public class STFLD : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Stfld };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
        {
            ObjectReference objectReference = (ObjectReference)cur.EvalStack.Peek(1);
            IDataElement valueToStore = cur.EvalStack.Peek(0);

            IObjectParameter objectParameter = null;
            IParameter valueParameter = null;

            // no parameter system is available or both the object and the value to store is not parametrized
            cur.TryGetParameterStore(out ParameterStore store);
            objectReference.TryGetParameter(cur, out objectParameter);
            valueToStore.TryGetParameter(cur, out valueParameter);

            if (store == null || 
                (objectParameter == null && valueParameter == null))
            {
                return next(instruction, cur);
            }

            // check for exceptions
            // - null reference
            if (objectReference.IsNull()) // check for non-static field as well
            {
                return next(instruction, cur);
            }

            // - missing field
            // TODO: add checking whether the field exists

            // no exception will be raised
            // either the object is parametrized OR the valueToStore is parametrized
            // either way, ensure both are...
            AllocatedObject allocatedObject = (AllocatedObject)cur.DynamicArea.Allocations[objectReference];
            ObjectModelInstructionExec objectModel = (ObjectModelInstructionExec)instruction;
            FieldDef field = objectModel.GetFieldDefinition();

            TypeSignature objectType = new TypeSignature(allocatedObject.Type);
            TypeSignature fieldType = new TypeSignature(field.FieldType.ToTypeDefOrRef());
            if (objectParameter == null)
            {
                objectParameter = (ObjectParameter)objectReference.GetOrCreateParameter(cur, objectType);
            }
            if (valueParameter == null)
            {
                // TODO: refactor the ObjectReference.Null usages...
                if (valueToStore is ObjectReference or && or.HashCode == ObjectReference.Null.HashCode)
                {
                    // the val is the GLOBAL null (ObjectReference.Null) => the GetOrCreate would actually associate the global NULL with one concrete parameter instance
                    // we need to avoid this => pop it and push there a new, unique NULL value
                    valueToStore = new ObjectReference(0);
                    cur.EvalStack.Pop();
                    cur.EvalStack.Push(valueToStore);
                }

                valueParameter = valueToStore.GetOrCreateParameter(cur, fieldType);
            }

            // now we can run the actual execution
            IIEReturnValue returnValue = next(instruction, cur);

            // now we can execute the instruction on the parameter level
            objectParameter.SetField(field.Name, valueParameter);

            return returnValue;
        }
    }
}
