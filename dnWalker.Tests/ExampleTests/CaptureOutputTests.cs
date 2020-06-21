using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace dnWalker.Tests.ExampleTests
{
    [Trait("Category", "Examples")]
    public class CaptureOutputTests : ExamplesTestBase
    {
        public CaptureOutputTests() : base(Lazy.Value)
        {
        }

        [Fact]
        public void CaptureOutputTest1()
        {
            Explore("Examples.CaptureOutput.Capture1",
                null,//c => {},
                (explorer) =>
                {
                    explorer.GetUnhandledException().Should().BeNull();
                    explorer.GetExploredPaths().Count().Should().Be(1);
                    var tw = explorer.GetExploredPaths().First().Get<TextWriter>("System.Console.Out");
                    tw.ToString().Should().Be($"X=1{Environment.NewLine}Y=1{Environment.NewLine}Z=1{Environment.NewLine}");
                });
        }

        [Fact]
        public void CaptureOutputTest2()
        {
            Explore("Examples.CaptureOutput.Capture2",
                null,//c => {},
                (explorer) =>
                {
                    explorer.GetUnhandledException().Should().BeNull();
                    explorer.GetExploredPaths().Count().Should().Be(1);
                    var tw = explorer.GetExploredPaths().First().Get<TextWriter>("System.Console.Out");
                    tw.ToString().Should().Be($"X=2{Environment.NewLine}X=2{Environment.NewLine}");
                });
        }
    }
}
