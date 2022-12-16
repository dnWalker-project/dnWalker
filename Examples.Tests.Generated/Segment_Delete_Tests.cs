using Examples.Demonstrations.ConcreteData;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Demonstrations.ConcreteData
{
    
    [Trait("dnWalkerGenerated", "Segment::Delete")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
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
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "7")]
        public void DeleteReturnValueSchema_7()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "8")]
        public void DeleteChangedObjectSchema_8()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = -1;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "8")]
        public void DeleteReturnValueSchema_9()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = -1;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "9")]
        public void DeleteChangedObjectSchema_10()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            Segment segment1 = new Segment();
            segment1.Data = intArr2;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "9")]
        public void DeleteReturnValueSchema_11()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            Segment segment1 = new Segment();
            segment1.Data = intArr2;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Same(intArr1, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "10")]
        public void DeleteReturnValueSchema_12()
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
        [Trait("Iteration", "11")]
        public void DeleteReturnValueSchema_13()
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
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "12")]
        public void DeleteReturnValueSchema_14()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            
            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr1;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment1, result);
            Assert.Null(result.Data);
            Assert.Null(result.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "13")]
        public void DeleteReturnValueSchema_15()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[0];
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr1;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "14")]
        public void DeleteChangedObjectSchema_16()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "14")]
        public void DeleteReturnValueSchema_17()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[0];
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "15")]
        public void DeleteChangedObjectSchema_18()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "15")]
        public void DeleteReturnValueSchema_19()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "16")]
        public void DeleteReturnValueSchema_20()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "17")]
        public void DeleteReturnValueSchema_21()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[1];
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Same(segment1, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "18")]
        public void DeleteChangedObjectSchema_22()
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
            int[] data = intArr1;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "18")]
        public void DeleteReturnValueSchema_23()
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
            int[] data = intArr1;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "19")]
        public void DeleteReturnValueSchema_24()
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
            int[] intArr2 = new int[1];
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "20")]
        public void DeleteReturnValueSchema_25()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "21")]
        public void DeleteChangedObjectSchema_26()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[2];
            intArr2[0] = 0;
            intArr2[1] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "21")]
        public void DeleteReturnValueSchema_27()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            int[] intArr2 = new int[2];
            intArr2[0] = 0;
            intArr2[1] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "22")]
        public void DeleteReturnValueSchema_28()
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
            int[] intArr1 = new int[0];
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr1;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Null(result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "23")]
        public void DeleteChangedObjectSchema_29()
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
            int[] data = null;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "23")]
        public void DeleteReturnValueSchema_30()
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
            int[] data = null;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Same(intArr1, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "24")]
        public void DeleteReturnValueSchema_31()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[0];
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
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "25")]
        public void DeleteChangedObjectSchema_32()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            int[] intArr2 = new int[2];
            intArr2[0] = -1;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[2];
            intArr3[0] = 0;
            intArr3[1] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            @this.Delete(data);
            Assert.Null(segment2.Next);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "25")]
        public void DeleteReturnValueSchema_33()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            int[] intArr2 = new int[2];
            intArr2[0] = -1;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr2;
            int[] intArr3 = new int[2];
            intArr3[0] = 0;
            intArr3[1] = 0;
            
            // Arrange method arguments
            Segment @this = segment2;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment2, result);
            Assert.Null(result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "26")]
        public void DeleteReturnValueSchema_34()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = -1;
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[1];
            intArr2[0] = -1;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 0;
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "27")]
        public void DeleteReturnValueSchema_35()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr2 = new int[1];
            intArr2[0] = 0;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = intArr1;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr2;
            int[] intArr3 = new int[1];
            intArr3[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr3;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Same(intArr2, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "28")]
        public void DeleteReturnValueSchema_36()
        {
            // Arrange input model heap
            Segment segment1 = new Segment();
            segment1.Data = null;
            segment1.Next = null;
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            Segment segment2 = new Segment();
            segment2.Next = segment1;
            segment2.Data = null;
            Segment segment3 = new Segment();
            segment3.Next = segment2;
            segment3.Data = intArr1;
            int[] intArr2 = new int[1];
            intArr2[0] = 1;
            
            // Arrange method arguments
            Segment @this = segment3;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.NotNull(result);
            Assert.Same(segment3, result);
            Assert.Same(segment2, result.Next);
            Assert.Same(intArr1, result.Data);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "29")]
        public void DeleteReturnValueSchema_37()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            int[] intArr2 = new int[2];
            intArr2[0] = 0;
            intArr2[1] = 0;
            Segment segment1 = new Segment();
            segment1.Data = intArr1;
            segment1.Next = null;
            
            // Arrange method arguments
            Segment @this = segment1;
            int[] data = intArr2;
            
            Segment result = @this.Delete(data);
            Assert.Null(result);
            
        }
    }
}
