using dnlib.DotNet;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public sealed class BasicActTemplate : IActTemplate
    {
        private static readonly string[] _namespaces = new[] { "System" };

        public static readonly BasicActTemplate Instance = new BasicActTemplate();

        public void WriteAct(IWriter output, IMethod method, string? instanceSymbol, string? returnSymbol = null)
        {
            if (method.MethodSig.RetType != null)
            {
                output.Write(method.MethodSig.RetType);
                output.Write($"{returnSymbol} = ");
            }

            WriteInvocation(output, method, instanceSymbol);

            output.WriteLine(";");
        }

        public void WriteActDelegate(IWriter output, IMethod method, string? instanceSymbol, string? returnSymbol = null, string delegateSymbol = "act")
        {
            if (method.MethodSig.RetType != null)
            {
                output.Write("Func<");
                output.Write(method.MethodSig.RetType);
                output.Write($"> {delegateSymbol} = () => ");
            }
            else
            {
                output.Write($"Action {delegateSymbol} = () => ");
            }
            
            WriteInvocation(output, method, instanceSymbol);

            output.WriteLine(";");
        }

        private static void WriteInvocation(IWriter output, IMethod method, string? instanceSymbol)
        {
            MethodDef md = method.ResolveMethodDefThrow();

            string[] argumentSymbols = md.Parameters
                .Select(static p => p.Name)
                .ToArray();

            if (md.IsStatic)
            {
                output.Write(md.DeclaringType.ToTypeSig());
                output.Write($".{method.Name}(");
                output.Write(string.Join(TemplateUtils.ComaSpace, argumentSymbols));
                output.Write(")");
            }
            else
            {
                System.Diagnostics.Debug.Assert(instanceSymbol != null, "Writing non static function, instance symbol must be specified.");
                output.Write($"{instanceSymbol}.{method.Name}(");
                output.Write(string.Join(TemplateUtils.ComaSpace, argumentSymbols));
                output.Write(")");
            }
        }

        public IEnumerable<string> Namespaces => _namespaces;
    }
}
