using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.Primitive
{
    
    [Trait("dnWalkerGenerated", "PrimitiveArguments::UseCos")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class PrimitiveArguments_UseCos_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void UseCosReturnValueSchema_1()
        {
            // Arrange method arguments
            double x = 0;
            double y = 0;
            
            int result = PrimitiveArguments.UseCos(x, y);
            Assert.Equal(1, result);
            
        }
    }
}
