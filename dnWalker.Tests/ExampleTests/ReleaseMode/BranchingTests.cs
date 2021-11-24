using dnWalker.Concolic;
using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.ReleaseMode
{
    public class BranchingTests : ReleaseExamplesTestBase
    {
        public BranchingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void CaptureOutputTest1()
        {
            ExploreModelChecker("Examples.Branches.Branch",
                null,
                finished: explorer =>
                {
                    explorer.GetUnhandledException().Should().BeNull();
                    explorer.PathStore.Paths.Count().Should().Be(1);
                    var output = explorer.PathStore.Paths.First().Output;
                    output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
                });
        }


        [Fact]
        public void NoBranching_ShouldEnd_Within_1_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.NoBranching",
                initializeConfig: cgf =>
                {
                    cgf.MaxIterations = 1;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(1);
                },
                data: new Dictionary<string, object> { ["x"] = 10 });
        }

        [Fact]
        public void SingleBranching_ShouldEnd_Within_2_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranching",
                initializeConfig: cgf =>
                {
                    cgf.MaxIterations = 2;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                data: new Dictionary<string, object> { ["x"] = 10 });
        }

        [Fact]
        public void InvertingVariableValue()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranchingWithModification",
                initializeConfig: cgf =>
                {
                    cgf.MaxIterations = 2;
                },
                finished: explorer =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                data: new Dictionary<string, object> { ["x"] = 10 });
        }

        [Fact]
        public void ExceedingMaxNumberOfIterations_Should_Throw()
        {
            Assert.Throws<MaxIterationsExceededException>(() =>
            {
                Explore("Examples.Concolic.Simple.Branches.MultipleBranchingWithMultipleParameters",
                    initializeConfig: cgf =>
                    {
                            cgf.MaxIterations = 2;
                        },
                    finished: explorer =>
                    {
                        //explorer.GetUnhandledException().Should().BeNull();
                        var paths = explorer.PathStore.Paths;

                        foreach (var p in paths)
                        {
                            System.Console.Out.WriteLine(p.GetPathInfo());
                        }

                        paths.Count().Should().Be(4);
                    },
                    data: new Dictionary<string, object> 
                    { 
                        ["x"] = 10,
                        ["y"] = -5,
                        ["z"] = 6
                    });
            });
        }

        // comparing variable "x" against 5

        [Fact]
        public void Test_Branch_Equals_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_Equals",
                initializeConfig: cgf =>
                {
                    cgf.MaxIterations = 5;
                },
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 5 });
        }
        [Fact]
        public void Test_Branch_Equals_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_Equals",
                initializeConfig: cgf =>
                {
                    cgf.MaxIterations = 5;
                },
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }

        [Fact]
        public void Test_Branch_NotEquals_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_NotEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }
        [Fact]
        public void Test_Branch_NotEquals_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_NotEquals",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 5 });
        }

        [Fact]
        public void Test_Branch_GreaterThan_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThan",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 7 });
        }
        [Fact]
        public void Test_Branch_GreaterThan_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThan",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }

        [Fact]
        public void Test_Branch_GreaterThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");

                },
                data: new Dictionary<string, object> { ["x"] = 7 });
        }
        [Fact]
        public void Test_Branch_GreaterThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }

        [Fact]
        public void Test_Branch_LowerThan_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThan",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }
        [Fact]
        public void Test_Branch_LowerThan_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThan",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();


                    pathConstraints[0].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 7 });
        }

        [Fact]
        public void Test_Branch_LowerThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 4 });
        }
        [Fact]
        public void Test_Branch_LowerThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals",
                initializeConfig: cfg=> cfg.MaxIterations = 5,
                finished: explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                },
                data: new Dictionary<string, object> { ["x"] = 7 });
        }
    }
}
