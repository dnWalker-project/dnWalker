using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.TestUtils;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;
//using dnWalker.TestWriter.Moq;
using dnWalker.TestWriter.Xunit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas
{
    public abstract class TestSchemaTestBase : DnlibTestBase<TestSchemaTestBase>
    {
        protected TestSchemaTestBase(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        protected IReadOnlyModel EmptyModel
        {
            get;
        } = new Model();

        protected IModel NewModel()
        {
            return new Model();
        }


        
        protected ITestContext GetTestContext(IMethod method, Action<ConcolicExplorationIteration.Builder>? initializeIteration = null, Action<ITestContext>? initializeTestContext = null)
        {
            ConcolicExplorationIteration iteration = GetIteration(method, initializeIteration);
            TestContext ctx = new TestContext(iteration);
            initializeTestContext?.Invoke(ctx);
            return ctx;
        }

        protected ConcolicExplorationIteration GetIteration(IMethod method, Action<ConcolicExplorationIteration.Builder>? initialize = null)
        {
            return GetIteration(builder =>
            {
                builder.MethodUnderTest = method;
                builder.AssemblyFileName = "DUMMY";
                builder.AssemblyName = "DUMMY";
                builder.Failed = false;
                builder.Solver = "DUMMY";
            }, initialize);
        }

        protected ConcolicExplorationIteration GetIteration(Action<ConcolicExploration.Builder> initializeExploration, Action<ConcolicExplorationIteration.Builder>? initialize = null)
        {
            ConcolicExploration.Builder builder = new ConcolicExploration.Builder();
            initializeExploration(builder);
            ConcolicExplorationIteration.Builder itBuilder = new ConcolicExplorationIteration.Builder()
            {
                StandardOutput = string.Empty,
                ErrorOutput = string.Empty,
                PathConstraint = "DUMMY",
                IterationNumber = 0,
                InputModel = EmptyModel,
                OutputModel = EmptyModel,
            };



            builder.Iterations.Add(itBuilder);
            initialize?.Invoke(itBuilder);

            return builder.Build().Iterations[0];
        }

        protected ITestTemplate GetTestTemplate()
        {
            return new TestTemplate(
                new IArrangePrimitives[]
                {
                    //new MoqArrange(),
                    new SimpleArrangePrimitives()
                },
                new IActPrimitives[]
                {
                    new SimpleActWriter(),
                },
                new IAssertPrimitives[]
                {
                    new XunitAssertions(),
                    new SimpleAssertPrimitives()
                });
        }
    }
}
