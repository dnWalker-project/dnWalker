using dnlib.DotNet;
using dnWalker.Explorations;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;
using dnWalker.TestWriter.Generators.Schemas.ChangedObject;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Schemas;
using FluentAssertions;
using dnWalker.Symbolic.Variables;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.ChangedObject
{
    public class ChangedObjectSchemaTests : TestWriterTestBase
    {
        private class TestClass
        {
            public TestClass? Next;
            public int IVal;
            public string SLabel;

            public void NoReturnValue(TestClass other)
            {
            }
        }

        public ChangedObjectSchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }


        // primitive value change
        [Fact]
        public void PrimitiveValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedObjectSchemaTests.TestClass changedObjectSchemaTests_TestClass1 = new ChangedObjectSchemaTests.TestClass();

            // Arrange method arguments
            ChangedObjectSchemaTests.TestClass @this = changedObjectSchemaTests_TestClass1;
            ChangedObjectSchemaTests.TestClass other = null;

            @this.NoReturnValue(other);
            Assert.Equal(5, changedObjectSchemaTests_TestClass1.IVal);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "IVal"), ValueFactory.GetValue(5));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }


        // string value changed
        [Fact]
        public void StringValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedObjectSchemaTests.TestClass changedObjectSchemaTests_TestClass1 = new ChangedObjectSchemaTests.TestClass();

            // Arrange method arguments
            ChangedObjectSchemaTests.TestClass @this = changedObjectSchemaTests_TestClass1;
            ChangedObjectSchemaTests.TestClass other = null;

            @this.NoReturnValue(other);
            Assert.Equal("hello world!", changedObjectSchemaTests_TestClass1.SLabel);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "SLabel"), new StringValue("hello world!"));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }


        // ref value changed
        [Fact]
        public void RefValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedObjectSchemaTests.TestClass changedObjectSchemaTests_TestClass1 = new ChangedObjectSchemaTests.TestClass();

            // Arrange method arguments
            ChangedObjectSchemaTests.TestClass @this = changedObjectSchemaTests_TestClass1;
            ChangedObjectSchemaTests.TestClass other = changedObjectSchemaTests_TestClass1;

            @this.NoReturnValue(other);
            Assert.NotNull(changedObjectSchemaTests_TestClass1.Next);
            Assert.Same(changedObjectSchemaTests_TestClass1, changedObjectSchemaTests_TestClass1.Next);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[1]), thisLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "Next"), thisLocation);

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void ComplexChange()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedObjectSchemaTests.TestClass changedObjectSchemaTests_TestClass1 = new ChangedObjectSchemaTests.TestClass();

            // Arrange method arguments
            ChangedObjectSchemaTests.TestClass @this = changedObjectSchemaTests_TestClass1;
            ChangedObjectSchemaTests.TestClass other = changedObjectSchemaTests_TestClass1;

            @this.NoReturnValue(other);
            Assert.NotNull(changedObjectSchemaTests_TestClass1.Next);
            Assert.Same(changedObjectSchemaTests_TestClass1, changedObjectSchemaTests_TestClass1.Next);
            Assert.Equal(5, changedObjectSchemaTests_TestClass1.IVal);
            Assert.Equal("hello world!", changedObjectSchemaTests_TestClass1.SLabel);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[1]), thisLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "Next"), thisLocation);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "IVal"), ValueFactory.GetValue(5));
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "SLabel"), new StringValue("hello world!"));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }
    }
}
