using dnlib.DotNet;

using dnWalker.Instructions.Extensions.Symbolic;
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
    [NativePeer("System.Math", "Max", "Min", "Abs", "Sin", "Cos")]
    public class SystemMath : SymbolicNativePeerBase
    {
        public override void Handle(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            switch (method.Name)
            {
                case "Max":
                    Max(method, args, cur, returnValue);
                    break;

                case "Min":
                    Min(method, args, cur, returnValue);
                    break;

                case "Abs":
                    Abs(method, args, cur, returnValue);
                    break;

                case "Pow":
                    Pow(method, args, cur, returnValue);
                    break;

                case "Sin":
                    Sin(method, args, cur, returnValue);
                    break;

                case "Cos":
                    Cos(method, args, cur, returnValue);
                    break;
            }
        }

        private void Sin(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "sin", ops));

                // TODO add range constraint on the result

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeGreaterThanOrEqual(ops[0], ef.MakeIntegerConstant(0)), ef.MakeLessThan(ops[0], ef.MakeIntegerConstant(0)));
            }
        }

        private void Cos(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "cos", ops));

                // TODO add range constraint on the result

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeGreaterThanOrEqual(ops[0], ef.MakeIntegerConstant(0)), ef.MakeLessThan(ops[0], ef.MakeIntegerConstant(0)));
            }
        }

        private void Pow(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "pow", ops));

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeGreaterThanOrEqual(ops[0], ef.MakeIntegerConstant(0)), ef.MakeLessThan(ops[0], ef.MakeIntegerConstant(0)));
            }
        }

        private void Abs(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "abs", ops));

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeGreaterThanOrEqual(ops[0], ef.MakeIntegerConstant(0)), ef.MakeLessThan(ops[0], ef.MakeIntegerConstant(0)));
            }
        }

        private void Min(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "min", ops));

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeLessThan(ops[0], ops[1]), ef.MakeGreaterThanOrEqual(ops[0], ops[1]));
            }
        }

        private void Max(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (TryGetExpressions(cur, args, out Expression[] ops))
            {
                ExpressionFactory ef = cur.GetExpressionFactory();

                IDataElement result = cur.EvalStack.Peek();
                result.SetExpression(cur, ef.MakeGeneric(method.ReturnType, "max", ops));

                //int d = result.Equals(args[0]) ? 0 : 1;

                //DecisionHelper.MakeDecision(cur, d, null, ef.MakeGreaterThan(ops[0], ops[1]), ef.MakeLessThanOrEqual(ops[0], ops[1]));
            }
        }
    }
}
