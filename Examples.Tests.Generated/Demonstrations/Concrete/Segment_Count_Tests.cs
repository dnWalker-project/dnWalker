using Examples.Demonstrations.Concrete;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Concrete
{

    [Trait("dnWalkerGenerated", "Segment::Count")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class Segment_Count_Tests
    {

        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void CountReturnValueSchema_1()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;

            int result = @this.Count();
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void CountReturnValueSchema_2()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;

            // Arrange method arguments
            Segment @this = segment2;

            int result = @this.Count();
            Assert.Equal(0, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void CountReturnValueSchema_3()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;

            // Arrange method arguments
            Segment @this = segment2;

            int result = @this.Count();
            Assert.Equal(0, result);

        }
    }
}
