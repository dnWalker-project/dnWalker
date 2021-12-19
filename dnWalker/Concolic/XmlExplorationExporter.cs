using dnWalker.Parameters;
using dnWalker.Parameters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Concolic
{
    public class XmlExplorationExporter : IExplorationExtension
    {
        private Explorer _explorer;
        private readonly string _file;

        private readonly XElement _rootElement;
        private XElement _currentExplorationElement;
        private XElement _currentIterationElement;

        private string _currentSUTName = "";

        public XmlExplorationExporter(string file)
        {
            _file = file;


            _rootElement = new XElement("Explorations");
        }

        public void Register(Explorer explorer)
        {
            _explorer = explorer;
            _explorer.ExplorationStarted += OnExplorationStarted;
            _explorer.ExplorationFinished += OnExplorationFinished;
            _explorer.ExplorationFailed += OnExplorationFailed;

            _explorer.IterationStarted += OnIterationStarted;
            _explorer.IterationFinished += OnIterationFinished;
        }

        public void Unregister(Explorer explorer)
        {
            _explorer.ExplorationStarted -= OnExplorationStarted;
            _explorer.ExplorationFinished -= OnExplorationFinished;
            _explorer.ExplorationFailed -= OnExplorationFailed;

            _explorer.IterationStarted -= OnIterationStarted;
            _explorer.IterationFinished -= OnIterationFinished;
        }


        private void SaveData()
        {
            string outFile = _file;

            // TODO: setup proper placeholders...
            if (outFile.Contains('{'))
            {
                outFile = outFile.Replace("{SUT}", _currentSUTName);
            }

            _rootElement.Save(outFile);
        }

        private void OnExplorationStarted(object sender, ExplorationStartedEventArgs e)
        {
            _currentSUTName = e.Method.Name;

            // create a new exploration element
            _currentExplorationElement = new XElement("Exploration");
            _currentExplorationElement.SetAttributeValue("AssemblyName", e.AssemblyName);
            _currentExplorationElement.SetAttributeValue("AssemblyFileName", e.AssemblyFileName);
            _currentExplorationElement.SetAttributeValue("MethodNameSignature", e.MethodSignature);
            _currentExplorationElement.SetAttributeValue("IsStatic", e.IsStatic);
            _currentExplorationElement.SetAttributeValue("Solver", e.SolverType.FullName);


            _rootElement.Add(_currentExplorationElement);
        }

        private void OnExplorationFinished(object sender, ExplorationFinishedEventArgs e)
        {
            SaveData();

            _currentSUTName = "";
        }

        private void OnExplorationFailed(object sender, ExplorationFailedEventArgs e)
        {
            _currentExplorationElement.SetAttributeValue("Failed", true);
            SaveData();

            _currentSUTName = "";
        }

        private void OnIterationStarted(object sender, IterationStartedEventArgs e)
        {
            _currentIterationElement = new XElement("Iteration");
            _currentIterationElement.SetAttributeValue("Number", e.IterationNmber);

            _currentIterationElement.Add(e.ParameterStore.BaseContext.ToXml());


            _currentExplorationElement.Add(_currentIterationElement);
        }

        private void OnIterationFinished(object sender, IterationFinishedEventArgs e)
        {
            SaveData();
        }
    }

    public static class XmlExplorationExporterExtensions
    {
        public static XmlExplorationExporter ExportXmlData(this IExplorer explorer, string outputFile = "data.xml")
        {
            XmlExplorationExporter exporter = new XmlExplorationExporter(outputFile);
            explorer.AddExtension(exporter);
            return exporter;
        }
    }
}
