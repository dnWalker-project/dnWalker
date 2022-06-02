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
        const string ExamplesAssembly_net48 = @"..\..\..\..\Examples\bin\Release\net48\Examples.dll";
        const string ExamplesAssembly_net50 = @"..\..\..\..\Examples\bin\Release\net5.0\Examples.dll";
        const string ExamplesAssembly_net60 = @"..\..\..\..\Examples\bin\Release\net6.0\Examples.dll";

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
    }
}
