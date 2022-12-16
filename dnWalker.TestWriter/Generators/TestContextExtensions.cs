﻿using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.Symbolic.Heap;
using dnWalker.TestWriter.Utils;

using System;
using System.Collections.Generic;
using System.Globalization;
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
            return "result";
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
            if (type.IsNumber()) return "0";
            if (type.IsChar()) return "'\0'";
            if (type.IsBoolean()) return "false";
            if (type.GetIsValueType()) return $"default({type.GetNameOrAlias()})";
            else return "null";
        }

        public static string? GetLiteral(this ITestContext context, IValue value)
        {
            return value switch
            {
                Location l => l == Location.Null ? "null" : context.GetSymbolContext(l)?.Literal,
                StringValue s => s == StringValue.Null ? "null" : $"\"{s.Content}\"",
                PrimitiveValue<bool> b => b.Value ? "true" : "false",
                PrimitiveValue<double> d => GetLiteral(d.Value),
                PrimitiveValue<float> f => GetLiteral(f.Value),
                _ => value.ToString()
            };
        }

        private static string GetLiteral(float f)
        {
            if (float.IsNaN(f)) return "float.NaN";
            else if (float.IsPositiveInfinity(f)) return "float.PositiveInfinity";
            else if (float.IsNegativeInfinity(f)) return "float.NegativeInfinity";
            else return $"{f.ToString(CultureInfo.InvariantCulture)}f";
        }

        private static string GetLiteral(double d)
        {
            if (double.IsNaN(d)) return "double.NaN";
            else if (double.IsPositiveInfinity(d)) return "double.PositiveInfinity";
            else if (double.IsNegativeInfinity(d)) return "double.NegativeInfinity";
            else return $"{d.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
