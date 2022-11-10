using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface.Commands
{
    internal class InvalidCommand : ICommand
    {
        private readonly string _commandString;
        private readonly string _message;

        public InvalidCommand(string commandString, string message)
        {
            _commandString = commandString;
            _message = message;
        }

        public CommandResult Execute(AppModel appModel)
        {
            Console.WriteLine($"ERROR: Invalid command '{_commandString}', '{_message}'");
            return CommandResult.BreakFail(-1);
        }
    }
}
