using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.Symbolic.Variables;
using dnWalker.TestUtils;
using dnWalker.TestWriter.Generators;

using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Moq.Tests
{

    public class MoqArrangeTests : DnlibTestBase<MoqArrangeTests>
    {
        protected class ConcreteTestClass
        {
            public virtual int VirtualMethod() => -5;
        }

        protected abstract class AbstractTestClass
        {
            public abstract int MethodNoArgs();
            public abstract int MethodWithArgs(int a, string other, IDictionary<int, string> dict);
            public abstract int AnotherMethodWithArgs(int a, int b, int c);
            public abstract int GenericMethodWithGenericArgs<T>(T a);

            public virtual int VirtualMethod() => -5;
            public int ConcreteMethod() => 5;
        }

        protected interface ITestInterface
        {
            int MethodNoArgs();
        }

        private ITestContext GetTestContext(TypeSig type)
        { 
            Mock<ITestContext> ctxMock = new Mock<ITestContext>();
            ctxMock.Setup(o => o.SymbolMapping)
                .Returns(new Dictionary<string, SymbolContext>()
                {
                    ["obj"] = new SymbolContext("obj", 0, "obj", type, default)
                });

            return ctxMock.Object;
        }

        public MoqArrangeTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void TestMethodNoArgsSingle()
        {
            const string Expected = 
            """
            obj_mock
                .Setup(o => o.MethodNoArgs())
                .Returns(-123);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.MethodNoArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestMethodNoArgsMultiple()
        {
            const string Expected =
            """
            obj_mock
                .SetupSequence(o => o.MethodNoArgs())
                .Returns(0)
                .Returns(1)
                .Returns(2)
                .Returns(3);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.MethodNoArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestMethodWithArgsSingle()
        {
            const string Expected =
            """
            obj_mock
                .Setup(o => o.MethodWithArgs(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IDictionary<int, string>>()))
                .Returns(-123);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.MethodWithArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestMethodWithArgsMultiple()
        {
            const string Expected =
            """
            obj_mock
                .SetupSequence(o => o.MethodWithArgs(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IDictionary<int, string>>()))
                .Returns(0)
                .Returns(1)
                .Returns(2)
                .Returns(3);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.MethodWithArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestVirtualMethod()
        {
            const string Expected =
            """
            obj_mock
                .Setup(o => o.VirtualMethod())
                .Returns(-123);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.VirtualMethod));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestVirtualMethodMultiple()
        {
            const string Expected =
            """
            obj_mock
                .SetupSequence(o => o.VirtualMethod())
                .Returns(0)
                .Returns(1)
                .Returns(2)
                .Returns(3);
            """;

            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.VirtualMethod));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestGenericMethodWithGenericArgs()
        {
            const string Expected =
            """
            obj_mock
                .Setup(o => o.GenericMethodWithGenericArgs<string>(It.IsAny<string>()))
                .Returns(-123);
            """;

            MethodDef md = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.GenericMethodWithGenericArgs));
            IMethod method = new MethodSpecUser(md, new GenericInstMethodSig(md.Module.CorLibTypes.String));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestGenericMethodWithGenericArgsMultiple()
        {
            const string Expected =
            """
            obj_mock
                .SetupSequence(o => o.GenericMethodWithGenericArgs<string>(It.IsAny<string>()))
                .Returns(0)
                .Returns(1)
                .Returns(2)
                .Returns(3);
            """;

            MethodDef md = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.GenericMethodWithGenericArgs));
            IMethod method = new MethodSpecUser(md, new GenericInstMethodSig(md.Module.CorLibTypes.String));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestConcreteMethod()
        {
            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.ConcreteMethod));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeFalse();
            writer.ToString().Should().BeEmpty();
        }

        [Fact]
        public void TestConcreteMethodMultiple()
        {
            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.ConcreteMethod));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeFalse();
            writer.ToString().Should().BeEmpty();
        }

        [Fact]
        public void TestInterfaceMethod()
        {
            const string Expected =
            """
            obj_mock
                .Setup(o => o.MethodNoArgs())
                .Returns(-123);
            """;

            IMethod method = GetMethod(typeof(ITestInterface), nameof(ITestInterface.MethodNoArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "-123" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestInterfaceMethodMultiple()
        {
            const string Expected =
            """
            obj_mock
                .SetupSequence(o => o.MethodNoArgs())
                .Returns(0)
                .Returns(1)
                .Returns(2)
                .Returns(3);
            """;

            IMethod method = GetMethod(typeof(ITestInterface), nameof(ITestInterface.MethodNoArgs));

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeMethod(GetTestContext(method.DeclaringType.ToTypeSig()), writer, "obj", method, new[] { "0", "1", "2", "3" }).Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestCreateConreteInstanceNoMembers()
        {
            TypeDef type = GetType(typeof(ConcreteTestClass));

            ITestContext testContext = GetTestContext(type.ToTypeSig());

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeCreateInstance(testContext, writer, "obj").Should().BeFalse();
            writer.ToString().Should().BeEmpty();
        }

        [Fact]
        public void TestCreateConreteInstanceWithVirtualMembers()
        {

            const string Expected =
            $"""
            Mock<MoqArrangeTests.ConcreteTestClass> obj_mock = new Mock<MoqArrangeTests.ConcreteTestClass>();
            MoqArrangeTests.ConcreteTestClass obj = obj_mock.Object;
            """;

            TypeDef type = GetType(typeof(ConcreteTestClass));


            IMethod method = GetMethod(typeof(ConcreteTestClass), nameof(ConcreteTestClass.VirtualMethod));

            ITestContext testContext = GetTestContext(type.ToTypeSig());
            testContext.SymbolMapping["obj"].MembersToArrange.Add(method);

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeCreateInstance(testContext, writer, "obj").Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestCreateAbstractInstance()
        {
            const string Expected =
            $"""
            Mock<MoqArrangeTests.AbstractTestClass> obj_mock = new Mock<MoqArrangeTests.AbstractTestClass>();
            MoqArrangeTests.AbstractTestClass obj = obj_mock.Object;
            """;

            TypeDef type = GetType(typeof(AbstractTestClass));
         
            ITestContext testContext = GetTestContext(type.ToTypeSig());
            
            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeCreateInstance(testContext, writer, "obj").Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestCreateInterfaceInstance()
        {
            const string Expected =
            $"""
            Mock<MoqArrangeTests.ITestInterface> obj_mock = new Mock<MoqArrangeTests.ITestInterface>();
            MoqArrangeTests.ITestInterface obj = obj_mock.Object;
            """;

            TypeDef type = GetType(typeof(ITestInterface));
            ITestContext testContext = GetTestContext(type.ToTypeSig());

            MoqArrange moq = new MoqArrange();

            Writer writer = new Writer();
            moq.TryWriteArrangeCreateInstance(testContext, writer, "obj").Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void TestArrangeConstrainedMethod()
        {
            const string Expected =
            """
            obj_mock
                .Setup(o => o.AnotherMethodWithArgs(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns<int, int, int>((a, b, c) =>
                {
                    if (((a < b) && (c == 5)))
                    {
                        return 123;
                    }
                    else
                    {
                        return 0;
                    }
                });
            """;

            TypeDef type = GetType(typeof(AbstractTestClass));
            IMethod method = GetMethod(typeof(AbstractTestClass), nameof(AbstractTestClass.AnotherMethodWithArgs));
            ITestContext testContext = GetTestContext(type.ToTypeSig());

            MoqArrange moq = new MoqArrange();

            ExpressionFactory ef = new CustomModuleExpressionFactory(DefinitionProvider.Context.MainModule);

            TypeSig tInt = DefinitionProvider.BaseTypes.Int32;

            Expression constraint = ef.MakeAnd
                (
                    ef.MakeLessThan
                    (
                        ef.MakeVariable(new NamedVariable(tInt, "a")),
                        ef.MakeVariable(new NamedVariable(tInt, "b"))
                    ),
                    ef.MakeEqual
                    (
                        ef.MakeVariable(new NamedVariable(tInt, "c")),
                        ef.MakeIntegerConstant(5)
                    )
                );


            KeyValuePair<Expression, string>[] constrainedLiterals = new[]
            {
                KeyValuePair.Create(constraint , "123")
            };

            Writer writer = new Writer();
            moq.TryWriteArrangeInitializeConstrainedMethod(testContext, writer, "obj", method, constrainedLiterals, "0").Should().BeTrue();
            writer.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }
    }
}
