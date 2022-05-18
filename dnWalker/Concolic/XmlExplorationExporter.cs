using dnWalker.Explorations;
using dnWalker.Explorations.Xml;
using dnWalker.TypeSystem;

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
        private IExplorer _explorer;
        private readonly string _file;

        private ConcolicExploration.Builder _currentExploration;
        private ConcolicExplorationIteration.Builder _currentIteration;

        // TODO: disgusting hidden dependency!
        private XmlExplorationSerializer _serializer;

        public XmlExplorationExporter(string file)
        {
            _file = file;
        }

        public void Register(IExplorer explorer)
        {
            _explorer = explorer;
            _explorer.ExplorationStarted += OnExplorationStarted;
            _explorer.ExplorationFinished += OnExplorationFinished;
            _explorer.ExplorationFailed += OnExplorationFailed;

            _explorer.IterationStarted += OnIterationStarted;
            _explorer.IterationFinished += OnIterationFinished;

            _serializer = new XmlExplorationSerializer(new XmlModelSerializer(new TypeTranslator(explorer.DefinitionProvider), new MethodTranslator(explorer.DefinitionProvider)));
        }

        public void Unregister(IExplorer explorer)
        {
            _explorer.ExplorationStarted -= OnExplorationStarted;
            _explorer.ExplorationFinished -= OnExplorationFinished;
            _explorer.ExplorationFailed -= OnExplorationFailed;

            _explorer.IterationStarted -= OnIterationStarted;
            _explorer.IterationFinished -= OnIterationFinished;
        }

        public bool SaveIntermidiateData
        {
            get;
            set;
        } = false;

        private void TrySaveData(bool fullData = false)
        {
            if (fullData || SaveIntermidiateData)
            {

                string outFile = _file;

                _serializer.ToXml(_currentExploration.Build()).Save(outFile);
            }
        }

        private void OnExplorationStarted(object sender, ExplorationStartedEventArgs e)
        {
            // create a new exploration element
            _currentExploration = new ConcolicExploration.Builder();

            _currentExploration.AssemblyName = e.AssemblyName;
            _currentExploration.AssemblyFileName = e.AssemblyFileName;
            _currentExploration.MethodSignature = e.MethodSignature;
            _currentExploration.Solver = e.SolverType.FullName;
            _currentExploration.Start = DateTime.Now;

            TrySaveData();
        }

        private void OnExplorationFinished(object sender, ExplorationFinishedEventArgs e)
        {
            _currentExploration.End = DateTime.Now;
            TrySaveData(true);
            _currentExploration = null;
        }

        private void OnExplorationFailed(object sender, ExplorationFailedEventArgs e)
        {
            _currentExploration.Failed = true;
            TrySaveData(true);
            _currentExploration = null;
        }

        private void OnIterationStarted(object sender, IterationStartedEventArgs e)
        {
            _currentIteration = new ConcolicExplorationIteration.Builder();
            _currentIteration.IterationNumber = e.IterationNmber;
            _currentIteration.Start = DateTime.Now;

            TrySaveData();
        }

        private void OnIterationFinished(object sender, IterationFinishedEventArgs e)
        {
            _currentIteration.InputModel = e.SymbolicContext.InputModel;
            _currentIteration.OutputModel = e.SymbolicContext.OutputModel;
            _currentIteration.PathConstraint = e.ExploredPath.PathConstraintString; //GetConstraintStringWithAccesses(e.ParameterStore.ExecutionSet);
            _currentIteration.End = DateTime.Now;

            _currentIteration.StandardOutput = e.ExploredPath.Output ?? string.Empty;
            _currentIteration.ErrorOutput = string.Empty;
            _currentIteration.Exception = e.ExploredPath.Exception?.Type.FullName ?? string.Empty;

            _currentExploration.Iterations.Add(_currentIteration);

            TrySaveData();
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
