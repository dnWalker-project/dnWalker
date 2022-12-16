using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.Graphs.ControlFlow;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Xml;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Benchmarks
{
    public class ExplorationOutputHelper
    {
        private readonly IDefinitionProvider _definitionProvider;
        private readonly string _baseOutput;
        private readonly XmlExplorationSerializer _serializer;

        private static string GetSimplifiedMethodName(IMethod methodUnderTest)
        {
            return $"{methodUnderTest.DeclaringType.Name}_{methodUnderTest.Name}";
        }
        private static string GetXMLFile(string baseOutput)
        {
            return Path.Combine(baseOutput, "exploration.xml");
        }
        private static string GetCFGFile(string baseOutput)
        {
            return Path.Combine(baseOutput, "cfg.dot");
        }
        private static string GetConstraintTreeFile(string baseOutput)
        {
            return Path.Combine(baseOutput, "constraint_trees.dot");
        }

        public ExplorationOutputHelper(IDefinitionProvider definitionProvider, string baseOutput)
        {
            Directory.CreateDirectory(baseOutput);

            _definitionProvider = definitionProvider;
            _baseOutput = baseOutput;
            TypeParser tp = new TypeParser(definitionProvider);
            MethodParser mp = new MethodParser(definitionProvider, tp);
            _serializer = new XmlExplorationSerializer(new XmlModelSerializer(tp, mp));
        }

        public void Write(ExplorationResult result, ConcolicExploration exploration)
        {
            string methodName = GetSimplifiedMethodName(exploration.MethodUnderTest);
            string dirName = Path.Combine(_baseOutput, methodName);
            Directory.CreateDirectory(dirName);

            // xml
            {
                string xmlFile = GetXMLFile(dirName);
                XElement xml = _serializer.ToXml(exploration);
                xml.Save(xmlFile);
            }

            // cfg
            {
                string cfgFile = GetCFGFile(dirName);
                ControlFlowGraphWriter.Write(result.MethodTracer.Graph, cfgFile);
            }

            // constrain tree
            {
                string ctFile = GetConstraintTreeFile(dirName);
                ConstraintTreeExplorerWriter.Write(result.ConstraintTrees, ctFile);

            }
        }

    }
}
