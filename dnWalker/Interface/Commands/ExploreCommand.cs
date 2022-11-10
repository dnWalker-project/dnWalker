using System;

namespace dnWalker.Interface.Commands
{
    internal class ExploreCommand : ICommand
    {
        private string _methodSpecification;

        public ExploreCommand(string methodSpecification)
        {
            _methodSpecification = methodSpecification;
        }

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"Running Explore command: '{_methodSpecification}'");
            return CommandResult.Success;
        }
    }
}