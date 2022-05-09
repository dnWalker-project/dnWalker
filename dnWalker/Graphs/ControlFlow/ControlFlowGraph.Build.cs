using dnlib.DotNet;
using dnlib.DotNet.Emit;

using QuikGraph;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public partial class ControlFlowGraph
    {
        public static ControlFlowGraph Build(MethodDef method)
        {
            BidirectionalGraph<ControlFlowNode, ControlFlowEdge> graph = new BidirectionalGraph<ControlFlowNode, ControlFlowEdge>();

            // build the instruction block nodes
            List<InstructionBlockNode> blockNodes = new List<InstructionBlockNode>();
            List<Instruction> currentBlock = new List<Instruction>();
            foreach (Instruction instruction in method.Body.Instructions)
            {
                currentBlock.Add(instruction);
                
                if (ControlFlowUtils.GetSuccessorCount(instruction) > 1 ||
                    ControlFlowUtils.EndsBlock(instruction))
                {
                    InstructionBlockNode node = new InstructionBlockNode(currentBlock);
                    blockNodes.Add(node);

                    currentBlock = new List<Instruction>();
                }
            }
            graph.AddVertexRange(blockNodes);

            // connect the instruction block nodes
            Dictionary<TypeDef, VirtualExceptionHandlerNode> exceptionHandlerLookup = new Dictionary<TypeDef, VirtualExceptionHandlerNode>(TypeEqualityComparer.Instance);
            Dictionary<string, TypeDef> exceptionCache = new Dictionary<string, TypeDef>();

            for (int i = 0; i < blockNodes.Count; ++i)
            {
                Instruction last = blockNodes[i].Footer;

                ControlFlowUtils.SuccessorInfo[] successors = ControlFlowUtils.GetSuccessors(last, method.Module, exceptionCache);

                foreach (ControlFlowUtils.SuccessorInfo info in successors)
                {
                    if (info == ControlFlowUtils.SuccessorInfo.NextInstruction)
                    {
                        // this should always happen iff (i < blockNodes.Count - 1) as the last instruction is RET
                        graph.AddEdge(new ControlFlowEdge(blockNodes[i], blockNodes[i + 1]));
                    }
                    else if (info.Instruction != null)
                    {
                        // branch to specific instruction
                        graph.AddEdge(new ControlFlowEdge(blockNodes[i], ControlFlowUtils.GetNode(blockNodes, info.Instruction)));
                    }
                    else
                    {
                        // branch to a (virtual) exception handler
                        if (ControlFlowUtils.TryGetHandler(last, info.ExceptionType, method, out Instruction handlerHeader))
                        {
                            graph.AddEdge(new ExceptionEdge(info.ExceptionType, blockNodes[i], ControlFlowUtils.GetNode(blockNodes, handlerHeader)));
                        }
                        else
                        {
                            if (!exceptionHandlerLookup.TryGetValue(info.ExceptionType, out VirtualExceptionHandlerNode handlerNode))
                            {
                                handlerNode = new VirtualExceptionHandlerNode(info.ExceptionType);
                                exceptionHandlerLookup.Add(info.ExceptionType, handlerNode);
                            }
                            graph.AddEdge(new ExceptionEdge(info.ExceptionType, blockNodes[i], handlerNode));
                        }
                    }
                }

            }

            return new ControlFlowGraph(graph);
        }
    }
}
