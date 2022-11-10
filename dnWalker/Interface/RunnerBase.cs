using dnWalker.Interface.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal abstract class RunnerBase : IAppRunner
    {
        protected RunnerBase(Options options)
        {
            Options = options;
        }

        protected Options Options
        {
            get;
        }

        protected AppModel AppModel
        {
            get;
        }

        protected abstract IEnumerable<ICommand> GetCommands();

        public int Run(AppModel appModel)
        {
            bool br;
            int exitCode = 0;

            try
            {
                foreach (ICommand command in GetCommands()) 
                {
                    (exitCode, br) = command.Execute(appModel);
                    if (br)
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
            }

            return exitCode;
        }

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
