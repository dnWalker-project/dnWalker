using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Echo.ControlFlow;
using Echo.Platforms.Dnlib;
using MMC.State;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs
{
    public class Node
    {
        private bool _isCovered;
        private readonly ControlFlowNode<Instruction> _controlFlowNode;
        private readonly ControlFlowGraph _controlFlowGraph;
        private Expression _expression;

        public Node(ControlFlowNode<Instruction> controlFlowNode, ControlFlowGraph controlFlowGraph)
        {
            _controlFlowNode = controlFlowNode;
            _controlFlowGraph = controlFlowGraph;
        }

        public void SetExpression(Expression expression)
        {
            _expression = expression;
        }

        public long Offset => _controlFlowNode.Offset;

        public void SetIsCovered() 
        {
            _isCovered = true;
        }

        public void SetIsCovered(Instruction next, ExplicitActiveState cur) 
        { 
            _isCovered = true;

            if (next == null)
            {
                var fallThroughEdge = GetOutgoingEdges().OfType<UnconditionalEdge>().Single();
                fallThroughEdge.Tag.IsCovered = true;
                fallThroughEdge.Tag.Times++;
                return;
            }

            var nextNode = _controlFlowGraph.GetNode(next);
            if (nextNode == null)
            {
                throw new NullReferenceException();
            }

            var edge = GetOutgoingEdges().Where(e => e.Target.Offset == nextNode.Offset).FirstOrDefault();
            edge.Tag.IsCovered = true;
            edge.Tag.Times++;
        }

        public Edge GetNextUncoveredOutgoingEdge()
        {
            var edges = GetOutgoingEdges();
            return edges
                .Where(e => e.Source.Offset < e.Target.Offset)
                .FirstOrDefault(e => !e.Tag.IsCovered);
        }

        public IEnumerable<Edge> GetOutgoingEdges()
        {
            return _controlFlowGraph.Edges.Where(e => e.Source.Offset == this.Offset).Cast<Edge>();
        }

        public override string ToString()
        {
            return _controlFlowNode.ToString();
        }
    }

    public abstract class Edge : QuikGraph.TaggedEdge<Node, CoverageInfo>
    {
        public Edge(Node source, Node target, CoverageInfo o) : base(source, target, o)
        {
        }
    }

    [DebuggerDisplay("Covered: {Times}, [{Coverage}]")]
    public class CoverageInfo
    {
        public int Times { get; set; }

        public bool IsCovered { get; set; }

        public decimal Coverage { get; set; }
    }

    public class UnconditionalEdge : Edge
    {
        public UnconditionalEdge(Node source, Node target, CoverageInfo coverageInfo) : base(source, target, coverageInfo)
        {
        }
    }

    public class ConditionalEdge : Edge
    {
        public ConditionalEdge(Node source, Node target, CoverageInfo coverageInfo) : base(source, target, coverageInfo)
        {
            
        }
    }

    public class ControlFlowGraph : AdjacencyGraph<Node, Edge<Node>>
    {
        private IDictionary<long, Node> _nodeMapping;
        private ControlFlowGraph<Instruction> _originalCfg;

        public ControlFlowGraph(MethodDef method)// ControlFlowGraph<Instruction> controlFlowGraph)
        {
            _originalCfg = method.ConstructStaticFlowGraph();
            _nodeMapping = new Dictionary<long, Node>();

            // ensure that entry point is in the graph => method with no branching will not throw error in GetNode(...);
            var entryPoint = _originalCfg.Entrypoint;
            var entryPointNode = new Node(entryPoint, this);

            AddVertex(entryPointNode);
            _nodeMapping[entryPointNode.Offset] = entryPointNode;

            foreach (var edge in _originalCfg.GetEdges())
            {
                var origin = new Node(edge.Origin, this);
                var target = new Node(edge.Target, this);

                _nodeMapping[edge.Origin.Offset] = origin;
                _nodeMapping[edge.Target.Offset] = target;

                Edge cfgEdge = null;
                switch (edge.Type)
                {
                    case ControlFlowEdgeType.None:
                    case ControlFlowEdgeType.Abnormal:
                    case ControlFlowEdgeType.FallThrough:
                    case ControlFlowEdgeType.Unconditional:
                        cfgEdge = new UnconditionalEdge(origin, target, new CoverageInfo());
                        break;
                    case ControlFlowEdgeType.Conditional:
                        cfgEdge = new ConditionalEdge(origin, target, new CoverageInfo());
                        break;
                }
                
                AddVerticesAndEdge(cfgEdge);
            }
        }

        public Node GetNode(Instruction instruction)
        {
            var node = _originalCfg.Nodes.FirstOrDefault(n => n.Contents.Instructions.Contains(instruction));
            if (node == null)
            {
                return null;
            }
            return _nodeMapping[node.Offset];
        }
    }
}
