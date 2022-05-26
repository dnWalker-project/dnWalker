using System;

using dnlib.DotNet;


using MMC.Data;
using MMC.State;

namespace dnWalker.Instructions.Extensions.Symbolic
{
    public static partial class CALLVIRT
    {
        private static IDataElement[] GetArguments(MethodDef method, ExplicitActiveState cur)
        {
            int argCount = method.GetParamCount() + (method.IsStatic ? 0 : 1);
            IDataElement[] args = new IDataElement[argCount];

            for (int i = 0; i < argCount; ++i)
            {
                args[i] = cur.EvalStack.Peek(argCount - i - 1);
            }

            return args;
        }

        private static ObjectReference GetInstance(MethodDef method, ExplicitActiveState cur)
        {
            return method.IsStatic ? ObjectReference.Null : (ObjectReference)cur.EvalStack.Peek(method.GetParamCount());
        }

    }
}

