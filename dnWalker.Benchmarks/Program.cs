// See https://aka.ms/new-console-template for more information
using CommandLine;

using dnWalker.Benchmarks;

using System.Globalization;
using System.Text;

Console.WriteLine("dnWalker benchmark");

// setup configuration
Parser.Default.ParseArguments<Options>(args)
    .WithParsed(RunBenchamrks)
    .WithNotParsed(PrintErrors);


Environment.Exit(2);

void PrintErrors(IEnumerable<Error> errors)
{
    foreach (Error error in errors)
    {
        Console.WriteLine(error);
    }
    Environment.Exit(1);
}

void RunBenchamrks(Options options)
{
    try
    {
        BenchmarkBuilder builder = new BenchmarkBuilder();
        builder.LoadOptions(options);

        Benchmark benchmark = builder.Build();

        benchmark.Run();

        Environment.Exit(0);
    }
    catch (Exception ex) 
    {
        Console.WriteLine("dnWalker benchmark failed");
        Console.WriteLine(ex);
    
        Environment.Exit(1);
    }



    //string statsOutput = Path.GetFullPath(options.StatisticsOutput ?? "stats.csv");

    //Directory.CreateDirectory(Path.GetDirectoryName(statsOutput) ?? ".");
    //using (StreamWriter writer = new StreamWriter(statsOutput)) 
    //{
    //    // write headers
    //    writer.WriteLine(
    //        """
    //        "Method";"Time Explr [ms]";"Delta Expl [ms]";"Time Gen [ms]";"Delta Gen [ms]";"Iterations []";"Tests []"
    //        """
    //    );

    //    // write data
    //    foreach (MethodBenchmarkInfo info in data) 
    //    {
    //        writer.WriteLine(GetCSV(info));
    //    }
    //}

    //Environment.Exit(0);
}

//string GetCSV(MethodBenchmarkInfo data)
//{
//    StringBuilder sb = new StringBuilder();
//    sb.Append($"\"{data.MethodName}\";");

//    if (data.Passed)
//    {
//        (double explMean, double explDelta) = GetConfidenceInteraval(data.ExplorationDurations.Select(ts => ts.TotalMilliseconds));
//        sb.Append($"\"{explMean.ToString(CultureInfo.InvariantCulture)}\";\"{explDelta.ToString(CultureInfo.InvariantCulture)}\";");

//        (double genMean, double genDelta) = GetConfidenceInteraval(data.TestGenerationDurations.Select(ts => ts.TotalMilliseconds));
//        sb.Append($"\"{genMean.ToString(CultureInfo.InvariantCulture)}\";\"{genDelta.ToString(CultureInfo.InvariantCulture)}\";");

//        sb.Append($"\"{data.IterationCount}\";\"{data.TestCount}\"");
//    }
//    else
//    {
//        sb.Append("Benchmark failed...");
//    }

//    return sb.ToString();
//}

//(double explMean, double explDelta) GetConfidenceInteraval(IEnumerable<double> explorationDurations)
//{
//    double[] samples = explorationDurations.ToArray();

//    (double m, double s) = MathNet.Numerics.Statistics.Statistics.MeanStandardDeviation(samples);

//    double t = MathNet.Numerics.Distributions.StudentT.InvCDF(m, s, samples.Length - 1, 0.025); // 95% confidence
//    t *= (s / Math.Sqrt(samples.Length));
//    return (m, t);
//}