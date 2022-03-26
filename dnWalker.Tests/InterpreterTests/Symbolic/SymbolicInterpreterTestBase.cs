using dnWalker.Symbolic;
using dnWalker.Traversal;
using dnWalker.TypeSystem;

using FluentAssertions;

using MMC;
using MMC.Data;
using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.InterpreterTests.Symbolic
{
    public class SymbolicInterpreterTestBase : TestBase
    {
        private const string AssemblyFilename = @"..\..\..\..\extras\dnSpy.Debugger.DotNet.Interpreter.Tests.dll";

        private static Lazy<IDefinitionProvider> Lazy = new Lazy<IDefinitionProvider>(() => new DefinitionProvider(TestBase.GetDefinitionContext(AssemblyFilename)));

        public SymbolicInterpreterTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper, Lazy.Value)
        {
            //_config.StateStorageSize = 5;
            OverrideModelCheckerExplorerBuilderInitialization(c => c.SetStateStorageSize(5));
        }

        protected void Test(string methodName, (object arg, string name)[] args , string pathCondition, string retValueExpression)
        {
            methodName = "dnSpy.Debugger.DotNet.Interpreter.Tests.TestClass." + methodName;

            IModelCheckerExplorerBuilder builder = GetModelCheckerBuilder();


            builder.SetArgs(cur =>
            {
                IDataElement[] argDE = new IDataElement[args.Length];
                for (int i = 0; i < args.Length; ++i)
                {
                    argDE[i] = DataElement.CreateDataElement(args[i].arg, cur.DefinitionProvider);
                    if (!string.IsNullOrWhiteSpace(args[i].name))
                    {
                        Expression argExpr = Expression.Parameter(args[i].arg.GetType(), args[i].name);
                        argDE[i].SetExpression(argExpr, cur);
                    }
                }
                return argDE;
            });
            builder.SetMethod(methodName);

            Explorer explorer = builder.Build();
            explorer.Run();

            ExplicitActiveState cur = explorer.cur;
            PathStore pathStore = cur.PathStore;
            IDataElement retValue = cur.CurrentThread.RetValue;

            if (pathCondition != null)
            {
                pathStore.CurrentPath.PathConstraintString.Should().Be(pathCondition);
            }

            if (retValueExpression != null)
            {
                retValue.TryGetExpression(cur, out Expression retExpression).Should().BeTrue();
                retExpression.ToString().Should().Be(retValueExpression);
            }
            else
            {
                retValue.TryGetExpression(cur, out _).Should().BeFalse();
            }
        }
    }
}
