using dnlib.DotNet;

using dnWalker.TypeSystem;

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
            if (method.HasReturnValue())
            {
                output.WriteNameOrAlias(method.MethodSig.RetType);
                output.Write($" {returnSymbol} = ");
            }

            WriteInvocation(output, method, instanceSymbol);

            output.WriteLine(";");
        }

        public void WriteActDelegate(IWriter output, IMethod method, string? instanceSymbol, string? returnSymbol = null, string delegateSymbol = "act")
        {
            if (method.HasReturnValue())
            {
                output.Write("Func<");
                output.WriteNameOrAlias(method.MethodSig.RetType);
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


            if (md.IsStatic)
            {
                string[] argumentSymbols = md.Parameters
                    .Select(static p => p.Name)
                    .ToArray();

                output.WriteNameOrAlias(md.DeclaringType.ToTypeSig());
                output.Write($".{method.Name}(");
                output.Write(string.Join(TemplateUtils.ComaSpace, argumentSymbols));
                output.Write(")");
            }
            else
            {
                System.Diagnostics.Debug.Assert(instanceSymbol != null, "Writing non static function, instance symbol must be specified.");

                string[] argumentSymbols = md.Parameters
                    .Skip(1)
                    .Select(static p => p.Name)
                    .ToArray();

                output.Write($"{instanceSymbol}.{method.Name}(");
                output.Write(string.Join(TemplateUtils.ComaSpace, argumentSymbols));
                output.Write(")");
            }
        }

        public IEnumerable<string> Namespaces => _namespaces;
    }
}
