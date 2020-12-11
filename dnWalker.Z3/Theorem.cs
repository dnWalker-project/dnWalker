using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z3.LinqBinding;
using Z3Theorem = Z3.LinqBinding.Theorem;

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

        protected override Dictionary<PropertyInfo, Expr> GetEnvironment<T>(Context context)
        {
            var environment = new Dictionary<PropertyInfo, Expr>();

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
                var parameterType = parameter.PropertyType;
                var parameterTypeMapping = (TheoremVariableTypeMappingAttribute)parameterType.GetCustomAttributes(typeof(TheoremVariableTypeMappingAttribute), false).SingleOrDefault();
                if (parameterTypeMapping != null)
                    parameterType = parameterTypeMapping.RegularType;

                //
                // Map the environment onto Z3-compatible types.
                //
                switch (Type.GetTypeCode(parameterType))
                {
                    case TypeCode.Boolean:
                        //environment.Add(parameter, context.MkConst(parameter.Name, context.MkBoolType()));
                        environment.Add(parameter, context.MkBoolConst(parameter.Name));
                        break;
                    case TypeCode.Int32:
                        //environment.Add(parameter, context.MkConst(parameter.Name, context.MkIntType()));
                        environment.Add(parameter, context.MkIntConst(parameter.Name));
                        break;
                    case TypeCode.Double:
                        environment.Add(parameter, context.MkRealConst(parameter.Name));
                        break;
                    default:
                        throw new NotSupportedException("Unsupported parameter type for " + parameter.Name + ".");
                }
            }

            return environment;
        }

        public object Solve()
        {
            return Solve<object>();
        }
    }
}
