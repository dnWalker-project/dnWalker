using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Z3.LinqBinding;
using Z3Environment = Z3.LinqBinding.Environment;

namespace dnWalker.Z3
{
    public class Environment : Z3Environment
    {
        private IDictionary<string, IDictionary<string, Expr>> _support = new Dictionary<string, IDictionary<string, Expr>>();

        public override void Add(PropertyInfo parameter, Context context)
        {
            var parameterType = parameter.PropertyType;
            var parameterTypeMapping = (TheoremVariableTypeMappingAttribute)parameterType.GetCustomAttributes(typeof(TheoremVariableTypeMappingAttribute), false).SingleOrDefault();
            if (parameterTypeMapping != null)
                parameterType = parameterTypeMapping.RegularType;

            if (parameterType.IsArray)
            {
                switch (Type.GetTypeCode(parameterType.GetElementType()))
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Char:
                        Add(parameter, context.MkArrayConst(parameter.Name, context.IntSort, context.IntSort));
                        var name = $"{parameter.Name}_Length";
                        _support.Add(parameter.Name, new Dictionary<string, Expr>
                        {
                            { name, context.MkIntConst(name) }
                        });
                        //environment.Add(parameter, context.MkArrayConst(parameter.Name, context.IntSort, context.IntSort));
                        break;
                    default:
                        throw new NotSupportedException($"Parameter type {parameterType.Name} of {parameter.Name} is not supported.");
                }

                return;
            }

            base.Add(parameter, context);
        }

        public override bool TryGetExpression(MemberExpression member, ParameterExpression param, out Expr expr)
        {
            if (_support.TryGetValue(param.Name, out var list))
            {
                if (list.TryGetValue(param.Name + "_" + member.Member.Name, out expr))
                {
                    return true;
                }
            }

            return base.TryGetExpression(member, param, out expr);
        }

        public bool TryGetSolutionValue(PropertyInfo parameter, Model model, out object solution)
        {
            solution = null;

            switch (Type.GetTypeCode(parameter.PropertyType))
            {
                case TypeCode.Object:
                    if (_support.TryGetValue(parameter.Name, out var list))
                    {
                        if (list.TryGetValue(parameter.Name + "_Length", out var expr))
                        {
                            var val = model.Eval(expr);
                            solution = Array.CreateInstance(parameter.PropertyType.GetElementType(), ((IntNum)val).Int);
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }

            return false;
        }
    }
}
