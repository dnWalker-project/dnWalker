using Examples.Demonstrations.Concrete;

using Moq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Concrete
{

    [Trait("dnWalkerGenerated", "Segment::Insert")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class Segment_Insert_Tests
    {

        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "1")]
        public void InsertChangedObjectSchema_1()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = null;

            @this.Insert(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);

        }
    }
}