using System;

namespace dnWalker.Interface.Commands
{
    internal class ExitCommand : ICommand
    {
        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine("INFO: Running exit command");
            return CommandResult.BreakSuccess;
        }
    }
}