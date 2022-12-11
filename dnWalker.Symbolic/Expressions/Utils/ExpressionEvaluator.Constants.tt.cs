using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public partial class ExpressionEvaluator
    {
        private static class Constants
        {

            public static IValue HandleConstant(Object? v)
            {
                switch (v)
                {
                    case byte c:
                        return ValueFactory.GetValue(c);
                
                    case ushort c:
                        return ValueFactory.GetValue(c);
                
                    case uint c:
                        return ValueFactory.GetValue(c);
                
                    case ulong c:
                        return ValueFactory.GetValue(c);
                
                    case sbyte c:
                        return ValueFactory.GetValue(c);
                
                    case short c:
                        return ValueFactory.GetValue(c);
                
                    case int c:
                        return ValueFactory.GetValue(c);
                
                    case long c:
                        return ValueFactory.GetValue(c);
                
                    case float c:
                        return ValueFactory.GetValue(c);
                
                    case double c:
                        return ValueFactory.GetValue(c);
                
                    case null:
                        return Location.Null;
                }
                throw new NotSupportedException();
            }
        }
    }
}