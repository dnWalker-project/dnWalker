using Examples.Demonstrations.ConcreteData;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Demonstrations.ConcreteData
{
    
    [Trait("dnWalkerGenerated", "Segment::Count")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
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
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void CountReturnValueSchema_4()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            
            // Arrange method arguments
            Segment @this = segment1;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "5")]
        public void CountReturnValueSchema_5()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment2;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "6")]
        public void CountReturnValueSchema_6()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            int[] intArr2 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            
            // Arrange method arguments
            Segment @this = segment2;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "7")]
        public void CountReturnValueSchema_7()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            
            // Arrange method arguments
            Segment @this = segment3;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "8")]
        public void CountReturnValueSchema_8()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            int[] intArr2 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr3 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr3;
            
            // Arrange method arguments
            Segment @this = segment3;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "9")]
        public void CountReturnValueSchema_9()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr3;
            
            // Arrange method arguments
            Segment @this = segment4;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "10")]
        public void CountReturnValueSchema_10()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            int[] intArr2 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr3 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr3;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr4;
            
            // Arrange method arguments
            Segment @this = segment4;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "11")]
        public void CountReturnValueSchema_11()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr3;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr4;
            
            // Arrange method arguments
            Segment @this = segment5;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "12")]
        public void CountReturnValueSchema_12()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            int[] intArr2 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr3 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr3;
            int[] intArr5 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr4;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr5;
            
            // Arrange method arguments
            Segment @this = segment5;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "13")]
        public void CountReturnValueSchema_13()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr3;
            int[] intArr5 = new int[0];
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr4;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            segment6.Data = intArr5;
            
            // Arrange method arguments
            Segment @this = segment6;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "14")]
        public void CountReturnValueSchema_14()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            int[] intArr2 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            int[] intArr3 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr3;
            int[] intArr5 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr4;
            int[] intArr6 = new int[0];
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr5;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            segment6.Data = intArr6;
            
            // Arrange method arguments
            Segment @this = segment6;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "15")]
        public void CountReturnValueSchema_15()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr3;
            int[] intArr5 = new int[0];
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            segment6.Data = intArr4;
            Segment segment7 = new Segment();
            segment7.Next = segment6;
            segment7.Data = intArr5;
            
            // Arrange method arguments
            Segment @this = segment7;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "16")]
        public void CountReturnValueSchema_16()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr2;
            int[] intArr4 = new int[0];
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr3;
            Segment segment6 = new Segment();
            segment6.Next = segment5;
            segment6.Data = intArr4;
            
            // Arrange method arguments
            Segment @this = segment6;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "17")]
        public void CountReturnValueSchema_17()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            int[] intArr3 = new int[0];
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr2;
            Segment segment5 = new Segment();
            segment5.Next = segment4;
            segment5.Data = intArr3;
            
            // Arrange method arguments
            Segment @this = segment5;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "18")]
        public void CountReturnValueSchema_18()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            Segment segment4 = new Segment();
            segment4.Next = segment3;
            segment4.Data = intArr2;
            
            // Arrange method arguments
            Segment @this = segment4;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "19")]
        public void CountReturnValueSchema_19()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment3;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "20")]
        public void CountReturnValueSchema_20()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = null;
            
            // Arrange method arguments
            Segment @this = segment3;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "21")]
        public void CountReturnValueSchema_21()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = null;
            
            // Arrange method arguments
            Segment @this = segment3;
            
            int result = @this.Count();
            Assert.Equal(0, result);
            
        }
    }
}
