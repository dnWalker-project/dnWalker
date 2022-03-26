// This file was generated by dnWalker.TestGenerator tool 09. 01. 2022 - 09:51:08
#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using FluentAssertions;

using Xunit;

using Moq;

namespace Examples.Concolic.Features.ReturnValue.Tests
{
	public class MethodsWithReturnValue_Tests_GetSumIfTrueDiffIfFalse
	{
		#region Test Methods

		[Fact]
		public void Test_Iteration_1()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32 a =  default(System.Int32) ;
			
			System.Int32 b =  default(System.Int32) ;
			
			System.Boolean flag =  default(System.Boolean) ;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse(a, b, flag);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			System.Int32  retValue = 8;
			Assert.Equal(result, retValue);



		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32 a =  default(System.Int32) ;
			
			System.Int32 b = 10;
			
			System.Boolean flag = false;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse(a, b, flag);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			System.Int32  retValue = -16;
			Assert.Equal(result, retValue);



		}

		[Fact]
		public void Test_Iteration_3()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32 a =  default(System.Int32) ;
			
			System.Int32 b = 10;
			
			System.Boolean flag = true;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse(a, b, flag);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			System.Int32  retValue = 18;
			Assert.Equal(result, retValue);



		}

		[Fact]
		public void Test_Iteration_4()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32 a = 10;
			
			System.Int32 b = 10;
			
			System.Boolean flag = true;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.GetSumIfTrueDiffIfFalse(a, b, flag);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			System.Int32  retValue = 14;
			Assert.Equal(result, retValue);



		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		#endregion Parameter Initialization Methods
	}
}

