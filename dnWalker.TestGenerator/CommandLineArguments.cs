using CommandLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public class CommandLineArguments
    {
        [Option('e', "exploration-data")]
        public string? ExplorationDataFileName { get; set; }

        [Option('o', "output", Default = ".")]
        public string? OutputDir { get; set; }
    }
}
