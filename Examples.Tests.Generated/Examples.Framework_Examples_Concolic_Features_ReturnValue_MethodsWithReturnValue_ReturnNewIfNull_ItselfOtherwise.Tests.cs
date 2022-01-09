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
	public class MethodsWithReturnValue_Tests_ReturnNewIfNull_ItselfOtherwise
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
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.ReturnNewIfNull_ItselfOtherwise(instance);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			// ERROR: method has a return type but no return value is specified!!!! Maybe because it returns another parametrized value and the alias parameter is not yet implemented.


		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all input parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.TestClass? instance = Create_0x00000008(out Mock<Examples.Concolic.Features.Objects.TestClass> methodsMock_0x00000008);
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			var result = Examples.Concolic.Features.ReturnValue.MethodsWithReturnValue.ReturnNewIfNull_ItselfOtherwise(instance);

			// construct all output parameters which are used in the test
			// 1. ret val if exists
			// ERROR: method has a return type but no return value is specified!!!! Maybe because it returns another parametrized value and the alias parameter is not yet implemented.


		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization
		private Examples.Concolic.Features.Objects.TestClass Create_0x00000008(out Mock<Examples.Concolic.Features.Objects.TestClass> methodsMock)
		{
			methodsMock = new Mock<Examples.Concolic.Features.Objects.TestClass>();
			var instance = methodsMock.Object;
			return instance;
		}
		


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		// initialize parameter 0x00000008
		// - instance
		#endregion Parameter Initialization Methods
	}
}


