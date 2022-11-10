using System;

namespace dnWalker.Interface.Commands
{
    internal class LoadConfigurationCommand : ICommand
    {
        private string _configurationFileName;

        public LoadConfigurationCommand(string configurationFileName)
        {
            _configurationFileName = configurationFileName;
        }

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"Running LoadConfiguration command: '{_configurationFileName}'");
            return CommandResult.Success;
        }
    }
}