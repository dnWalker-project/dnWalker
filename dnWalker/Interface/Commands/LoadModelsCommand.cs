using dnWalker.Input;
using dnWalker.Input.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dnWalker.Interface.Commands
{
    internal class LoadModelsCommand : ICommand
    {
        private readonly string[] _modelsFiles;

        public LoadModelsCommand(params string[] modelsFiles)
        {
            _modelsFiles = modelsFiles;
        }
        public LoadModelsCommand(IEnumerable<string> modelsFiles)
        {
            _modelsFiles = modelsFiles.ToArray();
        }

        public IReadOnlyList<string> ModelsFile
        {
            get
            {
                return _modelsFiles;
            }
        }

        public CommandResult Execute(IAppModel appModel)
        {
            if (_modelsFiles.All(f => appModel.LoadModels(f)))
            {
                return CommandResult.Success;
            }
            return CommandResult.BreakFail(-1);
        }
    }
}
