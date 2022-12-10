using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.TestWriter.Generators.Arrange;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public static class TestTemplateExtensions
    {
        private static bool TryExecutePrimitive<T>(IEnumerable<T> primitives, Func<T, bool> exec)
        {
            return primitives.Any(x => exec(x));
        }

        private static void ExecutePrimitiveOrThrow<T>(IEnumerable<T> primitives, Func<T, bool> exec, string? message = null) 
        {
            if (!TryExecutePrimitive(primitives, exec))
            {
                throw new InvalidOperationException(message);
            }
        }

        #region Arrange
        public static void WriteArrange(this ITestTemplate testTemplate, ITestContext testContext, IWriter output)
        {
            Arranger arranger = new Arranger(testTemplate, testContext, output);
            arranger.WriteArrangeHeap();
            arranger.WriteArrangeStaticFields();
            arranger.WriteArrangeMethodArguments();
        }

        public static void WriteArrangeCreateInstance(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string symbol)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeCreateInstance(testContext, output, symbol));
        }

        public static void WriteArrangeInitializeField(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string symbol, IField field, string literal)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeField(testContext, output, symbol, field, literal));
        }

        public static void WriteArrangeInitializeStaticField(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, IField field, string literal)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeStaticField(testContext, output, field, literal));
        }

        public static void WriteArrangeInitializeMethod(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<string> literals)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeMethod(testContext, output, symbol, method, literals));
        }

        public static void WriteArrangeConstrainedInitializeMethod(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<KeyValuePair<Expression, string>> constrainedLiterals, string fallbackLiteral)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeConstrainedMethod(testContext, output, symbol, method, constrainedLiterals, fallbackLiteral));
        }

        public static void WriteArrangeInitializeStaticMethod(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, IMethod method, IReadOnlyList<string> literals)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeStaticMethod(testContext, output, method, literals));
        }

        public static void WriteArrangeInitializeArrayElement(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string symbol, int index, string literal)
        {
            ExecutePrimitiveOrThrow(testTemplate.ArrangeWriters, a => a.TryWriteArrangeInitializeArrayElement(testContext, output, symbol, index, literal));
        }
        #endregion Assert

        #region Act
        public static void WriteAct(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string? returnSymbol = null)
        {
            ExecutePrimitiveOrThrow(testTemplate.ActWriters, act => act.TryWriteAct(testContext, output, returnSymbol), $"Failed to write 'act'");
        }

        public static void WriteActDelegate(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string? returnSymbol = null, string delegateSymbol = "act")
        {
            ExecutePrimitiveOrThrow(testTemplate.ActWriters, act => act.TryWriteActDelegate(testContext, output, returnSymbol, delegateSymbol), $"Failed to write 'act delegate'");
        }
        #endregion Act


        #region Assert
        public static void WriteAssertNull(this ITestTemplate testTemplate, ITestContext context, IWriter output, string symbol)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNull(context, output, symbol), $"Failed to write 'assert null'");
        }

        public static void WriteAssertNotNull(this ITestTemplate testTemplate, ITestContext context, IWriter output, string symbol)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNotNull(context, output, symbol), $"Failed to write 'assert not null'");
        }

        public static void WriteAssertEqual(this ITestTemplate testTemplate, ITestContext context, IWriter output, string actual, string expected)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertEqual(context, output, actual, expected), $"Failed to write 'assert equal'");
        }

        public static void WriteAssertNotEqual(this ITestTemplate testTemplate, ITestContext context, IWriter output, string actual, string expected)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNotEqual(context, output, actual, expected), $"Failed to write 'assert not equal'");
        }

        public static void WriteAssertSame(this ITestTemplate testTemplate, ITestContext context, IWriter output, string actual, string expected)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertSame(context, output, actual, expected), $"Failed to write 'assert same'");
        }

        public static void WriteAssertNotSame(this ITestTemplate testTemplate, ITestContext context, IWriter output, string actual, string expected)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNotSame(context, output, actual, expected), $"Failed to write 'assert not same'");
        }

        public static void WriteAssertLength(this ITestTemplate testTemplate, ITestContext context, IWriter output, string arraySymbol, string expected)
        {
            string arrayLengthSymbol = $"{arraySymbol}.Length";
            WriteAssertEqual(testTemplate, context, output, arrayLengthSymbol, expected);
        }
        public static void WriteAssertCount(this ITestTemplate testTemplate, ITestContext context, IWriter output, string collectionSymbol, string expected)
        {
            string arrayLengthSymbol = $"{collectionSymbol}.Count()";
            WriteAssertEqual(testTemplate, context, output, arrayLengthSymbol, expected);
        }

        public static void WriteAssertExceptionThrown(this ITestTemplate testTemplate, ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertExceptionThrown(context, output, delegateSymbol, exceptionType), $"Failed to write 'assert exception thrown'");
        }
        public static void WriteAssertExceptionNotThrown(this ITestTemplate testTemplate, ITestContext context, IWriter output, string delegateSymbol, string exceptionType)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertExceptionNotThrown(context, output, delegateSymbol, exceptionType), $"Failed to write 'assert exception not thrown'");
        }
        public static void WriteAssertNoExceptionThrown(this ITestTemplate testTemplate, ITestContext context, IWriter output, string delegateSymbol)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNoExceptionThrown(context, output, delegateSymbol), $"Failed to write 'assert no exception thrown'");
        }

        public static void WriteAssertOfType(this ITestTemplate testTemplate, ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertOfType(context, output, objectSymbol, expectedType), $"Failed to write 'assert of type'");
        }
        public static void WriteAssertNotOfType(this ITestTemplate testTemplate, ITestContext context, IWriter output, string objectSymbol, string expectedType)
        {
            ExecutePrimitiveOrThrow(testTemplate.AssertWriters, assert => assert.TryWriteAssertNotOfType(context, output, objectSymbol, expectedType), $"Failed to write 'assert not of type'");
        }

        public static IEnumerable<string> GahterNamspaces(this ITestTemplate testTemplate)
        {
            return testTemplate.ArrangeWriters.SelectMany(p => p.Namespaces)
           .Concat(testTemplate.ActWriters.SelectMany(p => p.Namespaces))
           .Concat(testTemplate.AssertWriters.SelectMany(p => p.Namespaces));
        }
        #endregion Assert
    }
}
