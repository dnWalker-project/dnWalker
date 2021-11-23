using CommandLine;

using dnWalker.Parameters;
using dnWalker.TestGenerator.Explorations.Xml;
using dnWalker.TestGenerator.Reflection;
using dnWalker.TestGenerator.XUnit;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace dnWalker.TestGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dnWalker.TestGenerator");

            CommandLine.Parser.Default.ParseArguments<CommandLineArguments>(args)
                .WithParsed(RunTestGenerator)
                .WithNotParsed(errors =>
                {
                    foreach (Error e in errors)
                    {
                        Console.WriteLine(e);
                    }
                });
        }

        private static void RunTestGenerator(CommandLineArguments args)
        {
            if (!File.Exists(args.ExplorationDataFileName)) throw new FileNotFoundException("ExplorationData file was not found!");

            IEnumerable<ExplorationData> explorations = XElement.Load(args.ExplorationDataFileName!).Elements("Exploration").Select(xe => xe.ToExplorationData());

            foreach (ExplorationData explorationData in explorations)
            {

                Assembly sutAssembly = Assembly.LoadFrom(explorationData.AssemblyFileName);

                TestGeneratorContext testData = new TestGeneratorContext(sutAssembly, explorationData);

                string? outputDirectory = Path.GetDirectoryName(args.OutputFileName);
                if (!string.IsNullOrWhiteSpace(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                string outputFile = $"{sutAssembly.GetName().Name}_{testData.SUTType.FullName!.Replace('.', '_')}_{testData.SUTMethod.Name}.Tests.cs";

                using (XUnitTestClassWriter testWriter = new XUnitTestClassWriter(new StreamWriter(outputFile)))
                {
                    testWriter.Write(testData);
                }
            }
        }
    }
}
