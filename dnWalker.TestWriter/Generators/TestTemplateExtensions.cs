﻿using dnWalker.TestWriter.Generators.Arrange;

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

        public static void WriteArrange(this ITestTemplate testTemplate, ITestContext testContext, IWriter output)
        {
            Arranger arranger = new Arranger(testTemplate.ArrangeWriters, testContext, output);
            arranger.Run();
        }

        public static void WriteAct(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string? returnSymbol = null)
        {
            ExecutePrimitiveOrThrow(testTemplate.ActWriters, act => act.TryWriteAct(testContext, output, returnSymbol), $"Failed to write 'act'");
        }

        public static void WriteActDelegate(this ITestTemplate testTemplate, ITestContext testContext, IWriter output, string? returnSymbol = null, string delegateSymbol = "act")
        {
            ExecutePrimitiveOrThrow(testTemplate.ActWriters, act => act.TryWriteActDelegate(testContext, output, returnSymbol, delegateSymbol), $"Failed to write 'act delegate'");
        }


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
    }
}