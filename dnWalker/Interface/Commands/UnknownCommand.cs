using System;

namespace dnWalker.Interface.Commands
{
    internal class UnknownCommand : ICommand
    {
        private readonly string _command;



        internal UnknownCommand(string command)
        {
            _command = command;
        }

        public string Command
        {
            get
            {
                return _command;
            }
        }

        public CommandResult Execute(IAppModel appModel)
        {
            Console.WriteLine($"ERROR: Unknown command '{_command}'");
            return CommandResult.BreakFail(-1);
        }
    }
}