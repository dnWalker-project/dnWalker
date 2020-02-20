using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;

namespace dnWalker.NativePeers
{
    public class SystemRandom : NativePeer
    {
        private static Random _random;

        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (methodDef.FullName == "System.Int32 System.Random::Next(System.Int32)")
            {
                cur.EvalStack.Push(cur.DefinitionProvider.CreateDataElement(_random.Next(((Int4)args[1]).Value)));
                iieReturnValue = InstructionExecBase.nextRetval;
                return true;
            }

            throw new NotImplementedException();
        }

        public override bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
        {
            ObjectReference objectRef = cur.DynamicArea.AllocateObject(
                cur.DynamicArea.DeterminePlacement(false),
                methodDef.DeclaringType);
            AllocatedObject allocatedObject = cur.DynamicArea.Allocations[objectRef] as AllocatedObject;

            _random = new Random(((Int4)args[1]).Value);

            cur.EvalStack.Push(objectRef);
            return true;
        }
    }
}
