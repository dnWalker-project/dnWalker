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
        public static IConfigurationBuilder InitializeDefaults(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetRunTimeParameters(Array.Empty<string>());
            configurationBuilder.SetUseInstructionCache(true);
            configurationBuilder.SetSymmetryReduction(true);
            configurationBuilder.SetUseDPORCollapser(true);
            configurationBuilder.SetUseObjectEscapePOR(true);
            configurationBuilder.SetUseStatefulDynamicPOR(true);
            configurationBuilder.SetStopOnError(true);
            configurationBuilder.SetTraceOnError(true);
            configurationBuilder.SetExPostFactoMerging(true);
            configurationBuilder.SetMaxExploreInMinutes(double.PositiveInfinity);
            configurationBuilder.SetOptimizeStorageAtMegabyte(double.PositiveInfinity);
            configurationBuilder.SetMemoryLimit(double.PositiveInfinity);
            configurationBuilder.SetStateStorageSize(20);
            configurationBuilder.SetEvaluateRandom(true);

            return configurationBuilder;
        }

        public static int MaxIterations(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<int>("MaxIterations");
        }
        public static IConfiguration SetMaxIterations(this IConfigurationBuilder configurationBuilder, int maxIterations)
        {
            configurationBuilder.SetValue("MaxIterations", maxIterations);
            return configurationBuilder;
        }

        public static string[] RunTimeParameters(this IConfiguration configuration)
        {
            // make copy of the array since it is muttable
            return configuration.GetValueOrDefault<string[]>("RunTimeParameters").ToArray();
        }
        public static IConfiguration SetRunTimeParameters(this IConfigurationBuilder configurationBuilder, string[] runTimeParameters)
        {
            // make copy of the array since it is muttable
            configurationBuilder.SetValue("RunTimeParameters", runTimeParameters);
            return configurationBuilder;
        }

        public static bool Verbose(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("Verbose");
        }
        public static IConfiguration SetVerbose(this IConfigurationBuilder configurationBuilder, bool verbose)
        {
            configurationBuilder.SetValue("Verbose", verbose);
            return configurationBuilder;
        }

        public static string AssemblyToCheckFileName(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<string>("AssemblyToCheckFileName");
        }
        public static IConfiguration SetAssemblyToCheckFileName(this IConfigurationBuilder configurationBuilder, string assemblyFileName)
        {
            configurationBuilder.SetValue("AssemblyToCheckFileName", assemblyFileName);
            return configurationBuilder;
        }

        public static bool ShowStatistics(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("ShowStatistics");
        }
        public static IConfiguration SetShowStatistics(this IConfigurationBuilder configurationBuilder, bool showStatistics)
        {
            configurationBuilder.SetValue("ShowStatistics", showStatistics);
            return configurationBuilder;
        }


        public static bool Quiet(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("Quiet");
        }
        public static IConfiguration SetQuiet(this IConfigurationBuilder configurationBuilder, bool quiet)
        {
            configurationBuilder.SetValue("Quiet", quiet);
            return configurationBuilder;
        }

        public static bool Interactive(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("Interactive");
        }
        public static IConfiguration SetInteractive(this IConfigurationBuilder configurationBuilder, bool interactive)
        {
            configurationBuilder.SetValue("Interactive", interactive);
            return configurationBuilder;
        }

        public static MMC.LogPriority LogFilter(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<MMC.LogPriority>("LogPriority");
        }
        public static IConfiguration SetLogFilter(this IConfigurationBuilder configurationBuilder, MMC.LogPriority logPriority)
        {
            configurationBuilder.SetValue("LogPriority", logPriority);
            return configurationBuilder;
        }

        public static bool UseInstructionCache(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("UseInstructionCache");
        }
        public static IConfiguration SetUseInstructionCache(this IConfigurationBuilder configurationBuilder, bool useInstructionCache)
        {
            configurationBuilder.SetValue("UseInstructionCache", useInstructionCache);
            return configurationBuilder;
        }

        public static bool UseRefCounting(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("UseRefCounting");
        }
        public static IConfiguration SetUseRefCounting(this IConfigurationBuilder configurationBuilder, bool useRefCounting)
        {
            configurationBuilder.SetValue("UseRefCounting", useRefCounting);
            return configurationBuilder;
        }

        public static bool UseMarkAndSweep(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("UseMarkAndSweep");
        }
        public static IConfiguration SetUseMarkAndSweep(this IConfigurationBuilder configurationBuilder, bool useMarkAndSweep)
        {
            configurationBuilder.SetValue("UseMarkAndSweep", useMarkAndSweep);
            return configurationBuilder;
        }

        public static bool SymmetryReduction(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("SymmetryReduction");
        }
        public static IConfiguration SetSymmetryReduction(this IConfigurationBuilder configurationBuilder, bool symmetryReduction)
        {
            configurationBuilder.SetValue("SymmetryReduction", symmetryReduction);
            return configurationBuilder;
        }

        public static bool NonStaticSafe(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("NonStaticSafe");
        }
        public static IConfiguration SetNonStaticSafe(this IConfigurationBuilder configurationBuilder, bool nonStaticSafe)
        {
            configurationBuilder.SetValue("NonStaticSafe", nonStaticSafe);
            return configurationBuilder;
        }

        public static bool MemoisedGC(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("MemoisedGC");
        }
        public static IConfiguration SetMemoisedGC(this IConfigurationBuilder configurationBuilder, bool memoisedGCC)
        {
            configurationBuilder.SetValue("MemoisedGC", memoisedGCC);
            return configurationBuilder;
        }

        public static bool UseDPORCollapser(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("UseDPORCollapse");
        }
        public static IConfiguration SetUseDPORCollapser(this IConfigurationBuilder configurationBuilder, bool useDPORCollapser)
        {
            configurationBuilder.SetValue("UseDPORCollapse", useDPORCollapser);
            return configurationBuilder;
        }

        public static bool UseObjectEscapePOR(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("UseObjectEscapePOR");
        }
        public static IConfiguration SetUseObjectEscapePOR(this IConfigurationBuilder configurationBuilder, bool useObjectEscapePOR)
        {
            configurationBuilder.SetValue("UseObjectEscapePOR", useObjectEscapePOR);
            return configurationBuilder;
        }

        public static bool UseStatefulDynamicPOR(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("UseStatefulDynamicPOR");
        }
        public static IConfiguration SetUseStatefulDynamicPOR(this IConfigurationBuilder configurationBuilder, bool useStatefulDynmicPOR)
        {
            configurationBuilder.SetValue("UseStatefulDynamicPOR", useStatefulDynmicPOR);
            return configurationBuilder;
        }

        public static bool StopOnError(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("StopOnError");
        }
        public static IConfiguration SetStopOnError(this IConfigurationBuilder configurationBuilder, bool stopOnError)
        {
            configurationBuilder.SetValue("StopOnError", stopOnError);
            return configurationBuilder;
        }

        public static bool TraceOnError(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("TraceOnError");
        }
        public static IConfiguration SetTraceOnError(this IConfigurationBuilder configurationBuilder, bool traceOnError)
        {
            configurationBuilder.SetValue("TraceOnError", traceOnError);
            return configurationBuilder;
        }

        public static bool OneTraceAndStop(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("OneTraceAndStop");
        }
        public static IConfiguration SetOneTraceAndStop(this IConfigurationBuilder configurationBuilder, bool oneTraceAndStop)
        {
            configurationBuilder.SetValue("OneTraceAndStop", oneTraceAndStop);
            return configurationBuilder;
        }

        public static bool ExPostFactoMerging(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<bool>("ExPostFactoMerging");
        }
        public static IConfiguration SetExPostFactoMerging(this IConfigurationBuilder configurationBuilder, bool exPostFactoMerging)
        {
            configurationBuilder.SetValue("ExPostFactoMerging", exPostFactoMerging);
            return configurationBuilder;
        }

        public static double MaxExploreInMinutes(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<double>("MaxExploreInMinutes");
        }
        public static IConfiguration SetMaxExploreInMinutes(this IConfigurationBuilder configurationBuilder, double maxExploreInMinutes)
        {
            configurationBuilder.SetValue("MaxExploreInMinutes", maxExploreInMinutes);
            return configurationBuilder;
        }

        public static double OptimizeStorageAtMegabyte(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<double>("OptimizeStorageAtMegabyte");
        }
        public static IConfiguration SetOptimizeStorageAtMegabyte(this IConfigurationBuilder configurationBuilder, double optimizeStorageAtMegabytes)
        {
            configurationBuilder.SetValue("OptimizeStorageAtMegabyte", optimizeStorageAtMegabytes);
            return configurationBuilder;
        }

        public static double MemoryLimit(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<double>("MemoryLimit");
        }
        public static IConfiguration SetMemoryLimit(this IConfigurationBuilder configurationBuilder, double memoryLimit)
        {
            configurationBuilder.SetValue("MemoryLimit", memoryLimit);
            return configurationBuilder;
        }

        public static string LogFileName(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<string>("LogFileName");
        }
        public static IConfiguration SetLogFileName(this IConfigurationBuilder configurationBuilder, string logFileName)
        {
            configurationBuilder.SetValue("LogFileName", logFileName);
            return configurationBuilder;
        }

        public static int StateStorageSize(this IConfiguration configuration)
		{
			return configuration.GetValueOrDefault<int>("StateStorageSize");
        }
        public static IConfiguration SetStateStorageSize(this IConfigurationBuilder configurationBuilder, int stateStorageSize)
        {
            configurationBuilder.SetValue("StateStorageSize", stateStorageSize);
            return configurationBuilder;
        }

        public static bool EvaluateRandom(this IConfiguration configuration)
        {
            return configuration.GetValueOrDefault<bool>("EvaluateRandom");
        }
        public static IConfiguration SetEvaluateRandom(this IConfigurationBuilder configurationBuilder, bool evaluateRandom)
        {
            configurationBuilder.SetValue("EvaluateRandom", evaluateRandom);
            return configurationBuilder;
        }
    }
}
