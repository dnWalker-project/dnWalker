using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates.Moq;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace dnWalker.TestGenerator.Tests.Templates.Moq
{
    public class MoqTemplateTests
    {
        private abstract class TestClass
        {
            public object PublicRefField;
            private object PrivateRefField;

            public int PublicPrimitiveField;
            private int PrivatePrimitiveField;

            public string PublicStringField;
            private string PrivateStringField;

            public abstract object RefMethodNoArgs();
            public abstract object RefMethodArgs(string name, TestClass other, int i);

            public abstract int PrimitiveMethodNoArgs();
            public abstract int PrimitiveMethodArgs(string name, TestClass other, int i);

            public static int MagicNumber;
            private static int PrivateMagicNumber;

            public static int TestedMethod(string strArg, TestClass refArg, int primitiveArg) => 0;
        }

        private MoqTemplate Template { get; } = new MoqTemplate();
        private TypeDef TestClassTD = TestUtils.DefinitionProvider.GetTypeDefinition("dnWalker.TestGenerator.Tests.Templates.Moq.MoqTemplateTests/TestClass");

        private IField GetField(string fieldName)
        {
            return TestClassTD.GetField(fieldName);
        }

        private IMethod GetMethod(string methodName)
        {
            return TestClassTD.FindMethods(methodName).First();
        }

        private IMethod TestedMethod => GetMethod("TestedMethod");


        [Fact]
        public void ArrangeEmptyModel()
        {
            Model model = new Model();
            DummyWriter output = new DummyWriter();
            IDictionary<Location, string> locationNames = Template.WriteArrange(output, model, TestedMethod);

            output.ToString().Trim().Should().Be(
                "string strArg = null;" + Environment.NewLine +
                "TestClass refArg = null;" + Environment.NewLine +
                "int primitiveArg = 0;");
            locationNames.Should().BeEmpty();
        }

        [Fact]
        public void ArrangeRefArray()
        {

        }

        [Fact]
        public void ArrangeRefArraySameElement()
        {

        }

        [Fact]
        public void ArrangePrimitiveArray()
        {

        }

        [Fact]
        public void ArrangeRefInstanceField()
        {

        }

        [Fact]
        public void ArrangePrimitiveInstanceField()
        {

        }

        [Fact]
        public void ArrangePrimitiveMethodResult()
        {

        }

        [Fact]
        public void ArrangeSkippedPrimitiveMethodResult()
        {

        }

        [Fact]
        public void ArrangeMultiplePrimitiveMethodResults()
        {

        }

        [Fact]
        public void ArrangeRefMethodResult()
        {

        }

        [Fact]
        public void ArrangeSkippedRefMethodResult()
        {

        }

        [Fact]
        public void ArrangeMultipleRefMethodResults()
        {

        }

        [Fact]
        public void ArrangePrimitiveMethodResultWithArgs()
        {

        }

        [Fact]
        public void ArrangeSkippedPrimitiveMethodResultWithArgs()
        {

        }

        [Fact]
        public void ArrangeMultiplePrimitiveMethodResultsWithArgs()
        {

        }

        [Fact]
        public void ArrangeRefMethodResultWithArgs()
        {

        }

        [Fact]
        public void ArrangeSkippedRefMethodResultWithArgs()
        {

        }

        [Fact]
        public void ArrangeMultipleRefMethodResultsWithArgs()
        {

        }

        [Fact]
        public void ArrangeRefStaticField()
        {

        }

        [Fact]
        public void ArrangePrimitiveStaticField()
        {

        }
    }
}

