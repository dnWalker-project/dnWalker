using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.Symbolic.Variables;
using dnWalker.TestGenerator.TestClasses;
using dnWalker.TestGenerator.TestClasses.Schemas;

using FluentAssertions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace dnWalker.TestGenerator.Tests.TestClasses.Schemas
{
    public class ReturnValueSchemaTests : TestSchemaTestBase
    {
        [Fact]
        public void PrimitiveReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.IntStaticMethod));

            Model inputModel = new Model();

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), ValueFactory.GetValue(5));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, just assert the return value
                "int result = ClassUnderTest.IntStaticMethod();",
                "result.Should().Be(5);"));
        }

        [Fact]
        public void PrimitiveReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.IntInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), ValueFactory.GetValue(5));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, 'this'
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "",
                "int result = objectUnderTest.IntInstanceMethod();",
                "result.Should().Be(5);"));
        }

        [Fact]
        public void StringReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.StringStaticMethod));

            Model inputModel = new Model();

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), new StringValue("Hello world!"));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, just assert the return value
                "string result = ClassUnderTest.StringStaticMethod();",
                "result.Should().Be(\"Hello world!\");"));
        }

        [Fact]
        public void StringReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.StringInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), new StringValue("Hello world!"));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, 'this'
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "",
                "string result = objectUnderTest.StringInstanceMethod();",
                "result.Should().Be(\"Hello world!\");"));
        }

        [Fact]
        public void NullStringReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.StringStaticMethod));

            Model inputModel = new Model();

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), new StringValue(null));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, just assert the return value
                "string result = ClassUnderTest.StringStaticMethod();",
                "result.Should().Be(null);"));
        }

        [Fact]
        public void NullStringReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.StringInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            Model outputModel = inputModel.Clone();
            outputModel.SetValue(new ReturnValueVariable(method), new StringValue(null));

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, 'this'
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "",
                "string result = objectUnderTest.StringInstanceMethod();",
                "result.Should().Be(null);"));
        }

        [Fact]
        public void FreshObjectReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceStaticMethod));

            Model inputModel = new Model();

            Model outputModel = inputModel.Clone();
            IObjectHeapNode resultNode = outputModel.HeapInfo.InitializeObject(DataClassSig);
            resultNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            outputModel.SetValue(new ReturnValueVariable(method), resultNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = null;",
                "",
                "DataClass result = ClassUnderTest.ObjectReferenceStaticMethod(i, refArg);",
                "result.Should().NotBeNull();",
                "result.StringField.Should().Be(\"Hello world!\");"));
        }

        [Fact]
        public void FreshObjectReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            Model outputModel = inputModel.Clone();
            IObjectHeapNode resultNode = outputModel.HeapInfo.InitializeObject(DataClassSig);
            resultNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));
            resultNode.SetField(DataClassTD.FindField("RefField"), instanceNode.Location);
            
            outputModel.SetValue(new ReturnValueVariable(method), resultNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                // no arrange, 'this'
                "// Arrange input model heap",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = null;",
                "",
                "DataClass result = objectUnderTest.ObjectReferenceInstanceMethod(i, refArg);",
                "result.Should().NotBeNull();",
                "result.StringField.Should().Be(\"Hello world!\");",
                "result.RefField.Should().BeSameAs(classUnderTest1);"));
        }

        [Fact]
        public void InputObjectReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceStaticMethod));

            Model inputModel = new Model();
            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[1]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), refArgsNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "",
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "",
                "DataClass result = ClassUnderTest.ObjectReferenceStaticMethod(i, refArg);",
                "result.Should().NotBeNull();",
                "result.Should().BeSameAs(dataClass1);",
                "result.StringField.Should().Be(\"Hello world!\");"));
        }

        [Fact]
        public void InputObjectReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[2]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), refArgsNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "",
                "DataClass result = objectUnderTest.ObjectReferenceInstanceMethod(i, refArg);",
                "result.Should().NotBeNull();",
                "result.Should().BeSameAs(dataClass1);",
                "result.StringField.Should().Be(\"Hello world!\");"));
        }

        [Fact]
        public void NullObjectReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceStaticMethod));

            Model inputModel = new Model();
            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[1]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), Location.Null);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "",
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "",
                "DataClass result = ClassUnderTest.ObjectReferenceStaticMethod(i, refArg);",
                "result.Should().BeNull();"));
        }

        [Fact]
        public void NullObjectReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ObjectReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[2]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), Location.Null);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "",
                "DataClass result = objectUnderTest.ObjectReferenceInstanceMethod(i, refArg);",
                "result.Should().BeNull();"));
        }

        [Fact]
        public void FreshArrayReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceStaticMethod));

            Model inputModel = new Model();

            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[1]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            IArrayHeapNode arrayNode = outputModel.HeapInfo.InitializeArray(DataClassSig, 4);
            arrayNode.SetElement(2, refArgsNode.Location);

            outputModel.SetValue(new ReturnValueVariable(method), arrayNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "",
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "DataClass[] arrArg = null;",
                "",
                "DataClass[] result = ClassUnderTest.ArrayReferenceStaticMethod(i, refArg, arrArg);",
                "result.Should().NotBeNull();",
                "result.Should().HaveLength(4);",
                "result[2].Should().BeSameAs(dataClass1);"));
        }

        [Fact]
        public void FreshArrayReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            IObjectHeapNode refArgsNode = inputModel.HeapInfo.InitializeObject(DataClassSig);
            refArgsNode.SetField(DataClassTD.FindField("StringField"), new StringValue("Hello world!"));

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[2]), refArgsNode.Location);

            Model outputModel = inputModel.Clone();

            IArrayHeapNode arrayNode = outputModel.HeapInfo.InitializeArray(DataClassSig, 4);
            arrayNode.SetElement(2, refArgsNode.Location);

            outputModel.SetValue(new ReturnValueVariable(method), arrayNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass dataClass1 = new DataClass();",
                "dataClass1.StringField = \"Hello world!\";",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = dataClass1;",
                "DataClass[] arrArg = null;",
                "",
                "DataClass[] result = objectUnderTest.ArrayReferenceInstanceMethod(i, refArg, arrArg);",
                "result.Should().NotBeNull();",
                "result.Should().HaveLength(4);",
                "result[2].Should().BeSameAs(dataClass1);"));
        }

        [Fact]
        public void InputArrayReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceStaticMethod));

            Model inputModel = new Model();

            IArrayHeapNode refArrNode = inputModel.HeapInfo.InitializeArray(DataClassSig, 3);

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[2]), refArrNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), refArrNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass[] dataClassArray1 = new DataClass[3];",
                "",
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = null;",
                "DataClass[] arrArg = dataClassArray1;",
                "",
                "DataClass[] result = ClassUnderTest.ArrayReferenceStaticMethod(i, refArg, arrArg);",
                "result.Should().NotBeNull();",
                "result.Should().BeSameAs(dataClassArray1);"));
        }

        [Fact]
        public void InputArrayReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            IArrayHeapNode refArrNode = inputModel.HeapInfo.InitializeArray(DataClassSig, 3);
            
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[3]), refArrNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), refArrNode.Location);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass[] dataClassArray1 = new DataClass[3];",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = null;",
                "DataClass[] arrArg = dataClassArray1;",
                "",
                "DataClass[] result = objectUnderTest.ArrayReferenceInstanceMethod(i, refArg, arrArg);",
                "result.Should().NotBeNull();",
                "result.Should().BeSameAs(dataClassArray1);"));
        }

        [Fact]
        public void NullArrayReferenceReturnValueStatic()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceStaticMethod));

            Model inputModel = new Model();
            IArrayHeapNode refArrNode = inputModel.HeapInfo.InitializeArray(DataClassSig, 3);

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[2]), refArrNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), Location.Null);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass[] dataClassArray1 = new DataClass[3];",
                "",
                "// Arrange method arguments",
                "int i = 0;",
                "DataClass refArg = null;",
                "DataClass[] arrArg = dataClassArray1;",
                "",
                "DataClass[] result = ClassUnderTest.ArrayReferenceStaticMethod(i, refArg, arrArg);",
                "result.Should().BeNull();"));
        }

        [Fact]
        public void NullArrayReferenceReturnValueInstance()
        {
            IMethod method = GetMethod(nameof(ClassUnderTest.ArrayReferenceInstanceMethod));

            Model inputModel = new Model();
            IHeapNode instanceNode = inputModel.HeapInfo.InitializeObject(TestClassSig);
            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[0]), instanceNode.Location);

            IArrayHeapNode refArrNode = inputModel.HeapInfo.InitializeArray(DataClassSig, 3);

            inputModel.SetValue(new MethodArgumentVariable(method.ResolveMethodDefThrow().Parameters[3]), refArrNode.Location);

            Model outputModel = inputModel.Clone();

            outputModel.SetValue(new ReturnValueVariable(method), Location.Null);

            ITestClassContext context = new TestClassContext.Builder()
            {
                Exception = null,
                InputModel = inputModel,
                OutputModel = outputModel,
                AssemblyFileName = "dnWalker.TestGenerator.Tests",
                AssemblyName = "dnWalker.TestGenerator.Tests",
                ErrorOutput = "",
                IterationNumber = 1,
                PathConstraint = "",
                StandardOutput = "",
                Method = method,
            }.Build();

            GenerateTestClass(new ReturnValueSchema(context)).Should().Be(string.Join(Environment.NewLine,
                "// Arrange input model heap",
                "DataClass[] dataClassArray1 = new DataClass[3];",
                "ClassUnderTest classUnderTest1 = new ClassUnderTest();",
                "",
                "// Arrange method arguments",
                "ClassUnderTest objectUnderTest = classUnderTest1;",
                "int i = 0;",
                "DataClass refArg = null;",
                "DataClass[] arrArg = dataClassArray1;",
                "",
                "DataClass[] result = objectUnderTest.ArrayReferenceInstanceMethod(i, refArg, arrArg);",
                "result.Should().BeNull();"));
        }
    }
}
