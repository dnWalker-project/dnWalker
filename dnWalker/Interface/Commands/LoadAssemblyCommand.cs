using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;

namespace dnWalker.Interface.Commands
{
    internal class LoadAssemblyCommand : ICommand
    {
        private readonly string[] _assemblyFileNames;

        public LoadAssemblyCommand(params string[] assemblyFileNames)
        {
            _assemblyFileNames = assemblyFileNames;
        }
        public LoadAssemblyCommand(IEnumerable<string> assemblyFileNames)
        {
            _assemblyFileNames = assemblyFileNames.ToArray();
        }

        public IReadOnlyList<string> AssemblyFileNames
        {
            get
            {
                return _assemblyFileNames;
            }
        }

        public CommandResult Execute(IAppModel appModel)
        {
            if (_assemblyFileNames.All(f => appModel.LoadAssembly(f)))
            {
               return CommandResult.Success;
            }
            return CommandResult.FailContinue(-1);
        }
    }
}