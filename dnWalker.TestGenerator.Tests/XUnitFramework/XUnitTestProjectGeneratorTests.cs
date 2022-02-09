using dnWalker.TestGenerator.XUnitFramework;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.XUnitFramework
{
    public class XUnitTestProjectGeneratorTests
    {
        const string BasicXUnitTestProject =
@"<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
        <IsPackable>false</IsPackable>
        <Nullable>enable</Nullable>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""17.0.0"" />
        <PackageReference Include=""xunit"" Version=""2.4.1"" />
        <PackageReference Include=""xunit.runner.visualstudio"" Version=""2.4.3"">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
            <PrivateAssets>all</PrivateAssets>
        </PackageReference>
        <PackageReference Include=""coverlet.collector"" Version=""3.1.0"">
            <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
            <PrivateAssets>all</PrivateAssets>
        </PackageReference>
        <PackageReference Include=""FluentAssertions"" Version=""6.3.0"" />
        <PackageReference Include=""Moq"" Version=""4.16.1"" />
        <PackageReference Include=""PrivateObjectExtensions"" Version=""1.4.0"" />
    </ItemGroup>

</Project>";

        [Fact]
        public void Test_BasicXUnitTestProject()
        {
            TestProjectContext context = new TestProjectContext();
            string generatedContent = new XUnitTestProjectGenerator().GenerateProjectFileContent(context).Trim();

            //string line463 = generatedContent.Substring(463 - 10, 30);

            generatedContent.Should().Be(BasicXUnitTestProject);
        }

    }
}
