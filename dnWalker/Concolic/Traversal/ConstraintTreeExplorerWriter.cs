using MMC;

using System;
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

        const string Edge = "\t{0} -> {1};";
        const string End = "}";

        public static void Write(ConstraintTreeExplorer explorer, string file)
        {
            Write(explorer.Trees, file);

        }

        public static void Write(IEnumerable<ConstraintTree> trees, string file)
        {
            // dirty way for setting node ids...
            Dictionary<ConstraintNode, int> idLookup = new Dictionary<ConstraintNode, int>();
            int lastId = 0;
            int currentId = 0;

            using (StreamWriter writer = new StreamWriter(file))
            {
                // header
                writer.WriteLine(Begin);

                foreach (ConstraintTree tree in trees)
                {
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
                }

                // footer
                writer.WriteLine(End);

                void WriteNode(ConstraintNode node)
                {
                    writer.Write($"\t{currentId}[");

                    writer.Write($"style=filled ");
                    writer.Write($"fillcolor={GetColor(node)} ");
                    writer.Write($"label=<{GetLabel(node)}> ");

                    writer.WriteLine("];");
                }
            }
        }

        public static bool TryWrite(ConstraintTreeExplorer explorer, string file)
        {
            try
            {
                Write(explorer, file);
                return true;
            }
            catch
            {
                return false;
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
            return $"{ToHtmlString(node.Constraint?.ToString())}<br/> {ToHtmlString(node.Edge?.ToString() ?? "no-edge")} <br/> ({ToHtmlString(string.Join(", ", node.Iterations))})";
        }

        private static string ToHtmlString(string v)
        {
            if (v == null) return "true";

            return v
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;");
        }
    }
}
