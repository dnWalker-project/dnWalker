using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace dnWalker.Interface
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            Console.WriteLine("dnWalker");

            Options options = Parser.Default.ParseArguments<Options>(args)
                .MapResult(o => o, errors => Options.Default);

            options.Write(Console.Out);

            return Run(options);
        }

        internal static int Run(Options options)
        {
            return Runner.Run(options);
        }
    }
}
