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
                    // => initialize a new parameter with default values
                    fieldParameter = store.ExecutionContext.CreateParameter(field.FieldType);
                    objectParameter.SetField(field.Name, fieldParameter);

                    // add it to the base context as well => we are lazily initializing start state...
                    IParameter baseFieldParameter = fieldParameter.Clone(store.BaseContext);
                    store.BaseContext.Parameters.Add(baseFieldParameter.Reference, baseFieldParameter);
                    objectParameter.Reference.Resolve<IObjectParameter>(store.ExecutionContext).SetField(field.Name, baseFieldParameter);
                }

                allocatedObject.Fields[fieldOffset] = fieldParameter.AsDataElement(cur);
            }
        }
    }

    public class CALLVIRT_ParameterHandler : ITryExecuteInstructionExtension
    {
        private static readonly Type[] _insturctions = new Type[] { typeof(CALLVIRT) };

        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                return _insturctions;
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            retValue = null;
            return false;
        }

    }
}
