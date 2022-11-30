using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class ConditionalResult
    {
        public Expression Condition { get; set; }
        public UserData Result { get; set; }
    }
}
