using FluentAssertions;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests
{
    public class CodeWriterTests
    {
        [Fact]
        public void WriteUsing()
        {
            string namespaceName = "System";
            //string expectedUsingString = $"using {namespaceName};\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteUsing(namespaceName);

            writer.ToString().Should().Be(CodeTexts.Using_System);
        }


        [Fact]
        public void WriteStatement()
        {
            string statement = "x = 5";
            //string expectedStatement = $"{statement};\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteStatement(statement);

            writer.ToString().Should().Be(CodeTexts.Assign_5_To_X);
        }

        [Fact]
        public void WriteVariableDeclaration_WithInitialValue()
        {
            string variableType = "int";
            string variableName = "x";
            string variableValue = "5";

            //string expected = $"{variableType} {variableName} = {variableValue};\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteVariableDeclaration(variableType, variableName, variableValue);

            writer.ToString().Should().Be(CodeTexts.Declare_X_WithInitialValue);
        }

        [Fact]
        public void WriteVariableDeclaration_WithoutInitialValue()
        {
            string variableType = "int";
            string variableName = "x";

            string expected = $"{variableType} {variableName};\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteVariableDeclaration(variableType, variableName);

            writer.ToString().Should().Be(CodeTexts.Declare_X_WithoutInitialValue);
        }

        [Fact]
        public void Test_DeclareEmptyClass()
        {
            string className = "MyClass";
            //string expectedText = $"public class {className}\r\n{{\r\n}}\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            using (IDisposable codeBlock = codeWriter.BeginClass(MemberModifiers.None, className))
            { }

            writer.ToString().Should().Be(CodeTexts.Declare_Empty_Class);
        }

        [Fact]
        public void Test_DeclareEmptyClass_WithMethod()
        {
            string className = "MyClass";
            string methodName = "MyMethod";
            //string expectedText = $"public class {className}\r\n{{\r\n}}\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            using (IDisposable classBlock = codeWriter.BeginClass(MemberModifiers.None, className))
            { 
                using (IDisposable methodBlock = codeWriter.BeginMethod(MemberModifiers.None, null, methodName))
                { }
            }

            writer.ToString().Should().Be(CodeTexts.Declare_Class_WithMethod);
        }

        [Fact]
        public void Test_DeclareClass_WithMethod_WithVariable()
        {
            string className = "MyClass";
            string methodName = "MyMethod";
            //string expectedText = $"public class {className}\r\n{{\r\n}}\r\n";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            using (IDisposable classBlock = codeWriter.BeginClass(MemberModifiers.None, className))
            {
                using (IDisposable methodBlock = codeWriter.BeginMethod(MemberModifiers.None, null, methodName))
                {
                    codeWriter.WriteVariableDeclaration("int", "x", "5");
                }
            }

            writer.ToString().Should().Be(CodeTexts.Declare_Class_WithMethod_WithVariable);
        }

        [Fact]
        public void Test_WriteAttribute()
        {
            string attributeName = "MyAttribute";

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteAttribute(attributeName);


            writer.ToString().Should().Be(CodeTexts.WriteAttribute);
        }

        [Fact]
        public void Test_WriteAttribute_WithArguments()
        {
            string attributeName = "MyAttribute";
            string[] arguments = { "true", "5" };

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteAttribute(attributeName, arguments);


            writer.ToString().Should().Be(CodeTexts.WriteAttribute_WithArguments);
        }

        [Fact]
        public void Test_WriteAttribute_WithArgumentsAndProperties()
        {
            string attributeName = "MyAttribute";
            string[] arguments = { "true", "5" };

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteAttribute(attributeName, arguments);


            writer.ToString().Should().Be(CodeTexts.WriteAttribute_WithArguments);
        }

        [Fact]
        public void Test_WriteAttribute_WithProperties()
        {
            string attributeName = "MyAttribute";
            string[] propertyNames = { "SomeBooleanProperty", "SomeIntProperty" };
            string[] propertyValues = { "false", "10" };

            StringWriter writer = new StringWriter();

            CodeWriter codeWriter = new CodeWriter(writer);
            codeWriter.WriteAttribute(attributeName, propertyNames, propertyValues);


            writer.ToString().Should().Be(CodeTexts.WriteAttribute_WithParameters);
        }
    }
}
