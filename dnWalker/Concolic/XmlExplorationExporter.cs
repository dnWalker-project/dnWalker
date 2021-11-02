using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Concolic
{
    public interface IExplorationExporter
    {
        void HookUp(Explorer2 explorer);
    }

    public static class ParameterSerializer
    {
        public static XElement ToXml(Parameter parameter)
        {
            switch (parameter)
            {
                case BooleanParameter p: return PrimitiveValueParameterToXml(p);
                case CharParameter p: return PrimitiveValueParameterToXml(p);
                case ByteParameter p: return PrimitiveValueParameterToXml(p);
                case SByteParameter p: return PrimitiveValueParameterToXml(p);
                case Int16Parameter p: return PrimitiveValueParameterToXml(p);
                case Int32Parameter p: return PrimitiveValueParameterToXml(p);
                case Int64Parameter p: return PrimitiveValueParameterToXml(p);
                case UInt16Parameter p: return PrimitiveValueParameterToXml(p);
                case UInt32Parameter p: return PrimitiveValueParameterToXml(p);
                case UInt64Parameter p: return PrimitiveValueParameterToXml(p);
                case SingleParameter p: return PrimitiveValueParameterToXml(p);
                case DoubleParameter p: return PrimitiveValueParameterToXml(p);
                default:
                    throw new NotSupportedException();
            }
            
        }

        private static XElement PrimitiveValueParameterToXml<T>(PrimitiveValueParameter<T> parameter) where T : struct
        {
            return new XElement(parameter.TypeName, new XAttribute("name", parameter.Name), parameter.Value);
        }
    }



    public class XmlExplorationExporter : IExplorationExporter, IDisposable
    {
        private Explorer2 _explorer;
        private readonly string _file;

        private readonly XElement _rootElement;
        private XElement _currentExplorationElement;
        private XElement _currentIterationElement;

        public XmlExplorationExporter(string file)
        {
            _file = file;


            _rootElement = new XElement("explorations");
        }

        public void HookUp(Explorer2 explorer)
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
            _currentExplorationElement = new XElement("exploration");
            _currentExplorationElement.SetAttributeValue("assembly-name", e.AssemblyName);
            _currentExplorationElement.SetAttributeValue("assembly-file-name", e.AssemblyFileName);
            _currentExplorationElement.SetAttributeValue("method-name", e.MethodName);
            _currentExplorationElement.SetAttributeValue("solver", e.Solver);


            _rootElement.Add(_currentExplorationElement);
        }

        private void OnExplorationFinished(object sender, ExplorationFinishedEventArgs e)
        {
            SaveData();
        }

        private void OnExplorationFailed(object sender, ExplorationFailedEventArgs e)
        {
            _currentExplorationElement.SetAttributeValue("failed", true);
            SaveData();
        }

        private void OnIterationStarted(object sender, IterationStartedEventArgs e)
        {
            _currentIterationElement = new XElement("iteration");
            _currentIterationElement.SetAttributeValue("number", e.IterationNmber);

            _currentIterationElement.Add(new XElement("input-parameters", e.InputParameters.RootParameters.Select(p => ParameterSerializer.ToXml(p))));


            _currentExplorationElement.Add(_currentIterationElement);
        }

        private void OnIterationFinished(object sender, IterationFinishedEventArgs e)
        {
            SaveData();
        }
    }
}
