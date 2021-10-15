using CommandLine;

using dnWalker.Concolic;

using System;
using System.Collections.Generic;
using System.Linq;

using Explorer = dnWalker.Concolic.Explorer;

namespace dnWalker.TestGenerator
{
    public class Program
    {
        static void Main(String[] args)
        {
            ParserResult<Configuration> result = Parser.Default.ParseArguments<Configuration>(args);

            result.WithParsed(configuration =>
            {
                RunGenerator(configuration);
            });
            result.WithNotParsed(errors =>
            {
                foreach(Error e in errors)
                {
                    Console.WriteLine(e.ToString());
                }
            });
        }

        public static void RunGenerator(Configuration configuration)
        {
            // setup dnWalker.Concolic.Explorer
            Explorer explorer = Explorer.ForAssembly(configuration.AssemblyPath, new Z3.Solver());

            Dictionary<String, IReadOnlyList<ExplorationIterationData>> iterationData = new Dictionary<String, IReadOnlyList<ExplorationIterationData>>();
            ITestSuitContext context = configuration.TestSuit.GetContext();

            String assemblyName = System.IO.Path.GetFileNameWithoutExtension(configuration.AssemblyPath);

            context.CreateProject(configuration.OutputFolder, assemblyName + ".Tests");

            // run it for each requested method
            foreach (String method in configuration.Methods)
            {
                explorer.Run(method);

                iterationData[method] = explorer.IterationData;

                context.WriteAsFacts(method, explorer.IterationData);
            }


            
        }

    }
}
