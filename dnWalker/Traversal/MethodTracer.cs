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
    // WIP: CoverageTracer?
    public class MethodTracer
    {
        private readonly MethodDef _method;
        private readonly ControlFlowGraph _graph;

        private Instruction _prevInstruction;

        public MethodTracer(MethodDef method, ControlFlowGraph graph)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public MethodDef Method => _method;

        public ControlFlowGraph Graph => _graph;

        // TODO: actual coverage calculation
        public virtual Coverage GetCoverage() => new Coverage(0, 0); //_graph.GetCoverage();

        public virtual void OnInstructionExecuted(Instruction instruction)
        {
            InstructionBlockNode executedNode = _graph.GetInstructionNode(instruction);
            // track covered nodes in some other structure than CFG
            executedNode.MarkCovered();

            if (_prevInstruction != null)
            {
                InstructionBlockNode prevNode = _graph.GetInstructionNode(_prevInstruction);
                if (!ReferenceEquals(executedNode, prevNode))
                {
                    // we have moved from one node to another...
                    prevNode.GetEdgeTo(executedNode).MarkCovered();
                }
                _prevInstruction = instruction;
            }
        }

        public virtual void OnExceptionThrown(Instruction instruction, TypeDef exception)
        {
            OnInstructionExecuted(instruction);

            InstructionBlockNode executedNode = _graph.GetInstructionNode(instruction);

            foreach (ExceptionEdge exceptionEdge in executedNode.OutEdges.OfType<ExceptionEdge>())
            {
                // TODO: better matching function, check inheritance etc...
                if (TypeEqualityComparer.Instance.Equals(exceptionEdge.ExceptionType, exception))
                {
                    exceptionEdge.MarkCovered();
                    return;
                }
            }

            // we have not found any exception edge which matches this exception
            // - create the virtual exception handler?
            System.Diagnostics.Debug.Fail($"We have not found correct exception edge ('{exception.FullName}') from the current node ('{executedNode}').");
        }
    }
}
