﻿using dnlib.DotNet;
using dnWalker.Symbolic;
using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators.Arrange
{
    /// <summary>
    /// Writes the arrange part of the unit test.
    /// </summary>
    public interface IArrangePrimitives : IPrimitives
    {
        bool TryWriteArrangeCreateInstance(ITestContext testContext, IWriter output, string symbol);

        bool TryWriteArrangeInitializeField(ITestContext testContext, IWriter output, string symbol, IField field, string literal);

        bool TryWriteArrangeInitializeStaticField(ITestContext testContext, IWriter output, IField field, string literal);

        bool TryWriteArrangeInitializeMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<string> literals);
        
        bool TryWriteArrangeInitializeConstrainedMethod(ITestContext testContext, IWriter output, string symbol, IMethod method, IReadOnlyList<KeyValuePair<Expression, string>> constrainedLiterals, string fallbackLiteral);

        bool TryWriteArrangeInitializeStaticMethod(ITestContext testContext, IWriter output, IMethod method, IReadOnlyList<string> literals);

        bool TryWriteArrangeInitializeArrayElement(ITestContext testContext, IWriter output, string symbol, int index, string literal);
    }
}
