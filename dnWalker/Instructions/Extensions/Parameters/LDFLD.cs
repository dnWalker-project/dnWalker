//using dnlib.DotNet;
//using dnlib.DotNet.Emit;

//using dnWalker.Parameters;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Instructions.Extensions.Parameters
//{
//    public class LDFLD : IInstructionExecutor
//    {
//        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Ldfld };

//        public virtual IEnumerable<OpCode> SupportedOpCodes
//        {
//            get
//            {
//                return _supportedCodes;
//            }
//        }

//        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
//        {
//            ObjectReference objectReference = (ObjectReference)cur.EvalStack.Peek(0);

//            cur.TryGetParameterStore(out ParameterStore store);
//            objectReference.TryGetParameter(cur, out IObjectParameter objectParameter);

//            if (store == null || objectParameter == null)
//            {
//                return next(instruction, cur);
//            }

//            // check for exceptions
//            // - null reference
//            if (objectReference.IsNull())
//            {
//                return next(instruction, cur);
//            }

//            // - missing field
//            // TODO: add checking whether the field exists

//            // the execution will not throw any exception & we have a parametrized object
//            // - ensure that the AllocatedObject actually has the proper element initialized
//            AllocatedObject allocatedObject = (AllocatedObject)cur.DynamicArea.Allocations[objectReference];
//            ObjectModelInstructionExec objectModel = (ObjectModelInstructionExec)instruction;
//            FieldDef field = objectModel.GetFieldDefinition();
//            int fieldOffset = objectModel.GetFieldOffset(field.DeclaringType);

//            IDataElement fieldElement = allocatedObject.Fields[fieldOffset];

//            if (!fieldElement.TryGetParameter(cur, out IParameter fieldParameter))
//            {
//                // the value at the specified field is not yet associated with any parameter
//                // 2 options:
//                // 1) no parameter is yet defined for the location => create a default one
//                // 2) a parameter is already defined do nothing
//                if (!objectParameter.TryGetField(field.Name, out fieldParameter))
//                {
//                    // 1st option, we need to create it
//                    // 2 options:
//                    // 1) the instance is an input => we need to create the item parameter within the BASE set & copy into the EXEC set
//                    // 2) the instance is a fresh object => we create the item parameter within the EXEC set and the BASE set is completely ignored
//                    if (objectParameter.Reference.TryResolve(store.BaseSet,  out IObjectParameter baseObjectParameter))
//                    {
//                        IParameter baseFieldParameter = store.BaseSet.CreateParameter(field.FieldType);
//                        baseObjectParameter.SetField(field.Name, baseFieldParameter);

//                        fieldParameter = baseFieldParameter.Clone(store.ExecutionSet);
//                    }
//                    else
//                    {
//                        fieldParameter = store.ExecutionSet.CreateParameter(field.FieldType);
//                    }

//                    // make the connection of the field parameter and the object parameter
//                    objectParameter.SetField(field.Name, fieldParameter);
//                }
//                // set the lazily initialized data element into the allocated object
//                allocatedObject.Fields[fieldOffset] = fieldParameter.AsDataElement(cur);
//            }

//            // do the actual execution
//            IIEReturnValue retValue = next(instruction, cur);

//            return retValue;
//        }
//    }
//}
