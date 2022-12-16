using CommandLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class Options
    {
        [Option("test-output", Default = "tests", HelpText = "File path to output test classes.")]
        public string TestOutput { get; set; } = string.Empty;

        [Option("expl-output", Default = "explorations", HelpText = "Path to output exploration data.")]
        public string ExplorationOutput { get; set; } = string.Empty;

        [Option("stats", Default = "stats.csv", HelpText = "File name for the statistics.")]
        public string StatisticsOutput { get; set; } = string.Empty;

        [Option("assembly", Default = "", Required = true, HelpText = "Path to the explored assembly.")]
        public string Assembly { get; set; } = string.Empty;

        [Option("methods", Default = "", Required = true, HelpText = "Path to file which contains methods to test.")]
        public string Methods { get; set; } = string.Empty;

        [Option("repetitions", Default = 10, HelpText = "Number of repetitions of each method exploration for statistics.")]
        public int Repeatitions { get; set; } = 10;

        [Option("warm-up", Default = 3, HelpText = "Number of repetitions to warmup the execution.")]
        public int WarmpUp { get; set; } = 10;
    }
}
