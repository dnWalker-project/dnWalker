using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::Min3")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_Min3_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void Min3ReturnValueSchema_1()
        {
            // Arrange method arguments
            int a = 0;
            int b = 0;
            int c = 0;
            
            int result = PrimitiveArguments.Min3(a, b, c);
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void Min3ReturnValueSchema_2()
        {
            // Arrange method arguments
            int a = 0;
            int b = 0;
            int c = -1;
            
            int result = PrimitiveArguments.Min3(a, b, c);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void Min3ReturnValueSchema_3()
        {
            // Arrange method arguments
            int a = 0;
            int b = -1;
            int c = 0;
            
            int result = PrimitiveArguments.Min3(a, b, c);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void Min3ReturnValueSchema_4()
        {
            // Arrange method arguments
            int a = 1;
            int b = 0;
            int c = -1;
            
            int result = PrimitiveArguments.Min3(a, b, c);
            Assert.Equal(-1, result);
            
        }
    }
}
