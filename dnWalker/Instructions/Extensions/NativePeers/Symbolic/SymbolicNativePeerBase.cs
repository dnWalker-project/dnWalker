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
    public abstract class SymbolicNativePeerBase : ISymbolicNativePeer
    {
        public abstract void Handle(MethodDef method, DataElementList args, ExplicitActiveState cur, IIEReturnValue returnValue);

        protected static bool TryGetExpressions(ExplicitActiveState cur, DataElementList args, out Expression[] expressions)
        {
            Expression[] tmp = new Expression[args.Length];

            bool any = false;
            for (int i = 0; i < tmp.Length; ++i)
            {
                any |= args[i].TryGetExpression(cur, out tmp[i]);
            }

            expressions = any ? tmp : null;
            return any;
        }
    }
}
