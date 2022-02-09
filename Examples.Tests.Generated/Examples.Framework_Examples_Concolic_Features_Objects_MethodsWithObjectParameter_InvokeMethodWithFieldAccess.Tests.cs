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

namespace Examples.Concolic.Features.Objects.Tests
{
	public class MethodsWithObjectParameter_Tests_InvokeMethodWithFieldAccess
	{
		#region Test Methods

		[Fact]
		public void Test_Iteration_1()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.TestClass? instance = null;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess(instance);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.TestClass? instance = Create_0x00000001(out Mock<Examples.Concolic.Features.Objects.TestClass> methodsMock_0x00000001);
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess(instance);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}

		[Fact]
		public void Test_Iteration_3()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.TestClass? instance = Create_0x00000001(out Mock<Examples.Concolic.Features.Objects.TestClass> methodsMock_0x00000001);
			
			System.Int32 var_0x00000002 = 3;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			InitializeFields_0x00000001
			(
				instance,
				arg__myField: var_0x00000002 
			);

			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.InvokeMethodWithFieldAccess(instance);

			// construct all output parameters which are used in the test
			// 1. ret val if exists


		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization
		private Examples.Concolic.Features.Objects.TestClass Create_0x00000001(out Mock<Examples.Concolic.Features.Objects.TestClass> methodsMock)
		{
			methodsMock = new Mock<Examples.Concolic.Features.Objects.TestClass>();
			var instance = methodsMock.Object;
			return instance;
		}
		


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		// initialize parameter 0x00000001
		// - instance
		private void InitializeFields_0x00000001(Examples.Concolic.Features.Objects.TestClass instance, System.Int32 arg__myField = default)
		{
			// initialize field using private object
			instance.SetPrivate("_myField", arg__myField);
		}
		#endregion Parameter Initialization Methods
	}
}


