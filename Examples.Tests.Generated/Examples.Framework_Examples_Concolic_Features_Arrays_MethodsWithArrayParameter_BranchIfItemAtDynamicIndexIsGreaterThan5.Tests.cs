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

namespace Examples.Concolic.Features.Arrays.Tests
{
	public class MethodsWithArrayParameter_Tests_BranchIfItemAtDynamicIndexIsGreaterThan5
	{
		#region Test Methods

		[Fact]
		public void Test_Iteration_1()
		{

			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Double[]? array = null;
			
			System.Int32 index =  default(System.Int32) ;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5(array, index);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Double[]? array = Create_0x00000004(0);
			
			System.Int32 index =  default(System.Int32) ;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5(array, index);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}

		[Fact]
		public void Test_Iteration_3()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Double[]? array = Create_0x00000004(1);
			
			System.Int32 index = 0;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			InitializeItems_0x00000004
			(
				array,
				default(System.Double)
			);
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5(array, index);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}

		[Fact]
		public void Test_Iteration_4()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Double[]? array = Create_0x00000004(1);
			
			System.Int32 index = 0;
			
			System.Double var_0x00000006 = 6;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			InitializeItems_0x00000004
			(
				array,
				var_0x00000006
			);
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfItemAtDynamicIndexIsGreaterThan5(array, index);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization
		private System.Double[] Create_0x00000004(int length)
		{
			return new System.Double[length];
		}
		


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		// initialize parameter 0x00000004
		// - array
		private void InitializeItems_0x00000004(System.Double[] array, params System.Double[] items)
		{
			items.CopyTo(array, System.Math.Min(items.Length, array.Length));
		}
		#endregion Parameter Initialization Methods
	}
}


