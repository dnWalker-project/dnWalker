using dnWalker.TypeSystem;

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
            appModel.Domain.Load(_assemblyFileName);
            return CommandResult.Success;
        }
    }
}