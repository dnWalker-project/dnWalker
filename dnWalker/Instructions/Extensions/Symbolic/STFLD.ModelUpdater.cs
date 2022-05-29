using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Utils;

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
    public abstract partial class STFLD
    {
        public class ModelUpdater : STFLD
        {
            public override IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
            {
                IDataElement instance = cur.EvalStack.Peek(1).ResolvePointer();
                IDataElement value = cur.EvalStack.Peek(0);

                IIEReturnValue returnValue = next(baseExecutor, cur);

                if (returnValue != InstructionExecBase.ehLookupRetval &&
                    cur.TryGetSymbolicContext(out SymbolicContext context))
                {
                    if (ExpressionUtils.GetExpressions(cur, instance, out Expression instanceExpression))
                    {
                        Debug.Assert(instanceExpression is VariableExpression);
                        IVariable instanceVar = ((VariableExpression)instanceExpression).Variable;

                        IField field = ((ObjectModelInstructionExec)baseExecutor).GetFieldDefinition();

                        IValue modelValue;
                        // field is primitive type
                        if (field.FieldSig.Type.IsPrimitive || field.FieldSig.Type.IsString())
                        {
                            // 1. primitive value type field or string
                            // just convert data element value to IValue
                            modelValue = value.AsModelValue(field.FieldSig.Type);
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

                        context.Update(instanceVar, field, modelValue);
                    }
                }

                return returnValue;
            }
        }
    }
}
