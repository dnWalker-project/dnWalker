using Examples.Demonstrations.AbstractData;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.AbstractData
{
    
    [Trait("dnWalkerGenerated", "SimpleMethod::MethodNoArgs")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class SimpleMethod_MethodNoArgs_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void MethodNoArgsExceptionSchema_1()
        {
            // Arrange input model heap
            SimpleMethod simpleMethod1 = new SimpleMethod();
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = null;
            
            Action methodNoArgs = () => @this.MethodNoArgs(bar);
            Assert.Throws<NullReferenceException>(methodNoArgs);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void MethodNoArgsReturnValueSchema_2()
        {
            // Arrange input model heap
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 0;
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValue())
                .Returns(0);
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            
            int result = @this.MethodNoArgs(bar);
            Assert.Equal(5, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void MethodNoArgsReturnValueSchema_3()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValue())
                .Returns(0);
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = -3;
            
            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            
            int result = @this.MethodNoArgs(bar);
            Assert.Equal(4, result);
            
        }
    }
}
