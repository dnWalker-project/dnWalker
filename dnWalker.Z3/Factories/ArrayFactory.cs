using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Z3.Factories
{
    public class ArrayFactory
    {
        public static object CreateArray(Type elementType, Expr expression, int length)
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
                var array = (Array)CreateArray(elementType, expression.Args[0] as ArrayExpr, length);
                //var index = 0;
                //foreach (var elementExpr in expression.Args)
                //{
                array.SetValue(Convert.ChangeType(((IntNum)expression.Args[2]).Int, elementType), ((IntNum)expression.Args[1]).Int);
                //}

                return array;
            }

            throw new Exception(expression.ToString());
        }
    }
}
