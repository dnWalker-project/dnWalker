using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

using dnWalker.Configuration;

namespace dnWalker.Interface
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            Console.WriteLine("dnWalker");

            Options options = Parser.Default.ParseArguments<Options>(args)
                .MapResult(o => o, errors => Options.Default);

            return Main(options);
        }

        internal static int Main(Options options)
        {
            options.Write(Console.Out);

            AppModel appModel = AppModel.Create(options);
            IAppRunner runner = Runner.GetRunner(options);
            return runner.Run(appModel);
        }

    }
}
