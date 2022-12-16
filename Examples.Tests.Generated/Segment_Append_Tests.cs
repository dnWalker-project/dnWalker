using Examples.Demonstrations.ConcreteData;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.ConcreteData
{
    
    [Trait("dnWalkerGenerated", "Segment::Append")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
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
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "3")]
        public void AppendChangedObjectSchema_3()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "4")]
        public void AppendChangedObjectSchema_4()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            
            // Arrange method arguments
            Segment @this = segment4;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "5")]
        public void AppendChangedObjectSchema_5()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            
            // Arrange method arguments
            Segment @this = segment5;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "6")]
        public void AppendChangedObjectSchema_6()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            
            // Arrange method arguments
            Segment @this = segment6;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "7")]
        public void AppendChangedObjectSchema_7()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            
            // Arrange method arguments
            Segment @this = segment7;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "8")]
        public void AppendChangedObjectSchema_8()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            
            // Arrange method arguments
            Segment @this = segment8;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "9")]
        public void AppendChangedObjectSchema_9()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            Segment segment9 = new Segment();
            segment9.Next = segment8;
            
            // Arrange method arguments
            Segment @this = segment9;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "10")]
        public void AppendChangedObjectSchema_10()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            Segment segment9 = new Segment();
            segment9.Next = segment8;
            Segment segment10 = new Segment();
            segment10.Next = segment9;
            
            // Arrange method arguments
            Segment @this = segment10;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "11")]
        public void AppendChangedObjectSchema_11()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            Segment segment9 = new Segment();
            segment9.Next = segment8;
            Segment segment10 = new Segment();
            segment10.Next = segment9;
            Segment segment11 = new Segment();
            segment11.Next = segment10;
            
            // Arrange method arguments
            Segment @this = segment11;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "12")]
        public void AppendChangedObjectSchema_12()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            Segment segment9 = new Segment();
            segment9.Next = segment8;
            Segment segment10 = new Segment();
            segment10.Next = segment9;
            Segment segment11 = new Segment();
            segment11.Next = segment10;
            Segment segment12 = new Segment();
            segment12.Next = segment11;
            
            // Arrange method arguments
            Segment @this = segment12;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "13")]
        public void AppendChangedObjectSchema_13()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            Segment segment8 = new Segment();
            segment8.Next = segment7;
            Segment segment9 = new Segment();
            segment9.Next = segment8;
            Segment segment10 = new Segment();
            segment10.Next = segment9;
            Segment segment11 = new Segment();
            segment11.Next = segment10;
            Segment segment12 = new Segment();
            segment12.Next = segment11;
            Segment segment13 = new Segment();
            segment13.Next = segment12;
            
            // Arrange method arguments
            Segment @this = segment13;
            int[] data = null;
            
            @this.Append(data);
            Assert.NotNull(segment1.Next);
            Assert.NotNull(segment1.Next);
            
        }
    }
}
