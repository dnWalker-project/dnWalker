using dnlib.DotNet;
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

namespace dnWalker.NativePeers
{
    public class SystemMath : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (method.FullName == "System.Double System.Math::Ceiling(System.Double)")
            {
                var dataElement = new Float8(Math.Ceiling(((Float8)args[0]).Value));

                var symb = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(args[0], "expression", out var expression);
                if (symb)
                {
                    Expression ceilingExpression = Expression.Negate(
                        Expression.Convert(
                            Expression.Convert(
                                Expression.Negate(expression), typeof(int)),
                            typeof(double)));
                    dataElement.SetExpression(ceilingExpression, cur);
                }
                cur.EvalStack.Push(dataElement);
                iieReturnValue = InstructionExecBase.nextRetval;
                return true;
            }

            if (method.FullName == "System.Double System.Math::Sqrt(System.Double)")
            {
                var arg = (Float8)args[0];
                var value = arg.Value;
                var dataElement = new Float8(Math.Sqrt(value));

                var symb = cur.PathStore.CurrentPath.TryGetObjectAttribute<Expression>(arg, "expression", out var expression);
                if (symb)
                {
                    Expression sqrtExpression = Expression.Call(typeof(Math).GetMethod("Sqrt"), expression);
                    dataElement.SetExpression(sqrtExpression, cur);
                }
                cur.EvalStack.Push(dataElement);
                iieReturnValue = InstructionExecBase.nextRetval;
                return true;
            }

            iieReturnValue = null;
            return false;
        }
    }
}
