using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3
{
    public class ExprVisitor
    {
        public object VisitExpression(Type elementType, Expr expression, int length)
        {
            if (expression.IsConstantArray)
            {
                var array = Array.CreateInstance(elementType, length);
                var value = (char)((IntNum)expression.Args[0]).Int;
                for (int i = 0; i < length; i++)
                {
                    array.SetValue(value, i);
                }

                return array;
            }

            if (expression.IsConst)
            {
                var array = Array.CreateInstance(elementType, length);
                var index = 0;
                foreach (var elementExpr in expression.Args)
                {
                    array.SetValue((char)((IntNum)elementExpr).Int, index++);
                }

                return array;
            }

            if (expression.IsArray && expression.IsStore)
            {
                var array = (Array)VisitExpression(elementType, expression.Args[0] as ArrayExpr, length);
                // (store a i v) returns a new array identical to a, but on position i it contains the value v.
                array.SetValue(((IntNum)expression.Args[1]).Int, ((IntNum)expression.Args[2]).Int);
                return array;
            }

            throw new Exception(expression.ToString());
        }
    }
}
