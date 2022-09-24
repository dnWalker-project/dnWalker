using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public partial class ControlFlowGraph
    {
        private readonly IReadOnlyList<InstructionBlockNode> _instructionBlockNodes;
        private readonly IReadOnlyDictionary<TypeDef, VirtualExceptionHandlerNode> _exceptionNodes;

        private readonly IReadOnlyList<ControlFlowNode> _nodes;
        private readonly IReadOnlyList<ControlFlowEdge> _edges;


        private ControlFlowGraph(
            [NotNull] IReadOnlyList<ControlFlowNode> nodes, 
            [NotNull] IReadOnlyList<InstructionBlockNode> instructionBlockNodes,
            [NotNull] IReadOnlyDictionary<TypeDef, VirtualExceptionHandlerNode> exceptionNodes)
        {
            _nodes = nodes;
            _edges = _nodes.SelectMany(static n => n.OutEdges).ToList();

            _instructionBlockNodes = instructionBlockNodes;

            _exceptionNodes = exceptionNodes;
        }

        public IReadOnlyCollection<ControlFlowNode> Nodes => _nodes;
        public IReadOnlyCollection<ControlFlowEdge> Edges => _edges;

        public ControlFlowNode EntryPoint => _instructionBlockNodes[0];

        public InstructionBlockNode GetInstructionNode(Instruction instruction)
        {
            return ControlFlowUtils.GetNode(_instructionBlockNodes, instruction);
        }

        public VirtualExceptionHandlerNode GetExceptionNode(TypeDef exceptionType)
        {
            return _exceptionNodes[exceptionType];
        }

        public IEnumerable<ControlFlowNode> GetSuccessors(Instruction instruction)
        {
            return GetInstructionNode(instruction)?.Successors ?? Array.Empty<ControlFlowNode>();
        }

    }
}
