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

namespace Examples.Concolic.Features.Objects.Tests
{
	public class MethodsWithObjectParameter_Tests_AbstractClass_ConcreteMethod
	{
		#region Test Methods

		[Fact]
		public void Test_Iteration_1()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.AbstractClass? var_0x00000011 = null;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.AbstractClass_ConcreteMethod(var_0x00000011);
		}

		[Fact]
		public void Test_Iteration_2()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.AbstractClass? var_0x00000011 = Create_0x00000011(out Mock<Examples.Concolic.Features.Objects.AbstractClass> methodsMock_0x00000011);
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.AbstractClass_ConcreteMethod(var_0x00000011);
		}

		[Fact]
		public void Test_Iteration_3()
		{
			// construct all parameters which are used in the test
			// TODO: construct return value comparer instance
			Examples.Concolic.Features.Objects.AbstractClass? var_0x00000011 = Create_0x00000011(out Mock<Examples.Concolic.Features.Objects.AbstractClass> methodsMock_0x00000011);
			
			System.Int32 var_0x00000013 = 5;
			
			// initialize all parameters which needs initialization
			// TODO: initialize return value comparer instance
			
			InitializeFields_0x00000011
			(
				var_0x00000011,
				arg__field: var_0x00000013
			);

			// execute the tested method
			// TODO: make assert for method exception
			// TODO: make assert for method return values
			// TODO: handle instance methods
			Examples.Concolic.Features.Objects.MethodsWithObjectParameter.AbstractClass_ConcreteMethod(var_0x00000011);
		}
		#endregion Test Methods

		#region Parameter Creation Methods
		// these methods constructs instances of parameters along with interfaces for their initialization
		private Examples.Concolic.Features.Objects.AbstractClass Create_0x00000011(out Mock<Examples.Concolic.Features.Objects.AbstractClass> methodsMock)
		{
			methodsMock = new Mock<Examples.Concolic.Features.Objects.AbstractClass>();
			var instance = methodsMock.Object;
			return instance;
		}
		


		#endregion Parameter Creation Methods

		#region Parameter Initialization Methods
		// initialize parameter 0x00000011
		// - instance
		private void InitializeFields_0x00000011(Examples.Concolic.Features.Objects.AbstractClass instance, System.Int32 arg__field = default)
		{
			// initialize field using private object
			instance.SetPrivate("_field", arg__field);
		}
		#endregion Parameter Initialization Methods
	}
}


