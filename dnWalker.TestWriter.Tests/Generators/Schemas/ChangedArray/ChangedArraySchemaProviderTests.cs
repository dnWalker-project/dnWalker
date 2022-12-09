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
namespace dnWalker.TestWriter.Tests.Generators.Schemas.ChangedArray
{
    public class ChangedArraySchemaProviderTests : TestWriterTestBase
    {
        private class TestClass
        {
            private TestClass? _next;
            private int _iVal;

            public void NoReturnValue(int[] iArr, string[] sArr, TestClass[] rArr)
            {
            }
        }

        public ChangedArraySchemaProviderTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoArrayChanged()
        {
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)));

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            provider.GetSchemas(iteration).Should().BeEmpty();
        }

        [Fact]
        public void SingleArrayChanged()
        {
            Location thisLocation = Location.Null;
            Location iArrLocation = Location.Null;
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)),
                it =>
                {
                    Model input = new Model();

                    thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    iArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(int)).ToTypeSig(), 5).Location;

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(iArrLocation, out var iArrNode);
                    ((IArrayHeapNode)iArrNode!).SetElement(3, ValueFactory.GetValue(10));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestSchema[] schemas = provider.GetSchemas(iteration).ToArray();
            schemas.Should().HaveCount(1);

            ((ChangedArraySchema)schemas[0]).ArrayLocation.Should().Be(iArrLocation);
        }

        [Fact]
        public void MultipleArraysChanged()
        {
            Location thisLocation = Location.Null;
            Location iArrLocation = Location.Null;
            Location sArrLocation = Location.Null;
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)),
                it =>
                {
                    Model input = new Model();

                    thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;

                    iArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(int)).ToTypeSig(), 5).Location;
                    sArrLocation = input.HeapInfo.InitializeArray(GetType(typeof(string)).ToTypeSig(), 5).Location;

                    IObjectHeapNode unchangedNode = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig());

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(iArrLocation, out var N);
                    IArrayHeapNode iArrNode = (IArrayHeapNode)N!;

                    output.HeapInfo.TryGetNode(sArrLocation, out N);
                    IArrayHeapNode sArrNode = (IArrayHeapNode)N!;

                    iArrNode.SetElement(3, new PrimitiveValue<int>(10));
                    sArrNode.SetElement(3, new StringValue("hello world!"));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            ITestSchema[] schemas = provider.GetSchemas(iteration).ToArray();
            schemas.Should().HaveCount(2);

            ((ChangedArraySchema)schemas[0]).ArrayLocation.Should().Be(iArrLocation);
            ((ChangedArraySchema)schemas[1]).ArrayLocation.Should().Be(sArrLocation);
        }


        [Fact]
        public void ExceptionThrown()
        {
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)),
                it =>
                {
                    Model input = new Model();

                    Location thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "_next"), output.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location);

                    it.InputModel = input;
                    it.OutputModel = output;
                    it.Exception = GetType(typeof(Exception)).ToTypeSig();
                });

            ChangedArraySchemaProvider provider = new ChangedArraySchemaProvider();
            provider.GetSchemas(iteration).Should().BeEmpty();
        }
    }
}
