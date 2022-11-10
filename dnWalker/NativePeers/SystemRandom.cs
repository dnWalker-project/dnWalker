//using dnlib.DotNet;
//using MMC.Data;
//using MMC.InstructionExec;
//using MMC.State;
//using System;
//using dnWalker.ChoiceGenerators;
//using MMC;

//namespace dnWalker.NativePeers
//{
//    public class SystemRandom : NativePeer
//    {
//        public override bool TryGetValue(MethodDef methodDef, DataElementList args, ExplicitActiveState cur, out IIEReturnValue iieReturnValue)
//        {
//            if (methodDef.FullName == "System.Int32 System.Random::Next(System.Int32)")
//            {
//                var maxValue = ((Int4) args[1]).Value;
//                var cg = new IntChoiceFromValueSet(0, maxValue);

//                if (cur.ChoiceGenerator is IntChoiceFromValueSet choiceFromValueSet)
//                {
//                    // push all arguments back on stack (they were popped before the execution got here)
//                    // push them back so current state can be collapsed to detect reaching the same Rand spot on the execution path
//                    foreach (var arg in args)
//                    {
//                        cur.EvalStack.Push(arg);
//                    }

//                    var schedulingData = cur.Collapse();
//                    // args are not needed any more on stack, their presence will cause problems
//                    foreach (var _ in args)
//                    {
//                        cur.EvalStack.Pop();
//                    }

//                    // execution has backtracked to the same spot on the execution path
//                    if (schedulingData.ID == choiceFromValueSet.SchedulingData.ID)
//                    {
//                        // advance and use the next rand value ...
//                        cur.EvalStack.Push(choiceFromValueSet.GetNextChoice());
//                        iieReturnValue = InstructionExecBase.nextRetval;
//                        return true;
//                    }
//                }

//                if (cur.Configuration.EvaluateRandom())
//                {
//                    // push all arguments back on stack (they were popped before the execution got here)
//                    // the execution will get back here, but next time a value from the choice generator will be used
//                    foreach (var arg in args)
//                    {
//                        cur.EvalStack.Push(arg);
//                    }

//                    cur.SetNextChoiceGenerator(cg);
//                    // do not incremenet the PC, repeat the execution
//                    iieReturnValue = InstructionExecBase.nincRetval;
//                    return true;
//                }

//                cur.EvalStack.Push(cg.GetNextChoice());
//                iieReturnValue = InstructionExecBase.nextRetval;
                    
//                return true;
//            }

//            throw new NotImplementedException();
//        }

//        public override bool TryConstruct(MethodDef methodDef, DataElementList args, ExplicitActiveState cur)
//        {
//            var objectRef = cur.DynamicArea.AllocateObject(
//                cur.DynamicArea.DeterminePlacement(false),
//                methodDef.DeclaringType);
//            cur.EvalStack.Push(objectRef);
//            return true;
//        }
//    }
//}
