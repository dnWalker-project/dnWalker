using Examples.Demonstrations.Primitive;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Primitive
{

    [Trait("dnWalkerGenerated", "MethodsWithPrimitiveArguments::Foo")]
    [Trait("ExplorationStrategy", "AllPathsCoverage")]
    public class MethodsWithPrimitiveArguments_Foo_Tests
    {

        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void FooReturnValueSchema_1()
        {
            // Arrange method arguments
            int x = 0;
            int y = 0;

            int result = MethodsWithPrimitiveArguments.Foo(x, y);
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "2")]
        public void FooExceptionSchema_2()
        {
            // Arrange method arguments
            int x = 314;
            int y = 0;

            Action foo = () => MethodsWithPrimitiveArguments.Foo(x, y);
            Assert.Throws<InvalidOperationException>(foo);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void FooReturnValueSchema_3()
        {
            // Arrange method arguments
            int x = -1;
            int y = 0;

            int result = MethodsWithPrimitiveArguments.Foo(x, y);
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "4")]
        public void FooExceptionSchema_4()
        {
            // Arrange method arguments
            int x = -314;
            int y = 0;

            Action foo = () => MethodsWithPrimitiveArguments.Foo(x, y);
            Assert.Throws<InvalidOperationException>(foo);

        }
    }
}
