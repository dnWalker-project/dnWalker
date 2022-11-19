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


        protected abstract IEnumerable<ICommand> GetCommands();

        public int Run(IAppModel appModel)
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
                exitCode = 100;
            }

            return exitCode;
        }
    }
}
