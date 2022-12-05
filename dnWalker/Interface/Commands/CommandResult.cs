using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface.Commands
{
    internal record struct CommandResult(int ExitCode, bool Break)
    {
        public static readonly CommandResult Success = new CommandResult(0, false);
        public static readonly CommandResult BreakSuccess = new CommandResult(0, true);
        public static CommandResult FailContinue(int exitCode) => new CommandResult(exitCode, false);
        public static CommandResult BreakFail(int exitCode) => new CommandResult(exitCode, true);
    }
}
