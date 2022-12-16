using ConsoleTables;

using dnlib.DotNet;

using dnWalker.Concolic;
using dnWalker.Explorations;
using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class Benchmark
    {
        private readonly string _assembly;
        private readonly string[] _methods;
        private readonly string _testOutput;
        private readonly string _explorationOutput;
        private readonly string _statsOutput;
        private readonly int _repetitions;
        private readonly int _warmUp;

        private readonly IDefinitionProvider _definitionProvider;
        private readonly ExplorerHelper _explorerHelper;
        private readonly ExplorationOutputHelper _explorationOutputHelper;
        private readonly TestOutputHelper _testOutputHelper;
        private readonly ConsoleStatsOutputHelper _consoleStatsOutputHelper;
        private readonly CsvStatsOutputHelper _csvStatsOutputHelper;

        public Benchmark(string assembly, IEnumerable<string> methods, string testOutput, string explorationOutput, string statsOutput, int repetitions, int warmUp)
        {
            _assembly = assembly;
            _definitionProvider = new DefinitionProvider(Domain.LoadFromFile(assembly));

            _methods = methods
                .Where(m => !string.IsNullOrWhiteSpace(m) && !m.StartsWith(';'))
                .ToArray();
            _testOutput = Path.GetFullPath(testOutput);
            _explorationOutput = Path.GetFullPath(explorationOutput);
            _statsOutput = Path.GetFullPath(statsOutput);
            _repetitions = Math.Max(1, repetitions);
            _warmUp = warmUp;

            _explorerHelper= new ExplorerHelper(_definitionProvider);
            _explorationOutputHelper = new ExplorationOutputHelper(_definitionProvider, explorationOutput);
            _testOutputHelper = new TestOutputHelper(_definitionProvider, _testOutput);
            _consoleStatsOutputHelper = new ConsoleStatsOutputHelper();
            _csvStatsOutputHelper = new CsvStatsOutputHelper(_statsOutput);
        }

        public string Assembly
        {
            get
            {
                return _assembly;
            }
        }

        public IReadOnlyList<string> Methods
        {
            get
            {
                return _methods;
            }
        }

        public string TestOutput
        {
            get
            {
                return _testOutput;
            }
        }

        public string ExplorationOutput
        {
            get
            {
                return _explorationOutput;
            }
        }

        public int Repetitions
        {
            get
            {
                return _repetitions;
            }
        }


        public void Run()
        {
            if (_methods.Length== 0)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] No methods to test.");
                return; 
            }

            {
                string warmupMethod = _methods[0];
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting warmup on {warmupMethod}");
                RunMethod(warmupMethod, _warmUp);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Finished warmup on {warmupMethod}");
            }


            List<MethodStatistics> stats = new List<MethodStatistics>();

            // gather the data
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting benchmark. Assembly = {_assembly}, Methods count = {_methods.Length}");
            foreach (string method in _methods) 
            {
                stats.Add(RunMethod(method, _repetitions));
            }
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Finished benchmark. Assembly = {_assembly}, Methods count = {_methods.Length}");

            // output the data to csv
            _csvStatsOutputHelper.WriteStats(stats);

            // output the data to console
            _consoleStatsOutputHelper.WriteStats(stats);
        }

        private MethodStatistics RunMethod(string method, int count)
        {

            Stopwatch sw = new Stopwatch();
            try
            {
                MethodDef md = _definitionProvider.GetMethodDefinition(method);
                string simplifiedName = $"{md.DeclaringType.Name}::{md.Name}";

                double[] explorationDurations = new double[count];
                double[] testGenerationDurations = new double[count];

                double[] okPaths = new double[count];
                double[] errorPaths = new double[count];
                double[] dontKnowPaths = new double[count];
                double[] testCount = new double[count];

                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Starting benchmarking method {method}");
                for (int i = 0; i < explorationDurations.Length; ++i)
                {
                    sw.Start();
                    // run the exploration
                    ExplorationResult result = _explorerHelper.Explore(method);

                    ConcolicExploration exploration = result.ToExplorationData().Build();
                    sw.Stop();

                    explorationDurations[i] = sw.Elapsed.TotalMilliseconds;
                    okPaths[i] = result.Iterations.Count(it => it.Exception == null && it.Statistics.AssertionViolations == 0 && it.Statistics.Deadlocks == 0);
                    errorPaths[i] = result.Iterations.Count - okPaths[i];
                    dontKnowPaths[i] = result.ConstraintTrees.SelectMany(tree => tree.EnumerateNodes()).Count(ct => ct.IsUndecidable);
                    _explorationOutputHelper.Write(result, exploration);
                    


                    sw.Restart();
                    // run the generation
                    testCount[i] = _testOutputHelper.GenerateTests(exploration).Methods.Count;

                    sw.Stop();
                    testGenerationDurations[i] = sw.Elapsed.TotalMilliseconds;
                    sw.Reset();
                }
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Finished benchmarking method {method}");

                (double explMean, double explDlt) = StatisticsHelper.GetConfidenceInterval(explorationDurations);
                (double genMean,  double genDlt)  = StatisticsHelper.GetConfidenceInterval(testGenerationDurations);
                (double okMean,   double okDlt)   = StatisticsHelper.GetConfidenceInterval(okPaths);
                (double errMean,  double errDlt)  = StatisticsHelper.GetConfidenceInterval(errorPaths);
                (double dnMean,   double dnDlt)   = StatisticsHelper.GetConfidenceInterval(dontKnowPaths);
                (double tstMean,  double tstDlt)  = StatisticsHelper.GetConfidenceInterval(testCount);


                return new MethodStatistics(simplifiedName, explMean, explDlt, genMean, genDlt, okMean, okDlt, errMean, errDlt, dnMean, dnDlt, tstMean, tstDlt, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Benchmarking method {method} failed:");
                Console.WriteLine(ex);
                return MethodStatistics.Failed(method);
            }
        }
    }
}
