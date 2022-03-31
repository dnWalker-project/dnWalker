using dnlib.DotNet.Emit;

using dnWalker.Parameters;
using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.Parameters
{
    public class CEQ : IInstructionExecutor
    {
        private static readonly OpCode[] _supportedCodes = new OpCode[] { OpCodes.Ceq };

        public virtual IEnumerable<OpCode> SupportedOpCodes
        {
            get
            {
                return _supportedCodes;
            }
        }

        public IIEReturnValue Execute(InstructionExecBase baseExecutor, ExplicitActiveState cur, InstructionExecution next)
        {
            // the problem is following:
            // - we need for the CEQ result to have attached proper IsNull or SameAs expression


            IDataElement lhs = cur.EvalStack.Peek(1);
            IDataElement rhs = cur.EvalStack.Peek(0);

            IIEReturnValue returnValue = next(baseExecutor, cur);

            if (lhs is not ObjectReference lhsOR ||
                rhs is not ObjectReference rhsOR)
            {
                // we are checking only the object reference equality...
                return returnValue;
            }

            IReferenceTypeParameter lhsParameter = null;
            IReferenceTypeParameter rhsParameter = null;

            if (!cur.TryGetParameterStore(out ParameterStore store) ||
                (!lhs.TryGetParameter(cur, out lhsParameter) &&
                 !rhs.TryGetParameter(cur, out rhsParameter)))
            {
                // store is null OR (neither lhs nor rhs are reference type parameter)
                return returnValue;
            }

            IDataElement resultDE = cur.EvalStack.Peek();
            bool result = resultDE.ToBool();

            // we need to handle 2 situations:
            // 1) null check, e.i.
            //  ld... obj
            //  ldnull
            //  ceq
            // 2) object identity, e.i.
            //  ld...
            //  ld...
            //  ceq

            if (lhsParameter != null &&
                rhsParameter == null &&
                rhsOR.IsGlobalNull())
            {
                // 1. situation
                Expression isNull = lhsParameter.GetIsNullExpression(cur);
                //resultDE.SetExpression(result ? isNull : Expression.Not(isNull), cur);
                resultDE.SetExpression(isNull, cur);
            }
            else if (lhsParameter != null &&
                     rhsParameter != null)
            {
                Expression refsEqual = lhsParameter.GetReferenceEqualsExpression(rhsParameter, cur);
                //resultDE.SetExpression(result ? refsEqual : Expression.Not(refsEqual), cur);
                resultDE.SetExpression(refsEqual, cur);
            }

            return returnValue;
        }
    }
}
