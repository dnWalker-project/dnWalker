using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.Symbolic
{
    [NativePeer(typeof(String), "op_Equality", "get_Length")]
    public class SystemString : SymbolicNativePeerBase
    {
        public override void Handle(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            switch (method.Name)
            {
                case "op_Equality":
                    op_Equality(method, args, cur, returnValue);
                    break;

                case "get_Length":
                    get_Length(method, args, cur, returnValue);
                    break;
            }
        }

#pragma warning disable IDE1006 // Naming Styles - method name must match native method names
        private static void op_Equality(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] expressions))
            {
                IDataElement result = cur.EvalStack.Peek();

                ExpressionFactory ef = cur.GetExpressionFactory();
                result.SetExpression(cur, ef.MakeEqual(expressions[0], expressions[1]));
            }
        }
#pragma warning restore IDE1006 // Naming Styles

#pragma warning disable IDE1006 // Naming Styles - method name must match native method names
        private static void get_Length(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] expressions))
            {
                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur,  new StringOperationExpression(Operator.StringLength, expressions));
            }
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
