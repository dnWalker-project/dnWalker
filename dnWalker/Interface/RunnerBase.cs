using dnWalker.Interface.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal abstract class RunnerBase : IRunner
    {
        protected RunnerBase(Options options)
        {
            Options = options;
            AppModel = new AppModel();
        }

        protected Options Options
        {
            get;
        }

        protected AppModel AppModel
        {
            get;
        }

        public abstract int Run();

        protected int RunCommands(IEnumerable<string> commands)
        {
            foreach (string command in commands) 
            {
                (int exitCode, bool br) = RunCommand(command);
                if (br)
                {
                    return exitCode;
                }
            }
            return 0;
        }


        protected CommandResult RunCommand(string commandString)
        {
            ICommand command = Command.GetCommand(commandString);
            return command.Execute(AppModel);
        }
    }
}
