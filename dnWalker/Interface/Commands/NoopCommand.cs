using System;

namespace dnWalker.Interface.Commands
{
    internal class NoopCommand : ICommand
    {
        public CommandResult Execute(IAppModel appModel)
        {
            Console.WriteLine("INFO: Running noop command");
            return CommandResult.Success;
        }
    }
}