using System;

namespace dnWalker.Interface.Commands
{
    internal class ExitCommand : ICommand
    {
        public CommandResult Execute(IAppModel appModel)
        {
            Console.WriteLine("INFO: Running exit command");
            return CommandResult.BreakSuccess;
        }
    }
}