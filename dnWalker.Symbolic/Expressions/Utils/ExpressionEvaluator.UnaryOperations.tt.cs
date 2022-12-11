using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic.Expressions.Utils
{
    public partial class ExpressionEvaluator
    {
        private static class UnaryOperations
        {

            public static IValue Negate(IValue v)
            {
                switch (v)
                {
                    case PrimitiveValue<byte> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<ushort> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<uint> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<sbyte> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<short> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<int> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<long> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<float> o:
                        return ValueFactory.GetValue( -o.Value);
                
                    case PrimitiveValue<double> o:
                        return ValueFactory.GetValue( -o.Value);
                
                }
                throw new NotSupportedException();
            }

        }
    }
}