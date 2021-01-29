using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Z3.LinqBinding;
using Z3Theorem = Z3.LinqBinding.Theorem;
using Z3Environment = Z3.LinqBinding.Environment;

namespace dnWalker.Z3
{
    public class Theorem : Z3Theorem
    {
        private readonly Type contextType;

        public Theorem(Type contextType, Z3Context context, IEnumerable<LambdaExpression> constraints)
            : base(context, constraints)
        {
            this.contextType = contextType;
        }

        protected override T CreateResultObject<T>()
        {
            return (T)Activator.CreateInstance(contextType);
        }

        protected override Z3Environment GetEnvironment<T>(Context context)
        {
            var environment = new Environment();

            //
            // All public properties are considered part of the theorem's environment.
            // Notice we can't require custom attribute tagging if we want the user to be able to
            // use anonymous types as a convenience solution.
            //
            foreach (var parameter in contextType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //
                // Normalize types when facing Z3. Theorem variable type mappings allow for strong
                // typing within the theorem, while underlying variable representations are Z3-
                // friendly types.
                //
                environment.Add(parameter, context);
            }

            return environment;
        }

        public object Solve()
        {
            return Solve<object>();
        }

        protected override object GetSolutionValue(PropertyInfo parameter, Model model, Z3Environment environment)
        {
            var env = environment as Environment;
            if(env.TryGetSolutionValue(parameter, model, out var solution))
            {
                return solution;
            }

            return base.GetSolutionValue(parameter, model, environment);
        }
    }
}
