using dnlib.DotNet;

using dnWalker.TestWriter.Generators;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Utils;
using dnWalker.TypeSystem;

using System.Text;

namespace dnWalker.TestWriter.Moq
{
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
                    .Where(p => p.IsNormalMethodParameter)
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
                    .Where(p => p.IsNormalMethodParameter)
                    .Select(p => $"It.IsAny<{GetTypeName(ms.GenericInstMethodSig, p.Type)}>()")));

                sb.Append(')');
                return sb.ToString();

            }
            throw new InvalidOperationException("Method should be method def or method spec.");

            static string GetTypeName(GenericInstMethodSig ms, TypeSig typeSig) 
            {
                if (typeSig.IsGenericMethodParameter) 
                {
                    typeSig = ms.GenericArguments[(int)typeSig.ToGenericMVar().Number];
                }
                return typeSig.GetNameOrAlias();
            }
        }

        public bool TryWriteArrangeCreateInstance(ITestContext testContext, IWriter output, string symbol)
        {
            SymbolContext symbolContext = testContext.SymbolMapping[symbol];

            if (symbolContext.MembersToArrange.Any(CanArrange))
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
        public bool TryWriteArrangeInitializeMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, params string[] literals)
        {
            if (CanArrange(method)) 
            {
                string mockSymbol = GetMockSymbol(symbol);
                string invokeExpression = GetInvokeExpression(method);

                if (literals.Length == 1)
                {
                    // mock
                    //     .Setup(o => o.Foo(It.IsAny<Arg>()...)
                    //     .Returns(retValue);


                    // do not setup sequence
                    output.WriteLine($"{mockSymbol}");
                    output.Indent++;

                    output.WriteLine($".Setup({invokeExpression})");
                    output.WriteLine($".Returns{literals[0]};");

                    output.Indent--;
                }
                else if (literals.Length > 1)
                {
                    // mock
                    //     .SetupSequence(o => o.Foo(It.IsAny<Arg>()...)
                    //     .Returns(retValue1)
                    //     .Returns(retValue2)
                    //     ....
                    //     .Returns(retValueN);


                    // do not setup sequence
                    output.WriteLine($"{mockSymbol}");
                    output.Indent++;

                    output.Write($".SetupSequence({invokeExpression})");

                    foreach (string literal in literals ) 
                    {
                        output.WriteLine(string.Empty);
                        output.Write($".Returns({literal})");
                    }

                    output.WriteLine(";");

                    output.Indent--;
                }

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


        public bool TryWriteArrangeInitializeStaticMethod(ITestContext testContext, IWriter output, IMethod method, params string[] literals)
        {
            return false;
        }

        public bool TryWriteArrangeInitializeArrayElement(ITestContext testContext, IWriter output, string symbol, int index, string literal)
        {
            return false;
        }
    }
}