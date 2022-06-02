using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestGenerator.Symbols;

namespace dnWalker.TestGenerator.Templates.Moq
{
    public class MoqTemplate : ArrangeTemplateBase
    {
        private static readonly string[] _namespaces = new[] { "Moq" }; 


        public override IEnumerable<string> Namespaces => _namespaces;

        protected override void WriteObjectCreation(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames)
        {
            // IMyInterface obj = new Mock<IMyInterface>().Object;
            TypeSig type = objectNode.Type;
            TypeDef td = type.ToTypeDefOrRef().ResolveTypeDefThrow();
            // we assume that td is NOT STATIC class (e.i. Sealed & Abstract
            if ((!td.IsAbstract && !td.IsInterface && !objectNode.HasMethodInvocations) || td.IsSealed)
            {
                base.WriteObjectCreation(output, objectNode, locationNames);
            }
            else
            {
                // create the object using Mock
                output.Write("new Mock<");
                output.Write(type);
                output.WriteLine(">().Object;");
            }
        }

        protected override void WriteObjectMethodsInitialization(IWriter output, IReadOnlyObjectHeapNode objectNode, IDictionary<Location, string> locationNames)
        {
            TypeSig type = objectNode.Type;

            // for greater clarity, setup new scope
            output.WriteLine("{");
            output.PushIndent(TemplateUtils.Indent);


            output.Write("Mock<");
            output.Write(type);
            output.WriteLine($"> mock = Mock.Get({GetName(objectNode, locationNames)});");

            foreach ((IMethod method, IValue[] results) in objectNode.GetMethodResults())
            {
                output.Write($"mock.SetupSequence(static o => o.{method.Name}(");

                if (method.HasParams())
                {
                    IList<TypeSig> p = method.GetParams();
                    WritePlaceholder(p[0]);

                    for (int i = 1; i< p.Count; ++i)
                    {
                        output.Write(TemplateUtils.ComaSpace);
                        WritePlaceholder(p[i]);
                    }
                }

                output.WriteLine("))");

                output.PushIndent(TemplateUtils.Indent);

                for (int i = 0; i < results.Length - 1; ++i)
                {
                    output.WriteLine($".Returns({GetLiteral(results[i], locationNames)})");
                }

                output.WriteLine($".Returns({GetLiteral(results[^1],locationNames)});");

                output.PopIndent();
            }

            output.PopIndent();
            output.WriteLine("}");

            void WritePlaceholder(TypeSig type)
            {
                output.Write("It.IsAny<");
                output.Write(type);
                output.Write(">()");
            }
        }
    }
}