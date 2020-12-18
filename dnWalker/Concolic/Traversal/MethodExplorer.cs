using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnWalker.Traversal;
using Echo.ControlFlow;
using Echo.Platforms.Dnlib;
using MMC.State;
using MMC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public class MethodExplorer
    {
        private readonly ControlFlowGraph<Instruction> _cfg;
        private readonly Dictionary<Instruction, int> _coverageMap;
        private readonly MethodDef _method;
        private readonly IDictionary<ControlFlowEdge<Instruction>, bool> _edges;
        private readonly IDictionary<ControlFlowNode<Instruction>, bool> _nodes;
        
        public MethodExplorer(MethodDef method)
        {
            _cfg = method.ConstructStaticFlowGraph();
            _coverageMap = method.Body.Instructions.ToDictionary(i => i, i => 0);
            _edges = _cfg.GetEdges().ToDictionary(e => e, e => false);
            _nodes = _cfg.Nodes.ToDictionary(n => n, n => false);            
            _method = method;
        }

        public void OnInstructionExecuted(CILLocation location, Path path)
        {
            if (location.Method != _method)
            {
                return;
            }

            _coverageMap[location.Instruction]++;

            var node = _cfg.Nodes.First(n => n.Contents.Instructions.Contains(location.Instruction));
            _nodes[node] = true;

            path.AddVisitedNode(node);
        }

        public Expression Flip(PathConstraint pathConstraint)
        {
            var node = _cfg.Nodes.First(n => n.Contents.Instructions.Contains(pathConstraint.Location.Instruction));
            var edges = node.GetOutgoingEdges();
            if (!edges.Any())
            {
                return pathConstraint.Expression;
            }

            // all outgoing edges are covered
            if (node.GetOutgoingEdges().All(edge => _edges[edge]))
            {
                return pathConstraint.Expression;
            }

            /*var next = pathConstraint.Next;
            if (next != null)
            {
                var conditionalEdge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.Conditional && e.Target.Offset == next.Offset);
                if (!_edges[conditionalEdge])
                {
                    return Expression.Not(pathConstraint.Expression);
                }
            }

            var fallThroughEdge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.FallThrough);
            if (!_edges[fallThroughEdge])
            {
                return Expression.Not(pathConstraint.Expression);
            }*/

            return Expression.Not(pathConstraint.Expression);
            //return pathConstraint.Expression;
        }

        public void OnConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            var instruction = cur.CurrentLocation.Instruction;
            var node = _cfg.Nodes.First(n => n.Contents.Instructions.Contains(instruction));
            _nodes[node] = true;

            var edges = node.GetOutgoingEdges();            
            if (!edges.Any())
            {
                return;
            }

            if (next == null)
            {
                var edge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.FallThrough);
                _edges[edge] = true;
            }
            else
            {
                var edge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.Conditional && e.Target.Offset == next.Offset);
                _edges[edge] = true;
            }
        }

        public Coverage GetCoverage()
        {
            return new Coverage
            {
                Nodes = _nodes.Count(n => n.Value) / (double)_nodes.Count,
                Edges = _edges.Count(e => e.Value) / (double)_edges.Count,
            };
        }
    }
}
