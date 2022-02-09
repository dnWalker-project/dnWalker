﻿using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.TypeSystem;

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
    public class LDFLD_ParameterHandler : IPreExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[] 
        {
            typeof(LDFLD) 
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
            ObjectReference objectReference = (ObjectReference)cur.EvalStack.Peek(0);

            if (objectReference.Location == ObjectReference.Null.Location)
            {
                // we have null reference => do nothing and let default behavior handle null reference exception
                return;
            }

            if (objectReference.TryGetParameter(cur, out IObjectParameter objectParameter))
            {
                // prepare allocation
                AllocatedObject allocatedObject = (AllocatedObject)cur.DynamicArea.Allocations[objectReference];

                ObjectModelInstructionExec objectModel = (ObjectModelInstructionExec)instruction;

                // prepare field info
                FieldDef field = objectModel.GetFieldDefinition();
                int fieldOffset = objectModel.GetFieldOffset(field.DeclaringType);

                // check whether the field was not already initialized
                if (allocatedObject.Fields[fieldOffset].TryGetParameter(cur, out IParameter fieldParameter))
                {
                    // already has a parameter => no need to initialize...
                    return;
                }

                cur.TryGetParameterStore(out ParameterStore store);

                if (!objectParameter.TryGetField(field.Name, out fieldParameter))
                {
                    // there is not information about the field
                    // => initialize a new parameter with default values within the base context
                    IParameter baseFieldParameter = store.BaseSet.CreateParameter(field.FieldType);

                    IObjectParameter baseObjectParameter = objectParameter.Reference.Resolve<IObjectParameter>(store.BaseSet);
                    baseObjectParameter.SetField(field.Name, baseFieldParameter);

                    // copy it into the execution context as well
                    fieldParameter = baseFieldParameter.CloneData(store.ExecutionSet);
                    store.ExecutionSet.Parameters.Add(fieldParameter.Reference, fieldParameter);
                    objectParameter.SetField(field.Name, baseFieldParameter);
                }

                allocatedObject.Fields[fieldOffset] = fieldParameter.AsDataElement(cur);
            }
        }
    }

    public class CALLVIRT_ParameterHandler : ITryExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[] { typeof(CALLVIRT) };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            MethodDef methodDefinition = (MethodDef) instruction.Operand;

            if (!methodDefinition.IsAbstract)
            {
                // not an abstract method => do nothing
                retValue = null;
                return false;
            }

            ObjectReference instance = (ObjectReference)cur.EvalStack.Peek(methodDefinition.GetParamCount());
            if (!instance.TryGetParameter(cur, out IObjectParameter objectParameter))
            {
                // the instance is not parametrized => do nothing
                retValue = null;
                return false;
            }

            // we will resolve this instruction
            // we do not care for the arguments => pop them out
            for (int i = 0; i < methodDefinition.GetParamCount() + 1; ++i)
            {
                _ = cur.EvalStack.Pop();
            }

            if (!methodDefinition.HasReturnType)
            {
                // the method returns nothing
                // somehow mock the changes in the instance? - way to hard (if possible...)
                
                retValue = nextRetval;
                return true;
            }

            MethodSignature signature = new MethodSignature(methodDefinition);
            int invocation = cur.IncreaseInvocationCount(instance, signature);


            if (!objectParameter.TryGetMethodResult(signature, invocation, out IParameter resultParameter))
            {
                cur.TryGetParameterStore(out ParameterStore store);

                // there is not any information about the method result
                // it is an uninitialized input
                // => create a default parameter in base context
                IParameter baseResultParameter = store.BaseSet.CreateParameter(methodDefinition.ReturnType);

                IObjectParameter baseObjectParameter = objectParameter.Reference.Resolve<IObjectParameter>(store.BaseSet);
                baseObjectParameter.SetMethodResult(signature, invocation, baseResultParameter);


                // copy it into the execution context as well
                resultParameter = baseResultParameter.CloneData(store.ExecutionSet);
                store.ExecutionSet.Parameters.Add(resultParameter.Reference, resultParameter);
                objectParameter.SetMethodResult(signature, invocation, resultParameter);
            }

            IDataElement resultDataElement = resultParameter.AsDataElement(cur);
            cur.EvalStack.Push(resultDataElement);

            retValue = nextRetval;
            return true;
        }

    }

    public class STFLD_ParameterHandler : IPreExecuteInstructionExtension
    {
        private static readonly Type[] _instructions = new Type[] { typeof(STFLD) };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _instructions;
            }
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            if (cur.TryGetParameterStore(out ParameterStore store))
            {
                IDataElement val = cur.EvalStack.Peek();
                IDataElement toChange = cur.EvalStack.Peek(1);

                if (val.Equals(ObjectReference.Null))
                {
                    // the val is the GLOBAL null => the GetOrCreate would actually associate the global NULL with one concrete parameter instance
                    // we need to avoid this => pop it and push there a new NULL value
                    val = new ObjectReference(0);
                    cur.EvalStack.Pop();
                    cur.EvalStack.Push(1);
                }

                if (toChange is LocalVariablePointer lvp)
                {
                    toChange = lvp.Value;
                }

                if (toChange.TryGetParameter(cur, out IObjectParameter objParameter))
                {
                    if (instruction.Operand is IField fld)
                    {
                        IParameter valParameter = val.GetOrCreateParameter(cur, new TypeSignature(fld.FieldSig.GetFieldType().ToTypeDefOrRef()));
                        objParameter.SetField(fld.Name, valParameter);
                    }
                }
            }
        }
    }
}
