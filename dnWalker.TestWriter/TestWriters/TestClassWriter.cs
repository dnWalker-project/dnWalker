using dnlib.DotNet;

using dnWalker.TestWriter.TestModels;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.TestWriters
{
    public class TestClassWriter : ITestClassWriter, IDisposable
    {
        private readonly ref struct Scope //: IDisposable
        {
            public static Scope NoScope() => new Scope();

            private readonly TestClassWriter? _writer;

            public Scope(TestClassWriter writer)
            {
                _writer = writer;
            }

            public void Dispose()
            {
                _writer?.EndScope();
            }
        }

        public const string TabString = "    ";

        // bad way... why does not the indentedtextwriter dispose the inner writer???
        private readonly TextWriter _realOutput;
        private readonly IndentedTextWriter _output;

        public TestClassWriter(string filePath, string tabString = TabString) : this(new StreamWriter(filePath), tabString)
        {
        }
        public TestClassWriter(TextWriter output, string tabString = TabString)
        {
            _output = new IndentedTextWriter(output, tabString);
            _realOutput = output;
        }

        public void Write(TestClass testClass)
        {
            // write usings
            WriteUsings(testClass.Usings);

            // write namespace
            using (var nsScope = WriteNamespace(testClass.Namespace))
            {
                _output.WriteLine();

                foreach (var attInfo in testClass.Attributes)
                {
                    WriteAttributeInfo(attInfo);
                }
                using (var classScope = WriteClass(testClass.Name))
                {
                    _output.WriteLine();

                    foreach (var method in testClass.Methods)
                    {
                        WriteMethod(method);
                    }
                }
            }
        }

        private void WriteMethod(TestMethod method)
        {
            foreach (var attInfo in method.Attributes)
            {
                WriteAttributeInfo(attInfo);
            }

            using (var scope = WriteMethodDefinition(method))
            {
                var body = method.Body;
                if (!string.IsNullOrWhiteSpace(body))
                {
                    foreach (var line in body.Split(_output.NewLine))
                    {
                        _output.WriteLine(line);
                    }
                }
            }
        }

        private Scope WriteMethodDefinition(TestMethod method)
        {
            if (string.IsNullOrWhiteSpace(method.Name))
            {
                return Scope.NoScope();
            }

            var returnType = method.ReturnTypeName ?? "void";

            _output.Write($"public {returnType} {method.Name!}");
            _output.Write('(');

            _output.Write(string.Join(", ", method.Arguments.Select(arg => $"{arg.TypeName} {arg.Name}")));

            _output.WriteLine(')');
            return BeginScope();
        }

        private Scope WriteClass(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Scope.NoScope();
            }

            _output.WriteLine($"public class {name}");

            return BeginScope();
        }

        private void WriteAttributeInfo(AttributeInfo attInfo)
        {
            _output.Write('[');
            _output.Write(attInfo.TypeName);
            if (attInfo.PositionalArguments.Count > 0 ||
                attInfo.InitializerArguments.Count > 0)
            {
                _output.Write('(');

                _output.Write(string.Join(", ", attInfo.PositionalArguments));

                if (attInfo.PositionalArguments.Count > 0 &&
                    attInfo.InitializerArguments.Count > 0)
                {
                    _output.Write(", ");
                }

                _output.Write(string.Join(", ", attInfo.InitializerArguments.Select(p => $"{p.Key} = {p.Value}")));

                _output.Write(')');
            }
            _output.WriteLine(']');
        }

        private Scope WriteNamespace(string? namespaceName)
        {
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                return Scope.NoScope();
            }

            _output.WriteLine($"namespace {namespaceName}");
            return BeginScope();
        }

        private void WriteUsings(IEnumerable<string> usings)
        {
            var groups = usings.GroupBy(s => GetFirstMember(s)).ToArray();
            Array.Sort(groups, (a, b) => StringComparer.OrdinalIgnoreCase.Compare(a.Key, b.Key));

            foreach (var group in groups)
            {
                WriteUsingsGroup(group, _output);
                _output.WriteLine();
            }

            static void WriteUsingsGroup(IEnumerable<string> usings, TextWriter writer)
            {
                var sorted = usings.ToArray();
                Array.Sort(sorted, StringComparer.OrdinalIgnoreCase);

                foreach (var s in sorted)
                {
                    writer.WriteLine($"using {s};");
                }
            }

            static string GetFirstMember(string ns)
            {
                var idx = ns.IndexOf('.');
                if (idx < 0) return ns;
                return ns.Substring(0, idx);
            }
        }

        private Scope BeginScope()
        {
            _output.WriteLine('{');
            _output.Indent++;

            return new Scope(this);
        }

        private void EndScope()
        {
            _output.Indent--;
            _output.WriteLine('}');
        }

        public void Dispose()
        {
            _output.Dispose();
            _realOutput.Dispose();
        }
    }
}
