using dnWalker.Symbolic;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.Symbolic
{
    public class SymbolicArgTest
    {
        [Fact]
        public void SameInstanceOfSymbolicArg_Equals()
        {
            SymbolicArg<int> a = SymbolicArgs.Arg("a", 1);

            a.Equals(a).Should().BeTrue();
        }

        [Fact]
        public void ArgsWithSameNameAndValue_Equal()
        {
            SymbolicArg<int> a1 = SymbolicArgs.Arg("a", 1);
            SymbolicArg<int> a2 = SymbolicArgs.Arg("a", 1);

            a1.Equals(a2).Should().BeTrue();
        }

        [Fact]
        public void ArgsWithSameNameAndValue_HashCode_Should_Equal()
        {
            SymbolicArg<int> a1 = SymbolicArgs.Arg("a", 1);
            SymbolicArg<int> a2 = SymbolicArgs.Arg("a", 1);

            a1.GetHashCode().Equals(a2.GetHashCode()).Should().BeTrue();
        }
    }
}
