using dnlib.DotNet;

using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Act
{
    internal class SimpleActWriter : IActPrimitives
    {
        public bool TryWriteAct(ITestContext context, IWriter output, string? returnSymbol = null)
        {
            MethodDef method = context.GetMethod().ResolveMethodDefThrow();

            if (returnSymbol != null && method.HasReturnType)
            {
                output.Write($"{method.ReturnType.GetNameOrAlias()} returnSymbol = ");
            }

            else
            {
                if (method.IsStatic)
                {
                    WriteStaticMethod(context, output, method);
                }
                else
                {
                    WriteInstanceMethod(context, output, method);
                }
            }

            output.WriteLine(";");
            return true;
        }

        public bool TryWriteActDelegate(ITestContext context, IWriter output, string? returnSymbol = null, string delegateSymbol = "act")
        {
            MethodDef method = context.GetMethod().ResolveMethodDefThrow();


            if (returnSymbol != null && method.HasReturnType)
            {
                output.Write($"{method.ReturnType.GetNameOrAlias()} {returnSymbol} = {context.GetDefaultLiteral(method.ReturnType)};");
                output.Write($"Action {delegateSymbol} = () => {{ {returnSymbol} = ");

                if (method.IsStatic)
                {
                    WriteStaticMethod(context, output, method);
                }

                output.WriteLine($"}};");
            }
            else
            {
                output.Write($"Action {delegateSymbol} = () => ");

                if (method.IsStatic)
                {
                    WriteStaticMethod(context, output, method);
                }

                output.WriteLine(";");
            }

            return true;
        }

        private void WriteStaticMethod(ITestContext context, IWriter output, MethodDef method)
        {
            output.Write($"{method.DeclaringType.ToTypeSig().GetNameOrAlias()}.{method.Name}(");
            output.Write(string.Join(", ", method.Parameters.Where(p => p.IsNormalMethodParameter).Select(p => p.Name)));
            output.Write($")");
        }

        private void WriteInstanceMethod(ITestContext context, IWriter output, MethodDef method)
        {
            output.Write($"@this.{method.Name}(");
            output.Write(string.Join(", ", method.Parameters.Where(p => p.IsNormalMethodParameter).Select(p => p.Name)));
            output.Write($")");
        }
    }
}
