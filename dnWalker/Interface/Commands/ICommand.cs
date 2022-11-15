using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface.Commands
{
    internal interface ICommand
    {
        CommandResult Execute(IAppModel appModel);
    }
}
