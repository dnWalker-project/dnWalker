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
                    InstructionBlockNode node = new InstructionBlockNode(currentBlock);
                    blockNodes.Add(node);

                    currentBlock = new List<Instruction>();
                }
            }
            nodes.AddRange(blockNodes);

            // connect the instruction block nodes
            Dictionary<TypeDef, VirtualExceptionHandlerNode> exceptionHandlerLookup = new Dictionary<TypeDef, VirtualExceptionHandlerNode>(TypeEqualityComparer.Instance);
            Dictionary<string, TypeDef> exceptionCache = new Dictionary<string, TypeDef>();
            Lazy<AssertViolationNode> assertViolationNode = new Lazy<AssertViolationNode>(() =>
            {
                AssertViolationNode node = new AssertViolationNode();
                nodes.Add(node);
                return node;
            });

            for (int i = 0; i < blockNodes.Count; ++i)
            {
                Instruction last = blockNodes[i].Footer;

                ControlFlowUtils.SuccessorInfo[] successors = ControlFlowUtils.GetSuccessors(last, method.Module, exceptionCache);

                foreach (ControlFlowUtils.SuccessorInfo info in successors)
                {
                    ControlFlowNode srcNode;
                    ControlFlowNode trgNode;
                    TypeDef exceptionType = null;

                    if (info.Type == ControlFlowUtils.SuccessorInfoType.Next)
                    {
                        // this should always happen iff (i < blockNodes.Count - 1) as the last instruction is RET
                        srcNode = blockNodes[i];
                        trgNode = blockNodes[i + 1];
                    }
                    else if (info.Type == ControlFlowUtils.SuccessorInfoType.Jump)
                    {
                        // branch to specific instruction
                        srcNode = blockNodes[i];
                        trgNode = ControlFlowUtils.GetNode(blockNodes, info.Instruction);
                    }
                    else if (info.Type == ControlFlowUtils.SuccessorInfoType.AssertViolation)
                    {
                        srcNode = blockNodes[i];
                        trgNode = assertViolationNode.Value;
                    }
                    else // if (info.Type == ControlFlowUtils.SuccessorInfoType.Exception)
                    {
                        // TODO: not yet implemented... branch to a concrete, known, exception handler - within this method
                        if (ControlFlowUtils.TryGetHandler(last, info.ExceptionType, method, out Instruction handlerHeader))
                        {
                            exceptionType = info.ExceptionType;
                            srcNode = blockNodes[i];
                            trgNode = ControlFlowUtils.GetNode(blockNodes, handlerHeader);
                        }
                        // branch to a virtual exception handler
                        else
                        {
                            if (!exceptionHandlerLookup.TryGetValue(info.ExceptionType, out VirtualExceptionHandlerNode handlerNode))
                            {
                                handlerNode = new VirtualExceptionHandlerNode(info.ExceptionType);
                                exceptionHandlerLookup.Add(info.ExceptionType, handlerNode);
                                nodes.Add(handlerNode);
                            }
                            srcNode = blockNodes[i];
                            trgNode = handlerNode;
                            exceptionType = info.ExceptionType;
                        }
                    }

                    ControlFlowUtils.CreateEdge(srcNode, trgNode, exceptionType);
                }

            }

            return new ControlFlowGraph(
                nodes,
                blockNodes,
                exceptionHandlerLookup);
        }
    }
}
