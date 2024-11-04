using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.TestWriters;

using FluentAssertions;

using Xunit.Abstractions;

namespace dnWalker.TestWriter.Tests.TestWriters
{
    public class TestClassWriterTests : TestWriterTestBase
    {
        public TestClassWriterTests(ITestOutputHelper textOutput) : base(textOutput)
        {
        }

        [Fact]
        public void WriteUsingsOnly()
        {
            const string Expected =
            """
            using A.aabb;

            using aa.aabb;
            
            using aaa.aabb;
            
            using bbb.ccc;
            using bbb.ccc.ddd;
            
            using e;
            
            using F;
            using F.G;
            
            using XXX.YYY;
            """;

            var testClass = new TestClass()
            {
                Usings = { "A.aabb", "XXX.YYY", "e", "F.G", "F", "aaa.aabb", "aa.aabb", "bbb.ccc.ddd", "bbb.ccc" },
                //Namespace = "TestNamespace",
                //Name = "TestClass"
            };

            var output = new StringWriter();

            var writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void WriteNoUsingsClassAttributes()
        {
            const string Expected =
            """
            namespace TestNamespace
            {
                
                [Fixture(5, Message = "hello world")]
                public class TestClass
                {
                    
                }
            }
            """;

            var testClass = new TestClass()
            {
                Usings = { },
                Attributes = { new AttributeInfo
                {
                    TypeName = "Fixture",
                    PositionalArguments = { "5" },
                    InitializerArguments = { { "Message", "\"hello world\"" } }
                } },
                Namespace = "TestNamespace",
                Name = "TestClass"
            };

            var output = new StringWriter();

            var writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Should().Be(Expected.Replace("\r\n", Environment.NewLine));
        }

        [Fact]
        public void WriteClassWithSingleMethodNoUsings()
        {
            const string Expected =
            """
            namespace TestNamespace
            {
                
                [FixtureNoArgs]
                [Fixture(5, Message = "hello world")]
                public class TestClass
                {
                    
                    [Test("check this out", 5)]
                    public void TestMethod(int x)
                    {
                        int y = 10;
                        Assert.Equals(x * 2, y);
                    }
                }
            }
            """;

            var testClass = new TestClass()
            {
                Usings = { },
                Namespace = "TestNamespace",
                Attributes =
                {
                    new AttributeInfo
                    {
                        TypeName = "FixtureNoArgs",
                    },
                    new AttributeInfo
                    {
                        TypeName = "Fixture",
                        PositionalArguments = { "5" },
                        InitializerArguments = { { "Message", "\"hello world\"" } }
                    }
                },
                Name = "TestClass",
                Methods =
                {
                    new TestMethod
                    {
                        Attributes =
                        {
                            new AttributeInfo
                            {
                                TypeName = "Test",
                                PositionalArguments = { "\"check this out\"", "5" }
                            }
                        },
                        ReturnTypeName = "void",
                        Name = "TestMethod",
                        Arguments= { new ArgumentInfo { TypeName = "int", Name = "x" } },
                        Body =
                        """
                        int y = 10;
                        Assert.Equals(x * 2, y);
                        """
                    }
                }
            };

            var output = new StringWriter();

            var writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Replace("\r\n", Environment.NewLine).Should().Be(Expected.Trim().Replace("\r\n", Environment.NewLine));
        }
    }
}