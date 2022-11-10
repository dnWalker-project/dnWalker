using dnWalker.Interface.Commands;

using System;
using System.Collections.Generic;

namespace dnWalker.Interface
{
    internal partial class InteractiveRunner : RunnerBase
    {
        public InteractiveRunner(Options options) : base(options)
        {
        }

        protected override IEnumerable<ICommand> GetCommands()
        {
            return new CommandsReader(Console.In);
        }
    }
}