using dnlib.DotNet;
using dnWalker.Explorations;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic;
using dnWalker.TestWriter.Generators.Schemas;
using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;
using dnWalker.TestWriter.Generators.Schemas.ChangedArray;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.Generators;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.ChangedArray
{
    public class ChangedArraySchemaTests : TestWriterTestBase
    {
        private class TestClass
        {
            private TestClass? _next;
            private int _iVal;

            public void NoReturnValue(int[] iArr, string[] sArr, TestClass[] rArr)
            {
            }
        }

        public ChangedArraySchemaTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        // primitive value change
        [Fact]
        public void PrimitiveValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            int[] intArr1 = new int[10];
            ChangedArraySchemaTests.TestClass changedArraySchemaTests_TestClass1 = new ChangedArraySchemaTests.TestClass();
            
            // Arrange method arguments
            ChangedArraySchemaTests.TestClass @this = changedArraySchemaTests_TestClass1;
            int[] iArr = intArr1;
            string[] sArr = null;
            ChangedArraySchemaTests.TestClass[] rArr = null;

            @this.NoReturnValue(iArr, sArr, rArr);
            Assert.Equal(5, intArr1[3]);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Location iArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(int)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[1]), iArrLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(iArrLocation, out var iArrNode);
                    ((IArrayHeapNode)iArrNode!).SetElement(3, ValueFactory.GetValue(5));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected);
        }


        // string value changed
        [Fact]
        public void StringValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            string[] stringArr1 = new string[10];
            ChangedArraySchemaTests.TestClass changedArraySchemaTests_TestClass1 = new ChangedArraySchemaTests.TestClass();
            
            // Arrange method arguments
            ChangedArraySchemaTests.TestClass @this = changedArraySchemaTests_TestClass1;
            int[] iArr = null;
            string[] sArr = stringArr1;
            ChangedArraySchemaTests.TestClass[] rArr = null;

            @this.NoReturnValue(iArr, sArr, rArr);
            Assert.Equal("hello world!", stringArr1[3]);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Location sArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(string)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[2]), sArrLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(sArrLocation, out var iArrNode);
                    ((IArrayHeapNode)iArrNode!).SetElement(3, new StringValue("hello world!"));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected);
        }


        // ref value changed
        [Fact]
        public void RefValueChanged()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedArraySchemaTests.TestClass[] changedArraySchemaTests_TestClassArr1 = new ChangedArraySchemaTests.TestClass[10];
            ChangedArraySchemaTests.TestClass changedArraySchemaTests_TestClass1 = new ChangedArraySchemaTests.TestClass();
            
            // Arrange method arguments
            ChangedArraySchemaTests.TestClass @this = changedArraySchemaTests_TestClass1;
            int[] iArr = null;
            string[] sArr = null;
            ChangedArraySchemaTests.TestClass[] rArr = changedArraySchemaTests_TestClassArr1;

            @this.NoReturnValue(iArr, sArr, rArr);
            Assert.NotNull(changedArraySchemaTests_TestClassArr1[3]);
            Assert.Same(changedArraySchemaTests_TestClass1, changedArraySchemaTests_TestClassArr1[3]);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Location rArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(TestClass)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[3]), rArrLocation);

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(rArrLocation, out var iArrNode);
                    ((IArrayHeapNode)iArrNode!).SetElement(3, thisLocation);

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void ComplexChange()
        {
            const string Expected =
            """
            // Arrange input model heap
            ChangedArraySchemaTests.TestClass[] changedArraySchemaTests_TestClassArr1 = new ChangedArraySchemaTests.TestClass[10];
            string[] stringArr1 = new string[10];
            int[] intArr1 = new int[10];
            ChangedArraySchemaTests.TestClass changedArraySchemaTests_TestClass1 = new ChangedArraySchemaTests.TestClass();
            
            // Arrange method arguments
            ChangedArraySchemaTests.TestClass @this = changedArraySchemaTests_TestClass1;
            int[] iArr = intArr1;
            string[] sArr = stringArr1;
            ChangedArraySchemaTests.TestClass[] rArr = changedArraySchemaTests_TestClassArr1;

            @this.NoReturnValue(iArr, sArr, rArr);
            Assert.Equal("hello world!3", stringArr1[3]);
            Assert.Equal("hello world!4", stringArr1[4]);
            Assert.Equal("hello world!5", stringArr1[5]);
            """;

            MethodDef theMethod = GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue));
            ConcolicExplorationIteration iteration = GetIteration(theMethod,
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[0]), thisLocation);

                    Location iArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(int)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[1]), iArrLocation);

                    Location sArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(string)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[2]), sArrLocation);

                    Location rArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(TestClass)).ToTypeSig(), 10).Location;
                    input.SetValue(new MethodArgumentVariable(theMethod.Parameters[3]), rArrLocation);

                    Model output = input.Clone();

                    //output.HeapInfo.TryGetNode(iArrLocation, out var iArrNode);
                    //((IArrayHeapNode)iArrNode!).SetElement(8, ValueFactory.GetValue(8));
                    //((IArrayHeapNode)iArrNode!).SetElement(9, ValueFactory.GetValue(9));

                    output.HeapInfo.TryGetNode(sArrLocation, out var sArrNode);
                    ((IArrayHeapNode)sArrNode!).SetElement(3, new StringValue("hello world!3"));
                    ((IArrayHeapNode)sArrNode!).SetElement(4, new StringValue("hello world!4"));
                    ((IArrayHeapNode)sArrNode!).SetElement(5, new StringValue("hello world!5"));

                    //output.HeapInfo.TryGetNode(rArrLocation, out var rArrNode);
                    //((IArrayHeapNode)rArrNode!).SetElement(2, thisLocation);
                    //((IArrayHeapNode)rArrNode!).SetElement(3, thisLocation);

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestTemplate testTemplate = GetTestTemplate();
            ITestSchema schema = provider.GetSchemas(iteration).Single();

            Writer output = new Writer();
            schema.Write(testTemplate, output);
            output.ToString().Trim().Should().Be(Expected);
        }
    }
}
