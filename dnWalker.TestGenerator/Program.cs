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
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Configuration>(args);

            result.WithParsed(configuration =>
            {
                RunGenerator(configuration);
            });
            result.WithNotParsed(errors =>
            {
                foreach(var e in errors)
                {
                    Console.WriteLine(e.ToString());
                }
            });
        }

        public static void RunGenerator(Configuration configuration)
        {
            // setup dnWalker.Concolic.Explorer
            var explorer = Explorer.ForAssembly(configuration.AssemblyPath, new Z3.Solver());

            var iterationData = new Dictionary<string, IReadOnlyList<ExplorationIterationData>>();
            var context = configuration.TestSuit.GetContext();

            var assemblyName = System.IO.Path.GetFileNameWithoutExtension(configuration.AssemblyPath);

            context.CreateProject(configuration.OutputFolder, assemblyName + ".Tests");

            // run it for each requested method
            foreach (var method in configuration.Methods)
            {
                explorer.Run(method);

                iterationData[method] = explorer.IterationData;

                context.WriteAsFacts(method, explorer.IterationData);
            }


            
        }

    }
}
