using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Xunit;
using FluentAssertions;

namespace dnWalker.Z3.Tests
{
    public class TheoryOfStrings
    {
        [Fact]
        public void Test_Length()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression getLengthExpr = Expression.Call(stringParamExpr, typeof(string).GetMethod("get_Length"));

            Expression lengthEqualTo5Expr = Expression.Equal(getLengthExpr, Expression.Constant(5));


            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(lengthEqualTo5Expr, new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().HaveLength(5);
        }

        [Fact]
        public void Test_StartsWith()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression startsWithExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                Expression.Constant("Hello World!"));

            // force the result to be longer than the "Hello World!" sequence
            MethodCallExpression getLengthExpr = Expression.Call(stringParamExpr, typeof(string).GetMethod("get_Length"));
            Expression lengthEqualsTo20Expr = Expression.Equal(getLengthExpr, Expression.Constant(20));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(Expression.And(lengthEqualsTo20Expr, startsWithExpr), new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().HaveLength(20);
            resultStr.Should().StartWith("Hello World!");
        }

        [Fact]
        public void Test_EndsWith()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression endsWithExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                Expression.Constant("Hello World!"));

            // force the result to be longer than the "Hello World!" sequence
            MethodCallExpression getLengthExpr = Expression.Call(stringParamExpr, typeof(string).GetMethod("get_Length"));
            Expression lengthEqualsTo20Expr = Expression.Equal(getLengthExpr, Expression.Constant(20));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(Expression.And(lengthEqualsTo20Expr, endsWithExpr), new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().HaveLength(20);
            resultStr.Should().EndWith("Hello World!");
        }

        [Fact]
        public void Test_Contains_SubString()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression containsExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.Contains), new Type[] {typeof(string)}),
                Expression.Constant("Hello World!"));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(containsExpr, new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().Contain("Hello World!");
        }

        [Fact(Skip = "Using a single char is not yet resolved. Char may be a single element string OR a single ")]
        public void Test_Contains_SingleChar()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression containsExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(char) }),
                Expression.Constant('a'));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(containsExpr, new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().Contain("a");
        }

        [Fact]
        public void Test_StartsWith_And_EndsWith()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression startsWithExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                Expression.Constant("AAAAA"));
            MethodCallExpression endsWithExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                Expression.Constant("BBBBB"));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(Expression.And(startsWithExpr, endsWithExpr), new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Should().Be("AAAAABBBBB");
        }

        [Fact]
        public void Test_Substring_OffsetOnly()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression substringExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.Substring), new Type[] { typeof(int) }),
                Expression.Constant(3));

            MethodCallExpression endsWithExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                Expression.Constant("ld!"));

            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(
                Expression.AndAlso(
                    Expression.Equal(substringExpr, Expression.Constant("Hello World!")),
                    endsWithExpr), 
                new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Substring(3, "Hello World!".Length).Should().Be("Hello World!");
            resultStr.Should().EndWith("ld!");

        }

        [Fact]
        public void Test_Substring_OffsetAndLength()
        {
            ParameterExpression stringParamExpr = Expression.Parameter(typeof(string), "str");
            MethodCallExpression substringExpr = Expression.Call(
                stringParamExpr,
                typeof(string).GetMethod(nameof(string.Substring), new Type[] { typeof(int), typeof(int) }),
                Expression.Constant(3),
                Expression.Constant(5));


            Solver solver = new Solver();
            IDictionary<string, object> result = solver.Solve(Expression.Equal(substringExpr, Expression.Constant("12345")), new ParameterExpression[] { stringParamExpr });

            string resultStr = result["str"] as string;

            resultStr.Substring(3, 5).Should().Be("12345");
        }

        [Fact]
        public void Test_Concat()
        {
            ParameterExpression str1Expr = Expression.Parameter(typeof(string), "str1");
            ParameterExpression str2Expr = Expression.Parameter(typeof(string), "str2");

            MethodCallExpression getStr1LengthExpr = Expression.Call(str1Expr, typeof(string).GetMethod("get_Length"));
            Expression lengthStr1GEqualTo5Expr = Expression.GreaterThanOrEqual(getStr1LengthExpr, Expression.Constant(5));

            MethodCallExpression getLengthStr2Expr = Expression.Call(str2Expr, typeof(string).GetMethod("get_Length"));
            Expression lengthStr2EqualTo5Expr = Expression.Equal(getLengthStr2Expr, Expression.Constant(5));

            MethodCallExpression concatExpr = Expression.Call(null, typeof(string).GetMethod(nameof(string.Concat), new Type[] { typeof(string), typeof(string) }), str1Expr, str2Expr);

            Solver solver = new Solver();
            var result = solver.Solve(
                Expression.AndAlso(
                    lengthStr1GEqualTo5Expr,
                    Expression.AndAlso(lengthStr2EqualTo5Expr,
                        Expression.Equal(concatExpr, Expression.Constant("123456ABCDEF"))
                        )
                    ),
                new ParameterExpression[] { str1Expr, str2Expr });


            string resultStr1 = result["str1"] as string;
            string resultStr2 = result["str2"] as string;
            
            resultStr1.Should().Be("123456A");
            resultStr2.Should().Be("BCDEF");
        }
    }
}
