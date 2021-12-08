using dnlib.DotNet;

using dnWalker.Parameters;
using dnWalker.Traversal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ExplorationStartedEventArgs : EventArgs
    {
        public ExplorationStartedEventArgs(string assemblyFileName, MethodDef method, Type solverType)
        {
            Method = method;
            SolverType = solverType;
            AssemblyFileName = assemblyFileName;
        }

        public string AssemblyFileName { get; }
        public string AssemblyName
        {
            get { return Method.Module.Assembly.Name; }
        }

        public string MethodSignature
        {
            get { return Method.FullName; }
        }

        public bool IsStatic
        {
            get { return Method.IsStatic; }
        }

        public Type SolverType { get; }

        public MethodDef Method { get; }
    }
    public class ExplorationFinishedEventArgs : EventArgs
    {
    }

    public class ExplorationFailedEventArgs : EventArgs
    {
        public ExplorationFailedEventArgs(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }

    public class IterationStartedEventArgs : EventArgs
    {
        public IterationStartedEventArgs(int iterationNmber, ParameterStore inputParameters)
        {
            IterationNmber = iterationNmber;
            InputParameters = inputParameters;
        }

        public int IterationNmber { get; }
        public ParameterStore InputParameters { get; }
    }

    public class IterationFinishedEventArgs : EventArgs
    {
        public IterationFinishedEventArgs(int iterationNumber, object methodResult, Path exploredPath)
        {
            IterationNumber = iterationNumber;
            MethodResult = methodResult;
            ExploredPath = exploredPath;
        }

        public int IterationNumber { get; }
        public Object MethodResult { get; }
        public Path ExploredPath { get; }
    }
}
