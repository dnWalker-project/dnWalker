using dnWalker.Concolic;
using dnWalker.TypeSystem;
using dnWalker.Z3;

using System;

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

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"Running Explore command: '{_methodSpecification}'");

            try
            {
                ConcolicExplorer explorer = new ConcolicExplorer
                    (
                        appModel.DefinitionProvider,
                        appModel.Configuration, 
                        new MMC.Logger(),
                        new Z3Solver()
                    );

                ExplorationResult explorationResult = explorer.Run(GetFullMethodName(_methodSpecification, appModel.DefinitionProvider));

                // write the output


                return CommandResult.Success;
            }
            catch
            {
                return CommandResult.BreakFail(-1);
            }
        }

        private static string GetFullMethodName(string methodSpecification, IDefinitionProvider definitionProvider)
        {
            throw new NotImplementedException();
        }
    }
}