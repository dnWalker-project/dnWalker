﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Traversal
{
    public static class ConstraintTreeExplorerWriter
    {
        const string Begin = "digraph G {\n";
        const string Node = "\t{0} [label=\"{1}, {2}\"];";
        const string NodeDUP = "\t{0} [label=\"{1}, {2}, DUPL\"];";
        const string NodeUNSAT = "\t{0} [label=\"{1}, {2}, UNSAT\"];";

        const string Edge = "\t{0} -> {1};";
        const string btedge = "\t{0} -> {1} [label=\"bt\",style=dotted];";
        const string End = "}";

        public static void Write(ConstraintTreeExplorer explorer, string file)
        {
            ConstraintTree tree = explorer.Trees[0];

            // dirty way for setting node ids...
            Dictionary<ConstraintNode, int> idLookup = new Dictionary<ConstraintNode, int>();
            int lastId = 0;
            int currentId = 0;

            using (StreamWriter writer = new StreamWriter(file))
            {
                // header
                writer.WriteLine(Begin);

                Stack<ConstraintNode> frontier = new Stack<ConstraintNode>();
                frontier.Push(tree.Root);
                while (frontier.TryPop(out ConstraintNode node))
                {
                    currentId = lastId++;
                    idLookup.Add(node, currentId);

                    WriteNode(node);


                    if (!node.IsRoot)
                    {
                        writer.WriteLine(Edge, idLookup[node.Parent], currentId);
                    }

                    foreach (ConstraintNode child in node.Children)
                    {
                        frontier.Push(child);
                    }
                }

                // footer
                writer.WriteLine(End);

                void WriteNode(ConstraintNode node)
                {
                    writer.Write($"\t{currentId}[");

                    writer.Write($"style=filled ");
                    writer.Write($"label=\"{GetLabel(node)}\" ");
                    writer.Write($"fillcolor={GetColor(node)} ");

                    writer.WriteLine("];");
                }
            }
        }

        static string GetColor(ConstraintNode node)
        {
            if (node.IsPreconditionSource)
            {
                return "greenyellow";
            }
            if (node.IsExplored)
            {
                return "lightblue";
            }
            if (!node.IsSatisfiable)
            {
                return "orangered";
            }
            return "white";
        }

        static string GetLabel(ConstraintNode node)
        {
            return $"{node.Condition?.ToString() ?? "true"}, {node.Location}, ({string.Join(", ", node.Iterations)})";
        }
    }
}
