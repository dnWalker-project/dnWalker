using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Graphs.ControlFlow
{
    public static class ControlFlowGraphWriter
    {
        const string Begin = "digraph G {\n";
        const string End = "}";

        public static void Write(ControlFlowGraph cfg, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine(Begin);

                foreach (ControlFlowNode node in cfg.Nodes)
                {
                    if (node is InstructionBlockNode block)
                    {
                        WriteBlockNode(block);
                    }
                    else if (node is VirtualExceptionHandlerNode virtualExceptionHandler)
                    {
                        WriteExceptionNode(virtualExceptionHandler);
                    }
                }

                foreach (ControlFlowEdge edge in cfg.Edges)
                {
                    WriteEdge(edge);
                }

                writer.WriteLine(End);

                void WriteEdge(ControlFlowEdge edge)
                {
                    writer.WriteLine($"\t{GetId(edge.Source)} -> {GetId(edge.Target)}");
                }

                void WriteBlockNode(InstructionBlockNode block)
                {
                    writer.Write($"\t{GetId(block)}[");
                    writer.Write($"label=<{string.Join("<br/> ", block.Instructions.Select(instr => $"{instr.Offset:X4}:{instr.OpCode}"))}>");
                    writer.WriteLine("];");
                }

                void WriteExceptionNode(VirtualExceptionHandlerNode exceptionHandler)
                {
                    writer.Write($"\t{GetId(exceptionHandler)}[");
                    writer.Write($"label=\"{exceptionHandler.ExceptionType.Name}\"");
                    writer.WriteLine("];");
                }

                static string GetId(ControlFlowNode node)
                {
                    if (node is InstructionBlockNode block) return block.Header.Offset.ToString();
                    if (node is VirtualExceptionHandlerNode exceptionHandler) return exceptionHandler.ExceptionType.Name;

                    throw new NotSupportedException();
                }
            }
        }
    }
}
