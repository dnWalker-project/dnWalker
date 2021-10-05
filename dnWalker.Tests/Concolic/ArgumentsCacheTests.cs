using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using dnWalker;
using dnWalker.Symbolic;
using dnWalker.Concolic;

using Xunit;
using FluentAssertions;

namespace dnWalker.Tests.Concolic
{

    [Trait("Category", "Functionality")]
    public class ArgumentsCacheTests
    {
        [Fact]
        public void WhenTryingToAddNull_Throws()
        {
            ArgumentsCache cache = new ArgumentsCache(3);

            IArg[] nullArgs = null;

            Assert.Throws<ArgumentNullException>(() => cache.TryAdd(nullArgs));
        }

        [Fact]
        public void WhenTryingToAddDifferntArgsNumber_Throws()
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] fourArgs = new IArg[4];

            Assert.Throws<ArgumentException>(() => threeArgsCache.TryAdd(fourArgs));
        }

        [Fact]
        public void AddingArgsToEmptyCache_Returns_True()
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] threeArgs = new IArg[]
            {
                SymbolicArgs.Arg("x", 5),
                SymbolicArgs.Arg("y", 4),
                SymbolicArgs.Arg("z", 3)
            };

            threeArgsCache.TryAdd(threeArgs).Should().BeTrue();
        }


        [Fact]
        public void AddingSameArgsToCache_Returns_False()
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] threeArgs = new IArg[]
            {
                SymbolicArgs.Arg("x", 5),
                SymbolicArgs.Arg("y", 4),
                SymbolicArgs.Arg("z", 3)
            };

            threeArgsCache.TryAdd(threeArgs);//.Should().BeTrue();
            threeArgsCache.TryAdd(threeArgs).Should().BeFalse();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" }, new object[] { 1, 2.5, "hi world"})]
        public void AddingSimiliarArgsToCache_Returns_True(object[] args1, object[] args2)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] threeArgs = new IArg[3];
            threeArgs[0] = SymbolicArgs.Arg("x", args1[0]);
            threeArgs[1] = SymbolicArgs.Arg("y", args1[1]);
            threeArgs[2] = SymbolicArgs.Arg("z", args1[2]);
            threeArgsCache.TryAdd(threeArgs);

            threeArgs[0] = SymbolicArgs.Arg("x", args2[0]);
            threeArgs[1] = SymbolicArgs.Arg("y", args2[1]);
            threeArgs[2] = SymbolicArgs.Arg("z", args2[2]);
            threeArgsCache.TryAdd(threeArgs).Should().BeTrue();
        }
    }
}
