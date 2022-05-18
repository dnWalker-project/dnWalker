using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Concolic;
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

        public void MarkCovered(Instruction current, Instruction next)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlockNodes, current);
            InstructionBlockNode targetNode = ControlFlowUtils.GetNode(_instructionBlockNodes, next);

            ControlFlowEdge edge = sourceNode.GetEdgeTo(targetNode);
            edge.MarkCovered();
        }

        public void MarkUnreachable(Instruction current, Instruction next)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlockNodes, current);
            InstructionBlockNode targetNode = ControlFlowUtils.GetNode(_instructionBlockNodes, next);

            ControlFlowEdge edge = sourceNode.GetEdgeTo(targetNode);
            ControlFlowUtils.MarkUnreachable(edge);
        }

        public void MarkCovered(Instruction current, TypeDef exception)
        {
            InstructionBlockNode sourceNode = GetInstructionNode(current);
            VirtualExceptionHandlerNode targetNode = GetExceptionNode(exception);

            ControlFlowEdge edge = sourceNode.GetEdgeTo(targetNode);
            edge.MarkCovered();
        }

        public void MarkUnreachable(Instruction current, TypeDef exception)
        {
            InstructionBlockNode sourceNode = GetInstructionNode(current);
            VirtualExceptionHandlerNode targetNode = GetExceptionNode(exception);

            ControlFlowEdge edge = sourceNode.GetEdgeTo(targetNode);
            ControlFlowUtils.MarkUnreachable(edge);
        }

        public Coverage GetCoverage()
        {
            int reachableNodes = _nodes.Count(static n => n.InEdges.Any(static e => e.IsReachable));
            int coveredNodes = _nodes.Count(static n => n.IsCovered);

            int reachableEdges = _edges.Count(static e => e.IsReachable);
            int coveredEdges = _edges.Count(static e => e.IsCovered);

            return new Coverage(coveredEdges / (double)reachableEdges, coveredNodes / (double)reachableNodes);
        }
    }
}
