using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public static class SubstituteIdWithAccess
    {
        private class SubstituteIdWithAccessVisitor : ExpressionVisitor
        {
            private readonly ParameterStore _store;

            const char ValueOf = 'V';
            const char Null = 'N';
            const char LengthOf = 'L';
            const char RefEquals = 'E';

            private static int GetId(string key)
            {
                return Convert.ToInt32(key, 16);
            }


            private bool IsValueOf(string key, out IPrimitiveValueParameter p)
            {
                if (key[0] == ValueOf)
                {
                    int id = GetId(key.Substring(1));
                    _store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IPrimitiveValueParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            private bool IsNull(string key, out IReferenceTypeParameter p)
            {

                if (key[0] == Null)
                {
                    int id = GetId(key.Substring(1));
                    _store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IReferenceTypeParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            private bool IsLengthOf(string key, out IArrayParameter p)
            {
                if (key[0] == LengthOf)
                {
                    int id = GetId(key.Substring(1));
                    _store.TryGetParameter(id, out IParameter parameter);
                    p = parameter as IArrayParameter;

                    return p != null;
                }
                p = null;
                return false;
            }
            private bool IsReferenceEquals(string key, out IReferenceTypeParameter lhs, out IReferenceTypeParameter rhs)
            {

                if (key[0] == RefEquals)
                {
                    int id1 = GetId(key.Substring(1, 4));
                    int id2 = GetId(key.Substring(5));
                    _store.TryGetParameter(id1, out IParameter p1);
                    _store.TryGetParameter(id2, out IParameter p2);
                    lhs = p1 as IReferenceTypeParameter;
                    rhs = p2 as IReferenceTypeParameter;

                    return lhs != null && rhs != null;
                }
                lhs = null;
                rhs = null;
                return false;
            }

            public SubstituteIdWithAccessVisitor(ParameterStore store)
            {
                _store = store;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                string name = node.Name;
                Type type = node.Type;

                if (IsValueOf(name, out IPrimitiveValueParameter value))
                {
                    // value of => just return its access string
                    name = value.GetAccessString();
                }
                else if (IsNull(name, out IReferenceTypeParameter np))
                {
                    name = $"IsNull({np.GetAccessString()})";
                }
                else if (IsLengthOf(name, out IArrayParameter lap))
                {
                    name = $"{lap.GetAccessString()}.Length";
                }
                else if (IsReferenceEquals(name, out IReferenceTypeParameter lhs, out IReferenceTypeParameter rhs))
                {
                    // make it a binary expression...
                    Expression lhsExpr = Expression.Parameter(typeof(object), lhs.GetAccessString());
                    Expression rhsExpr = Expression.Parameter(typeof(object), rhs.GetAccessString());

                    return Expression.ReferenceEqual(lhsExpr, rhsExpr);
                }

                return Expression.Parameter(type, name);
            }
        }

        public static Expression SubstitueIdWithAccess(this Expression expression, ParameterStore store)
        {
            return new SubstituteIdWithAccessVisitor(store).Visit(expression);
        }
    }
}
