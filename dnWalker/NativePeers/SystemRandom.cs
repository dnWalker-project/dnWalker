﻿using dnlib.DotNet;
using MMC.Data;
using MMC.InstructionExec;
using MMC.State;
using System;
using dnWalker.ChoiceGenerators;
using System.Text;

namespace dnWalker.NativePeers
{
    public class SystemRandom : NativePeer
    {
        public static StringBuilder Sb = new StringBuilder();

        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
        {
            if (methodDef.FullName == "System.Int32 System.Random::Next(System.Int32)")
            {
                var maxValue = ((Int4) args[1]).Value;
                var cg = new IntChoiceFromValueSet(0, maxValue);

                if (cur.ChoiceGenerator is IntChoiceFromValueSet choiceFromValueSet)
                {
                    foreach (var arg in args)
                    {
                        cur.EvalStack.Push(arg);
                    }

                    var schedulingData = cur.Collapse();
                    foreach (var _ in args)
                    {
                        cur.EvalStack.Pop();
                    }

                    if (schedulingData.ID == choiceFromValueSet.SchedulingData.ID)
                    {
                        cur.EvalStack.Push(choiceFromValueSet.GetNextChoice());
                        iieReturnValue = InstructionExecBase.nextRetval;

                        Sb.AppendLine($"{schedulingData.ID} {cur.CurrentThread.CurrentLocation} {cur.EvalStack}");

                        return true;
                    }
                }

                if (cur.Configuration.GetCustomSetting<bool>("evaluateRandom"))
                {
                    foreach (var arg in args)
                    {
                        cur.EvalStack.Push(arg);
                    }

                    cur.SetNextChoiceGenerator(cg);
                    
                    /*foreach (var arg in args)
                    {
                        cur.EvalStack.Pop();
                    }*/
                    iieReturnValue = InstructionExecBase.nincRetval;
                    return true;
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
            cur.EvalStack.Push(objectRef);
            return true;
        }
    }
}
