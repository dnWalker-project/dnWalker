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

        public CommandResult Execute(AppModel appModel)
        {
            string extension = System.IO.Path.GetExtension(_modelsFile);

            if (extension == ".xml")
            {
                //using (XmlUserModelParser userModelParser = new XmlUserModelParser(appModel.DefinitionProvider))
                XmlUserModelParser userModelParser = new XmlUserModelParser(appModel.DefinitionProvider);
                {
                    foreach (UserModel userModel in userModelParser.ParseModelCollection(XElement.Load(_modelsFile)))
                    {
                        appModel.UserModels.Add(userModel);
                    }
                }

            }

            return CommandResult.Success;
        }
    }
}
