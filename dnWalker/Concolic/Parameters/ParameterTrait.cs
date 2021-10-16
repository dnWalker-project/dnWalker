using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic.Parameters
{
    public class ParameterTrait
    {

    }

    public class NamedParameterTrait : ParameterTrait
    {
        public NamedParameterTrait()
        { }

        public NamedParameterTrait(String name)
        {
            Name = name ?? String.Empty;
        }

        public String Name { get; set; }
    }

    public class ValueTrait : ParameterTrait
    {
        public static readonly ValueTrait Unknown = new ValueTrait(null);

        public ValueTrait(Object value)
        {
            Value = value;
        }

        public ValueTrait() : this(null)
        { }


        public Object Value
        {
            get;
            set;
        }
    }

    public class ValueTrait<TValue> : ValueTrait // ParameterTrait
    {
        public static readonly new ValueTrait<TValue> Unknown = new ValueTrait<TValue>(default);

        public ValueTrait(TValue value) : base(value)
        {
            Value = value;
        }

        public ValueTrait() : this(default(TValue))
        { }


        public new TValue Value 
        {
            get 
            {
                return (TValue)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }

    public class FieldValueTrait : ParameterTrait
    {
        public FieldValueTrait()
        {
        }

        public FieldValueTrait(String typeName, String fieldName, Parameter value)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public String TypeName
        {
            get;
            set;
        }

        public String FieldName 
        {
            get; 
            set; 
        }

        public Parameter Value
        {
            get;
            set;
        }
    }

    public class IsNullTrait : ValueTrait<Boolean>
    {
        public IsNullTrait()
        {
        }

        public IsNullTrait(Boolean value) : base(value)
        {
        }
    }

    public class MethodResultTrait : ParameterTrait
    {
        public MethodResultTrait()
        {
        }

        public MethodResultTrait(String typeName, String methodName, Parameter value)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public String TypeName
        {
            get;
            set;
        }

        public String MethodName
        {
            get;
            set;
        }

        public Parameter Value
        {
            get;
            set;
        }
    }
}
