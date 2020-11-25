﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
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

        public void OnInstructionExecuted(CILLocation location)
        {
            if (location.Method != _method)
            {
                return;
            }

            _coverageMap[location.Instruction]++;
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
    }
}
