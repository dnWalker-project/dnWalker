using dnWalker.TestGenerator.Templates;
using dnWalker.TestGenerator.Tests.XunitProvider;

using System;
using System.Linq;

using FluentAssertions;
using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates
{
    public class ActTestClass
    {
        public static void NoReturnNoArgsStatic() { }

        public void NoReturnNoArgsInstance() { }

        public static int ReturnNoArgsStatic() => 0;

        public int ReturnNoArgsInstance() => 0;

        public static int ReturnArgsStatic(string strArg, double dblArg) => 0;

        public int ReturnArgsInstance(string strArg, double dblArg) => 0;

        public static void NoReturnArgsStatic(string strArg, double dblArg) { }

        public void NoReturnArgsInstance(string strArg, double dblArg) { }
    }

    public class BasicActTemplateTests
    {
        [Theory]
        [InlineData(nameof(ActTestClass.NoReturnNoArgsStatic), null, null, "ActTestClass.NoReturnNoArgsStatic();")]
        [InlineData(nameof(ActTestClass.NoReturnNoArgsInstance), "obj", null, "obj.NoReturnNoArgsInstance();")]
        [InlineData(nameof(ActTestClass.ReturnNoArgsStatic), null, "result", "int result = ActTestClass.ReturnNoArgsStatic();")]
        [InlineData(nameof(ActTestClass.ReturnNoArgsInstance), "obj", "result", "int result = obj.ReturnNoArgsInstance();")]
        [InlineData(nameof(ActTestClass.ReturnArgsStatic), null, "result", "int result = ActTestClass.ReturnArgsStatic(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.ReturnArgsInstance), "obj", "result", "int result = obj.ReturnArgsInstance(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.NoReturnArgsStatic), null, null, "ActTestClass.NoReturnArgsStatic(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.NoReturnArgsInstance), "obj", null, "obj.NoReturnArgsInstance(strArg, dblArg);")]
        public void WriteInvocation(string method, string? instanceSymbol, string? returnSymbol, string expected)
        {
            TestWriter output = new TestWriter();
            BasicActTemplate template = new BasicActTemplate();

            template.WriteAct(
                output,
                TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.ActTestClass")
                    .FindMethods(method).First(),
                instanceSymbol,
                returnSymbol);

            output.ToString().Should().Be(expected);
        }


        [Theory]
        [InlineData(nameof(ActTestClass.NoReturnNoArgsStatic), null, null, "method", "Action method = () => ActTestClass.NoReturnNoArgsStatic();")]
        [InlineData(nameof(ActTestClass.NoReturnNoArgsInstance), "obj", null, "method", "Action method = () => obj.NoReturnNoArgsInstance();")]
        [InlineData(nameof(ActTestClass.ReturnNoArgsStatic), null, "result", "method", "Func<int> method = () => ActTestClass.ReturnNoArgsStatic();")]
        [InlineData(nameof(ActTestClass.ReturnNoArgsInstance), "obj", "result", "method", "Func<int> method = () => obj.ReturnNoArgsInstance();")]
        [InlineData(nameof(ActTestClass.ReturnArgsStatic), null, "result", "method", "Func<int> method = () => ActTestClass.ReturnArgsStatic(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.ReturnArgsInstance), "obj", "result", "method", "Func<int> method = () => obj.ReturnArgsInstance(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.NoReturnArgsStatic), null, null, "method", "Action method = () => ActTestClass.NoReturnArgsStatic(strArg, dblArg);")]
        [InlineData(nameof(ActTestClass.NoReturnArgsInstance), "obj", null, "method", "Action method = () => obj.NoReturnArgsInstance(strArg, dblArg);")]
        public void WriteInvocationDelegate(string method, string? instanceSymbol, string? returnSymbol, string? delegateSymbol, string expected)
        {
            TestWriter output = new TestWriter();
            BasicActTemplate template = new BasicActTemplate();

            template.WriteActDelegate(
                output,
                TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.ActTestClass")
                    .FindMethods(method).First(),
                instanceSymbol,
                returnSymbol,
                delegateSymbol ?? "act");

            output.ToString().Should().Be(expected);
        }
    }
}

