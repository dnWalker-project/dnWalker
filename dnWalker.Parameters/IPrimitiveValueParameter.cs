using dnWalker.Parameters.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IValueTypeParameter : IParameter
    {
    }

    public interface IPrimitiveValueParameter : IValueTypeParameter
    {
        object? Value 
        { 
            get; 
            set; 
        }
    }

    public interface IPrimitiveValueParameter<T> : IPrimitiveValueParameter
        where T : struct
    {
        new T? Value 
        {
            get; 
            set; 
        }

        object? IPrimitiveValueParameter.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (T?)value;
            }
        }
    }

    public static class PrimitiveValueParameterExtensions
    {
        public static T GetValue<T>(this IPrimitiveValueParameter<T> primitiveValue)
            where T : struct
        {
            return primitiveValue.Value ?? default(T);
        }
    }
}
