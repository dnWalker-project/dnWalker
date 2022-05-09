using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Concolic;
using dnWalker.Symbolic;

using QuikGraph;

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
        private readonly BidirectionalGraph<ControlFlowNode, ControlFlowEdge> _graph;
        private readonly InstructionBlockNode[] _instructionBlocks;

        private ControlFlowGraph([NotNull] BidirectionalGraph<ControlFlowNode, ControlFlowEdge> graph)
        {
            _graph = graph;
            _instructionBlocks = graph.Vertices.OfType<InstructionBlockNode>().ToArray();
            Array.Sort(_instructionBlocks, (a, b) => (int)(a.Header.Offset - (long)b.Header.Offset));
        }

        public void MarkCovered(Instruction current, Instruction next, Constraint proof)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlocks, current);
            InstructionBlockNode targetNode = ControlFlowUtils.GetNode(_instructionBlocks, next);
            if (!_graph.TryGetEdge(sourceNode, targetNode, out ControlFlowEdge edge)) throw new InvalidOperationException($"Cannot mark edge between '{current}' and '{next}' as it does not exist.");

            edge.MarkCovered(proof);
        }

        public void MarkUnreachable(Instruction current, Instruction next, Constraint proof)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlocks, current);
            InstructionBlockNode targetNode = ControlFlowUtils.GetNode(_instructionBlocks, next);
            if (!_graph.TryGetEdge(sourceNode, targetNode, out ControlFlowEdge edge)) throw new InvalidOperationException($"Cannot mark edge between '{current}' and '{next}' as it does not exist.");

            MarkUnreachable(edge, proof);
        }

        public void MarkCovered(Instruction current, TypeDef exception, Constraint proof)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlocks, current);

            if (!_graph.TryGetOutEdges(sourceNode, out IEnumerable<ControlFlowEdge> edges)) throw new InvalidOperationException($"Cannot mark exception for '{exception.Name}' edge from '{current}', because it does not exists.");

            foreach (ExceptionEdge eEdge in edges.OfType<ExceptionEdge>())
            {
                if (TypeEqualityComparer.Instance.Equals(eEdge.ExceptionType, exception))
                {
                    eEdge.MarkCovered(proof);
                    return;
                }
            }

            throw new InvalidOperationException($"Cannot mark exception for '{exception.Name}' edge from '{current}', because it does not exists.");

        }

        public void MarkUnreachable(Instruction current, TypeDef exception, Constraint proof)
        {
            InstructionBlockNode sourceNode = ControlFlowUtils.GetNode(_instructionBlocks, current);

            if (!_graph.TryGetOutEdges(sourceNode, out IEnumerable<ControlFlowEdge> edges)) throw new InvalidOperationException($"Cannot mark exception for '{exception.Name}' edge from '{current}', because it does not exists.");

            foreach (ExceptionEdge exceptionEdge in edges.OfType<ExceptionEdge>())
            {
                if (TypeEqualityComparer.Instance.Equals(exceptionEdge.ExceptionType, exception))
                {
                    MarkUnreachable(exceptionEdge, proof);
                    return;
                }
            }

            throw new InvalidOperationException($"Cannot mark exception for '{exception.Name}' edge from '{current}', because it does not exists.");
        }


        
        private void MarkUnreachable(ControlFlowEdge edge, Constraint proof)
        {
            edge.MarkUnreachable(proof);

            ControlFlowNode target = edge.Target;
            if (_graph.TryGetInEdges(target, out IEnumerable<ControlFlowEdge> inEdges))
            {
                if (inEdges.All(static e => !e.IsReachable))
                {
                    // every in edge is marked as unreachable => we can marking every out edge in same manner
                    if (_graph.TryGetOutEdges(target, out IEnumerable<ControlFlowEdge> outEdges))
                    {
                        foreach (ControlFlowEdge outEdge in outEdges)
                        {
                            MarkUnreachable(outEdge, proof);
                        }
                    }
                }
            }
        }

        public Coverage GetCoverage()
        {
            int reachableNodes = _graph.Vertices.Count(v =>
            {
                _graph.TryGetInEdges(v, out IEnumerable<ControlFlowEdge> inEdges);
                return inEdges != null && inEdges.Any(static e => e.IsReachable);
            });

            int coveredNodes = _graph.Vertices.Count(static v => v.IsCovered);
            
            int reachableEdges = _graph.Edges.Count(static e => e.IsReachable);
            int coveredEdges = _graph.Edges.Count(v => v.IsCovered);

            return new Coverage(coveredEdges / (double)reachableEdges, coveredNodes / (double)reachableNodes);
        }
    }
}
