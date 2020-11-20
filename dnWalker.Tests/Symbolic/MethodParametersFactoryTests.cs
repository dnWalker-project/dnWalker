using dnWalker.Symbolic;
using dnWalker.Tests.ExampleTests;
using FluentAssertions;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Z3.LinqBinding;

namespace dnWalker.Tests.Symbolic
{
    public class MethodParametersFactoryTests : SymbolicExamplesTestBase
    {
        public MethodParametersFactoryTests()
        {
        }

        [Fact]
        public void ArgsTest()
        {
            Explore("Examples.Simple.Branches.Branch",
                null,
                (explorer) =>
                {
                    //explorer.GetUnhandledException().Should().BeNull();
                    var paths = explorer.PathStore.Paths;
                    paths.Count().Should().Be(1);
                    var path = paths.First();
                    path.PathConstraints.Should().HaveCount(3);
                    string.Join("; ", path.PathConstraints).Should().Be("Not((x < 0)); (y < 0); (x < -y)");

                    var pc = //path.PathConstraints.Take(0).ToList();// 
                        path.PathConstraints.Take(path.PathConstraints.Count - 1).ToList();
                    pc.Add(Expression.Not(path.PathConstraints.Last()));

                    var andExpression = pc.Aggregate((a, b) => Expression.And(a, b));

                    using (var ctx = new Z3Context())
                    {
                        ctx.Log = Console.Out; // see internal logging
                        var theorem = ctx.NewTheorem(new { x = default(int), y = default(int) })
                            .Where(Expression.Lambda(andExpression, 
                                Expression.Parameter(typeof(int), "x"),
                                Expression.Parameter(typeof(int), "y")));
                        /*
                            from t in ctx.NewTheorem(new { x = default(bool), y = default(bool) })
                                      where t.x ^ t.y
                                      select t;*/

                        var result = theorem.Solve();
                        Console.WriteLine(result);
                    }

                    //output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
                },
                SymbolicArgs.Arg<int>("x"),
                SymbolicArgs.Arg("y", -2));
        }
    }
}
