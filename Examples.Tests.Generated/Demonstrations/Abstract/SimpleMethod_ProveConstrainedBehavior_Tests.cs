using Examples.Demonstrations.Abstract;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Abstract
{

    [Trait("dnWalkerGenerated", "SimpleMethod::ProveConstrainedBehavior")]
    [Trait("ExplorationStrategy", "AllPathsCoverage")]
    public class SimpleMethod_ProveConstrainedBehavior_Tests
    {

        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void ProveConstrainedBehaviorReturnValueSchema_1()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double, double>((x, y) =>
                {
                    if ((x < y))
                    {
                        return 5;
                    }
                    if ((x >= y))
                    {
                        return 10;
                    }
                    else
                    {
                        return 0;
                    }
                });
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 0;

            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 0;

            int result = @this.ProveConstrainedBehavior(bar, x, y);
            Assert.Equal(1, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void ProveConstrainedBehaviorReturnValueSchema_2()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double, double>((x, y) =>
                {
                    if ((x < y))
                    {
                        return 5;
                    }
                    if ((x >= y))
                    {
                        return 10;
                    }
                    else
                    {
                        return 0;
                    }
                });
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 10;

            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 0;

            int result = @this.ProveConstrainedBehavior(bar, x, y);
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void ProveConstrainedBehaviorReturnValueSchema_3()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double, double>((x, y) =>
                {
                    if ((x < y))
                    {
                        return 5;
                    }
                    if ((x >= y))
                    {
                        return 10;
                    }
                    else
                    {
                        return 0;
                    }
                });
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 0;

            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 1;

            int result = @this.ProveConstrainedBehavior(bar, x, y);
            Assert.Equal(1, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void ProveConstrainedBehaviorReturnValueSchema_4()
        {
            // Arrange input model heap
            Mock<IBar> iBar1_mock = new Mock<IBar>();
            IBar iBar1 = iBar1_mock.Object;
            iBar1_mock
                .Setup(o => o.GetValueWithArgs(It.IsAny<double>(), It.IsAny<double>()))
                .Returns<double, double>((x, y) =>
                {
                    if ((x < y))
                    {
                        return 5;
                    }
                    if ((x >= y))
                    {
                        return 10;
                    }
                    else
                    {
                        return 0;
                    }
                });
            SimpleMethod simpleMethod1 = new SimpleMethod();
            simpleMethod1.Value = 5;

            // Arrange method arguments
            SimpleMethod @this = simpleMethod1;
            IBar bar = iBar1;
            double x = 0;
            double y = 1;

            int result = @this.ProveConstrainedBehavior(bar, x, y);
            Assert.Equal(0, result);

        }
    }
}
