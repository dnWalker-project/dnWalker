﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                // TODO
                var theorem = ctx.NewTheorem(new { d = default(double) })//, y = default(int) })
                    .Where(Expression.Lambda(expression,
                    // TODO
                        Expression.Parameter(typeof(double), "d")/*,
                        Expression.Parameter(typeof(int), "y"))*/));
                
                var result = theorem.Solve();
                if (result == null)
                {
                    return null;
                }

                return result.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(result, null));
            }
        }
    }
}
