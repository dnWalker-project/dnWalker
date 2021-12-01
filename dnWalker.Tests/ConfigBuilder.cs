using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public abstract class ConfigBuilder<T> : IConfigBuilder<T> 
        where T : IConfigBuilder<T>
    {
        private readonly Config _config = new Config();

        protected Config Config
        {
            get { return _config; }
        }

        protected abstract T GetOutterBuilder();

        #region IConfigBuilder
        public T SetMaxIterations(int maxIterations)
        {
            _config.MaxIterations = maxIterations;
            return GetOutterBuilder();
        }

        public T SetVerbose(bool value = true)
        {
            _config.Verbose = value;
            return GetOutterBuilder();
        }

        public T SetSymmetryReduction(bool value = true)
        {
            _config.SymmetryReduction = value;
            return GetOutterBuilder();
        }

        public T UseRefCounting(bool value = true)
        {
            _config.UseRefCounting = value;
            return GetOutterBuilder();
        }

        public T SetNonStaticSafe(bool value = true)
        {
            _config.NonStaticSafe = value;
            return GetOutterBuilder();
        }

        public T UseMarkAndSweep(bool value = true)
        {
            _config.UseMarkAndSweep = value;
            return GetOutterBuilder();
        }

        public T UseDPORCollapser(bool value = true)
        {
            _config.UseDPORCollapser = value;
            return GetOutterBuilder();
        }

        public T UseInstructionCache(bool value = true)
        {
            _config.UseInstructionCache = value;
            return GetOutterBuilder();
        }

        public T SetOneTraceAndStop(bool value = true)
        {
            _config.OneTraceAndStop = value;
            return GetOutterBuilder();
        }

        public T SetMaxExploreInMinutes(double value)
        {
            _config.MaxExploreInMinutes = value;
            return GetOutterBuilder();
        }

        public T StopOnError(bool value = true)
        {
            _config.StopOnError = value;
            return GetOutterBuilder();
        }

        public T SetInteractive(bool value = true)
        {
            _config.Interactive = value;
            return GetOutterBuilder();
        }

        public T SetMemoisedGC(bool value = true)
        {
            _config.MemoisedGC = value;
            return GetOutterBuilder();
        }

        public T SetMemoryLimit(double value)
        {
            _config.MemoryLimit = value;
            return GetOutterBuilder();
        }

        public T UseStatefulDynamicPOR(bool value = true)
        {
            _config.UseStatefulDynamicPOR = value;
            return GetOutterBuilder();
        }

        public T UseObjectEscapePOR(bool value = true)
        {
            _config.UseObjectEscapePOR = value;
            return GetOutterBuilder();
        }

        public T ShowStatistics(bool value = true)
        {
            _config.ShowStatistics = value;
            return GetOutterBuilder();
        }

        public T TraceOnError(bool value = true)
        {
            _config.TraceOnError = value;
            return GetOutterBuilder();
        }

        public T Quiet(bool value = true)
        {
            _config.Quiet = value;
            return GetOutterBuilder();
        }

        public T OptimizeStorageAtMegabyte(double value)
        {
            _config.OptimizeStorageAtMegabyte = value;
            return GetOutterBuilder();
        }

        public T SetStateStorageSize(int value)
        {
            _config.StateStorageSize = value;
            return GetOutterBuilder();
        }
        #endregion IConfigBuilder
    }
}
