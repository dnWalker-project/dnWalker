using CommandLine;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    public class Configuration
    {
        [Option(shortName: 'a', longName: "assembly", Required = true)]
        public string AssemblyPath { get; set; }

        [Option(shortName: 'm', longName: "methods", Required = false)]
        public IEnumerable<string> Methods { get; set; }

        [Option(shortName: 'o', longName: "output", Required = false, Default = "/GeneratedTests")]
        public string OutputFolder { get; set; }

        //[Option(shortName: 's', longName: "suit", Required = false, Default = TestSuit.xUnit)]
        //public TestSuit TestSuit { get; set; }
    }
}
