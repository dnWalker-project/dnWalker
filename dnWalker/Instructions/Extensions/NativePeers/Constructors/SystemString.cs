using dnlib.DotNet;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.Constructors
{
    [NativePeer(typeof(System.String), ".ctor")]
    public class SystemString : ConstructorCallNativePeerBase
    {
        public override bool TryExecute(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            if (method.Parameters[0].Type.IsArray && method.Parameters[0].Type.Next.IsChar())
            {
                var arrayRef = (IReferenceType)args[1];
                var theArray = (AllocatedArray)cur.DynamicArea.Allocations[arrayRef];
                if (theArray.Fields.Length > 0)
                {
                    // TODO improve
                    var s = new string(theArray.Fields.Cast<INumericElement>().Select(i => (char)i.ToInt4(true).Value).ToArray());
                    cur.EvalStack.Push(new ConstantString(s));
                    return Next(out returnValue);
                }

                var dataElement = new ConstantString();// new string((char)Convert.ChangeType(args[0], typeof(char)));
                cur.EvalStack.Push(dataElement);
                return Next(out returnValue);
            }

            return Fail(out returnValue);
        }
    }
}
