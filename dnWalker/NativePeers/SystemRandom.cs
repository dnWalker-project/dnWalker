using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using dnWalker.ChoiceGenerators;

namespace dnWalker.NativePeers
{
    public class SystemRandom : NativePeer
    {
        private static Random _random;

        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (methodDef.FullName == "System.Int32 System.Random::Next(System.Int32)")
            {
                var maxValue = ((Int4) args[1]).Value;
                var cg = new IntChoiceFromValueSet(0, maxValue);

                if (cur.Configuration.GetCustomSetting<bool>("evaluateRandom"))
                {
                    foreach (var arg in args)
                    {
                        cur.EvalStack.Push(arg);
                    }

                    cur.SetNextChoiceGenerator(cg);
                    
                    foreach (var arg in args)
                    {
                        cur.EvalStack.Pop();
                    }
                }

                cur.EvalStack.Push(cg.GetNextChoice());
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
