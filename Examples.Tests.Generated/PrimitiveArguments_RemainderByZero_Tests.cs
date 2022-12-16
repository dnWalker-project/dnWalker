using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::RemainderByZero")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_RemainderByZero_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void RemainderByZeroExceptionSchema_1()
        {
            // Arrange method arguments
            int x = 0;
            int y = 0;
            int z = 0;
            
            Action remainderByZero = () => PrimitiveArguments.RemainderByZero(x, y, z);
            Assert.Throws<DivideByZeroException>(remainderByZero);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void RemainderByZeroReturnValueSchema_2()
        {
            // Arrange method arguments
            int x = 0;
            int y = -1;
            int z = 0;
            
            int result = PrimitiveArguments.RemainderByZero(x, y, z);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void RemainderByZeroReturnValueSchema_3()
        {
            // Arrange method arguments
            int x = 0;
            int y = -1;
            int z = 4;
            
            int result = PrimitiveArguments.RemainderByZero(x, y, z);
            Assert.Equal(0, result);
            
        }
    }
}
