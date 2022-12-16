using Examples.Demonstrations.ConcreteData;

using Moq;

using System;

using Xunit;

namespace Examples.Tests.Demonstrations.ConcreteData
{
    
    [Trait("dnWalkerGenerated", "Arrays::IndexOf")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
    public class Arrays_IndexOf_Tests
    {
        
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void IndexOfExceptionSchema_1()
        {
            // Arrange method arguments
            int value = 0;
            int[] array = null;
            
            Action indexOf = () => Arrays.IndexOf(value, array);
            Assert.Throws<ArgumentNullException>(indexOf);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "2")]
        public void IndexOfReturnValueSchema_2()
        {
            // Arrange input model heap
            int[] intArr1 = new int[0];
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void IndexOfReturnValueSchema_3()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(0, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void IndexOfReturnValueSchema_4()
        {
            // Arrange input model heap
            int[] intArr1 = new int[1];
            intArr1[0] = 0;
            
            // Arrange method arguments
            int value = 1;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "5")]
        public void IndexOfReturnValueSchema_5()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[0] = 0;
            intArr1[1] = 0;
            
            // Arrange method arguments
            int value = 1;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "6")]
        public void IndexOfReturnValueSchema_6()
        {
            // Arrange input model heap
            int[] intArr1 = new int[2];
            intArr1[1] = 0;
            intArr1[0] = -1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "7")]
        public void IndexOfReturnValueSchema_7()
        {
            // Arrange input model heap
            int[] intArr1 = new int[3];
            intArr1[1] = -1;
            intArr1[0] = -1;
            intArr1[2] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(2, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "8")]
        public void IndexOfReturnValueSchema_8()
        {
            // Arrange input model heap
            int[] intArr1 = new int[3];
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "9")]
        public void IndexOfReturnValueSchema_9()
        {
            // Arrange input model heap
            int[] intArr1 = new int[4];
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = 1;
            intArr1[3] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(3, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "10")]
        public void IndexOfReturnValueSchema_10()
        {
            // Arrange input model heap
            int[] intArr1 = new int[4];
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = 1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "11")]
        public void IndexOfReturnValueSchema_11()
        {
            // Arrange input model heap
            int[] intArr1 = new int[5];
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = 1;
            intArr1[4] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(4, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "12")]
        public void IndexOfReturnValueSchema_12()
        {
            // Arrange input model heap
            int[] intArr1 = new int[5];
            intArr1[4] = -1;
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "13")]
        public void IndexOfReturnValueSchema_13()
        {
            // Arrange input model heap
            int[] intArr1 = new int[6];
            intArr1[4] = -1;
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            intArr1[5] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(5, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "14")]
        public void IndexOfReturnValueSchema_14()
        {
            // Arrange input model heap
            int[] intArr1 = new int[6];
            intArr1[5] = -1;
            intArr1[4] = -1;
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "15")]
        public void IndexOfReturnValueSchema_15()
        {
            // Arrange input model heap
            int[] intArr1 = new int[7];
            intArr1[5] = -1;
            intArr1[4] = -1;
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            intArr1[6] = 0;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(6, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "16")]
        public void IndexOfReturnValueSchema_16()
        {
            // Arrange input model heap
            int[] intArr1 = new int[7];
            intArr1[6] = -1;
            intArr1[5] = -1;
            intArr1[4] = -1;
            intArr1[3] = -1;
            intArr1[2] = -1;
            intArr1[1] = -1;
            intArr1[0] = -1;
            
            // Arrange method arguments
            int value = 0;
            int[] array = intArr1;
            
            int result = Arrays.IndexOf(value, array);
            Assert.Equal(-1, result);
            
        }
    }
}
