using dnlib.DotNet;
using dnlib.DotNet.Emit;

using dnWalker.Graphs.ControlFlow;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Traversal
{
    public class MethodTracer
    {
        private abstract class CoverageInfo
        {
            private bool _isReachable;
            private bool _isCovered;

            public virtual void MarkCovered()
            {
                _isReachable = true;
                _isCovered = true;
            }

            public virtual void MarkUnreachable()
            {
                System.Diagnostics.Debug.Assert(!_isCovered, "We are marking edge/node unreachable but it is already covered.");
                _isReachable = false;
            }

            public bool IsReachable
            {
                get
                {
                    return _isReachable;
                }
            }

            public bool IsCovered
            {
                get
                {
                    return _isCovered;
                }
            }
        }

        private class InstructionNodeCoverage : CoverageInfo
        {
            private readonly Instruction _header;

            public InstructionNodeCoverage(Instruction header)
            {
                _header = header;
            }

            public Instruction Header
            {
                get
                {
                    return _header;
                }
            }
        }

        private class ExceptionNodeCoverage : CoverageInfo
        {
            private readonly TypeDef _exception;

            public ExceptionNodeCoverage(TypeDef exception)
            {
                _exception = exception;
            }

            public TypeDef Exception
            {
                get
                {
                    return _exception;
                }
            }
        }

        private class EdgeCoverage : CoverageInfo
        { }

        private readonly Dictionary<Instruction, InstructionNodeCoverage> _instructionNodeCoverage = new Dictionary<Instruction, InstructionNodeCoverage>();
        private readonly Dictionary<TypeDef, ExceptionNodeCoverage> _exceptionNodeCoverage = new Dictionary<TypeDef, ExceptionNodeCoverage>();

        private readonly Dictionary<(uint prev, uint cur), EdgeCoverage> _instruction2InstructionCoverage = new Dictionary<(uint prev, uint cur), EdgeCoverage>();
        private readonly Dictionary<(uint prev, TypeDef exception), EdgeCoverage> _instruction2ExceptionCoverage = new Dictionary<(uint prev, TypeDef exception), EdgeCoverage>();

        private bool _coverageDirty = true;

        private void MarkCovered(Instruction instruction)
        {
            _instructionNodeCoverage[instruction].MarkCovered();

            if (_prevInstruction.HasValue)
            {
                _instruction2InstructionCoverage[(_prevInstruction.Value, instruction.Offset)].MarkCovered();
            }

            _coverageDirty = true;
        }

        private void MarkCovered(TypeDef exception)
        {
            _exceptionNodeCoverage[exception].MarkCovered();

            if (_prevInstruction >= 0)
            {
                _instruction2ExceptionCoverage[(_prevInstruction.Value, exception)].MarkCovered();
            }

            _coverageDirty = true;
        }

        private readonly MethodDef _method;
        private readonly ControlFlowGraph _graph;

        private uint? _prevInstruction = null;

        public MethodTracer(MethodDef method, ControlFlowGraph graph)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));

            foreach (ControlFlowNode node in _graph.Nodes)
            {
                if (node is InstructionBlockNode iNode)
                {
                    _instructionNodeCoverage[iNode.Header] = new InstructionNodeCoverage(iNode.Header);
                }
                else if (node is VirtualExceptionHandlerNode eNode)
                {
                    _exceptionNodeCoverage[eNode.ExceptionType] = new ExceptionNodeCoverage(eNode.ExceptionType);
                }
            }

            foreach (ControlFlowEdge edge in _graph.Edges)
            {
                if (edge is ExceptionEdge eEdge)
                {
                    // TODO: virtual vs concrete exception handlers!!!!
                    InstructionBlockNode prevNode = (InstructionBlockNode)eEdge.Source;
                    VirtualExceptionHandlerNode nextNode = (VirtualExceptionHandlerNode)eEdge.Target;
                    _instruction2ExceptionCoverage[(prevNode.Header.Offset, nextNode.ExceptionType)] = new EdgeCoverage();
                }
                else
                {
                    InstructionBlockNode prevNode = (InstructionBlockNode)edge.Source;
                    InstructionBlockNode nextNode = (InstructionBlockNode)edge.Target;
                    _instruction2InstructionCoverage[(prevNode.Header.Offset, nextNode.Header.Offset)] = new EdgeCoverage();
                }
            }
        }

        public MethodDef Method => _method;

        public ControlFlowGraph Graph => _graph;


        private Coverage _coverage;
        public virtual Coverage GetCoverage()
        {
            if (_coverageDirty)
            {
                double nodeCount = _instructionNodeCoverage.Values.Count(n => n.IsReachable) +
                                   _exceptionNodeCoverage.Values.Count(n => n.IsReachable);
                double coveredNodes = _instructionNodeCoverage.Values.Count(n => n.IsCovered) +
                                   _exceptionNodeCoverage.Values.Count(n => n.IsCovered);

                double edgeCount = _instruction2InstructionCoverage.Values.Count(n => n.IsReachable) +
                                   _instruction2ExceptionCoverage.Values.Count(n => n.IsReachable);
                double coveredEdges = _instruction2InstructionCoverage.Values.Count(n => n.IsCovered) +
                                      _instruction2ExceptionCoverage.Values.Count(n => n.IsCovered);

                _coverage = new Coverage(coveredEdges / edgeCount, coveredNodes / nodeCount);
            }
            return _coverage;
        }

        public virtual void OnInstructionExecuted(Instruction instruction)
        {
            MarkCovered(instruction);
            _prevInstruction = instruction.Offset;
        }

        public virtual void OnExceptionThrown(Instruction instruction, TypeDef exception)
        {
            OnInstructionExecuted(instruction);
            MarkCovered(exception);
        }
    }
}
