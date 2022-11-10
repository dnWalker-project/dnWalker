using CommandLine;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Interface
{
    internal class Options
    {
        public static Options Default { get; } = new Options()
        {
            ConfigurationFiles = Array.Empty<string>()
        };

        [Option('c', "configuration", Required = false, Separator = ',')]
        public IEnumerable<string> ConfigurationFiles { get; init; }

        [Option('b', "batch", Required = false, Default = false)]
        public bool Batch { get; init; }

        [Option('s', "script", Required = false, Default = null)]
        public string Script { get; init; }



        internal void Write(TextWriter writer)
        {
            writer.WriteLine($"Configuration: {string.Join(", ", ConfigurationFiles.Select(f => $"'{f}'"))}");
            writer.WriteLine($"Mode:          {(Batch ? "interactive" : "batch")}");
            writer.WriteLine($"Script:        {Script}");
        }
    }
}
