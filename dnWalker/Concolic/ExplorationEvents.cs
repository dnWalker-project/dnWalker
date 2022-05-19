using dnlib.DotNet;

using dnWalker.Symbolic;
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
        private readonly ExplorationResult _result;

        public ExplorationFinishedEventArgs(ExplorationResult result)
        {
            _result = result;
        }

        public ExplorationResult Result
        {
            get
            {
                return _result;
            }
        }
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
        public IterationStartedEventArgs(int iterationNmber, IReadOnlyModel inputModel)
        {
            IterationNmber = iterationNmber;
            InputModel = inputModel ?? throw new ArgumentNullException(nameof(inputModel));
        }

        public int IterationNmber { get; }
        public IReadOnlyModel InputModel { get; }
        public Constraint PreCondition => InputModel.Precondition;
    }

    public class IterationFinishedEventArgs : EventArgs
    {
        public IterationFinishedEventArgs(ExplorationIterationResult iterationResult, Path exploredPath)
        {
            Result = iterationResult ?? throw new ArgumentNullException(nameof(iterationResult));
            ExploredPath = exploredPath ?? throw new ArgumentNullException(nameof(exploredPath));
        }

        public ExplorationIterationResult Result { get; }
        public Path ExploredPath { get; }
    }
}
