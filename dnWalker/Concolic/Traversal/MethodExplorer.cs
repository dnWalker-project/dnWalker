using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnWalker.Traversal;
using Echo.Platforms.Dnlib;
using MMC.State;
using MMC.Util;
using QuikGraph.Graphviz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Traversal
{
    public class MethodExplorer
    {
        private readonly Graphs.ControlFlowGraph _cfg;
        private readonly Dictionary<Instruction, int> _coverageMap;
        private readonly MethodDef _method;
        
        public MethodExplorer(MethodDef method)
        {
            _coverageMap = method.Body.Instructions.ToDictionary(i => i, i => 0);
            _method = method;
            _cfg = new Graphs.ControlFlowGraph(method);

            //string graphAsSvg = _cfg.ToGraphviz();

            //System.IO.File.WriteAllText(@"c:\temp\g.dot", graphAsSvg);

            // Or using a custom init for the algorithm
            /*string graphAsSvg = _cfg.ToSvg(algorithm =>
            {
                // Custom init example
                algorithm.CommonVertexFormat.Shape = GraphvizVertexShape.Diamond;
                algorithm.CommonEdgeFormat.ToolTip = "Edge tooltip";
                algorithm.FormatVertex += (sender, args) =>
                {
                    args.VertexFormat.Label = $"Vertex {args.Vertex}";
                };
            });*/
        }

        public void OnInstructionExecuted(CILLocation location, Path path)
        {
            if (location.Method != _method)
            {
                return;
            }

            _coverageMap[location.Instruction]++;

            var node = _cfg.GetNode(location.Instruction);
            node.SetIsCovered();

            path.AddVisitedNode(node);
        }

        public Expression Flip(PathConstraint pathConstraint)
        {
            var node = _cfg.GetNode(pathConstraint.Location.Instruction);
            var edge = node.GetNextUncoveredOutgoingEdge(); 
            if (edge == null)
            {
                return pathConstraint.Expression;
            }
            //if (!edges.Any())
            //{
            //    return pathConstraint.Expression;
            //}

            // all outgoing edges are covered
            //if (edges.All(edge => _edges[edge] != 0))
            //{
            //    return pathConstraint.Expression;
            //}

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
            // TODO: there could be more edges out (switch statement... not very likely.... usually it is indeed transformed into series of binary branching...
            return Expression.Not(pathConstraint.Expression).Optimize();
            //return pathConstraint.Expression;
        }

        public Expression FlipBackEdge(PathConstraint pathConstraint)
        {
            var cov = _coverageMap.Count(n => n.Value != 0) / (double)_coverageMap.Count;
            if (!_coverageMap.Any(c => c.Value == 0))
            {
                return pathConstraint.Expression; // everything has been covered
            }

            //var node = _cfg.Nodes.First(n => n.Contents.Instructions.Contains(pathConstraint.Location.Instruction));
            var node = _cfg.GetNode(pathConstraint.Location.Instruction);
            var edges = node.GetOutgoingEdges();
            if (!edges.Any())
            {
                return pathConstraint.Expression;
            }

            foreach (var edge in edges.Where(e => e.Source.Offset > e.Target.Offset))
            {
                if (edge.Tag.Times > 20)
                {
                    throw new Exception("Max cycles reached");
                }

                //if (edge.Tag.Times > 1)
                //{
                return Expression.Not(pathConstraint.Expression);
                //}
            }
            
            //if (_cfg.GetEdges().First())

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

            return pathConstraint.Expression;
            //return pathConstraint.Expression;
        }

        public void OnConstraint(Expression expression, Instruction next, ExplicitActiveState cur)
        {
            var instruction = cur.CurrentLocation.Instruction;
            //var node = _cfg.Nodes.First(n => n.Contents.Instructions.Contains(instruction));
            var node = _cfg.GetNode(instruction);
            node.SetExpression(expression);
            node.SetIsCovered(next, cur);
            //_nodes[node] = true;
            /*
            var edges = node.GetOutgoingEdges();
            if (!edges.Any())
            {
                return;
            }

            if (next == null)
            {
                var edge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.FallThrough);
                _edges[edge]++;
            }
            else
            {
                var edge = edges.FirstOrDefault(e => e.Type == ControlFlowEdgeType.Conditional && e.Target.Offset == next.Offset);
                _edges[edge]++;
            }*/
        }

        public Coverage GetCoverage()
        {
            int nodeCount = 0;
            int edgeCount = 0;
            int coveredNodes = 0;
            int coveredEdges = 0;

            foreach (Graphs.Edge edge in _cfg.Edges)
            {
                edgeCount++;
                if (edge.Tag.IsCovered)
                {
                    coveredEdges++;
                }
            }

            foreach (Graphs.Node node in _cfg.Vertices)
            {
                nodeCount++;
                if (node.IsCovered)
                {
                    coveredNodes++;
                }
            }

            return new Coverage((double)coveredNodes / nodeCount, (double)coveredEdges / edgeCount);
        }
    }
}
