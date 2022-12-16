using Examples.Demonstrations.AbstractData;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.AbstractData
{
    
    [Trait("dnWalkerGenerated", "SimpleMethod::MethodWithArgs")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class SimpleMethod_MethodWithArgs_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void MethodWithArgsExceptionSchema_1()
        {
            // Arrange input model heap
            SimpleMethod simpleMethod1 = new SimpleMethod();
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = null;
            double x = 0;
            double y = 0;
            
            Action methodWithArgs = () => @this.MethodWithArgs(bar, x, y);
            Assert.Throws<NullReferenceException>(methodWithArgs);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void MethodWithArgsReturnValueSchema_2()
        {
            // Arrange input model heap
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 0;
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns(0);
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 0;
            
            int result = @this.MethodWithArgs(bar, x, y);
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void MethodWithArgsReturnValueSchema_3()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns(0);
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 1;
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 0;
            
            int result = @this.MethodWithArgs(bar, x, y);
            Assert.Equal(1, result);
            
        }
    }
}
