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
    [NativePeer(typeof(System.Diagnostics.Debug), "Assert")]
    public class SystemDiagnosticsDebugAssert : SymbolicNativePeerBase
    {
        public override void Handle(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue)
        {
            if (args[0].TryGetExpression(cur, out Expression conditionExpression))
            {
                DecisionHelper.Assert(cur, args[0].ToBool(), conditionExpression, Expression.MakeNot(conditionExpression));
            }
        }
    }
}
