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
        string Type
        {
            get;
        }
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
        string IValueTypeParameter.Type
        {
            get
            {
                return typeof(T).FullName!;
            }
        }

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
}
