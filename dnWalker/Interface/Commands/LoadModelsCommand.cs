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
        private readonly string _modelsFile;

        public LoadModelsCommand(string modelsFile)
        {
            _modelsFile = modelsFile;
        }

        public CommandResult Execute(IAppModel appModel)
        {
            if (appModel.LoadModels(_modelsFile))
            {
                return CommandResult.Success;
            }
            return CommandResult.BreakFail(-1);
        }
    }
}
