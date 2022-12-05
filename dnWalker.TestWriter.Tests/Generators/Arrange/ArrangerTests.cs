using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Tests.Generators.Schemas;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.Generators.Arrange
{

    public class ArrangerTests : TestSchemaTestBase
    {
        private class TestClass
        {
            public TestClass ReturnSelf(int x)
            {
                return this;
            }

            public TestClass? Next;

            public static string? StaticField;

            private static string? PrivateStaticField;

            public int ValueField;
            public readonly int ConstValueField;

            private int _valueField;
            private readonly int _constValueField;

            public int[]? ArrayField;

            public int MethodWithManyArgs(TestClass? tc1, TestClass? tc2, string? str)
            {
                return ValueField + (ArrayField?.Length ?? 0) + (tc1?.ValueField ?? 0) + (tc2?.ValueField ?? 0) - (str?.Length ?? 0);
            }

            public virtual int VirtualMethod()
            {
                return 0;
            }
        }
        public ArrangerTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void TestArrangeMethodArgs()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange method arguments
            ArrangerTests.TestClass @this = arrangerTests_TestClass1;
            ArrangerTests.TestClass tc1 = arrangerTests_TestClass1;
            ArrangerTests.TestClass tc2 = null;
            string str = "hello world";
            """;

            IModel input = NewModel();
            IHeapInfo heap = input.HeapInfo;

            IObjectHeapNode thisNode = heap.InitializeObject(td.ToTypeSig());
            

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);
            input.SetValue(new MethodArgumentVariable(md.Parameters[1]), thisNode.Location);
            //input.SetValue(new MethodArgumentVariable(md.Parameters[2]), Location.Null);
            input.SetValue(new MethodArgumentVariable(md.Parameters[3]), new StringValue("hello world"));

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeMethodArguments();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePublicStaticFields()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange static fields
            ArrangerTests.TestClass.StaticField = "a string";
            """;

            IModel input = NewModel();
            input.SetValue(new StaticFieldVariable(td.FindField("StaticField")), new StringValue("a string"));

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeStaticFields();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePrivateStaticFields()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange static fields
            typeof(ArrangerTests.TestClass).GetField("PrivateStaticField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, "a string");
            """;

            IModel input = NewModel();
            input.SetValue(new StaticFieldVariable(td.FindField("PrivateStaticField")), new StringValue("a string"));

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeStaticFields();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePrivateField()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            typeof(ArrangerTests.TestClass).GetField("_valueField", System.Reflection.BindingFlags.NonPublic).SetValue(arrangerTests_TestClass1, 5);
            """;

            IModel input = NewModel();
            IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            thisNode.SetField(td.FindField("_valueField"), new PrimitiveValue<int>(5));

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePrivateReadonlyField()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            typeof(ArrangerTests.TestClass).GetField("_constValueField", System.Reflection.BindingFlags.NonPublic).SetValue(arrangerTests_TestClass1, 5);
            """;

            IModel input = NewModel();
            IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            thisNode.SetField(td.FindField("_constValueField"), new PrimitiveValue<int>(5));

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePublicField()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            arrangerTests_TestClass1.ValueField = 5;
            """;

            IModel input = NewModel();
            IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            thisNode.SetField(td.FindField("ValueField"), new PrimitiveValue<int>(5));

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangePublicReadonlyField()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            typeof(ArrangerTests.TestClass).GetField("ConstValueField", System.Reflection.BindingFlags.Public).SetValue(arrangerTests_TestClass1, 5);
            """;

            IModel input = NewModel();
            IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            thisNode.SetField(td.FindField("ConstValueField"), new PrimitiveValue<int>(5));

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangeNonCylicHeap()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            // some arguments will be dependent on each other

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            ArrangerTests.TestClass arrangerTests_TestClass2 = new ArrangerTests.TestClass();
            arrangerTests_TestClass2.Next = arrangerTests_TestClass1;
            """;


            IModel input = NewModel();
            IObjectHeapNode nextNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            IObjectHeapNode thisNode = input.HeapInfo.InitializeObject(td.ToTypeSig());
            thisNode.SetField(td.FindField("Next"), nextNode.Location);

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), thisNode.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }

        [Fact]
        public void TestArrangeCylicHeap()
        {
            TypeDef td = GetType(typeof(TestClass));
            MethodDef md = GetMethod(typeof(TestClass), nameof(TestClass.MethodWithManyArgs));

            // some arguments will be dependent on each other

            const string Expected =
            """
            // Arrange input model heap
            ArrangerTests.TestClass arrangerTests_TestClass1 = new ArrangerTests.TestClass();
            ArrangerTests.TestClass arrangerTests_TestClass2 = new ArrangerTests.TestClass();
            ArrangerTests.TestClass arrangerTests_TestClass3 = new ArrangerTests.TestClass();
            arrangerTests_TestClass1.Next = arrangerTests_TestClass2;
            arrangerTests_TestClass2.Next = arrangerTests_TestClass3;
            arrangerTests_TestClass3.Next = arrangerTests_TestClass1;
            """;


            IModel input = NewModel();
            IObjectHeapNode node1 = input.HeapInfo.InitializeObject(td.ToTypeSig());
            IObjectHeapNode node2 = input.HeapInfo.InitializeObject(td.ToTypeSig());
            IObjectHeapNode node3 = input.HeapInfo.InitializeObject(td.ToTypeSig());
            node1.SetField(td.FindField("Next"), node2.Location);
            node2.SetField(td.FindField("Next"), node3.Location);
            node3.SetField(td.FindField("Next"), node1.Location);

            input.SetValue(new MethodArgumentVariable(md.Parameters[0]), node1.Location);

            ITestContext testContext = GetTestContext(md, b => { b.InputModel = input; b.OutputModel = input.Clone(); });

            Writer writer = new Writer();

            Arranger arranger = new Arranger(GetTestTemplate(), testContext, writer);

            arranger.WriteArrangeHeap();

            writer.ToString().Trim().Should().Be(Expected);
        }
    }
}
