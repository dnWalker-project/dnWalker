using dnWalker.Concolic.Parameters;
using dnWalker.Traversal;

using MMC;

using System;
using System.Collections.Generic;

namespace dnWalker.Concolic
{
    public interface IExplorer
    {
        event EventHandler<ExplorationFailedEventArgs> ExplorationFailed;
        event EventHandler<ExplorationFinishedEventArgs> ExplorationFinished;
        event EventHandler<ExplorationStartedEventArgs> ExplorationStarted;
        event EventHandler<IterationFinishedEventArgs> IterationFinished;
        event EventHandler<IterationStartedEventArgs> IterationStarted;

        void AddExtension(IExplorationExtension extension);
        void RemoveExtension(IExplorationExtension extension);
        void Run(string methodName, IDictionary<string, object> data = null);

        public dnWalker.Concolic.Traversal.PathStore PathStore { get; }
        public ParameterStore ParameterStore { get; }
        public int IterationCount { get; }

        public IConfig GetConfiguration();
    }

    public static class ExplorerExtensions
    {
        public static TExtension AddExtension<TExtension>(this IExplorer explorer) where TExtension : IExplorationExtension, new()
        {
            TExtension extension = new TExtension();
            explorer.AddExtension(extension);
            return extension;
        }
        public static TExtension AddExtension<TExtension>(this IExplorer explorer, Func<TExtension> extensionFactory) where TExtension : IExplorationExtension
        {
            TExtension extension = extensionFactory();
            explorer.AddExtension(extension);
            return extension;
        }
    }

}