using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::UsePow")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_UsePow_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void UsePowReturnValueSchema_1()
        {
            // Arrange method arguments
            double x = 0;
            double y = 0;
            double z = 0;
            
            int result = PrimitiveArguments.UsePow(x, y, z);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void UsePowReturnValueSchema_2()
        {
            // Arrange method arguments
            double x = 0;
            double y = 0;
            double z = 2;
            
            int result = PrimitiveArguments.UsePow(x, y, z);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void UsePowReturnValueSchema_3()
        {
            // Arrange method arguments
            double x = 0;
            double y = 0;
            double z = 37;
            
            int result = PrimitiveArguments.UsePow(x, y, z);
            Assert.Equal(0, result);
            
        }
    }
}
