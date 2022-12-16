using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class BenchmarkBuilder
    {
        private readonly List<string> _methods = new List<string>();

        public IList<string> Methods => _methods;
        public string? TestOutput { get; set; }
        public string? ExplorationOutput { get; set; }
        public string? StatisticsOutput { get; set; }
        public string? Assembly { get; set; }
        public int Repetitions { get; set; }
        public int WarmUp { get; set; }

        public void LoadOptions(Options options) 
        {
            foreach (string line in File.ReadAllLines(options.Methods))
            {
                _methods.Add(line);
            }

            TestOutput = options.TestOutput;
            ExplorationOutput = options.ExplorationOutput;
            StatisticsOutput = options.StatisticsOutput;
            Assembly = options.Assembly;
            Repetitions = options.Repeatitions;
            WarmUp = options.WarmpUp;
        }

        public Benchmark Build()
        {
            return new Benchmark(Assembly ?? throw new InvalidOperationException("Assembly must be specified!"), 
                                 Methods, 
                                 TestOutput ?? throw new InvalidOperationException("TestOutput must be specified"),
                                 ExplorationOutput ?? throw new InvalidOperationException("TestOutput must be specified"),
                                 StatisticsOutput ?? "stats.csv",
                                 Repetitions,
                                 WarmUp);
        }
    }
}
