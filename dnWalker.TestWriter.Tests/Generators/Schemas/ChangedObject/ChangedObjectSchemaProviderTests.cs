using dnlib.DotNet;

using dnWalker.Explorations;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Generators.Schemas;
using dnWalker.TestWriter.Generators.Schemas.ChangedObject;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Schemas.ChangedObject
{
    public class ChangedObjectSchemaProviderTests : TestWriterTestBase
    {
        private class TestClass
        {
            private TestClass? _next;
            private int _iVal;

            public void NoReturnValue()
            {
            }
        }

        public ChangedObjectSchemaProviderTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void NoObjectChanged()
        {
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)));

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            provider.GetSchemas(iteration).Should().BeEmpty();
        }

        [Fact]
        public void SingleObjectChanged()
        {
            Location thisLocation = Location.Null;
            Location nextLocation = Location.Null;
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)),
                it =>
                {
                    Model input = new Model();

                    thisLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;
                    nextLocation = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location;

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisNode);
                    ((IObjectHeapNode)thisNode!).SetField(GetField(typeof(TestClass), "_next"), nextLocation);

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestSchema[] schemas = provider.GetSchemas(iteration).ToArray();
            schemas.Should().HaveCount(1);

            ((ChangedObjectSchema)schemas[0]).ObjectLocation.Should().Be(thisLocation);
        }

        [Fact]
        public void MultipleObjectsChanged()
        {
            Location thisLocation = Location.Null;
            Location nextLocation = Location.Null;
            ConcolicExplorationIteration iteration = GetIteration(GetMethod(typeof(TestClass), nameof(TestClass.NoReturnValue)),
                it =>
                {
                    Model input = new Model();

                    IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig());
                    thisLocation = thisNode.Location;

                    IObjectHeapNode nextNode = input.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig());
                    nextLocation = nextNode.Location;

                    Model output = input.Clone();

                    output.HeapInfo.TryGetNode(thisLocation, out var thisN);
                    thisNode = (IObjectHeapNode)thisN!;

                    output.HeapInfo.TryGetNode(nextLocation, out var nextN);
                    nextNode = (IObjectHeapNode)nextN!;

                    thisNode.SetField(GetField(typeof(TestClass), "_next"), output.HeapInfo.InitializeObject(GetType(typeof(TestClass)).ToTypeSig()).Location);
                    thisNode.SetField(GetField(typeof(TestClass), "_iVal"), ValueFactory.GetValue(5));
                    nextNode.SetField(GetField(typeof(TestClass), "_iVal"), ValueFactory.GetValue(15));

                    it.InputModel = input;
                    it.OutputModel = output;
                });

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            ITestSchema[] schemas = provider.GetSchemas(iteration).ToArray();
            schemas.Should().HaveCount(2);

            ((ChangedObjectSchema)schemas[0]).ObjectLocation.Should().Be(thisLocation);
            ((ChangedObjectSchema)schemas[1]).ObjectLocation.Should().Be(nextLocation);
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

            ChangedObjectSchemaProvider provider = new ChangedObjectSchemaProvider();
            provider.GetSchemas(iteration).Should().BeEmpty();
        }
    }
}
