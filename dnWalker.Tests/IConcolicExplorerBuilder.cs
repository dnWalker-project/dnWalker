﻿using dnWalker.Concolic;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public interface IConcolicExplorerBuilder : IConfigBuilder<IConcolicExplorerBuilder>
    {
        IConcolicExplorerBuilder With(IExplorationExtension extension);

        IConcolicExplorerBuilder OverrideSolver(Func<ISolver> solverFactory);
        IConcolicExplorerBuilder OverrideLogger(Func<Logger> loggerFactory);
        IConcolicExplorerBuilder OverrideDefinitionProvider(Func<DefinitionProvider> definitionProviderFactory);

        IExplorer Build();

    }

    public static class TestExplorerBuilderExtensions
    {
        public static IConcolicExplorerBuilder With<TExtension>(this IConcolicExplorerBuilder builder)
            where TExtension : IExplorationExtension, new()
        {
            return builder.With(new TExtension());
        }

        public static IConcolicExplorerBuilder SetSolver<TSolver>(this IConcolicExplorerBuilder builder)
            where TSolver : ISolver, new()
        {
            return builder.OverrideSolver(static () => new TSolver());
        }
    }
}
