using Examples.Demonstrations.ConcreteData;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Demonstrations.ConcreteData
{
    
    [Trait("dnWalkerGenerated", "Segment::Foo")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class Segment_Foo_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void FooReturnValueSchema_1()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            
            // Arrange method arguments
            Segment @this = segment1;
            Segment other = null;
            int x = 0;
            
            int result = @this.Foo(other, x);
            Assert.Equal(2, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void FooReturnValueSchema_2()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            
            // Arrange method arguments
            Segment @this = segment1;
            Segment other = null;
            int x = 5;
            
            int result = @this.Foo(other, x);
            Assert.Equal(2, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void FooReturnValueSchema_3()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment1;
            Segment other = null;
            int x = 5;
            
            int result = @this.Foo(other, x);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedArraySchema")]
        [Trait("Iteration", "4")]
        public void FooChangedArraySchema_4()
        {
            // Arrange input model heap
            int[] intArr1 = new int[4];
            intArr1[3] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment1;
            Segment other = null;
            int x = 5;
            
            @this.Foo(other, x);
            Assert.Equal(5, intArr1[3]);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void FooReturnValueSchema_5()
        {
            // Arrange input model heap
            int[] intArr1 = new int[4];
            intArr1[3] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment1;
            Segment other = null;
            int x = 5;
            
            int result = @this.Foo(other, x);
            Assert.Equal(0, result);
            
        }
    }
}
