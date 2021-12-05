using dnWalker.Concolic.Parameters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Concolic
{

    // TODO: reference the class lib dnWalker.Parameters => change target framework from .net framework to .net5 or .net6

    public static class XmlSerializer
    {
        public static XElement ToXml(this Parameter parameter)
        {
            switch (parameter)
            {
                case BooleanParameter p: return ToXml(p);
                case CharParameter p: return ToXml(p);
                case ByteParameter p: return ToXml(p);
                case SByteParameter p: return ToXml(p);
                case Int16Parameter p: return ToXml(p);
                case Int32Parameter p: return ToXml(p);
                case Int64Parameter p: return ToXml(p);
                case UInt16Parameter p: return ToXml(p);
                case UInt32Parameter p: return ToXml(p);
                case UInt64Parameter p: return ToXml(p);
                case SingleParameter p: return ToXml(p);
                case DoubleParameter p: return ToXml(p);
                case ObjectParameter p: return ToXml(p);
                case InterfaceParameter p: return ToXml(p);
                case ArrayParameter p: return ToXml(p);
                default:
                    throw new NotSupportedException();
            }

        }

        public static XElement ToXml(this CharParameter parameter)
        {
            char symbol = parameter.Value.HasValue ? parameter.Value.Value : default(char);

            string unicodeFormat = string.Format(@"U+{0:x4}", (int)symbol).ToUpper();

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), unicodeFormat);
        }

        public static XElement ToXml(this SingleParameter parameter)
        {
            float number = parameter.Value.HasValue ? parameter.Value.Value : default(float);

            string numberRepr;
            if (float.IsNaN(number))
            {
                numberRepr = "NAN";
            }
            else if (float.IsPositiveInfinity(number))
            {
                numberRepr = "INF";
            }
            else if (float.IsNegativeInfinity(number))
            {
                numberRepr = "-INF";
            }
            else
            {
                numberRepr = number.ToString();
            }

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), numberRepr);
        }

        public static XElement ToXml(this DoubleParameter parameter)
        {
            double number = parameter.Value.HasValue ? parameter.Value.Value : default(double);

            string numberRepr;
            if (double.IsNaN(number))
            {
                numberRepr = "NAN";
            }
            else if (double.IsPositiveInfinity(number))
            {
                numberRepr = "INF";
            }
            else if (double.IsNegativeInfinity(number))
            {
                numberRepr = "-INF";
            }
            else
            {
                numberRepr = number.ToString();
            }

            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), numberRepr);
        }

        public static XElement ToXml<T>(this PrimitiveValueParameter<T> parameter) where T : struct
        {
            return new XElement("PrimitiveValue", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), parameter.Value.HasValue ? parameter.Value.Value : default(T));
        }

        public static XElement ToXml(this ObjectParameter parameter)
        {
            bool isNull = parameter.IsNull ?? true;
            XElement xml = new XElement("Object", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownFields()
                                 .Select(p =>
                                 {
                                     XElement fieldXml = new XElement("Field", new XAttribute("Name", p.Key), ToXml(p.Value));
                                     return fieldXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this InterfaceParameter parameter)
        {
            bool isNull = parameter.IsNull ?? true;

            XElement xml = new XElement("Interface", new XAttribute("Type", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownMethodResults()
                                 .Select(p =>
                                 {
                                     XElement methodResultXml = new XElement("MethodResult", new XAttribute("Name", p.Key));

                                     methodResultXml.Add(p.Value.Select(r =>
                                     {
                                         XElement callResultXml = new XElement("Call", new XAttribute("CallNumber", r.Key), ToXml(r.Value));
                                         return callResultXml;
                                     }));

                                     return methodResultXml;
                                 }));
            }
            return xml;
        }

        public static XElement ToXml(this ArrayParameter parameter)
        {
            bool isNull = parameter.IsNull ?? true;

            XElement xml = new XElement("Array", new XAttribute("ElementType", parameter.TypeName), new XAttribute("Name", parameter.Name), new XAttribute("IsNull", isNull), new XAttribute("Length", parameter.Length ?? 0));

            if (!isNull)
            {
                xml.Add(parameter.GetKnownItems()
                                 .Select(p =>
                                 {
                                     XElement itemXml = new XElement("Item", new XAttribute("Index", p.Key), ToXml(p.Value));

                                     return itemXml;
                                 }));
            }

            return xml;
        }

        public static XElement ToXml(this ParameterStore store)
        {
            XElement storeXml = new XElement("ParameterStore", store.RootParameters.Select(p => ToXml(p)));
            return storeXml;
        }
    }


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

            _currentIterationElement.Add(e.InputParameters.ToXml());


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
