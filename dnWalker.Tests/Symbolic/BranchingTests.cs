﻿using dnWalker.Concolic;
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

namespace dnWalker.Tests.Symbolic
{
    public class BranchingTests : SymbolicExamplesTestBase2
    {
        public BranchingTests(Xunit.Abstractions.ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void NoBranching_ShouldEnd_Within_1_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.NoBranching",
                (cgf) =>
                {
                    cgf.MaxIterations = 1;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(1);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void SingleBranching_ShouldEnd_Within_2_Iteration()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranching",
                (cgf) =>
                {
                    cgf.MaxIterations = 2;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void InvertingVariableValue()
        {
            Explore("Examples.Concolic.Simple.Branches.SingleBranchingWithModification",
                (cgf) =>
                {
                    cgf.MaxIterations = 2;
                },
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;

                    foreach (var p in paths)
                    {
                        System.Console.Out.WriteLine(p.GetPathInfo());
                    }

                    paths.Count().Should().Be(2);
                },
                SymbolicArgs.Arg("x", 10));
        }

        [Fact]
        [Trait("Category", "Concolic")]
        public void ExceedingMaxNumberOfIterations_Should_Throw()
        {
            Assert.Throws<MaxIterationsExceededException>(() =>
            {
                Explore("Examples.Concolic.Simple.Branches.MultipleBranchingWithMultipleParameters",
                    (cgf) =>
                    {
                        cgf.MaxIterations = 2;
                    },
                    (explorer) =>
                    {
                        //explorer.GetUnhandledException().Should().BeNull();
                        var paths = explorer.PathStore.Paths;

                        foreach (var p in paths)
                        {
                            System.Console.Out.WriteLine(p.GetPathInfo());
                        }

                        paths.Count().Should().Be(4);
                    },
                    SymbolicArgs.Arg("x", 10),
                    SymbolicArgs.Arg("y", -5),
                    SymbolicArgs.Arg("z", 6));
            });
        }

        // comparing variable "x" against 5

        [Fact]
        public void Test_Branch_Equals_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_Equals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x == 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x != 5)");
                },
                new Dictionary<string, object> { ["x"] = 5 });
        }
        [Fact]
        public void Test_Branch_Equals_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_Equals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x != 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x == 5)");
                },
                new Dictionary<string, object> { ["x"] = 4 });
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

                    //pathConstraints[0].Should().BeEquivalentTo("(x != 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("Not((x != 5))"); // receiving Not((x == 5)
                },
                new Dictionary<string, object> { ["x"] = 4 });
        }
        [Fact]
        public void Test_Branch_NotEquals_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_NotEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x == 5)" || pc == "Not((x != 5))" || pc == "Not(Not((x == 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x != 5)" || pc == "Not((x == 5))" || pc == "Not(Not((x != 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("Not((x != 5))");
                    //pathConstraints[1].Should().BeEquivalentTo("(x != 5)");
                },
                new Dictionary<string, object> { ["x"] = 5 });
        }

        [Fact]
        public void Test_Branch_GreaterThan_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThan",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x > 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x <= 5)");
                },
                new Dictionary<string, object> { ["x"] = 7 });
        }
        [Fact]
        public void Test_Branch_GreaterThan_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThan",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x <= 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x > 5)");
                },
                new Dictionary<string, object> { ["x"] = 4 });
        }

        [Fact]
        public void Test_Branch_GreaterThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x >= 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x < 5)");
                },
                new Dictionary<string, object> { ["x"] = 7 });
        }
        [Fact]
        public void Test_Branch_GreaterThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_GreaterThanOrEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x < 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("Not((x < 5))"); //(x => 5)
                },
                new Dictionary<string, object> { ["x"] = 4 });
        }

        [Fact]
        public void Test_Branch_LowerThan_TruePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThan",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x < 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x >= 5)");
                },
                new Dictionary<string, object> { ["x"] = 4 });
        }
        [Fact]
        public void Test_Branch_LowerThan_FalsePathFirst()
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThan",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();


                    pathConstraints[0].Should().Match(pc => pc == "(x >= 5)" || pc == "Not((x < 5))" || pc == "Not(Not((x >= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x < 5)" || pc == "Not((x >= 5))" || pc == "Not(Not((x < 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x >= 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x < 5)");
                },
                new Dictionary<string, object> { ["x"] = 7 });
        }

        [Fact]
        public void Test_Branch_LowerThanOrEquals_TruePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x <= 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x > 5)");
                },
                new Dictionary<string, object> { ["x"] =  4 });
        }
        [Fact]
        public void Test_Branch_LowerThanOrEquals_FalsePathFirst() // this throws MaxIterationExceeded
        {
            Explore("Examples.Concolic.Simple.Branches.Branch_LowerThanOrEquals",
                cfg => cfg.MaxIterations = 5,
                explorer =>
                {
                    PathStore pathStore = explorer.PathStore;

                    pathStore.Paths.Count().Should().Be(2);

                    String[] pathConstraints = pathStore.Paths.Select(p => p.PathConstraintString).ToArray();

                    pathConstraints[0].Should().Match(pc => pc == "(x > 5)" || pc == "Not((x <= 5))" || pc == "Not(Not((x > 5)))");
                    pathConstraints[1].Should().Match(pc => pc == "(x <= 5)" || pc == "Not((x > 5))" || pc == "Not(Not((x <= 5)))");

                    //pathConstraints[0].Should().BeEquivalentTo("(x > 5)");
                    //pathConstraints[1].Should().BeEquivalentTo("(x <= 5)");
                },
                new Dictionary<string, object> { ["x"] = 7 });
        }
    }
}
