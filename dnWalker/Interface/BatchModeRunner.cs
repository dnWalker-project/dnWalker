using dnWalker.Interface.Commands;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dnWalker.Interface
{
    internal class BatchModeRunner : RunnerBase
    {
        public BatchModeRunner(Options options) : base(options)
        {
        }

        protected override IEnumerable<ICommand> GetCommands()
        {
            string scriptFile = Options.Script;
            if (string.IsNullOrWhiteSpace(scriptFile))
            {
                throw new InvalidOperationException("Commands not specified. The options -s --script must contain path to the command file.");
            }
            if (!File.Exists(scriptFile))
            {
                throw new InvalidOperationException("Commands file does not exists.");
            }

            return new CommandsReader(new StreamReader(scriptFile));
        }
    }
}