﻿using dnlib.DotNet;

using dnWalker.Configuration;
using dnWalker.Input;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

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
        ExplorationResult Run(MethodDef entryPoint, IEnumerable<UserModel> userModel = null);

        public IDefinitionProvider DefinitionProvider { get; }
        public IConfiguration Configuration { get; }
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
        public static ExplorationResult Run(this IExplorer explorer, string methodName, IEnumerable<UserModel> inputModels = null)
        {
            return explorer.Run(explorer.DefinitionProvider.GetMethodDefinition(methodName), inputModels);
        }
}

}