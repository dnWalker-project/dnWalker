using dnlib.DotNet;

using dnWalker.Concolic.Traversal;
using dnWalker.Configuration;
using dnWalker.Input;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using MMC;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace dnWalker.Concolic
{
    public interface IExplorer
    {
        event EventHandler<ExplorationFailedEventArgs> ExplorationFailed;
        event EventHandler<ExplorationFinishedEventArgs> ExplorationFinished;
        event EventHandler<ExplorationStartedEventArgs> ExplorationStarted;
        event EventHandler<IterationFinishedEventArgs> IterationFinished;
        event EventHandler<IterationStartedEventArgs> IterationStarted;

        ExplorationResult Run(MethodDef entryPoint, IExplorationStrategy strategy, IEnumerable<Constraint> preconditions = null);

        public IDefinitionProvider DefinitionProvider { get; }
        public IConfiguration Configuration { get; }
    }

    public static class ExplorerExtensions
    {
        public static ExplorationResult Run(this IExplorer explorer, MethodDef method, IEnumerable<UserModel> inputModels = null)
        {
            return explorer.Run(method, new SmartAllPathsCoverage(), inputModels.Select(m => GetConstraint(m, new CustomModuleExpressionFactory(explorer.DefinitionProvider.Context.MainModule))));
        }

        public static ExplorationResult Run(this IExplorer explorer, MethodDef method, IEnumerable<Constraint> constraints = null)
        {
            return explorer.Run(method, new SmartAllPathsCoverage(), constraints);
        }

        public static ExplorationResult Run(this IExplorer explorer, string methodName, IEnumerable<UserModel> inputModels = null)
        {
            return explorer.Run(methodName, new SmartAllPathsCoverage(), inputModels);
        }
        public static ExplorationResult Run(this IExplorer explorer, string methodName, IExplorationStrategy strategy, IEnumerable<UserModel> inputModels = null)
        {
            return explorer.Run(explorer.DefinitionProvider.GetMethodDefinition(methodName), strategy, inputModels?.Select(m => GetConstraint(m, new CustomModuleExpressionFactory(explorer.DefinitionProvider.Context.MainModule))));
        }

        private static Constraint GetConstraint(UserModel m, ExpressionFactory expressionFactory)
        {
            Constraint c = new Constraint();
            c.AddExpressionConstraint(m.Build().GetFormula(expressionFactory));
            return c;
        }
    }

}