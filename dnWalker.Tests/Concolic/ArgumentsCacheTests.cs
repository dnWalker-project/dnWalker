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
        private const String ArgNames = "abcdefghijklmnopqrstuvwxyz";

        private static IArg[] ConstructArgs(params object[] values)
        {
            int cnt = values.Length;

            if (cnt > ArgNames.Length) throw new Exception("Too many arguments, only up to: " + ArgNames.Length + " received: " + cnt);

            IArg[] args = new IArg[cnt];
            for (int i = 0; i < cnt; ++i)
            {
                args[i] = SymbolicArgs.Arg(ArgNames[i].ToString(), values[i]);
            }
            return args;
        }

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

            IArg[] threeArgs = ConstructArgs(5, 4, 3);

            threeArgsCache.TryAdd(threeArgs).Should().BeTrue();
        }


        [Theory]
        [InlineData(new object[] { 5, 4, 3})]
        public void AddingSameArgsToCache_Returns_False(params object[] args)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] threeArgs = ConstructArgs(args);

            threeArgsCache.TryAdd(threeArgs);//.Should().BeTrue();

            threeArgs = ConstructArgs(args);

            threeArgsCache.TryAdd(threeArgs).Should().BeFalse();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" }, new object[] { 1, 2.5, "hi world"})]
        public void AddingSimiliarArgsToCache_Returns_True(object[] args1, object[] args2)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);

            IArg[] threeArgs = ConstructArgs(args1);
            threeArgsCache.TryAdd(threeArgs);

            threeArgs = ConstructArgs(args2);
            threeArgsCache.TryAdd(threeArgs).Should().BeTrue();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        [InlineData(new object[] { 1, 2.5, "hi world" })]
        public void RemovingFromEmptyCache_Returns_False(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Remove(threeArgs).Should().BeFalse();
        }


        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        [InlineData(new object[] { 1, 2.5, "hi world" })]
        public void RemovingArgsWhichAreInTheCache_Returns_False(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.TryAdd(threeArgs);

            threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Remove(threeArgs).Should().BeTrue();
        }



        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        [InlineData(new object[] { 1, 2.5, "hi world" })]
        public void RemovingRemovedArgs_Returns_False(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.TryAdd(threeArgs);

            threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Remove(threeArgs);
            threeArgsCache.Remove(threeArgs).Should().BeFalse();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        public void Contains_OnEmptyCache_Returns_False(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Contains(threeArgs).Should().BeFalse();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        public void After_TryAdd_Contains_Returns_True(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.TryAdd(threeArgs);

            threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Contains(threeArgs).Should().BeTrue();
        }

        [Theory]
        [InlineData(new object[] { 1, 2.5, "hello world" })]
        public void After_Remove_Contains_Returns_False(params object[] argsArray)
        {
            ArgumentsCache threeArgsCache = new ArgumentsCache(3);
            IArg[] threeArgs = ConstructArgs(argsArray);

            threeArgsCache.TryAdd(threeArgs);

            threeArgs = ConstructArgs(argsArray);

            threeArgsCache.Remove(threeArgs);

            threeArgsCache.Contains(threeArgs).Should().BeFalse();
        }

    }
}
