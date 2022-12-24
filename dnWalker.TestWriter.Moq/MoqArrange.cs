using dnlib.DotNet;

using dnWalker.Symbolic.Expressions;
using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Utils;
using dnWalker.TypeSystem;

using System.Text;

namespace dnWalker.TestWriter.Moq
{
    [AddPackage("Moq", Version = "4.18.3")]
    [AddNamespace("Moq")]
    public class MoqArrange : IArrangePrimitives
    {
        private static bool CanArrange(IMemberRef memberRef)
        {
            if (memberRef is IMethod m)
            {
                MethodDef md = m.ResolveMethodDefThrow();


                return md.DeclaringType.IsInterface ||
                       md.IsAbstract ||
                       md.IsVirtual;
            }

            return false;
        }

        private static string GetMockSymbol(string symbol)
        {
            return $"{symbol}_mock";
        }

        private static string GetMockType(string type)
        {
            return $"Mock<{type}>";
        }

        private static string GetInvokeExpression(IMethod method)
        {
            if (method is IMethodDefOrRef defOrRef) 
            {
                method = defOrRef.ResolveMethodDefThrow();
            }

            if (method is MethodDef md)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"o => o.{method.Name}(");

                sb.Append(string.Join(", ", md.Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => $"It.IsAny<{p.Type.GetNameOrAlias()}>()")));

                sb.Append(')');
                return sb.ToString();
            }
            else if (method is MethodSpec ms)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"o => o.{method.Name}");

                sb.Append($"<{string.Join(", ", ms.GetGenericParameters().Select(t => t.GetNameOrAlias()))}>");
                sb.Append('(');

                sb.Append(string.Join(", ", ms.Method.ResolveMethodDefThrow().Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => $"It.IsAny<{GetTypeName(ms.GenericInstMethodSig, p.Type)}>()")));

                sb.Append(')');
                return sb.ToString();

            }
            throw new InvalidOperationException("Method should be method def or method spec.");
        }

        static string GetTypeName(GenericInstMethodSig ms, TypeSig typeSig)
        {
            if (typeSig.IsGenericMethodParameter)
            {
                typeSig = ms.GenericArguments[(int)typeSig.ToGenericMVar().Number];
            }
            return typeSig.GetNameOrAlias();
        }

        private static string GetReturnsExpression(IMethod method)
        {
            MethodDef md = method.ResolveMethodDefThrow();

            if (method is MethodSpec ms)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Join(", ", md.Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => $"{GetTypeName(ms.GenericInstMethodSig, p.Type)}")));
                sb.Append(">((");

                sb.Append(string.Join(", ", md.Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => p.Name)));

                sb.Append(") =>");

                return sb.ToString();
            }
            else //if (method is MethodDef md)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Join(", ", md.Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => $"{p.Type.GetNameOrAlias()}")));
                sb.Append(">((");

                sb.Append(string.Join(", ", md.Parameters
                    .Where(p => !p.IsHiddenThisParameter && !p.IsReturnTypeParameter)
                    .Select(p => p.Name)));

                sb.Append(") =>");

                return sb.ToString();
            }


            throw new InvalidOperationException("Method should be method def or method spec.");
        }


        public bool TryWriteArrangeCreateInstance(ITestContext testContext, IWriter output, string symbol)
        {
            SymbolContext symbolContext = testContext.SymbolMapping[symbol];
            TypeDef td = symbolContext.Type.ToTypeDefOrRef().ResolveTypeDefThrow();

            if (symbolContext.MembersToArrange.Any(CanArrange) ||
                td.IsAbstract ||
                td.IsInterface)
            {
                string mockSymbol = GetMockSymbol(symbol);
                string type = symbolContext.Type.GetNameOrAlias();
                string mockType = GetMockType(type);

                output.WriteLine($"{mockType} {mockSymbol} = new {mockType}();");
                output.WriteLine($"{type} {symbol} = {mockSymbol}.Object;");

                return true;
            }

            return false;
        }

        private static string HandleNull(TypeSig returnType, string literal)
        {
            if (literal == "null")
            {
                return $"({returnType.GetNameOrAlias()})null";
            }
            else
            {
                return literal;
            }
        }

        public bool TryWriteArrangeInitializeMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<string> literals)
        {
            if (CanArrange(method)) 
            {
                string mockSymbol = GetMockSymbol(symbol);
                string invokeExpression = GetInvokeExpression(method);

                if (literals.Count == 1)
                {
                    // mock
                    //     .Setup(o => o.Foo(It.IsAny<Arg>()...)
                    //     .Returns(retValue);


                    // do not setup sequence
                    output.WriteLine($"{mockSymbol}");
                    output.Indent++;

                    output.WriteLine($".Setup({invokeExpression})");
                    output.WriteLine($".Returns({HandleNull(method.MethodSig.RetType, literals[0])});");

                    output.Indent--;
                }
                else if (literals.Count > 1)
                {
                    // mock
                    //     .SetupSequence(o => o.Foo(It.IsAny<Arg>()...)
                    //     .Returns(retValue1)
                    //     .Returns(retValue2)
                    //     ....
                    //     .Returns(retValueN);


                    output.WriteLine($"{mockSymbol}");
                    output.Indent++;

                    output.Write($".SetupSequence({invokeExpression})");

                    foreach (string literal in literals ) 
                    {
                        output.WriteLine(string.Empty);
                        output.Write($".Returns({HandleNull(method.MethodSig.RetType, literal)})");
                    }

                    output.WriteLine(";");

                    output.Indent--;
                }

                return true;
            }
            return false;
        }

        public bool TryWriteArrangeInitializeConstrainedMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<KeyValuePair<Expression, string>> constrainedLiterals, string fallbackLiteral)
        {
            // mock.Setup(o => o.Foo(It.IsAny<T1>() ... ))
            //     .Returns<T1, ...>((a1, ... ) =>
            //     {
            //         if (pred_1) return ...;
            //         else if (pred_2) return ...;
            //         ...
            //         else return 0;
            //     });

            if (CanArrange(method))
            {
                string mockSymbol = GetMockSymbol(symbol);
                string invokeExpression = GetInvokeExpression(method);

                output.WriteLine($"{mockSymbol}");
                output.Indent++;
                output.WriteLine($".Setup({invokeExpression})");
                output.WriteLine($".Returns<{GetReturnsExpression(method)}"); // .Returns<T1...>((arg1,... arg2) => 
                output.WriteLine("{");
                output.Indent++;
                foreach((Expression e, string literal) in constrainedLiterals) 
                {
                    string predicate = CSharpExpressionWriter.GetCSharpExpression(e);
                    output.WriteLine($"if ({predicate})");
                    output.WriteLine("{");
                    output.Indent++;
                    output.WriteLine($"return {literal};");
                    output.Indent--;
                    output.WriteLine("}");
                }
                output.WriteLine("else");
                output.WriteLine("{");
                output.Indent++;
                output.WriteLine($"return {fallbackLiteral};");
                output.Indent--;
                output.WriteLine("}");
                output.Indent--;
                output.WriteLine("});");
                output.Indent--;

                return true;
            }

            return false;
        }

        public bool TryWriteArrangeInitializeField(ITestContext testContext, IWriter output, string symbol, IField field, string literal)
        {
            return false;
        }

        public bool TryWriteArrangeInitializeStaticField(ITestContext testContext, IWriter output, IField field, string literal)
        {
            return false;
        }


        public bool TryWriteArrangeInitializeStaticMethod(ITestContext testContext, IWriter output, IMethod method, IReadOnlyList<string> literals)
        {
            return false;
        }

        public bool TryWriteArrangeInitializeArrayElement(ITestContext testContext, IWriter output, string symbol, int index, string literal)
        {
            return false;
        }

        private static readonly string[] _ns = new[] { "Moq" };

        public IEnumerable<string> Namespaces => _ns;
    }
}