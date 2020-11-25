using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Z3.LinqBinding;

namespace dnWalker.Z3
{
    public class Solver : ISolver
    {
        public Dictionary<string, object> Solve(Expression expression)
        {
            using (var ctx = new Z3Context())
            {
                ctx.Log = Console.Out; // see internal logging
                var theorem = ctx.NewTheorem(new { x = default(int), y = default(int) })
                    .Where(Expression.Lambda(expression,
                        Expression.Parameter(typeof(int), "x"),
                        Expression.Parameter(typeof(int), "y")));
                /*
                    from t in ctx.NewTheorem(new { x = default(bool), y = default(bool) })
                              where t.x ^ t.y
                              select t;*/

                var result = theorem.Solve();
                Console.WriteLine(result);
                return null;
            }
        }
    }
}
