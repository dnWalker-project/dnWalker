using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions
{
    public class Variable<T> : IExpression
    {
        private readonly string _name;

        public Variable(string name)
        {
            _name = name;
        }
    }
}
