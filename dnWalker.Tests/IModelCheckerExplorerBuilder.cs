using MMC;
using MMC.Data;
using MMC.State;

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

        //public string MethodName
        //{
        //    get; set;
        //}

        //public IDataElement[] Args
        //{
        //    get; set;
        //}

        IModelCheckerExplorerBuilder SetMethod(string methodName);
        IModelCheckerExplorerBuilder SetArgs(params IDataElement[] args);

        IModelCheckerExplorerBuilder SetArgs(Func<ExplicitActiveState, IDataElement[]> args);

        Explorer Build();
    }
}
