using dnlib.DotNet.Emit;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class FlowGraphWriter : IExplorationExtension
    {
        public string OutputFile { get; set; }

        public void Register(IExplorer explorer)
        {
            explorer.ExplorationStarted += OnExplorationStarted;
        }

        private void OnExplorationStarted(object sender, ExplorationStartedEventArgs e)
        {
            string outFile = OutputFile;

            // TODO: setup proper placeholders...
            if (outFile.Contains('{'))
            {
                outFile = outFile.Replace("{SUT}", e.Method.Name);
            }

            //using (TextWriter writer = File.CreateText(outFile))
            //{
            //    var dotWriter = new Echo.Core.Graphing.Serialization.Dot.DotWriter(writer);
            //    dotWriter.SubGraphAdorner = new ExceptionHandlerAdorner<Instruction>();
            //    dotWriter.NodeAdorner = new ControlFlowNodeAdorner<Instruction>();
            //    dotWriter.EdgeAdorner = new ControlFlowEdgeAdorner<Instruction>();
            //    dotWriter.Write(e.Method.ConstructStaticFlowGraph());
            //}


        }

        public void Unregister(IExplorer explorer)
        {
            explorer.ExplorationStarted -= OnExplorationStarted;
        }
    }

    public static class FlowGraphWriterExtensions
    {
        public static FlowGraphWriter WriteFlowGraph(this IExplorer explorer, string outputFile = "flowgraph.dot")
        {
            FlowGraphWriter writer = new FlowGraphWriter() { OutputFile = outputFile };
            explorer.AddExtension(writer);
            return writer;
        }
    }
}
