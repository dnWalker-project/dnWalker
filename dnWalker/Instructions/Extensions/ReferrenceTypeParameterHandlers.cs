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
    public class BRTRUE_ParameterHandler : IPreExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(BRTRUE);
            }
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            IDataElement operand = cur.EvalStack.Peek();
            if (operand.TryGetParameter(cur, out IReferenceTypeParameter referenceTypeParameter))
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Push(cur.GetIsNotNullDataElement(referenceTypeParameter));
            }
        }
    }

    public class BRFALSE_ParameterHandler : IPreExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(BRFALSE);
            }
        }

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            IDataElement operand = cur.EvalStack.Peek();
            if (operand.TryGetParameter(cur, out IReferenceTypeParameter referenceTypeParameter))
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Push(cur.GetIsNotNullDataElement(referenceTypeParameter));
            }
        }
    }

    public class CEQ_ReferenceTypeParameterHandler : ITryExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(CEQ);
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            lhs.TryGetParameter(cur, out IReferenceTypeParameter lhsRefType);
            rhs.TryGetParameter(cur, out IReferenceTypeParameter rhsRefType);

            cur.TryGetParameterContext(out IParameterContext context);

            // lhs is IReferenceTypeParameter, rhs is IReferenceTypeParameter
            // -> check whether one is an alias of the other... the GetRefsEqualDataElement(..., ...)
            if (lhsRefType != null && rhsRefType != null)
            {
                // both are IReferenceTypeParmaeter
                cur.EvalStack.Pop();
                cur.EvalStack.Pop();

                cur.EvalStack.Push(cur.GetRefsEqualDataElement(lhsRefType, rhsRefType));

                retValue = nextRetval;
                return true;
            }
            // lhs is IReferenceTypeParameter, rhs is an ObjectReference, null
            // -> check lhs.isNull value
            else if (lhsRefType != null && rhs.Equals(ObjectReference.Null))
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Pop();

                cur.EvalStack.Push(cur.GetIsNullDataElement(lhsRefType));

                retValue = nextRetval;
                return true;
            }
            // rhs is IReferenceTypeParameter, lhs is an ObjectReference, null
            // -> check rhs.isNull value
            else if (lhs.Equals(ObjectReference.Null) && rhsRefType != null)
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Pop();

                cur.EvalStack.Push(cur.GetIsNullDataElement(rhsRefType));

                retValue = nextRetval;
                return true;
            }

            // they are not parameters
            // => do nothing, let default behavior take care of it
            // one is parameter, other is not and is not null
            // => do nothing, let default behavior take care of it (their ObjectReference.Location will be different...)

            retValue = null;
            return false;

        }
    }

    public class CNE_ReferenceTypeParameterHandler : ITryExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(CGT);
                yield return typeof(CLT);
            }
        }

        public bool TryExecute(InstructionExecBase instruction, ExplicitActiveState cur, [NotNullWhen(true)] out IIEReturnValue retValue)
        {
            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            lhs.TryGetParameter(cur, out IReferenceTypeParameter lhsRefType);
            rhs.TryGetParameter(cur, out IReferenceTypeParameter rhsRefType);

            cur.TryGetParameterContext(out IParameterContext context);
            
            // lhs is IReferenceTypeParameter, rhs is an ObjectReference, null
            // -> check lhs.isNull value
            if (lhsRefType != null && rhs.Equals(ObjectReference.Null))
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Pop();

                cur.EvalStack.Push(cur.GetIsNotNullDataElement(lhsRefType));

                retValue = nextRetval;
                return true;
            }
            // rhs is IReferenceTypeParameter, lhs is an ObjectReference, null
            // -> check rhs.isNull value
            else if (lhs.Equals(ObjectReference.Null) && rhsRefType != null)
            {
                cur.EvalStack.Pop();
                cur.EvalStack.Pop();

                cur.EvalStack.Push(cur.GetIsNotNullDataElement(rhsRefType));

                retValue = nextRetval;
                return true;
            }

            retValue = null;
            return false;
        }
    }
}
