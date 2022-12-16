using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::MixedNumbers")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_MixedNumbers_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void MixedNumbersReturnValueSchema_1()
        {
            // Arrange method arguments
            double x = 0;
            int n = 0;
            
            double result = PrimitiveArguments.MixedNumbers(x, n);
            Assert.Equal(double.NaN, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void MixedNumbersReturnValueSchema_2()
        {
            // Arrange method arguments
            double x = -2147483648;
            int n = -2147483648;
            
            double result = PrimitiveArguments.MixedNumbers(x, n);
            Assert.Equal(-4294967296, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void MixedNumbersReturnValueSchema_3()
        {
            // Arrange method arguments
            double x = 0.5;
            int n = 0;
            
            double result = PrimitiveArguments.MixedNumbers(x, n);
            Assert.Equal(0.5, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void MixedNumbersReturnValueSchema_4()
        {
            // Arrange method arguments
            double x = -1;
            int n = 0;
            
            double result = PrimitiveArguments.MixedNumbers(x, n);
            Assert.Equal(-0, result);
            
        }
    }
}
