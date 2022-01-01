﻿using MMC.State;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static partial class ParameterExtensions
    {
        private const string Parameter2Expression = "PRM2EXPR";

        public static IDictionary<string, ParameterExpression> GetExpressionLookup(this ExplicitActiveState cur)
        {
            if (!cur.PathStore.CurrentPath.TryGetPathAttribute(Parameter2Expression, out IDictionary<string, ParameterExpression> lookup))
            {
                lookup = new Dictionary<string, ParameterExpression>();
                cur.PathStore.CurrentPath.SetPathAttribute(Parameter2Expression, lookup);
            }

            return lookup;
        }

        public static Expression GetValueExpression(this IPrimitiveValueParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"V{parameter.Reference}"; // VALUE parameter name in expression is in format V<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {
                Type parameterType = Type.GetTypeCode(Type.GetType(parameter.Type)) switch
                {
                    TypeCode.SByte => typeof(sbyte),
                    TypeCode.Int16 => typeof(short),
                    TypeCode.Int32 => typeof(int),
                    TypeCode.Int64 => typeof(long),
                    TypeCode.Byte => typeof(byte),
                    TypeCode.UInt16 => typeof(ushort),
                    TypeCode.UInt32 => typeof(uint),
                    TypeCode.UInt64 => typeof(ulong),
                    TypeCode.Char => typeof(char),

                    TypeCode.Boolean => typeof(bool),

                    TypeCode.Double => typeof(double),
                    TypeCode.Single => typeof(float),

                    _ => throw new NotSupportedException()
                };

                expression = Expression.Parameter(parameterType, name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetIsNullExpression(this IReferenceTypeParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"N{parameter.Reference}"; // NULL parameter name in expression is in format N<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {

                expression = Expression.Parameter(typeof(bool), name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetLengthExpression(this IArrayParameter parameter, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name = $"L{parameter.Reference}"; // LENGTH parameter name in expression is in format L<ID AS HEX NUMBER>

            if (!lookup.TryGetValue(name, out ParameterExpression expression))
            {

                expression = Expression.Parameter(typeof(uint), name);

                lookup[name] = expression;
            }

            return expression;
        }

        public static Expression GetReferenceEqualsExpression(this IReferenceTypeParameter p1, IReferenceTypeParameter p2, ExplicitActiveState cur)
        {
            IDictionary<string, ParameterExpression> lookup = GetExpressionLookup(cur);
            string name1 = $"E{p1.Reference}{p2.Reference}"; // REF EQUAL parameter name in expression is in format E<ID1 AS HEX NUMBER><ID2 AS HEX NUMBER>
            string name2 = $"E{p2.Reference}{p1.Reference}"; // REF EQUAL parameter name in expression is in format E<ID1 AS HEX NUMBER><ID2 AS HEX NUMBER>

            ParameterExpression expression = null;
            if (lookup.TryGetValue(name1, out expression) || lookup.TryGetValue(name2, out expression))
            {
                return expression;
            }
            else
            {
                expression = Expression.Parameter(typeof(bool), name1);
                lookup[name1] = expression;
                return expression;
            }

        }
    }
}