using ConsoleTables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class ConsoleStatsOutputHelper
    {
        private static readonly ConsoleTableOptions TableOptions = new ConsoleTableOptions()
        {
            Columns = new[] { "Method", "Expl. Time [ms]", "Gen. Time [ms]", "OK", "ERROR", "DON'T KNOW", "Tests" },
            NumberAlignment = Alignment.Right
        };

        public void WriteStats(IEnumerable<MethodStatistics> stats)
        {
            ConsoleTable tbl = new ConsoleTable(TableOptions);

            // add headers
            //tbl.AddRow("Method", "Expl. Time [ms]", "Gen. Time [ms]", "OK Paths", "ERROR Paths", "Tests");

            foreach (MethodStatistics ms in stats.Where(ms => ms.Passed))
            {
                tbl.AddRow
                (
                    ms.Name,
                    $"{ms.ExplorationDuration:0.00} +/- {ms.ExplorationDurationDelta:0.00}",
                    $"{ms.GenerationDuration:0.00} +/- {ms.GenerationDurationDelta:0.00}",
                    $"{ms.OkPaths:0.00} +/- {ms.OkPathsDelta:0.00}",
                    $"{ms.ErrorPaths:0.00} +/- {ms.ErrorPathsDelta:0.00}",
                    $"{ms.DontKnowPaths:0.00} +/- {ms.DontKnowPathsDelta:0.00}",
                    $"{ms.Tests:0.00} +/- {ms.TestsDelta:0.00}"
                );
            }
            foreach (MethodStatistics ms in stats.Where(ms => !ms.Passed))
            {
                tbl.AddRow
                (
                    ms.Name,
                    $"FAILED",
                    $"",
                    $"",
                    $"",
                    $"",
                    $""
                );
            }

            Console.WriteLine(tbl.ToString());
        }
    }
}
