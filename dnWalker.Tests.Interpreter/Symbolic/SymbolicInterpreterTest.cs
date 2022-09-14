
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit.Abstractions;

namespace dnWalker.Tests.Interpreter.Symbolic
{
    public class SymbolicInterpreterTest : SymbolicTestBase
    {
        public SymbolicInterpreterTest(ITestOutputHelper output) : base(output)
        {
        }


        [Theory]
        [InlineData(new object[] {5, 6}, new string?[] {null, null}, null, "True")]
        [InlineData(new object[] {5, 6}, new string?[] {"a", null}, "(a + 6)", "True")]
        [InlineData(new object[] {5, 6}, new string?[] {null, "b"}, "(5 + b)", "True")]
        [InlineData(new object[] {5, 6}, new string?[] {"a", "b"}, "(a + b)", "True")]
        public void Test_ADD__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint) 
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, null, "(a == 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a != 5)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, null, "(5 == b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 != b)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, null, "(a == b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a != b)")]
        public void Test_BEQ__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, null, "(a == 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a != 5)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, null, "(5 == b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 != b)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, null, "(a == b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a != b)")]
        public void Test_BNE_UN__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, null, "(a < 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a >= 5)")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, null, "(4 < b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 >= b)")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, null, "(a < b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a >= b)")]
        public void Test_BGE__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, null, "(a <= 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a > 5)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, null, "(5 <= b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 > b)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, null, "(a <= b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a > b)")]
        public void Test_BGT__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, null, "(a <= 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a > 5)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, null, "(5 <= b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 > b)")]

        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, null, "(a <= b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a > b)")]
        public void Test_BLE__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, null, "(a < 5)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", null }, null, "(a >= 5)")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, null, "(4 < b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { null, "b" }, null, "(6 >= b)")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, null, "(a < b)")]
        [InlineData(new object[] { 6, 5 }, new string?[] { "a", "b" }, null, "(a >= b)")]
        public void Test_BLT__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] {0}, new string?[] {null}, null, "True")]
        [InlineData(new object[] {1}, new string?[] {null}, null, "True")]

        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 1 }, new string?[] { "a" }, null, "(a != 0)")]

        [InlineData(new object[] { false }, new string?[] { "a" }, null, "!a")]
        [InlineData(new object[] { true }, new string?[] { "a" }, null, "a")]

        public void Test_BRTRUE__Int32(object[]args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 1 }, new string?[] { null }, null, "True")]

        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 1 }, new string?[] { "a" }, null, "(a != 0)")]

        [InlineData(new object[] { false }, new string?[] { "a" }, null, "!a")]
        [InlineData(new object[] { true }, new string?[] { "a" }, null, "a")]

        public void Test_BRFALSE__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a == 5)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, "(a == 5)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 == b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, "(5 == b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a == b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, "(a == b)", "True")]
        public void Test_CEQ__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a > 5)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, "(a > 5)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 > b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, "(5 > b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a > b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, "(a > b)", "True")]
        public void Test_CGT__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a < 5)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, "(a < 5)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 < b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, "(5 < b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a < b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, "(a < b)", "True")]
        public void Test_CLT__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, "(a / 5)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, "(5 / b)", "(b != 0)")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, "(a / b)", "(b != 0)")]
        public void Test_DIV__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 5 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 5 }, new string?[] { "a" }, "a", "True")]
        public void Test_LDARG__0(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", null }, "(a * 5)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { null, "b" }, "(5 * b)", "True")]
        [InlineData(new object[] { 5, 5 }, new string?[] { "a", "b" }, "(a * b)", "True")]
        public void Test_MUL__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 1 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 1 }, new string?[] { "a" }, "-a", "True")]

        public void Test_NEG__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { true }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { true }, new string?[] { "a" }, "!a", "True")]

        public void Test_NOT__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { true, true }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { true, false }, new string?[] { null, null }, null, "True")]
        
        [InlineData(new object[] { true, true }, new string?[] { "a", null }, "(a | True)", "True")]
        [InlineData(new object[] { true, false }, new string?[] { "a", null }, "(a | False)", "True")]

        [InlineData(new object[] { true, true }, new string?[] { null, "b" }, "(True | b)", "True")]
        [InlineData(new object[] { false, false }, new string?[] { null, "b" }, "(False | b)", "True")]


        [InlineData(new object[] { true, true }, new string?[] { "a", "b" }, "(a | b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a | 5)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 | b)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a | b)", "True")]

        public void Test_OR__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { true, true }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { true, false }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { true, true }, new string?[] { "a", null }, "(a ^ True)", "True")]
        [InlineData(new object[] { true, false }, new string?[] { "a", null }, "(a ^ False)", "True")]

        [InlineData(new object[] { true, true }, new string?[] { null, "b" }, "(True ^ b)", "True")]
        [InlineData(new object[] { false, false }, new string?[] { null, "b" }, "(False ^ b)", "True")]


        [InlineData(new object[] { true, true }, new string?[] { "a", "b" }, "(a ^ b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a ^ 5)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 ^ b)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a ^ b)", "True")]

        public void Test_XOR__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { true, true }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { true, false }, new string?[] { null, null }, null, "True")]

        [InlineData(new object[] { true, true }, new string?[] { "a", null }, "(a & True)", "True")]
        [InlineData(new object[] { true, false }, new string?[] { "a", null }, "(a & False)", "True")]

        [InlineData(new object[] { true, true }, new string?[] { null, "b" }, "(True & b)", "True")]
        [InlineData(new object[] { false, false }, new string?[] { null, "b" }, "(False & b)", "True")]


        [InlineData(new object[] { true, true }, new string?[] { "a", "b" }, "(a & b)", "True")]

        [InlineData(new object[] { 4, 5 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", null }, "(a & 5)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { null, "b" }, "(4 & b)", "True")]
        [InlineData(new object[] { 4, 5 }, new string?[] { "a", "b" }, "(a & b)", "True")]

        public void Test_AND__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }


        [Theory]
        [InlineData(new object[] { 5, 6 }, new string?[] { null, null }, null, "True")]
        [InlineData(new object[] { 5, 6 }, new string?[] { "a", null }, "(a - 6)", "True")]
        [InlineData(new object[] { 5, 6 }, new string?[] { null, "b" }, "(5 - b)", "True")]
        [InlineData(new object[] { 5, 6 }, new string?[] { "a", "b" }, "(a - b)", "True")]
        public void Test_SUB__Int32(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a >= 0)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 0)")]
        // 0 labels => any nonnegative value will enforce path constraint arg >= 0
        public void Test_SWITCH__0(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 1 }, new string?[] { "a" }, null, "(a >= 1)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 1)")]
        // 1 label => 0 - PC: arg == 0, 1 - PC: arg >= 1
        public void Test_SWITCH__1(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 1 }, new string?[] { "a" }, null, "(a == 1)")]
        [InlineData(new object[] { 2 }, new string?[] { "a" }, null, "(a >= 2)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 2)")]
        public void Test_SWITCH__2(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 2 }, new string?[] { "a" }, null, "(a == 2)")]
        [InlineData(new object[] { 3 }, new string?[] { "a" }, null, "(a >= 3)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 3)")]
        public void Test_SWITCH__3(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 3 }, new string?[] { "a" }, null, "(a == 3)")]
        [InlineData(new object[] { 4 }, new string?[] { "a" }, null, "(a >= 4)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 4)")]
        public void Test_SWITCH__4(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 4 }, new string?[] { "a" }, null, "(a == 4)")]
        [InlineData(new object[] { 5 }, new string?[] { "a" }, null, "(a >= 5)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 5)")]
        public void Test_SWITCH__5(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }

        [Theory]
        [InlineData(new object[] { 0 }, new string?[] { null }, null, "True")]
        [InlineData(new object[] { 0 }, new string?[] { "a" }, null, "(a == 0)")]
        [InlineData(new object[] { 5 }, new string?[] { "a" }, null, "(a == 5)")]
        [InlineData(new object[] { 6 }, new string?[] { "a" }, null, "(a >= 6)")]
        [InlineData(new object[] { int.MaxValue }, new string?[] { "a" }, null, "(a >= 6)")]
        public void Test_SWITCH__6(object[] args, string?[] argNames, string resultExpr, string pathConstraint)
        {
            TestAndCompare(SymbolicArgument.Build(args, argNames), SymbolicResult.Build(pathConstraint, resultExpr));
        }
    }
}
