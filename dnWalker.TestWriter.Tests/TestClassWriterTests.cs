using dnWalker.TestWriter.TestModels;
using dnWalker.TestWriter.TestWriters;

using FluentAssertions;

namespace dnWalker.TestWriter.Tests
{
    public class TestClassWriterTests
    {

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

            TestClass testClass = new TestClass()
            {
                Usings = { "A.aabb", "XXX.YYY", "e", "F.G", "F", "aaa.aabb", "aa.aabb", "bbb.ccc.ddd", "bbb.ccc" },
                //Namespace = "TestNamespace",
                //Name = "TestClass"
            };

            StringWriter output = new StringWriter();

            TestClassWriter writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Should().Be(Expected);
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

            TestClass testClass = new TestClass()
            {
                Usings = {  },
                Attributes = { new AttributeInfo
                {
                    TypeName = "Fixture",
                    PositionalArguments = { "5" },
                    InitializerArguments = { { "Message", "\"hello world\"" } }
                } },
                Namespace = "TestNamespace",
                Name = "TestClass"
            };

            StringWriter output = new StringWriter();

            TestClassWriter writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Should().Be(Expected);
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

            TestClass testClass = new TestClass()
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

            StringWriter output = new StringWriter();

            TestClassWriter writer = new TestClassWriter(output);
            writer.Write(testClass);

            output.ToString().Trim().Should().Be(Expected);
        }
    }
}