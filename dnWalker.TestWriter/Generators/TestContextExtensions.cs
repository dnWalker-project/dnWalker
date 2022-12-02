using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    internal static class TestContextExtensions
    {
        public static SymbolContext CreateSymbolContext(this ITestContext context, string symbol, IReadOnlyHeapNode node) 
        {
            SymbolContext sCtx = SymbolContext.Create(symbol, node);
            context.SymbolMapping[symbol] = sCtx;
            context.LocationMapping[node.Location] = sCtx;
            return sCtx;
        }
        public static SymbolContext CreateSymbolContext(this ITestContext context, string symbol, TypeSig type)
        {
            SymbolContext sCtx = SymbolContext.Create(symbol, type);
            context.SymbolMapping[symbol] = sCtx;
            return sCtx;
        }

        public static string GetReturnSymbol(this ITestContext context)
        {
            string symbol = GetFreeResultLikeSymbol(context);
            return symbol;
            //IReadOnlyModel outputModel = context.GetOutputModel();
            //if (outputModel.TryGetReturnValue(context.GetMethod(), out IValue? retValue))
            //{
            //    MethodDef md = context.GetMethod().ResolveMethodDefThrow();
            //    if (retValue is Location l)
            //    {
            //        if (l != Location.Null)
            //        {
            //            return CreateSymbolContext(context, symbol, outputModel.HeapInfo.GetNode(l));
            //        }
            //    }
            //}
            //return CreateSymbolContext(context, symbol, output)
        }

        private static string GetFreeResultLikeSymbol(ITestContext context)
        {
            // TODO: some smart algorithm...
            return "RESULT";
        }

        public static SymbolContext? GetSymbolContext(this ITestContext context, string symbol)
        {
            context.SymbolMapping.TryGetValue(symbol, out SymbolContext? sc);
            return sc;
        }

        public static SymbolContext? GetSymbolContext(this ITestContext context, Location location)
        {
            context.LocationMapping.TryGetValue(location, out SymbolContext? sc);
            return sc;
        }

        public static IReadOnlyModel GetInputModel(this ITestContext context) 
        {
            return context.Iteration.InputModel;
        }

        public static IReadOnlyModel GetOutputModel(this ITestContext context)
        {
            return context.Iteration.OutputModel;
        }

        public static IMethod GetMethod(this ITestContext context)
        {
            return context.Iteration.Exploration.MethodUnderTest;
        }

        public static string GetDefaultLiteral(this ITestContext context, TypeSig type)
        {
            return $"default({type.GetNameOrAlias()})";
        }

        public static string? GetLiteral(this ITestContext context, IValue value)
        {
            return value switch
            {
                Location l => l == Location.Null ? "null" : context.GetSymbolContext(l)?.Literal,
                StringValue s => s == StringValue.Null ? "null" : $"{s.Content}",
                _ => value.ToString()
            };
        }
    }
}
