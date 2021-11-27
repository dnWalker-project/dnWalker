using MMC;
using MMC.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public interface IModelCheckerExplorerBuilder : IConfigBuilder<IModelCheckerExplorerBuilder>
    {
        IModelCheckerExplorerBuilder OverrideLogger(Func<Logger> provideLogger);
        IModelCheckerExplorerBuilder OverrideDefinitionProvider(Func<DefinitionProvider> provideDefinitionProvider);
        IModelCheckerExplorerBuilder OverrideStatistics(Func<IStatistics> provideStatistics);

        public string MethodName
        {
            get; set;
        }

        public IDataElement[] Args
        {
            get; set;
        }

        IModelCheckerExplorerBuilder WithMethod(string methodName);
        IModelCheckerExplorerBuilder WithArgs(IDataElement[] args);

        Explorer BuildAndRun();
    }
}
