using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;

using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public partial class SymbolicContext
    {
        public void Update(IVariable instance, IField field, IValue value)
        {
            if (!_outputModel.TryGetValue(instance, out IValue instanceValue))
            {
                Debug.Fail("The instance variable should have resolved...");
            }

            if (!_outputModel.HeapInfo.TryGetNode((Location)instanceValue, out IHeapNode node))
            {
                Debug.Fail("The instance location should have resolved...");
            }

            IObjectHeapNode objectNode = (IObjectHeapNode)node;
            objectNode.SetField(field, value);
        }

        public void Update(IVariable array, int index, IValue value)
        {
            if (!_outputModel.TryGetValue(array, out IValue instanceValue))
            {
                Debug.Fail("The instance variable should have resolved...");
            }

            if (!_outputModel.HeapInfo.TryGetNode((Location)instanceValue, out IHeapNode node))
            {
                Debug.Fail("The instance location should have resolved...");
            }

            IArrayHeapNode arrayNode = (IArrayHeapNode)node;
            arrayNode.SetElement(index, value);
        }

        public void SetReturnValue(IDataElement retVal, MethodDef method, ExplicitActiveState cur, TypeSig retType)
        {
            IModel outputModel = _outputModel;

            IValue modelValue;
            if (retVal is ObjectReference or)
            {
                modelValue = ProcessExistingReference(or, cur);
            }
            else
            {
                modelValue = retVal.AsModelValue(retType);
            }

            outputModel.SetValue(new ReturnValueVariable(method), modelValue);
        }
    }
}
