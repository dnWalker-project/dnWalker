using dnWalker.Symbolic;

using MMC.Data;
using MMC.InstructionExec;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Instructions.Extensions
{
    public class CONV_SymbolicExecutionExtension : IPreExecuteInstructionExtension, IPostExecuteInstructionExtension
    {
        public IEnumerable<Type> SupportedInstructions
        {
            get
            {
                yield return typeof(CONV);
                yield return typeof(CONV_OVF_I8);
                yield return typeof(CONV_OVF_I);
                yield return typeof(CONV_OVF_I_UN);
                yield return typeof(CONV_OVF_I8_UN);
                yield return typeof(CONV_OVF_U_UN);
                yield return typeof(CONV_OVF_U4_UN);
                yield return typeof(CONV_OVF_U8_UN);
                yield return typeof(CONV_R4);
                yield return typeof(CONV_R8);
                yield return typeof(CONV_R_UN);
                yield return typeof(CONV_I8);
                yield return typeof(CONV_U2);
                yield return typeof(CONV_U8);
            }
        }

        private INumericElement _operand;

        public void PreExecute(InstructionExecBase instruction, ExplicitActiveState cur)
        {
            IDataElement top = cur.EvalStack.Peek();

            _operand = (top is IManagedPointer) ? (top as IManagedPointer).ToInt4() : (INumericElement)top;
        }
        public void PostExecute(InstructionExecBase instruction, ExplicitActiveState cur, IIEReturnValue retValue)
        {
            if (_operand.TryGetExpression(cur, out Expression expression))
            {
                // get the target type...
                string[] tokens = instruction.Instruction.OpCode.Name.Split('.');

                // conv_[ovf?]_[format][bytes]_[unsigned?]
                // { conv, i4 }

                if (instruction.CheckOverflow)
                {
                    tokens[1] = tokens[2];
                }

                switch (tokens[1][0])
                {
                    case 'i':
                        

                        break; 
                }


            }
        }
    }
}
