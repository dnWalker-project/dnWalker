using dnWalker.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMC
{
    public static class ExplorerIConfigurationuration
    {
        public static IConfiguration InitializeDefaults(this IConfiguration configuration)
        {
            configuration.SetRunTimeParameters(Array.Empty<string>());
            configuration.SetUseInstructionCache(true);
            configuration.SetSymmetryReduction(true);
            configuration.SetUseDPORCollapser(true);
            configuration.SetUseObjectEscapePOR(true);
            configuration.SetUseStatefulDynamicPOR(true);
            configuration.SetStopOnError(true);
            configuration.SetTraceOnError(true);
            configuration.SetExPostFactoMerging(true);
            configuration.SetMaxExploreInMinutes(double.PositiveInfinity);
            configuration.SetOptimizeStorageAtMegabyte(double.PositiveInfinity);
            configuration.SetMemoryLimit(double.PositiveInfinity);
            configuration.SetStateStorageSize(20);
            configuration.SetEvaluateRandom(true);

            return configuration;
        }

        public static int MaxIterations(this IConfiguration configuration)
        {
            return configuration.GetValue<int>("MaxIterations");
        }
        public static IConfiguration SetMaxIterations(this IConfiguration configuration, int maxIterations)
        {
            configuration.SetValue("MaxIterations", maxIterations);
            return configuration;
        }

        public static string[] RunTimeParameters(this IConfiguration configuration)
        {
            // make copy of the array since it is muttable
            return configuration.GetValue<string[]>("RunTimeParameters").ToArray();
        }
        public static IConfiguration SetRunTimeParameters(this IConfiguration configuration, string[] runTimeParameters)
        {
            // make copy of the array since it is muttable
            configuration.SetValue("RunTimeParameters", runTimeParameters);
            return configuration;
        }

        public static bool Verbose(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("Verbose");
        }
        public static IConfiguration SetVerbose(this IConfiguration configuration, bool verbose)
        {
            configuration.SetValue("Verbose", verbose);
            return configuration;
        }

        public static string AssemblyToCheckFileName(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("AssemblyToCheckFileName");
        }
        public static IConfiguration SetAssemblyToCheckFileName(this IConfiguration configuration, string assemblyFileName)
        {
            configuration.SetValue("AssemblyToCheckFileName", assemblyFileName);
            return configuration;
        }

        public static bool ShowStatistics(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("ShowStatistics");
        }
        public static IConfiguration SetShowStatistics(this IConfiguration configuration, bool showStatistics)
        {
            configuration.SetValue("ShowStatistics", showStatistics);
            return configuration;
        }


        public static bool Quiet(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("Quiet");
        }
        public static IConfiguration SetQuiet(this IConfiguration configuration, bool quiet)
        {
            configuration.SetValue("Quiet", quiet);
            return configuration;
        }

        public static bool Interactive(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("Interactive");
        }
        public static IConfiguration SetInteractive(this IConfiguration configuration, bool interactive)
        {
            configuration.SetValue("Interactive", interactive);
            return configuration;
        }

        public static MMC.LogPriority LogFilter(this IConfiguration configuration)
        {
            return configuration.GetValue<MMC.LogPriority>("LogPriority");
        }
        public static IConfiguration SetLogFilter(this IConfiguration configuration, MMC.LogPriority logPriority)
        {
            configuration.SetValue("LogPriority", logPriority);
            return configuration;
        }

        public static bool UseInstructionCache(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("UseInstructionCache");
        }
        public static IConfiguration SetUseInstructionCache(this IConfiguration configuration, bool useInstructionCache)
        {
            configuration.SetValue("UseInstructionCache", useInstructionCache);
            return configuration;
        }

        public static bool UseRefCounting(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("UseRefCounting");
        }
        public static IConfiguration SetUseRefCounting(this IConfiguration configuration, bool useRefCounting)
        {
            configuration.SetValue<bool>("UseRefCounting", useRefCounting);
            return configuration;
        }

        public static bool UseMarkAndSweep(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("UseMarkAndSweep");
        }
        public static IConfiguration SetUseMarkAndSweep(this IConfiguration configuration, bool useMarkAndSweep)
        {
            configuration.SetValue<bool>("UseMarkAndSweep", useMarkAndSweep);
            return configuration;
        }

        public static bool SymmetryReduction(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("SymmetryReduction");
        }
        public static IConfiguration SetSymmetryReduction(this IConfiguration configuration, bool symmetryReduction)
        {
            configuration.SetValue<bool>("SymmetryReduction", symmetryReduction);
            return configuration;
        }

        public static bool NonStaticSafe(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("NonStaticSafe");
        }
        public static IConfiguration SetNonStaticSafe(this IConfiguration configuration, bool nonStaticSafe)
        {
            configuration.SetValue<bool>("NonStaticSafe", nonStaticSafe);
            return configuration;
        }

        public static bool MemoisedGC(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("MemoisedGC");
        }
        public static IConfiguration SetMemoisedGC(this IConfiguration configuration, bool memoisedGCC)
        {
            configuration.SetValue<bool>("MemoisedGC", memoisedGCC);
            return configuration;
        }

        public static bool UseDPORCollapser(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("UseDPORCollapse");
        }
        public static IConfiguration SetUseDPORCollapser(this IConfiguration configuration, bool useDPORCollapser)
        {
            configuration.SetValue<bool>("UseDPORCollapse", useDPORCollapser);
            return configuration;
        }

        public static bool UseObjectEscapePOR(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("UseObjectEscapePOR");
        }
        public static IConfiguration SetUseObjectEscapePOR(this IConfiguration configuration, bool useObjectEscapePOR)
        {
            configuration.SetValue<bool>("UseObjectEscapePOR", useObjectEscapePOR);
            return configuration;
        }

        public static bool UseStatefulDynamicPOR(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("UseStatefulDynamicPOR");
        }
        public static IConfiguration SetUseStatefulDynamicPOR(this IConfiguration configuration, bool useStatefulDynmicPOR)
        {
            configuration.SetValue<bool>("UseStatefulDynamicPOR", useStatefulDynmicPOR);
            return configuration;
        }

        public static bool StopOnError(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("StopOnError");
        }
        public static IConfiguration SetStopOnError(this IConfiguration configuration, bool stopOnError)
        {
            configuration.SetValue<bool>("StopOnError", stopOnError);
            return configuration;
        }

        public static bool TraceOnError(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("TraceOnError");
        }
        public static IConfiguration SetTraceOnError(this IConfiguration configuration, bool traceOnError)
        {
            configuration.SetValue<bool>("TraceOnError", traceOnError);
            return configuration;
        }

        public static bool OneTraceAndStop(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("OneTraceAndStop");
        }
        public static IConfiguration SetOneTraceAndStop(this IConfiguration configuration, bool oneTraceAndStop)
        {
            configuration.SetValue<bool>("OneTraceAndStop", oneTraceAndStop);
            return configuration;
        }

        public static bool ExPostFactoMerging(this IConfiguration configuration)
		{
			return configuration.GetValue<bool>("ExPostFactoMerging");
        }
        public static IConfiguration SetExPostFactoMerging(this IConfiguration configuration, bool exPostFactoMerging)
        {
            configuration.SetValue<bool>("ExPostFactoMerging", exPostFactoMerging);
            return configuration;
        }

        public static double MaxExploreInMinutes(this IConfiguration configuration)
		{
			return configuration.GetValue<double>("MaxExploreInMinutes");
        }
        public static IConfiguration SetMaxExploreInMinutes(this IConfiguration configuration, double maxExploreInMinutes)
        {
            configuration.SetValue<double>("MaxExploreInMinutes", maxExploreInMinutes);
            return configuration;
        }

        public static double OptimizeStorageAtMegabyte(this IConfiguration configuration)
		{
			return configuration.GetValue<double>("OptimizeStorageAtMegabyte");
        }
        public static IConfiguration SetOptimizeStorageAtMegabyte(this IConfiguration configuration, double optimizeStorageAtMegabytes)
        {
            configuration.SetValue<double>("OptimizeStorageAtMegabyte", optimizeStorageAtMegabytes);
            return configuration;
        }

        public static double MemoryLimit(this IConfiguration configuration)
		{
			return configuration.GetValue<double>("MemoryLimit");
        }
        public static IConfiguration SetMemoryLimit(this IConfiguration configuration, double memoryLimit)
        {
            configuration.SetValue<double>("MemoryLimit", memoryLimit);
            return configuration;
        }

        public static string LogFileName(this IConfiguration configuration)
		{
			return configuration.GetValue<string>("LogFileName");
        }
        public static IConfiguration SetLogFileName(this IConfiguration configuration, string logFileName)
        {
            configuration.SetValue<string>("LogFileName", logFileName);
            return configuration;
        }

        public static int StateStorageSize(this IConfiguration configuration)
		{
			return configuration.GetValue<int>("StateStorageSize");
        }
        public static IConfiguration SetStateStorageSize(this IConfiguration configuration, int stateStorageSize)
        {
            configuration.SetValue<int>("StateStorageSize", stateStorageSize);
            return configuration;
        }

        public static bool EvaluateRandom(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("EvaluateRandom");
        }
        public static IConfiguration SetEvaluateRandom(this IConfiguration configuration, bool evaluateRandom)
        {
            configuration.SetValue("EvaluateRandom", evaluateRandom);
            return configuration;
        }
    }
}
