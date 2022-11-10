using System;

namespace dnWalker.Interface.Commands
{
    internal class LoadAssemblyCommand : ICommand
    {
        private readonly string _assemblyFileName;

        public LoadAssemblyCommand(string assemblyFileName)
        {
            _assemblyFileName = assemblyFileName;
        }

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"Running LoadAssembly command: '{_assemblyFileName}'");
            return CommandResult.Success;
        }
    }
}