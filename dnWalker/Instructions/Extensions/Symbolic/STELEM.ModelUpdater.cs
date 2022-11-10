using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;


using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IVariable = dnWalker.Symbolic.IVariable;


namespace dnWalker.Instructions.Extensions.Symbolic
{
    public abstract partial class STELEM
    {
        public class ModelUpdater : STELEM
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                IDataElement array = cur.EvalStack.Peek(2).ResolvePointer();
                IDataElement index = cur.EvalStack.Peek(1);
                IDataElement value = cur.EvalStack.Peek(0);

                IIEReturnValue returnValue = next(baseExecutor, cur);

                if (returnValue != InstructionExecBase.ehLookupRetval &&
                    cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    if (ExpressionUtils.GetExpressions(cur, array, out Expression arrayExpression))
                    {
                        Debug.Assert(arrayExpression is VariableExpression);
                        IVariable arrayVar = ((VariableExpression)arrayExpression).Variable;

                        int idx = ((INumericElement)index).ToInt4(false).Value;

                        IValue modelValue;
                        
                        // field is primitive type
                        if (value is not ObjectReference)
                        {
                            // 1. primitive value type field or string
                            // just convert data element value to IValue
                            modelValue = value.AsModelValue(cur.DynamicArea.Allocations[(ObjectReference)array].Type.ToTypeSig());
                        }
                        // field is reference type
                        else
                        {
                            if (ExpressionUtils.GetExpressions(cur, value, out Expression valueExpression))
                            {
                                // already initialized...
                                Debug.Assert(valueExpression is VariableExpression);
                                IVariable valueVariable = ((VariableExpression)valueExpression).Variable;
                                modelValue = context.OutputModel.GetValueOrDefault(valueVariable);
                            }
                            else
                            {
                                // not yet initialized...
                                modelValue = context.ProcessExistingReference((ObjectReference)value, cur);
                            }
                        }

                        context.Update(arrayVar, idx, modelValue);
                    }
                }

                return returnValue;
            }
        }
    }
}
