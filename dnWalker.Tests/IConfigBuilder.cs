using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public interface IConfigBuilder<TConfigBuilder> where TConfigBuilder : IConfigBuilder<TConfigBuilder>
    {
        TConfigBuilder SetAssemblyFileName(string assemblyFileName);
        TConfigBuilder SetMaxIterations(int maxIterations);
        TConfigBuilder SetVerbose(bool value = true);
        TConfigBuilder SetSymmetryReduction(bool value = true);
        TConfigBuilder UseRefCounting(bool value = true);
        TConfigBuilder SetNonStaticSafe(bool value = true);
        TConfigBuilder UseMarkAndSweep(bool value = true);
        TConfigBuilder UseDPORCollapser(bool value = true);
        TConfigBuilder UseInstructionCache(bool value = true);
        TConfigBuilder SetOneTraceAndStop(bool value = true);
        TConfigBuilder SetMaxExploreInMinutes(double value);
        TConfigBuilder StopOnError(bool value = true);
        TConfigBuilder SetInteractive(bool value = true);
        TConfigBuilder SetMemoisedGC(bool value = true);
        TConfigBuilder SetMemoryLimit(double value);
        TConfigBuilder UseStatefulDynamicPOR(bool value = true);
        TConfigBuilder UseObjectEscapePOR(bool value = true);
        TConfigBuilder ShowStatistics(bool value = true);
        TConfigBuilder TraceOnError(bool value = true);
        TConfigBuilder Quiet(bool value = true);
        TConfigBuilder OptimizeStorageAtMegabyte(double value);

        TConfigBuilder SetStateStorageSize(int value);
    }
}
