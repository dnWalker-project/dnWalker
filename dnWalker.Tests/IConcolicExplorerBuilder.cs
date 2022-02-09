using dnWalker.Concolic;
using dnWalker.TypeSystem;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests
{
    public interface IConcolicExplorerBuilder : IConfigBuilder<IConcolicExplorerBuilder>
    {
        IConcolicExplorerBuilder With(IExplorationExtension extension);

        IConcolicExplorerBuilder OverrideSolver(Func<ISolver> solverFactory);
        IConcolicExplorerBuilder OverrideLogger(Func<Logger> loggerFactory);
        IConcolicExplorerBuilder OverrideDefinitionProvider(Func<IDefinitionProvider> definitionProviderFactory);

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

        public static IConcolicExplorerBuilder ExportXmlData(this IConcolicExplorerBuilder builder, [CallerMemberName]string methodName = null)
        {
            methodName ??= "data";
            return builder.With(new XmlExplorationExporter(methodName + ".xml"));
        }
    }
}
