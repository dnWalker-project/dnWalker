using dnWalker.TestWriter.Generators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Benchmarks
{
    public class CsvStatsOutputHelper
    {
        private readonly string _file;
        private readonly string _sepString;

        public CsvStatsOutputHelper(string file, char separator = ';')
        {
            _file = file;
            _sepString = $"{separator} ";
        }

        public void WriteStats(IEnumerable<MethodStatistics> stats)
        {
            using (TextWriter writer = new StreamWriter(_file)) 
            {
                WriteHeaders(writer);
                foreach (MethodStatistics ms in stats)
                {
                    WriteData(writer, ms);
                }
            }
        }

        private void WriteHeaders(TextWriter writer)
        {
            WriteRow(writer, "Method", "Expl Time [ms]", "Expl Time Dlt [ms", "Gen Time [ms]", "Gen Time Dlt [ms]", "OK Paths", "ERROR Paths", "DON'T KNOW Paths", "Tests");
        }

        private void WriteData(TextWriter writer, MethodStatistics ms)
        {
            WriteRow(writer, ms.Name, ms.ExplorationDuration, ms.ExplorationDurationDelta, ms.GenerationDuration, ms.GenerationDurationDelta, ms.OkPaths, ms.ErrorPaths, ms.DontKnowPaths, ms.Tests);
        }

        private void WriteRow(TextWriter writer, params object[] data)
        {
            string row = string.Join(_sepString, data.Select(d => $"\"{d}\""));
            writer.WriteLine(row);
        }
    }
}
