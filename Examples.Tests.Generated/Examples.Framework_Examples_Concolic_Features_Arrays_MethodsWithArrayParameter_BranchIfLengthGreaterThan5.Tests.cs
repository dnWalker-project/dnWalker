// This file was generated by dnWalker.TestGenerator tool 01. 01. 2022 - 22:53:50
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
	public class MethodsWithArrayParameter_Tests_BranchIfLengthGreaterThan5
	{
		#region Test Methods

		[Fact]
		public void Test_Iteration_1()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32[]? var_0x00000001 = null;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5(var_0x00000001);
		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32[]? var_0x00000001 = Create_0x00000001(0);
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5(var_0x00000001);
		}

		[Fact]
		public void Test_Iteration_3()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			System.Int32[]? var_0x00000001 = Create_0x00000001(6);
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			InitializeItems_0x00000001
			(
				var_0x00000001,
				default(System.Int32),
				default(System.Int32),
				default(System.Int32),
				default(System.Int32),
				default(System.Int32),
				default(System.Int32)
			);
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Arrays.MethodsWithArrayParameter.BranchIfLengthGreaterThan5(var_0x00000001);
		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization
		private System.Int32[] Create_0x00000001(int length)
		{
			return new System.Int32[length];
		}
		


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		// initialize parameter 0x00000001
		// - instance
		private void InitializeItems_0x00000001(System.Int32[] array, params System.Int32[] items)
		{
			items.CopyTo(array, System.Math.Min(items.Length, array.Length));
		}
		#endregion Parameter Initialization Methods
	}
}


