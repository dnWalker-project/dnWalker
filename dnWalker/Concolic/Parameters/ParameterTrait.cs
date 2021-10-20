//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace dnWalker.Concolic.Parameters
//{
//    public class ParameterTrait
//    {

//    }

//    //public class NamedParameterTrait : ParameterTrait
//    //{
//    //    public NamedParameterTrait()
//    //    { }

//    //    public NamedParameterTrait(String name)
//    //    {
//    //        Name = name ?? String.Empty;
//    //    }

//    //    public String Name { get; set; }
//    //}

//    public class ValueTrait : ParameterTrait
//    {
//        public static readonly ValueTrait Unknown = new ValueTrait(null);

//        public ValueTrait(Object value)
//        {
//            Value = value;
//        }

//        public ValueTrait() : this(null)
//        { }


//        public Object Value
//        {
//            get;
//            set;
//        }
//    }

//    public class ValueTrait<TValue> : ValueTrait // ParameterTrait
//    {
//        public static readonly new ValueTrait<TValue> Unknown = new ValueTrait<TValue>(default);

//        public ValueTrait(TValue value) : base(value)
//        {
//            Value = value;
//        }

//        public ValueTrait() : this(default(TValue))
//        { }


//        public new TValue Value 
//        {
//            get 
//            {
//                return (TValue)base.Value;
//            }
//            set
//            {
//                base.Value = value;
//            }
//        }
//    }

//    public class FieldValueTrait : ParameterTrait
//    {
//        public FieldValueTrait()
//        {
//        }

//        public FieldValueTrait(String fieldName, Parameter value)
//        {
//            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
//            FieldValueParameter = value ?? throw new ArgumentNullException(nameof(value));
//        }

//        public String FieldName 
//        {
//            get; 
//            set; 
//        }

//        public Parameter FieldValueParameter
//        {
//            get;
//            set;
//        }
//    }

//    public class IndexValueTrait : ParameterTrait
//    {
//        public IndexValueTrait()
//        {
//        }

//        public IndexValueTrait(Int32 index, Parameter value)
//        {
//            Index = index;
//            IndexValueParameter = value ?? throw new ArgumentNullException(nameof(value));
//        }

//        public Int32 Index
//        {
//            get;
//            set;
//        }

//        public Parameter IndexValueParameter
//        {
//            get;
//            set;
//        }
//    }
    
//    public class LengthTrait : ParameterTrait
//    {
//        public LengthTrait()
//        {

//        }

//        public LengthTrait(Int32 length)
//        {
//            LengthParameter.Value = length;
//        }

//        public Int32Parameter LengthParameter
//        {
//            get;
//            set;
//        }
//    }

//    public class IsNullTrait : ParameterTrait
//    {
//        public IsNullTrait()
//        {
//        }

//        public IsNullTrait(Boolean isNull)
//        {
//            IsNullParameter.Value = isNull;
//        }

//        public BooleanParameter IsNullParameter
//        {
//            get;
//            set;
//        }
//    }

//    public class MethodResultTrait : ParameterTrait
//    {
//        public MethodResultTrait()
//        {
//        }

//        public MethodResultTrait(String methodName, Int32 callIndex, Parameter value)
//        {
//            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
//            CallIndex = callIndex;
//            Value = value ?? throw new ArgumentNullException(nameof(value));
//        }

//        public String MethodName
//        {
//            get;
//            set;
//        }

//        public Parameter Value
//        {
//            get;
//            set;
//        }

//        public Int32 CallIndex
//        {
//            get;
//            set;
//        }
//    }
//}
