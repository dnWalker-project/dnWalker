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
    public class XmlExplorationExporter : IExplorationExtension, IDisposable
    {
        private Explorer2 _explorer;
        private readonly string _file;

        private readonly XElement _rootElement;
        private XElement _currentExplorationElement;
        private XElement _currentIterationElement;

        public XmlExplorationExporter(string file)
        {
            _file = file;


            _rootElement = new XElement("Explorations");
        }

        public void Register(Explorer2 explorer)
        {
            _explorer = explorer;
            _explorer.ExplorationStarted += OnExplorationStarted;
            _explorer.ExplorationFinished += OnExplorationFinished;
            _explorer.ExplorationFailed += OnExplorationFailed;

            _explorer.IterationStarted += OnIterationStarted;
            _explorer.IterationFinished += OnIterationFinished;
        }

        public void Dispose()
        {
            _explorer.ExplorationStarted -= OnExplorationStarted;
            _explorer.ExplorationFinished -= OnExplorationFinished;
            _explorer.ExplorationFailed -= OnExplorationFailed;

            _explorer.IterationStarted -= OnIterationStarted;
            _explorer.IterationFinished -= OnIterationFinished;
        }

        private void SaveData()
        {
            _rootElement.Save(_file);
        }

        private void OnExplorationStarted(object sender, ExplorationStartedEventArgs e)
        {
            // create a new exploration element
            _currentExplorationElement = new XElement("Exploration");
            _currentExplorationElement.SetAttributeValue("AssemblyName", e.AssemblyName);
            _currentExplorationElement.SetAttributeValue("AssemblyFileName", e.AssemblyFileName);
            _currentExplorationElement.SetAttributeValue("MethodNameSignature", e.MethodName);
            _currentExplorationElement.SetAttributeValue("IsStatic", e.IsStatic);
            _currentExplorationElement.SetAttributeValue("Solver", e.Solver);


            _rootElement.Add(_currentExplorationElement);
        }

        private void OnExplorationFinished(object sender, ExplorationFinishedEventArgs e)
        {
            SaveData();
        }

        private void OnExplorationFailed(object sender, ExplorationFailedEventArgs e)
        {
            _currentExplorationElement.SetAttributeValue("Failed", true);
            SaveData();
        }

        private void OnIterationStarted(object sender, IterationStartedEventArgs e)
        {
            _currentIterationElement = new XElement("Iteration");
            _currentIterationElement.SetAttributeValue("Number", e.IterationNmber);

            _currentIterationElement.Add(e.InputParameters.ToXml());


            _currentExplorationElement.Add(_currentIterationElement);
        }

        private void OnIterationFinished(object sender, IterationFinishedEventArgs e)
        {
            SaveData();
        }
    }
}
