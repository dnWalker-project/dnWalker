using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::DivideByZero")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_DivideByZero_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void DivideByZeroExceptionSchema_1()
        {
            // Arrange method arguments
            int x = 0;
            int y = 0;
            int z = 0;
            
            Action divideByZero = () => PrimitiveArguments.DivideByZero(x, y, z);
            Assert.Throws<DivideByZeroException>(divideByZero);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void DivideByZeroReturnValueSchema_2()
        {
            // Arrange method arguments
            int x = 0;
            int y = -1;
            int z = 0;
            
            int result = PrimitiveArguments.DivideByZero(x, y, z);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void DivideByZeroReturnValueSchema_3()
        {
            // Arrange method arguments
            int x = 0;
            int y = -1;
            int z = 4;
            
            int result = PrimitiveArguments.DivideByZero(x, y, z);
            Assert.Equal(0, result);
            
        }
    }
}
