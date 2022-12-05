using System;
using System.Diagnostics;
using System.Linq;

namespace dnWalker.Interface.Commands
{
    internal class ExploreCommand : ICommand
    {
        private readonly string _outputFile;
        private readonly string _methodSpecification;

        public ExploreCommand(string methodSpecification, string outputFile)
        {
            _methodSpecification = methodSpecification;
            _outputFile = outputFile;
        }

        public string OutputFile
        {
            get
            {
                return _outputFile;
            }
        }

        public string MethodSpecification
        {
            get
            {
                return _methodSpecification;
            }
        }

        public CommandResult Execute(IAppModel appModel)
        {
            Console.WriteLine($"Running Explore command: '{_methodSpecification}'");

            if (appModel.Explore(_methodSpecification, _outputFile))
            {
                return CommandResult.Success;
            }
            return CommandResult.FailContinue(-1);
        }

        
    }
}