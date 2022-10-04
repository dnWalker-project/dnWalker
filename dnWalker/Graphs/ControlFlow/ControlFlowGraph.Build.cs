using dnlib.DotNet;
using dnlib.DotNet.Emit;

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
            List<ControlFlowNode> nodes = new List<ControlFlowNode>();

            // build the instruction block nodes
            List<InstructionBlockNode> blockNodes = new List<InstructionBlockNode>();
            List<Instruction> currentBlock = new List<Instruction>();
            foreach (Instruction instruction in method.Body.Instructions)
            {
                currentBlock.Add(instruction);
                
                if (ControlFlowUtils.EndsBlock(instruction))
                {
                    InstructionBlockNode node = new InstructionBlockNode(currentBlock, method);
                    blockNodes.Add(node);

                    currentBlock = new List<Instruction>();
                }
            }
            nodes.AddRange(blockNodes);

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
                        ControlFlowNode srcNode = blockNodes[i];
                        ControlFlowNode trgNode = blockNodes[i + 1];

                        ControlFlowUtils.CreateNextEdge(srcNode, trgNode);
                    }
                    else if (info.Instruction != null)
                    {
                        // branch to specific instruction
                        ControlFlowNode srcNode = blockNodes[i];
                        ControlFlowNode trgNode = ControlFlowUtils.GetNode(blockNodes, info.Instruction);

                        ControlFlowUtils.CreateJumpEdge(srcNode, trgNode);
                    }
                    else
                    {
                        // TODO: not yet implemented... branch to a concrete, known, exception handler - within this method
                        if (ControlFlowUtils.TryGetHandler(last, info.ExceptionType, method, out Instruction handlerHeader))
                        {
                            TypeDef exceptionType = info.ExceptionType;
                            ControlFlowNode srcNode = blockNodes[i];
                            ControlFlowNode trgNode = ControlFlowUtils.GetNode(blockNodes, handlerHeader);

                            ControlFlowUtils.CreateExceptionEdge(exceptionType, srcNode, trgNode);
                        }
                        // branch to a virtual exception handler
                        else
                        {
                            if (!exceptionHandlerLookup.TryGetValue(info.ExceptionType, out VirtualExceptionHandlerNode handlerNode))
                            {
                                handlerNode = new VirtualExceptionHandlerNode(info.ExceptionType, method);
                                exceptionHandlerLookup.Add(info.ExceptionType, handlerNode);
                                nodes.Add(handlerNode);
                            }
                            ControlFlowNode srcNode = blockNodes[i];
                            ControlFlowNode trgNode = handlerNode;
                            TypeDef exceptionType = info.ExceptionType;

                            ControlFlowUtils.CreateExceptionEdge(exceptionType, srcNode, trgNode);
                        }
                    }

                    //ControlFlowUtils.CreateEdge(srcNode, trgNode, exceptionType);
                }

            }

            return new ControlFlowGraph(
                nodes,
                blockNodes,
                exceptionHandlerLookup);
        }
    }
}
