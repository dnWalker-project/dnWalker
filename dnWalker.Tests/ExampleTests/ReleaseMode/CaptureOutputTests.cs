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
    public class CaptureOutputTests : ReleaseExamplesTestBase
    {
        public CaptureOutputTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void CaptureOutputTest1()
        {
            MMC.Explorer explorer = GetModelCheckerBuilder("Examples.CaptureOutput.Capture1").Build();
            explorer.Run();

            explorer.GetUnhandledException().Should().BeNull();
            explorer.PathStore.Paths.Count().Should().Be(1);
            string output = explorer.PathStore.Paths.First().Output;
            output.Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");

        }

        [Fact]
        public void CaptureOutputTest2()
        {
            MMC.Explorer explorer = GetModelCheckerBuilder("Examples.CaptureOutput.Capture2").Build();
            explorer.Run();

            explorer.GetUnhandledException().Should().BeNull();
            explorer.PathStore.Paths.Count().Should().Be(1);
            string output = explorer.PathStore.Paths.First().Output;
            output.Should().Be($"X=2{Environment.NewLine}X=2{Environment.NewLine}");
        }
    }
}
