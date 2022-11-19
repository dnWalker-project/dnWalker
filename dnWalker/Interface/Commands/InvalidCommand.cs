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

        public string CommandString
        {
            get
            {
                return _commandString;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public CommandResult Execute(IAppModel appModel)
        {
            Console.WriteLine($"ERROR: Invalid command '{_commandString}', '{_message}'");
            return CommandResult.BreakFail(-1);
        }
    }
}
