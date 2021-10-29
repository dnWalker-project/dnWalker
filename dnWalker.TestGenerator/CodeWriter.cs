using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator
{
    [Flags]
    public enum MemberModifiers
    {
        None      =  0,
        Public    =  1,
        Internal  =  2,
        Protected =  4,
        Private   =  8,
        Static    = 16,
        Readonly  = 32
    }

    public class CodeWriter : IDisposable
    {
        private struct CodeBlock : IDisposable
        {
            private readonly CodeWriter _codeWriter;

            public CodeBlock(CodeWriter codeWriter)
            {
                _codeWriter = codeWriter;
                _codeWriter.WriteLine("{");
                _codeWriter.Indent();
            }

            public void Dispose()
            {
                _codeWriter.Outdent();
                _codeWriter.WriteLine("}");
            }
        }

        private readonly TextWriter _output;
        private readonly bool _disposeOnClose;

        private int _indent = 0;

        public CodeWriter(TextWriter output, bool disposeOnClose = true)
        {
            _output = output;
            _disposeOnClose = disposeOnClose;
        }

        public void Dispose()
        {
            if (_disposeOnClose)
            {
                _output.Dispose();
            }
        }

        public IDisposable BeginCodeBlock()
        {
            return new CodeBlock(this);
        }

        public void Indent()
        {
            _indent++;
        }

        public void Outdent()
        {
            _indent = Math.Max(_indent - 1, 0);
        }

        public string TabString
        {
            get;
            set;
        } = "    ";

        private void WriteLine(string line)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _indent; ++i)
            {
                builder.Append(TabString);
            }

            builder.Append(line);
            _output.WriteLine(builder);
        }

        public void WriteStatement(string statement)
        {
            WriteLine(statement + ";");
        }

        public void WriteUsing(string namespaceName)
        {
            WriteStatement("using " + namespaceName);
        }

        public void WriteVariableDeclaration(string variableType, string variableName)
        {
            WriteStatement($"{variableType} {variableName}");
        }
        public void WriteVariableDeclaration(string variableType, string variableName, string initialValue)
        {
            WriteStatement($"{variableType} {variableName} = {initialValue}");
        }

        public void WriteMethodInvocation(string methodName, params string[] args)
        {
            string statement = $"{methodName}({string.Join(", ", args)})";
            WriteStatement(statement);
        }

        public void WriteMethodInvocation(string resultVariableName, string methodName, params string[] args)
        {
            string statement =  $"{resultVariableName} = {methodName}({string.Join(", ", args)})";
            WriteStatement(statement);
        }

        public void WriteMethodInvocation(string resultVariableType, string resultVariableName, string methodName, params string[] args)
        {
            string statement = $"{resultVariableType} {resultVariableName} = {methodName}({string.Join(", ", args)})";
            WriteStatement(statement);
        }

        public void WriteEmptyLine()
        {
            _output.WriteLine();
        }

        public void WriteComment(string comment)
        {
            WriteLine("// " + comment);
        }

        public IDisposable BeginClass(MemberModifiers modifiers, string className, params string[] inheritingTypes)
        {
            StringBuilder builder = new StringBuilder();
            
            if (inheritingTypes.Length == 0)
            {
                builder.AppendFormat("public class {0}", className);
            }
            else
            {
                builder.AppendFormat("public class {0}: {1}", className, string.Join(", ", inheritingTypes));
            }

            WriteLine(builder.ToString());
            return BeginCodeBlock();
        }

        public IDisposable BeginMethod(MemberModifiers memberModifiers, string returnType, string methodName)
        {
            WriteLine($"public {returnType ?? "void"} {methodName}()");
            return BeginCodeBlock();
        }

        public IDisposable BeginMethod(MemberModifiers memberModifiers, string returnType, string methodName, string[] argumentTypes, string[] argumentNames)
        {
            IEnumerable<String> paramDeclarations = Enumerable.Range(0, argumentTypes.Length).Select(i => $"{argumentTypes[i]} {argumentNames[i]}");
            WriteLine($"public {returnType ?? "void"} {methodName}({String.Join(", ", paramDeclarations)})");
            return BeginCodeBlock();
        }

        public IDisposable BeginNamespace(string namespaceName)
        {
            WriteLine("namespace " + namespaceName);
            return BeginCodeBlock();
        }

        public void WriteAttribute(string attributeName)
        {
            WriteLine($"[{attributeName}]");
        }

        public void WriteAttribute(string attributeName, params string[] arguments)
        {
            WriteLine($"[{attributeName}({string.Join(", ", arguments)})]");
        }

        public void WriteAttribute(string attributeName, string[] propertyNames, string[] propertyValues)
        {
            IEnumerable<string> propertyDeclarations = Enumerable.Range(0, propertyNames.Length).Select(i => $"{propertyNames[i]} = {propertyValues[i]}");
            WriteLine($"[{attributeName}({string.Join(", ", propertyDeclarations)})]");
        }

        public void WriteAttribute(string attributeName, string[] arguments, string[] propertyNames, string[] propertyValues)
        {
            IEnumerable<string> propertyDeclarations = Enumerable.Range(0, propertyNames.Length).Select(i => $"{propertyNames[i]} = {propertyValues[i]}");
            WriteLine($"[{attributeName}({string.Join(", ", arguments)}, {string.Join(", ", propertyDeclarations)})]");
        }
    }
}
