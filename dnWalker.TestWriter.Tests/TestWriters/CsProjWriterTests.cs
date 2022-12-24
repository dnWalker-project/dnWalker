using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.TestWriters;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Tests.TestWriters
{
    public class CsProjWriterTests
    {
        [Fact]
        public void EmptyProject()
        {
            // cannot be used on locals :(
            // [StringSyntax(StringSyntaxAttribute.Xml)]
            const string Expected =
            """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net7.0</TargetFramework>
                    <IsPackable>false</IsPackable>
                </PropertyGroup>
            </Project>
            """;

            StringWriter output = new StringWriter();
            using (CsProjWriter writer = new CsProjWriter(output))
            {
                writer.Write(new TestProject());
            }

            output.ToString().Should().Be(Expected);
        }

        [Fact]
        public void WithProperties()
        {
            const string Expected =
            """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <IsPackable>false</IsPackable>
                    <TargetFramework>net5.0</TargetFramework>
                </PropertyGroup>
            </Project>
            """;

            TestProject project = new TestProject()
            {
                Properties =
                {
                    { "TargetFramework", "net5.0" }
                }
            };

            StringWriter output = new StringWriter();
            using (CsProjWriter writer = new CsProjWriter(output))
            {
                writer.Write(project);
            }

            output.ToString().Should().Be(Expected);
        }

        [Fact]
        public void WithSimplePackages()
        {
            const string Expected =
            """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net7.0</TargetFramework>
                    <IsPackable>false</IsPackable>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="PREF1" Version="1.0.0" />
                    <PackageReference Include="PREF2" Version="1.0.5-beta" />
                </ItemGroup>
            </Project>
            """;

            TestProject project = new TestProject()
            {
                Packages =
                {
                    new PackageReference
                    {
                        Name = "PREF1",
                        Version = PackageVersion.Parse("1.0.0"),
                    },
                    new PackageReference
                    {
                        Name = "PREF2",
                        Version = PackageVersion.Parse("1.0.5-beta"),
                    }
                }
            };

            StringWriter output = new StringWriter();
            using (CsProjWriter writer = new CsProjWriter(output))
            {
                writer.Write(project);
            }

            output.ToString().Should().Be(Expected);
        }

        [Fact]
        public void WithComplexPackages()
        {
            const string Expected =
            """
            <Project Sdk="Microsoft.NET.Sdk">
                <PropertyGroup>
                    <TargetFramework>net7.0</TargetFramework>
                    <IsPackable>false</IsPackable>
                </PropertyGroup>
                <ItemGroup>
                    <PackageReference Include="PREF1" Version="1.0.0">
                        <IncludeAssets>ass1; asset2; contentfiles</IncludeAssets>
                    </PackageReference>
                    <PackageReference Include="PREF2" Version="1.0.5-beta">
                        <ExcludeAssets>ass1; asset2; contentfiles</ExcludeAssets>
                    </PackageReference>
                    <PackageReference Include="PREF3" Version="1.0.3-alpha">
                        <PrivateAssets>ass1; asset2; contentfiles</PrivateAssets>
                    </PackageReference>
                    <PackageReference Include="PREF4" Version="31.0.8874">
                        <IncludeAssets>ass1; asset2; contentfiles</IncludeAssets>
                        <ExcludeAssets>all</ExcludeAssets>
                        <PrivateAssets>ass1; asset2; contentfiles</PrivateAssets>
                    </PackageReference>
                </ItemGroup>
            </Project>
            """;

            TestProject project = new TestProject()
            {
                Packages =
                {
                    new PackageReference
                    {
                        Name = "PREF1",
                        Version = PackageVersion.Parse("1.0.0"),
                        IncludeAssets = {"ass1", "asset2", "contentfiles"}
                    },
                    new PackageReference
                    {
                        Name = "PREF2",
                        Version = PackageVersion.Parse("1.0.5-beta"),
                        ExcludeAssets = {"ass1", "asset2", "contentfiles"}
                    },
                    new PackageReference
                    {
                        Name = "PREF3",
                        Version = PackageVersion.Parse("1.0.3-alpha"),
                        PrivateAssets = {"ass1", "asset2", "contentfiles"}
                    },
                    new PackageReference
                    {
                        Name = "PREF4",
                        Version = PackageVersion.Parse("31.0.8874"),
                        IncludeAssets = {"ass1", "asset2", "contentfiles"},
                        ExcludeAssets = {"all"},
                        PrivateAssets = {"ass1", "asset2", "contentfiles"}
                    }
                }
            };

            StringWriter output = new StringWriter();
            using (CsProjWriter writer = new CsProjWriter(output))
            {
                writer.Write(project);
            }

            output.ToString().Should().Be(Expected);
        }
    }
}
