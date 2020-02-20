﻿using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System.Text;

namespace dnWalker.NativePeers
{
    public class SystemString : NativePeer
    {
        public override bool TryGetValue(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            IDataElement dataElement = null;

            if (method.FullName == "System.Boolean System.String::op_Equality(System.String,System.String)")
            {
                dataElement = new Int4(args[0].CompareTo(args[1]) == 0 ? 1 : 0);
            }

            if (method.FullName == "System.String System.String::Trim()")
            {
                var s = (ConstantString)args[0];
                dataElement = new ConstantString(s.Value.Trim());
            }

            if (method.FullName == "System.Int32 System.String::get_Length()")
            {
                var s = (ConstantString)args[0];
                dataElement = new Int4(s.Value.Length);
            }

            if (method.FullName.StartsWith("System.String System.String::Concat("))
            {
                var sb = new StringBuilder();
                foreach (var arg in args)
                {
                    if (arg is ConstantString cs)
                    {
                        sb.Append(cs.Value);
                        continue;
                    }
                    sb.Append(arg.ToString());
                }
                dataElement = new ConstantString(sb.ToString());
            }

            if (method.FullName == "System.Boolean System.String::IsNullOrEmpty(System.String)")
            {

            }

            if (dataElement != null)
            {
                cur.EvalStack.Push(dataElement);
                iieReturnValue = InstructionExecBase.nextRetval;
                return true;
            }

            iieReturnValue = null;
            return false;
        }
    }
}
