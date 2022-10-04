using dnlib.DotNet;
using dnlib.DotNet.Emit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public class InstructionBlockNode : ControlFlowNode
    {
        private readonly IReadOnlyList<Instruction> _instructions;

        public InstructionBlockNode(IReadOnlyList<Instruction> instructions, MethodDef method) : base(method)
        {
            _instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
        }

        public IReadOnlyList<Instruction> Instructions => _instructions;

        public Instruction Header => _instructions[0];
        public Instruction Footer => _instructions[^1];

        public bool Contains(Instruction instruction)
        {
            return Header.Offset <= instruction.Offset && Footer.Offset >= instruction.Offset;
        }

        public override string ToString()
        {
            return $"Instructions: {Header} ... {Footer}";
        }
    }
}
