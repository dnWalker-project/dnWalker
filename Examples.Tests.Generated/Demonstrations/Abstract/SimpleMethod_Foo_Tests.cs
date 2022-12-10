using Examples.Demonstrations.Abstract;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Abstract
{

    [Trait("dnWalkerGenerated", "SimpleMethod::Foo")]
    [Trait("ExplorationStrategy", "")]
    public class SimpleMethod_Foo_Tests
    {

        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void FooExceptionSchema_1()
        {
            // Arrange input model heap
            SimpleMethod simpleMethod1 = new SimpleMethod();

            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = null;

            Action foo = () => @this.Foo(bar);
            Assert.Throws<NullReferenceException>(foo);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void FooReturnValueSchema_2()
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

            int result = @this.Foo(bar);
            Assert.Equal(5, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void FooReturnValueSchema_3()
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

            int result = @this.Foo(bar);
            Assert.Equal(4, result);

        }
    }
}
