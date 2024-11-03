using dnWalker.TypeSystem;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.Tests.TypeSystem
{
    public class DefinitionProviderTests
    {
        public class TestClass
        {
            public void Method()
            {

            }

            public void MethodWithOverloads()
            {

            }

            public void MethodWithOverloads(int x, DefinitionProviderTests y)
            {

            }
        }

#if Linux
        const string ExamplesAssembly_net48 = @"../../../../Examples/bin/Release/net48/Examples.dll";
        const string ExamplesAssembly_net50 = @"../../../../Examples/bin/Release/net5.0/Examples.dll";
        const string ExamplesAssembly_net60 = @"../../../../Examples/bin/Release/net6.0/Examples.dll"; 
#elif Windows
        const string ExamplesAssembly_net48 = @"..\..\..\..\Examples\bin\Release\net48\Examples.dll";
        const string ExamplesAssembly_net50 = @"..\..\..\..\Examples\bin\Release\net5.0\Examples.dll";
        const string ExamplesAssembly_net60 = @"..\..\..\..\Examples\bin\Release\net6.0\Examples.dll"; 
#endif

        [Theory]
        [InlineData(ExamplesAssembly_net48)]
        [InlineData(ExamplesAssembly_net50)]
        [InlineData(ExamplesAssembly_net60)]
        public void Test_DefinitionProviderResolves_TextWriter(string location)
        {

            IDomain domain = Domain.LoadFromFile(location);
            DefinitionProvider definitionProvider = new DefinitionProvider(domain);

            definitionProvider.GetTypeDefinition("System.IO.TextWriter").Should().NotBeNull();
        }

        [Fact]
        public void TestPartialTypeFinding()
        {
            IDomain domain = Domain.LoadFromAppDomain(typeof(DefinitionProviderTests).Assembly);
            DefinitionProvider definitionProvider = new DefinitionProvider(domain);
            definitionProvider.GetTypeDefinition(null, "DefinitionProviderTests").Should().NotBeNull();
        }

        [Theory]
        [InlineData("DefinitionProviderTests/TestClass::Method")]
        [InlineData("DefinitionProviderTests/TestClass::MethodWithOverloads()")]
        [InlineData("DefinitionProviderTests/TestClass::MethodWithOverloads(Int32,DefinitionProviderTests)")]
        public void TestPartialMethodFinding(string methodSpecification)
        {
            IDomain domain = Domain.LoadFromAppDomain(typeof(DefinitionProviderTests).Assembly);
            DefinitionProvider definitionProvider = new DefinitionProvider(domain);
            definitionProvider.GetMethodDefinitionPartial(methodSpecification).Should().NotBeNull();
        }
    }
}
