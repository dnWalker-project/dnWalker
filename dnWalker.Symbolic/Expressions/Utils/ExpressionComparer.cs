using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public sealed partial class ExpressionComparer : IEqualityComparer<Expression>
    {
        private ExpressionComparer() { }

        public static readonly IEqualityComparer<Expression> Instance = new ExpressionComparer();

        public static bool Equals(Expression? x, Expression? y) => Instance.Equals(x, y);

        public static int GetHashCode([DisallowNull]Expression expression) => Instance.GetHashCode(expression);

        bool IEqualityComparer<Expression>.Equals(Expression? x, Expression? y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return BaseEquals(x!, y!);
        }

        int IEqualityComparer<Expression>.GetHashCode([DisallowNull] Expression obj)
        {
            HashCode hash = new HashCode();
            BaseHash(obj, ref hash);
            return hash.ToHashCode();
        }
    }
}
