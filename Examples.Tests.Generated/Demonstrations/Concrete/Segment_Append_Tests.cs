using Examples.Demonstrations.Concrete;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Concrete
{

    [Trait("dnWalkerGenerated", "Segment::Append")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class Segment_Append_Tests
    {

        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "1")]
        public void AppendChangedObjectSchema_1()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = null;

            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);

        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "2")]
        public void AppendChangedObjectSchema_2()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;

            // Arrange method arguments
            Segment @this = segment2;
            int[] data = null;

            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);

        }
    }
}
