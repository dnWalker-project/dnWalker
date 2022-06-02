using dnWalker.TestGenerator.TestProjects;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.TestProjects
{
    public class TestProjectWriterTests
    {
        [Fact]
        public void DefaultProject()
        {
            const string ExpectedOutput =
@"<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
        <IsPackable>false</IsPackable>
        <Nullable>enable</Nullable>
    </PropertyGroup>

</Project>";


            TestProjectContext context = new TestProjectContext();

            TestProjectWriter projectWriter = new TestProjectWriter();

            StringWriter output = new StringWriter();
            projectWriter.WriteProject(output, context);
            string generatedContent = output.ToString().Trim();

            generatedContent.Should().Be(ExpectedOutput);
        }

        [Fact]
        public void XunitProject()
        {
            const string ExpectedOutput =
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
    </ItemGroup>

</Project>";

            TestProjectContext context = new TestProjectContext();

            TestProjectWriter projectWriter = new TestProjectWriter();

            new XunitProvider.XunitFramework().InitializeProjectContext(context);

            StringWriter output = new StringWriter();
            projectWriter.WriteProject(output, context);
            string generatedContent = output.ToString().Trim();

            generatedContent.Should().Be(ExpectedOutput);
        }
    }
}
