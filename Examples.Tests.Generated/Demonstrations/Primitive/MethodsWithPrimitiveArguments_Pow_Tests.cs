using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Primitive
{

    [Trait("dnWalkerGenerated", "MethodsWithPrimitiveArguments::Pow")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class MethodsWithPrimitiveArguments_Pow_Tests
    {

        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void PowReturnValueSchema_1()
        {
            // Arrange method arguments
            int a = 0;
            int n = 0;

            int result = MethodsWithPrimitiveArguments.Pow(a, n);
            Assert.Equal(1, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void PowReturnValueSchema_2()
        {
            // Arrange method arguments
            int a = 0;
            int n = 1;

            int result = MethodsWithPrimitiveArguments.Pow(a, n);
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void PowReturnValueSchema_3()
        {
            // Arrange method arguments
            int a = 0;
            int n = 2;

            int result = MethodsWithPrimitiveArguments.Pow(a, n);
            Assert.Equal(0, result);

        }
    }
}

