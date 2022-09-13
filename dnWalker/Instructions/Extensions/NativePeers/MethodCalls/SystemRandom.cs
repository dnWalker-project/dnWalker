using dnlib.DotNet;

using dnWalker.ChoiceGenerators;

using MMC.Data;
using MMC;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions.NativePeers.MethodCalls
{
    [NativePeer("System.Random", MatchMethods = true)]
    public class SystemRandom : CompiledMethodCallNativePeer<SystemRandom>
    {
        private static bool Next(MethodDef method, DataElementList args, ExplicitActiveState cur, out IIEReturnValue returnValue)
        {
            var maxValue = ((Int4)args[1]).Value;
            var cg = new IntChoiceFromValueSet(0, maxValue);

            if (cur.ChoiceGenerator is IntChoiceFromValueSet choiceFromValueSet)
            {
                // push all arguments back on stack (they were popped before the execution got here)
                // push them back so current state can be collapsed to detect reaching the same Rand spot on the execution path
                foreach (var arg in args)
                {
                    cur.EvalStack.Push(arg);
                }

                var schedulingData = cur.Collapse();
                // args are not needed any more on stack, their presence will cause problems
                foreach (var _ in args)
                {
                    cur.EvalStack.Pop();
                }

                // execution has backtracked to the same spot on the execution path
                if (schedulingData.ID == choiceFromValueSet.SchedulingData.ID)
                {
                    // advance and use the next rand value ...
                    cur.EvalStack.Push(choiceFromValueSet.GetNextChoice());
                    returnValue = InstructionExecBase.nextRetval;
                    return true;
                }
            }

            if (cur.Configuration.EvaluateRandom())
            {
                // push all arguments back on stack (they were popped before the execution got here)
                // the execution will get back here, but next time a value from the choice generator will be used
                foreach (var arg in args)
                {
                    cur.EvalStack.Push(arg);
                }

                cur.SetNextChoiceGenerator(cg);
                // do not incremenet the PC, repeat the execution
                returnValue = InstructionExecBase.nincRetval;
                return true;
            }

            cur.EvalStack.Push(cg.GetNextChoice());
            returnValue = InstructionExecBase.nextRetval;

            return true;
        }
    }
}
