using Examples.Demonstrations.Concrete;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Concrete
{

    [Trait("dnWalkerGenerated", "Segment::Delete")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class Segment_Delete_Tests
    {

        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "1")]
        public void DeleteReturnValueSchema_1()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = null;

            Segment result = @this.Delete(data);
            Assert.Null(result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void DeleteReturnValueSchema_2()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr2 = new int[0];

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr2;

            Segment result = @this.Delete(data);
            Assert.Null(result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void DeleteReturnValueSchema_3()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr2;

            Segment result = @this.Delete(data);
            Assert.Null(result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void DeleteReturnValueSchema_4()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr2 = new int[1];
            intArr2[0] = -1;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr2;

            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment1, result);
            Assert.Same(intArr1, result.Data);
            Assert.Null(result.Next);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "5")]
        public void DeleteReturnValueSchema_5()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;

            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;

            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Same(intArr1, result.Data);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "6")]
        public void DeleteReturnValueSchema_6()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr2 = new int[1];

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr2;

            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment1, result);
            Assert.Same(intArr1, result.Data);
            Assert.Null(result.Next);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "7")]
        public void DeleteReturnValueSchema_7()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;

            // Arrange method arguments
            Segment @this = segment1;
            int[] data = null;

            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment1, result);
            Assert.Same(intArr1, result.Data);
            Assert.Null(result.Next);

        }
    }
}
