using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Parameters;
using dnWalker.Symbolic;

using MMC;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static MMC.InstructionExec.InstructionExecBase;

namespace dnWalker.Instructions.Extensions
{
    public static class ParameterHandlersInstructionFactoryExtensions
    {
        public static ExtendableInstructionFactory AddParameterHandlers(this ExtendableInstructionFactory factory)
        {
            factory.RegisterExtension(new LDELEM_ParameterLoaderExtension());
            factory.RegisterExtension(new LDLEN_ParameterLoaderExtension());



            return factory;
        }
    }

    /// <summary>
    /// When a data element with a parameter is loaded from an array to stack, the data element is initialized, proper symbolic expression is assigned and marked as used in this execution.
    /// </summary>
    public class LDELEM_ParameterLoaderExtension : IPreExecuteInstructionExtension
    {
        public bool CanExecute(Code code)
        {
            return code == Code.Ldlen;
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            // idx can be symbolic and/or parametric
            // array can be parametric

            Int4 idx = (Int4)cur.EvalStack.Peek(0);
            ObjectReference arrayRef = (ObjectReference)cur.EvalStack.Peek(1);

            // if array is parametric => make sure the element exists, if not, create it and add the parameter stuff
            if (arrayRef.TryGetParameter(cur, out IArrayParameter arrayParameter))
            {
                int index = idx.Value;

                AllocatedArray allocatedArray = cur.DynamicArea.Allocations[arrayRef] as AllocatedArray;

                if (allocatedArray != null && index >= 0 && index < allocatedArray.Fields.Length)
                {
                    IDataElement item = allocatedArray.Fields[index];

                    // either it was already initialized before (else) or not (if)
                    if (!item.TryGetParameter(cur, out IParameter itemParameter))
                    {
                        // we need to get the parameter instance
                        if (!arrayParameter.TryGetItem(index, out itemParameter))
                        {
                            // no parameter is specified for this index => create a new one with default values
                            TypeSig elementTypeSig = cur.DefinitionProvider.GetTypeSig(arrayParameter.ElementTypeName);
                            itemParameter = ParameterFactory.CreateParameter(elementTypeSig);
                            arrayParameter.SetItem(index, itemParameter);
                        }

                        // now item should be associated with the proper parameter
                        item = itemParameter.AsDataElement(cur);

                        //cur.ParentWatcher.RemoveParentFromChild(arrayRef, allocatedArray.Fields[index], cur.Configuration.MemoisedGC);
                        //allocatedArray.Fields[index] = item;
                        //cur.ParentWatcher.AddParentToChild(arrayRef, item, cur.Configuration.MemoisedGC);

                        allocatedArray.Fields[index] = item;
                    }
                }
            }
        }
    }

    /// <summary>
    /// When a length of array with a parameter is loaded from an array to stack, the data element is initialized, proper symbolic expression is assigned and marked as used in this execution.
    /// </summary>
    public class LDLEN_ParameterLoaderExtension : ITryExecuteInstructionExtension
    {
        public bool CanExecute(Code code)
        {
            return code == Code.Ldlen;
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            ObjectReference arrayRef = (ObjectReference)cur.EvalStack.Peek(0);


            if (arrayRef.TryGetParameter(cur, out IArrayParameter arrayParameter))
            {
                cur.EvalStack.Pop(); // actually pop the element

                UnsignedInt4 lengthDE = new UnsignedInt4((uint)arrayParameter.Length);
                Expression lengthExpression = arrayParameter.GetLengthExpression(cur);
                lengthDE.SetExpression(lengthExpression, cur);

                cur.EvalStack.Push(lengthDE);
                retValue = nextRetval;
                return true;
            }

            retValue = null;
            return false;
        }
    }

    public class LDFLD_ParameterLoaderExtension : IPreExecuteInstructionExtension
    {
        public bool CanExecute(Code code)
        {
            return code == Code.Ldfld;
        }

        private static bool TryGetAllocation(IDataElement dataElement, ExplicitActiveState cur, out ObjectReference objectReference, out AllocatedObject allocatedObject)
        {
            if (dataElement is ObjectReference or)
            {
                objectReference = or;
                allocatedObject = cur.DynamicArea.Allocations[objectReference] as AllocatedObject;
            }
            else if (dataElement is MethodMemberPointer methodMemberPointer)
            {
                // Points to a ObjectReference
                objectReference = (ObjectReference)(methodMemberPointer).Value;
                allocatedObject = cur.DynamicArea.Allocations[objectReference] as AllocatedObject;
            }
            else
            {
                objectReference = ObjectReference.Null;
                allocatedObject = null;
            }

            return allocatedObject != null;
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (instruction is ObjectModelInstructionExec objectModel)
            {
                IDataElement dataElement = cur.EvalStack.Peek();
                if (dataElement.TryGetParameter(cur, out IObjectParameter objectParameter) &&
                    TryGetAllocation(cur.EvalStack.Peek(), cur, out ObjectReference objectReference, out AllocatedObject allocatedObject))
                {
                    FieldDef fieldDef = objectModel.GetFieldDefinition();
                    int fieldOffset = objectModel.GetFieldOffset(fieldDef.DeclaringType);


                    IDataElement fieldValue = allocatedObject.Fields[fieldOffset];

                    if (!fieldValue.TryGetParameter(cur, out IParameter fieldParameter))
                    {
                        // the field is not associated with any parameter => set it up...
                        if (!objectParameter.TryGetField(fieldDef.Name, out fieldParameter))
                        {
                            // object parameter has no information about the field => intialize it as default
                            fieldParameter = ParameterFactory.CreateParameter(fieldDef.FieldType);
                            objectParameter.SetField(fieldDef.Name, fieldParameter);
                        }

                        fieldValue = fieldParameter.AsDataElement(cur);

                        // store the new IDataElement into the allocated object
                        // Can be the case that an object reference was written, thereby changing the object graph
                        // on the other hand, we are only overwriting uninitialized fields, e.h. ObjectReference.Null and 0s...

                        //ObjectEscapePOR.UpdateReachability(allocatedObject.ThreadShared, allocatedObject.Fields[fieldOffset], fieldValue, cur);
                        //allocatedObject.Fields[fieldOffset] = fieldValue;
                        //cur.ParentWatcher.AddParentToChild(objectReference, fieldValue, cur.Configuration.MemoisedGC);

                        allocatedObject.Fields[fieldOffset] = fieldValue;
                    }
                }
            }
        }
    }
}
