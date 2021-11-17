using dnWalker.Parameters;
using dnWalker.Parameters.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.TestGenerator
{
    /// <summary>
    /// Represents data from a single exploration, exported by dnWalker.
    /// </summary>
    public class ExplorationData
    {
        public static ExplorationData FromXml(XElement xml)
        {
            ExplorationData ed = new ExplorationData();
            ed._assemblyName = xml.Attribute("AssemblyName")?.Value ?? throw new Exception("Exploration data XML must contain 'AssemblyName' attribute.");
            ed._assemblyFileName = xml.Attribute("AssemblyFileName")?.Value ?? throw new Exception("Exploration data XML must contain '_assemblyFileName' attribute.");
            ed._methodSignature = xml.Attribute("MethodSignature")?.Value ?? throw new Exception("Exploration data XML must contain 'MethodName' attribute.");
            ed._isStatic = bool.Parse(xml.Attribute("IsStatic")?.Value ?? throw new Exception("Exploration data XML must contain 'IsStatic' attribute."));

            ed._iterations = xml.Elements("Iteration").Select(xe => ExplorationIterationData.FromXml(xe)).ToArray();

            return ed;
        }


        private ExplorationIterationData[] _iterations = Array.Empty<ExplorationIterationData>();
        private string? _assemblyName;
        private string? _assemblyFileName;
        private string? _methodSignature;
        private bool _isStatic;

        private ExplorationData()
        { }
        internal ExplorationData(ExplorationIterationData[] iterations, string assemblyName, string assemblyFilePath, string fullMethodName, bool isStatic)
        {
            _iterations = iterations ?? throw new ArgumentNullException(nameof(iterations));
            _assemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
            _assemblyFileName = assemblyFilePath ?? throw new ArgumentNullException(nameof(assemblyFilePath));
            _methodSignature = fullMethodName ?? throw new ArgumentNullException(nameof(fullMethodName));
            _isStatic = isStatic;
        }

        public ExplorationIterationData[] Iterations 
        {
            get { return _iterations ?? throw new InvalidOperationException("Not initialized instance."); }
        }

        public string AssemblyName
        {
            get
            {
                return _assemblyName ?? throw new InvalidOperationException("Not initialized instance.");
            }
        }

        public string AssemblyFileName
        {
            get
            {
                return _assemblyFileName ?? throw new InvalidOperationException("Not initialized instance.");
            }
        }

        public string MethodSignature
        {
            get
            {
                return _methodSignature ?? throw new InvalidOperationException("Not initialized instance.");
            }
        }

        public bool IsStatic
        {
            get
            {
                return _isStatic;
            }
        }
    }

    /// <summary>
    /// Represents data from a single exploration iteration.
    /// </summary>
    public class ExplorationIterationData
    {
        public static ExplorationIterationData FromXml(XElement xml)
        {
            ExplorationIterationData eid = new ExplorationIterationData();

            eid._iterationNumber = int.Parse(xml.Attribute("Number")?.Value ?? throw new Exception("Exploration iteration XML must have a 'Number' attribute."));
            eid._pathConstraint = xml.Attribute("PathConstraint")?.Value ?? string.Empty;

            eid._parameters = xml.Element("ParameterStore")?.ToParameterStore() ?? throw new Exception("Exploration iteration XML must have a 'Iteration/InputParamters/ParameterStore' element.");
            //eid._resultParameters = xml.Element("ResultParameters")?.Element("ParameterStore")?.ToParameterStore() ?? throw new Exception("Exploration iteration XML must have a 'Iteration/ResultParameters/ParameterStore' element.");


            // TODO: when implemented, uncomment...
            // eid._exception = xml.Element("Exception")?.ToException();
            // eid._result = xml.Element("Result")?.ToParameter();
            // eid._stdOutput = xml.Element("StdOutput")?.Value ?? string.Empty;
            // eid._errOutput = xml.Element("ErrOutput")?.Value ?? string.Empty;

            return eid;
        }

        private ParameterStore? _parameters = null;
        //private ParameterStore? _resultParameters = null;

        private int _iterationNumber = 0;

        // TODO: not yet exporting
        private Exception? _exception = null;
        private string _stdOutput = string.Empty;
        private string _errOutput = string.Empty;
        private string _pathConstraint = string.Empty;

        private ExplorationIterationData()
        { }

        //internal ExplorationIterationData(ParameterStore inputParameters, ParameterStore resultParameters, int iterationNumber, Exception? exception, string stdOutput, string errOutput)
        internal ExplorationIterationData(ParameterStore inputParameters, int iterationNumber, Exception? exception, string stdOutput, string errOutput)
        {
            _parameters = inputParameters ?? throw new ArgumentNullException(nameof(inputParameters));
            //_resultParameters = resultParameters ?? throw new ArgumentNullException(nameof(resultParameters));
            _iterationNumber = iterationNumber;
            _exception = exception;
            _stdOutput = stdOutput ?? throw new ArgumentNullException(nameof(stdOutput));
            _errOutput = errOutput ?? throw new ArgumentNullException(nameof(errOutput));
        }

        public ParameterStore Parameters 
        {
            get { return _parameters ?? throw new InvalidOperationException("Not initialized instance."); }
        }
        //public ParameterStore ResultParameters
        //{
        //    get { return _resultParameters ?? throw new InvalidOperationException("Not initialized instance."); }
        //}

        public int IterationNumber
        {
            get
            {
                return _iterationNumber;
            }
        }

        public Exception? Exception
        {
            get
            {
                return _exception;
            }
        }

        public string StdOutput
        {
            get
            {
                return _stdOutput ?? throw new InvalidOperationException("Not initialized instance.");
            }
        }

        public string ErrOutput
        {
            get
            {
                return _errOutput ?? throw new InvalidOperationException("Not initialized instance.");
            }
        }

        public string PathConstraint 
        {
            get 
            { 
                return _pathConstraint; 
            }
        }
    }
}
