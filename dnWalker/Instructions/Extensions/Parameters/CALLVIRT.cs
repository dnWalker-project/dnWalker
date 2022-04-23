//using dnlib.DotNet;
//using dnlib.DotNet.Emit;

//using dnWalker.Parameters;
//using dnWalker.Symbolic;
//using dnWalker.Symbolic.Heap;
//using dnWalker.TypeSystem;

//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Instructions.Extensions.Parameters
//{
//    public class CALLVIRT : IInstructionExecutor
//    {
//        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Callvirt };

//        public IEnumerable<OpCode> SupportedOpCodes
//        {
//            get
//            {
//                return _supportedCodes;
//            }
//        }

//        public IIEReturnValue Execute(InstructionExecBase instruction, ExplicitActiveState cur, InstructionExecution next)
//        {
//            // 1st, we check whether the operand (method) is one we can substitute
//            CallInstructionExec callModel = (CallInstructionExec)instruction;
//            MethodDef method = callModel.Method;

//            if (!method.IsAbstract || method.IsStatic)
//            {
//                return next(instruction, cur);
//            }

//            // check whether we can use the parameter system
//            ObjectReference instance = (ObjectReference)cur.EvalStack.Peek(method.GetParamCount());
//            Allocation allocation = cur.DynamicArea.Allocations[instance];
//            if (!allocation.TryGetHeapNode(cur, out IHeapNode heapNode))
//            {
//                //  the instance itself is not a parameter
//                return next(instruction, cur);
//            }

//            // check whether the instance has an overload for the method
//            if (instance.TryFindVirtualMethod(method, cur, out _))
//            {
//                return next(instruction, cur);
//            }

//            // so, now we have a parametrized instance and an abstract method => we will handle this execution here
//            // pop out the arguments, we don't care
//            // TODO: setup precondition & postcondition summaries for the faked methods, so that the operands may be used...
//            for (int i = 0; i < method.GetParamCount() + 1; ++i)
//            {
//                cur.EvalStack.Pop();
//            }

//            if (!method.HasReturnType)
//            {
//                // nothing happens, we do not 'simulate' change of state of the arguments
//                return InstructionExecBase.nextRetval;
//            }

//            // !!! generic method info is lost this way !!!
//            // during the instruction executor creation the IMethod type is transformed into MethodDef
//            // IMethod may be in fact a MethodSpec instance, which is a generic instantiation of a generic MethodDef
//            // in such a case, the CallInstructionExec.Method is actually a generic method without the specified generic arguments !!!
            
//            //int invocation = cur.IncreaseInvocationCount(instance, signature);
//            int invocation = 0; // TODO - where should it be stored?? as the allocation attribute?

//            IObjectHeapNode objNode = heapNode as IObjectHeapNode;
//            if (objNode == null) throw new InvalidOperationException("A non object heap node is attached to a object allocation!!");

//            // ensure that the objectParameter has this invocation specified, if not, create a default
//            IValue symResult = objNode.GetMethodResult(method, invocation);
//            // either symResult was specified => some value
//            // OR it was not specified and a default value was returned, either way, there is NO NEED TO EDIT THE HEAP MODEL
//            // which is great!!




//            if (!objNode.TryGetMethodResult(method, invocation, out IParameter resultParameter))
//            {
//                // there is not any information about the method result
//                // 2 options:
//                // 1) the instance is an input => we need to create the method result parameter within the BASE set & copy into the EXEC set
//                // 2) the instance is a fresh object => we create the method result within the EXEC set and the BASE set is completely ignored
//                //    !! THIS SHOULD NOT HAPPEN !! - fresh objects should be completely defined !!!
//                if (objectParameter.Reference.TryResolve(store.BaseSet, out IObjectParameter baseObjectParameter))
//                {
//                    IParameter baseResultParameter = store.BaseSet.CreateParameter(method.ReturnType);
//                    baseObjectParameter.SetMethodResult(signature, invocation, baseResultParameter);

//                    resultParameter = baseResultParameter.Clone(store.ExecutionSet);
//                }
//                else
//                {
//                    // this is just for debugging purposes to detect & debug out corner cases 
//                    // this branch should NEVER be executed
//                    Debug.Assert(false, "Trying to fake a method on a fresh object!!!");
//                }

//                objectParameter.SetMethodResult(signature, invocation, resultParameter);
//            }

//            cur.EvalStack.Push(resultParameter.AsDataElement(cur));

//            return InstructionExecBase.nextRetval;
//        }
//    }
//}
