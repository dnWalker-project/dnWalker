using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Parameters.Tests
{
    public class TransferTraitsExtensionsTests
    {
        [Fact]
        public void Test_Fields_NoConclict()
        {
            string[] fieldNames = new string[] { "Field1", "Field2", "Field3" };
            IParameter[] fieldValues = new IParameter[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };

            // target does not have defined the copied fields
            IFieldOwnerParameter src = new ObjectParameter("MyObject");
            IFieldOwnerParameter trg = new ObjectParameter("MyObject");

            for (int i = 0; i < fieldNames.Length; ++i)
            {
                src.SetField(fieldNames[i], fieldValues[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < fieldNames.Length; ++i)
            {
                src.TryGetField(fieldNames[i], out _).Should().BeFalse();
                trg.TryGetField(fieldNames[i], out IParameter? fv).Should().BeTrue();
                fv.Should().BeSameAs(fieldValues[i]);
            }
        }

        [Fact]
        public void Test_Fields_WithConclict()
        {
            string[] fieldNames = new string[] { "Field1", "Field2", "Field3" };
            IParameter?[] fieldValues1 = new IParameter?[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };
            IParameter?[] fieldValues2 = new IParameter?[] { new Int32Parameter(), null, new BooleanParameter() };

            // target does not have defined the copied fields
            IFieldOwnerParameter src = new ObjectParameter("MyObject");
            IFieldOwnerParameter trg = new ObjectParameter("MyObject");

            for (int i = 0; i < fieldNames.Length; ++i)
            {
                src.SetField(fieldNames[i], fieldValues1[i]);
                trg.SetField(fieldNames[i], fieldValues2[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < fieldNames.Length; ++i)
            {
                src.TryGetField(fieldNames[i], out _).Should().BeFalse();
                trg.TryGetField(fieldNames[i], out IParameter? fv).Should().BeTrue();
                if (fieldValues2[i] == null)
                {
                    // no conflit
                    fv.Should().BeSameAs(fieldValues1[i]);
                }
                else
                {
                    // with conflit => it is not overwritten
                    fv.Should().BeSameAs(fieldValues2[i]);
                }
            }
        }

        [Fact]
        public void Test_Items_NoConclict()
        {
            IParameter[] itemValues = new IParameter[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };

            // target does not have defined the copied Items
            IItemOwnerParameter src = new ArrayParameter("MyObject") {  Length = 3 };
            IItemOwnerParameter trg = new ArrayParameter("MyObject") { Length = 3 };

            for (int i = 0; i < itemValues.Length; ++i)
            {
                src.SetItem(i, itemValues[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < itemValues.Length; ++i)
            {
                src.TryGetItem(i, out _).Should().BeFalse();
                trg.TryGetItem(i, out IParameter? fv).Should().BeTrue();
                fv.Should().BeSameAs(itemValues[i]);
            }
        }

        [Fact]
        public void Test_Items_WithConclict()
        {
            IParameter?[] itemValues1 = new IParameter?[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };
            IParameter?[] itemValues2 = new IParameter?[] { new Int32Parameter(), null, new BooleanParameter() };

            // target does not have defined the copied Items
            IItemOwnerParameter src = new ArrayParameter("MyObject") { Length = 3 };
            IItemOwnerParameter trg = new ArrayParameter("MyObject") { Length = 3 };

            for (int i = 0; i < itemValues1.Length; ++i)
            {
                src.SetItem(i, itemValues1[i]);
                trg.SetItem(i, itemValues2[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < itemValues1.Length; ++i)
            {
                src.TryGetItem(i, out _).Should().BeFalse();
                trg.TryGetItem(i, out IParameter? fv).Should().BeTrue();
                if (itemValues2[i] == null)
                {
                    // no conflit
                    fv.Should().BeSameAs(itemValues1[i]);
                }
                else
                {
                    // with conflit => it is not overwritten
                    fv.Should().BeSameAs(itemValues2[i]);
                }
            }
        }

        [Fact]
        public void Test_MethodResults_NoConflict()
        {
            MethodSignature[] signatures = new MethodSignature[] { "System.Void MyObject::MyMethod1()", "System.Void MyObject::MyMethod2()", "System.Void MyObject::MyMethod2()" };
            int[] invocations = new int[] { 0, 1, 2 };
            IParameter[] methodResultValues = new IParameter[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };

            // target does not have defined the copied MethodResults
            IMethodResolverParameter src = new InterfaceParameter("MyObject");
            IMethodResolverParameter trg = new InterfaceParameter("MyObject");

            for (int i = 0; i < methodResultValues.Length; ++i)
            {
                src.SetMethodResult(signatures[i], invocations[i], methodResultValues[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < methodResultValues.Length; ++i)
            {
                src.TryGetMethodResult(signatures[i], invocations[i], out _).Should().BeFalse();
                trg.TryGetMethodResult(signatures[i], invocations[i], out IParameter? fv).Should().BeTrue();
                fv.Should().BeSameAs(methodResultValues[i]);
            }
        }

        [Fact]
        public void Test_MethodResults_WithConclict()
        {
            MethodSignature[] signatures = new MethodSignature[] { "System.Void MyObject::MyMethod1()", "System.Void MyObject::MyMethod2()", "System.Void MyObject::MyMethod2()" };
            int[] invocations = new int[] { 0, 1, 2 };

            IParameter?[] methodResultValues1 = new IParameter?[] { new Int32Parameter(), new DoubleParameter(), new BooleanParameter() };
            IParameter?[] methodResultValues2 = new IParameter?[] { new Int32Parameter(), null, new BooleanParameter() };

            // target does not have defined the copied MethodResults
            IMethodResolverParameter src = new InterfaceParameter("MyObject");
            IMethodResolverParameter trg = new InterfaceParameter("MyObject");

            for (int i = 0; i < methodResultValues1.Length; ++i)
            {
                src.SetMethodResult(signatures[i], invocations[i], methodResultValues1[i]);
                trg.SetMethodResult(signatures[i], invocations[i], methodResultValues2[i]);
            }

            src.TransferTraitsInto(trg);

            for (int i = 0; i < methodResultValues1.Length; ++i)
            {
                src.TryGetMethodResult(signatures[i], invocations[i], out _).Should().BeFalse();
                trg.TryGetMethodResult(signatures[i], invocations[i], out IParameter? fv).Should().BeTrue();
                if (methodResultValues2[i] == null)
                {
                    // no conflit
                    fv.Should().BeSameAs(methodResultValues1[i]);
                }
                else
                {
                    // with conflit => it is not overwritten
                    fv.Should().BeSameAs(methodResultValues2[i]);
                }
            }
        }
    }
}
