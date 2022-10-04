using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;
using dnWalker.Traversal;

using MMC.State;
using MMC.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace dnWalker.Concolic.Traversal
{
    public class MethodTracer
    {
        private class CoverageInfo
        {
            private readonly ControlFlowEdge _edge;
            private bool _isCovered;
            private bool _isUnreachable;
            public CoverageInfo(ControlFlowEdge edge)
            {
                _edge = edge;
            }

            public ControlFlowEdge Edge
            {
                get
                {
                    return _edge;
                }
            }

            public void MarkCovered(Func<ControlFlowEdge, CoverageInfo> getCoverage)
            {
                _isCovered = true;
                if (_isUnreachable)
                {
                    // we have marked this edge as unreachable, but now we have a proof it can be reached => fix it
                    MarkReachable(getCoverage);
                }
            }

            public void MarkUnreachable(Func<ControlFlowEdge, CoverageInfo> getCoverage)
            {
                if (_isCovered)
                {
                    // we want to mark this edge as unreachable but we have already reached it before => do nothing
                    return;
                }

                if (_isUnreachable)
                {
                    // already marked as unreachable => do nothing
                    return;
                }

                _isUnreachable = true;
                // traverse the CFG in order to mark the following edges unreachable, if possible
                if (_edge.Target.InEdges.All(e => getCoverage(e).IsUnreachable))
                {
                    // when all edges which comes into the target node are unreachable, we should propagate the unreachability further
                    foreach (ControlFlowEdge outEdge in _edge.Target.OutEdges)
                    {
                        getCoverage(outEdge).MarkUnreachable(getCoverage);
                    }
                }
            }

            public void MarkReachable(Func<ControlFlowEdge, CoverageInfo> getCoverage)
            {
                if (!_isUnreachable)
                {
                    // already marked as reachable => do nothing
                    return;
                }

                _isUnreachable = false;

                // since we are fixing reachability of this edge, we should fix the following edges as well since their reachability have changed as well
                foreach (ControlFlowEdge outEdge in _edge.Target.OutEdges)
                {
                    getCoverage(_edge).MarkReachable(getCoverage);
                }
            }

            public bool IsCovered
            {
                get
                {
                    return _isCovered;
                }
            }

            public bool IsUnreachable
            {
                get
                {
                    return _isUnreachable;
                }
            }
        }

        private readonly ControlFlowGraph _cfg;
        private readonly MethodDef _method;

        private readonly IReadOnlyDictionary<ControlFlowEdge, CoverageInfo> _edgeCoverage;

        private Coverage? _coverage = null;
        
        public MethodTracer(MethodDef method, ControlFlowGraph cfg)
        {
            _method = method;
            _cfg = cfg;

            _edgeCoverage = _cfg.Edges.ToDictionary(e => e, e => new CoverageInfo(e));
        }

        public ControlFlowGraph Graph => _cfg;

        public Coverage GetCoverage()
        {
            if (!_coverage.HasValue)
            {
                _coverage = ComputeCoverage();
            }

            return _coverage.Value;
        }

        private Coverage ComputeCoverage()
        {
            double coveredNodes = _cfg.Nodes.Count(IsCovered);
            double reachableNodes = _cfg.Nodes.Count(IsReachable);

            double coveredEdges = _cfg.Edges.Count(IsCovered);
            double reachableEdges = _cfg.Edges.Count(IsReachable);

            return new Coverage(coveredEdges / reachableEdges, coveredNodes / reachableNodes);
        }

        private bool IsReachable(ControlFlowNode node) => node.InEdges.Any(e => IsReachable(e));
        private bool IsCovered(ControlFlowNode node) => node.InEdges.Any(e => IsCovered(e));
        private bool IsUnreachable(ControlFlowNode node) => node.InEdges.All(e => IsUnreachable(e));

        private bool IsReachable(ControlFlowEdge e) => !GetCoverageInfo(e).IsUnreachable;
        private bool IsCovered(ControlFlowEdge e) => GetCoverageInfo(e).IsCovered;
        private bool IsUnreachable(ControlFlowEdge e) => GetCoverageInfo(e).IsUnreachable;

        public IEnumerable<ControlFlowNode> GetCoveredNodes()
        {
            // node is covered when any incoming edge is covered
            return _cfg.Nodes.Where(IsCovered);
        }

        public IEnumerable<ControlFlowNode> GetUnreachableNodes()
        {
            // node is unreachable when all incoming edges are unreachable
            return _cfg.Nodes.Where(IsUnreachable);
        }

        public IEnumerable<ControlFlowNode> GetReachableNodes()
        {
            // node is reachable when any incoming edge is reachable
            return _cfg.Nodes.Where(IsReachable);
        }

        public IEnumerable<ControlFlowEdge> GetCoveredEdges()
        {
            return _cfg.Edges.Where(IsCovered);
        }

        public IEnumerable<ControlFlowEdge> GetUnreachableEdges()
        {
            return _cfg.Edges.Where(IsUnreachable);
        }

        public IEnumerable<ControlFlowEdge> GetReachableEdges()
        {
            return _cfg.Edges.Where(IsReachable);
        }

        public void MarkCovered(ControlFlowEdge edge)
        {
            _coverage = null;
            GetCoverageInfo(edge).MarkCovered(GetCoverageInfo);
        }

        public void MarkUnreachable(ControlFlowEdge edge)
        {
            _coverage = null;
            GetCoverageInfo(edge).MarkUnreachable(GetCoverageInfo);
        }

        private CoverageInfo GetCoverageInfo(ControlFlowEdge edge)
        {
            return _edgeCoverage[edge];
        }

        internal void OnExceptionThrown(Instruction instruction, TypeDef exceptionType)
        {
            // obsolete usage
        }

        internal void OnInstructionExecuted(Instruction instruction)
        {
            // obsolete usage
        }
    }
}
