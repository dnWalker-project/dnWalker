using dnWalker.Concolic;
using dnWalker.Parameters;
using dnWalker.Traversal;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace dnWalker.Tests.ExampleTests.DebugMode.Features.Arrays
{
    public class ArraySubstituteTests : DebugExamplesTestBase
    {
        public ArraySubstituteTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }


        [Fact]
        public void Test_ArraySubstitute_NullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ArraySubstitute_NotNullEquality()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfNotNull");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(2);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ArraySubstitute_LengthLowerThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthLowerThan5");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ArraySubstitute_LengthGreaterThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ArraySubstitute_ItemAtStaticIndexIsGreaterThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtStaticIndexIsGreaterThan5");

            var paths = explorer.PathStore.Paths;

            paths.Count().Should().Be(4);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_ArraySubstitute_ItemAtDynamicIndexIsGreaterThan5()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5");

            var paths = explorer.PathStore.Paths;

            foreach (Path p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(4);

            Fail("Check the input/output models.");
        }

        [Fact]
        public void Test_StoreElement_FromInput()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElement_InputParameter");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");

            //IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[2];
            //IBaseParameterSet baseSet = execSet.BaseSet;

            //IArrayParameter array = execSet.Parameters[0x00000001] as IArrayParameter;
            //array.Should().NotBeNull();
            //array.GetIsNull().Should().BeFalse();
            //array.GetLength().Should().Be(3);

            //array.TryGetItem(2, out IObjectParameter o2).Should().BeTrue();
            //o2.Reference.Should().Be((ParameterRef)0x00000002);
        }

        [Fact]
        public void Test_StoreElement_Constructed_Object()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElement_ConstructParameter_Object");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");

            //IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[2];
            //IBaseParameterSet baseSet = execSet.BaseSet;

            //IArrayParameter array = execSet.Parameters[0x00000001] as IArrayParameter;
            //array.Should().NotBeNull();
            //array.GetIsNull().Should().BeFalse();
            //array.GetLength().Should().Be(3);

            //array.TryGetItem(2, out IObjectParameter o2).Should().BeTrue();
            //baseSet.Parameters.Should().NotContainKey(o2.Reference);
        }

        [Fact]
        public void Test_StoreElement_Constructed_Primitive()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElement_ConstructParameter_Primitive");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");

            //IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[2];
            //IBaseParameterSet baseSet = execSet.BaseSet;

            //IArrayParameter array = execSet.Parameters[0x00000001] as IArrayParameter;
            //array.Should().NotBeNull();
            //array.GetIsNull().Should().BeFalse();
            //array.GetLength().Should().Be(3);

            //array.TryGetItem(0, out IInt32Parameter i0).Should().BeTrue();
            //array.TryGetItem(1, out IInt32Parameter i1).Should().BeTrue();
            //array.TryGetItem(2, out IInt32Parameter i2).Should().BeTrue();
            
            //baseSet.Parameters.Should().NotContainKey(i0.Reference);
            //baseSet.Parameters.Should().NotContainKey(i1.Reference);
            //baseSet.Parameters.Should().NotContainKey(i2.Reference);
        }

        [Fact]
        public void Test_StoreElement_Constructed_Array()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElement_ConstructParameter_Array");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");

            //IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[2];
            //IBaseParameterSet baseSet = execSet.BaseSet;

            //IArrayParameter array = execSet.Parameters[0x00000001] as IArrayParameter;
            //array.Should().NotBeNull();
            //array.GetIsNull().Should().BeFalse();
            //array.GetLength().Should().Be(3);


            //array.TryGetItem(2, out IArrayParameter a2).Should().BeTrue();

            //baseSet.Parameters.Should().NotContainKey(a2.Reference);
            //a2.GetIsNull().Should().BeFalse();
            //a2.GetLength().Should().Be(5);
        }

        [Fact]
        public void Test_StoreElement_Constructed_Null()
        {
            IExplorer explorer = GetConcolicExplorerBuilder()
                .SetMaxIterations(10)
                .Build();

            explorer.Run("Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.SetElement_ConstructParameter_Null");

            var paths = explorer.PathStore.Paths;

            foreach (var p in paths)
            {
                Output.WriteLine(p.GetPathInfo());
            }

            paths.Count().Should().Be(3);

            Fail("Check the input/output models.");

            //IExecutionParameterSet execSet = explorer.ParameterStore.ExecutionSets[2];
            //IBaseParameterSet baseSet = execSet.BaseSet;

            //IArrayParameter array = execSet.Parameters[0x00000001] as IArrayParameter;
            //array.Should().NotBeNull();
            //array.GetIsNull().Should().BeFalse();
            //array.GetLength().Should().Be(3);


            //array.TryGetItem(1, out IObjectParameter o1).Should().BeTrue();

            //baseSet.Parameters.Should().NotContainKey(o1.Reference);
            //o1.GetIsNull().Should().BeTrue();
        }
    }
}
