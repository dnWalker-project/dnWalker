using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class FieldParameterAccessorTests
    {
        [Fact]
        public void Test_SettingAccessor()
        {
            const string ObjectTypeName = "MyClass";
            const string FieldName = "myField";

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p = new Int32Parameter();

            op1.SetField(FieldName, p);
            p.Accessor.Should().BeOfType<FieldParameterAccessor>();
        }

        [Fact]
        public void Test_GetAccessString()
        {
            const string ObjectTypeName = "MyClass";
            const string FieldName = "myField";
            const string ObjectAccessor = "MyObject";

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            op1.Accessor = new RootParameterAccessor(ObjectAccessor);

            Int32Parameter p = new Int32Parameter();

            op1.SetField(FieldName, p);
            p.GetAccessString().Should().Be(ObjectAccessor + "." + FieldName);
        }

        [Fact]
        public void Test_Parent()
        {
            const string ObjectTypeName = "MyClass";
            const string FieldName = "myField";

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p = new Int32Parameter();

            op1.SetField(FieldName, p);
            p.Accessor?.Parent.Should().Be(op1);
        }

        [Fact]
        public void Test_ChangeTarget()
        {
            const string ObjectTypeName = "MyClass";
            const string FieldName = "myField";

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p1 = new Int32Parameter();
            Int32Parameter p2 = new Int32Parameter();

            op1.SetField(FieldName, p1);

            p1.Accessor?.ChangeTarget(p2);

            op1.TryGetField(FieldName, out IParameter? fp2).Should().BeTrue();
            fp2.Should().Be(p2);
            p2.Accessor?.Parent.Should().Be(op1);
        }
    }
    public class ItemParameterAccessorTests
    {
        [Fact]
        public void Test_SettingAccessor()
        {
            const string ObjectTypeName = "MyClass";
            const int Index = 2;

            ArrayParameter ap1 = new ArrayParameter(ObjectTypeName) { Length = Index + 1 };
            Int32Parameter p = new Int32Parameter();

            ap1.SetItem(Index, p);
            p.Accessor.Should().BeOfType<ItemParameterAccessor>();
        }

        [Fact]
        public void Test_GetAccessString()
        {
            const string ObjectTypeName = "MyClass";
            const int Index = 2;
            const string ObjectAccessor = "MyObject";

            ArrayParameter ap1 = new ArrayParameter(ObjectTypeName) { Length = Index + 1 };
            ap1.Accessor = new RootParameterAccessor(ObjectAccessor);

            Int32Parameter p = new Int32Parameter();

            ap1.SetItem(Index, p);
            p.GetAccessString().Should().Be(ObjectAccessor + "[" + Index + "]");
        }

        [Fact]
        public void Test_Parent()
        {
            const string ObjectTypeName = "MyClass";
            const int Index = 2;

            ArrayParameter ap1 = new ArrayParameter(ObjectTypeName) { Length = Index + 1 };
            Int32Parameter p = new Int32Parameter();

            ap1.SetItem(Index, p);
            p.Accessor?.Parent.Should().Be(ap1);
        }

        [Fact]
        public void Test_ChangeTarget()
        {
            const string ObjectTypeName = "MyClass";
            const int Index = 2;

            ArrayParameter ap1 = new ArrayParameter(ObjectTypeName) { Length = Index + 1 };
            Int32Parameter p1 = new Int32Parameter();
            Int32Parameter p2 = new Int32Parameter();

            ap1.SetItem(Index, p1);

            p1.Accessor?.ChangeTarget(p2);

            ap1.TryGetItem(Index, out IParameter? fp2).Should().BeTrue();
            fp2.Should().Be(p2);
            p2.Accessor?.Parent.Should().Be(ap1);
        }
    }

    public class MethodResultParameterAccessorTests
    {
        [Fact]
        public void Test_SettingAccessor()
        {
            const string ObjectTypeName = "MyNamespace.MyClass";
            const string MethodSignature = "System.Void MyNamespace.MyClass::Execute(System.String)";
            const int Invocation = 2;

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p = new Int32Parameter();

            op1.SetMethodResult(MethodSignature, Invocation, p);
            p.Accessor.Should().BeOfType<MethodResultParameterAccessor>();
        }

        [Fact]
        public void Test_GetAccessString()
        {
            const string ObjectTypeName = "MyNamespace.MyClass";
            const string MethodSignature = "System.Void MyNamespace.MyClass::Execute(System.String)";
            const int Invocation = 2;
            const string ObjectAccessor = "MyObject";

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            op1.Accessor = new RootParameterAccessor(ObjectAccessor);

            Int32Parameter p = new Int32Parameter();

            op1.SetMethodResult(MethodSignature, Invocation, p);
            p.GetAccessString().Should().Be(ObjectAccessor + ".Execute(System.String)[" + Invocation + "]");
        }

        [Fact]
        public void Test_Parent()
        {
            const string ObjectTypeName = "MyNamespace.MyClass";
            const string MethodSignature = "System.Void MyNamespace.MyClass::Execute(System.String)";
            const int Invocation = 2;

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p = new Int32Parameter();

            op1.SetMethodResult(MethodSignature, Invocation, p);
            p.Accessor?.Parent.Should().Be(op1);
        }

        [Fact]
        public void Test_ChangeTarget()
        {
            const string ObjectTypeName = "MyNamespace.MyClass";
            const string MethodSignature = "System.Void MyNamespace.MyClass::Execute(System.String)";
            const int Invocation = 2;

            ObjectParameter op1 = new ObjectParameter(ObjectTypeName);
            Int32Parameter p1 = new Int32Parameter();
            Int32Parameter p2 = new Int32Parameter();

            op1.SetMethodResult(MethodSignature, Invocation, p1);

            p1.Accessor?.ChangeTarget(p2);

            op1.TryGetMethodResult(MethodSignature, Invocation, out IParameter? fp2).Should().BeTrue();
            fp2.Should().Be(p2);
            p2.Accessor?.Parent.Should().Be(op1);
        }
    }
}
