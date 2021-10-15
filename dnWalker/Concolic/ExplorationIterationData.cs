using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class ParameterTrait
    {
        public String Accessor { get; set; }
        public ParameterInfo Result { get; set; }
    }

    public class ParameterInfo
    {
        public String Name { get; set; }
        public Object Value { get; set; }

        public String TypeName { get; set; }

        public Boolean IsPrimitive => Value != null;

        public IList<ParameterTrait> Traits { get; } = new List<ParameterTrait>();

        public static ParameterInfo FromValue(Object value)
        {
            return new ParameterInfo { Value = value, TypeName = value.GetType().FullName };
        }

        public static ParameterInfo FromValue(String name, Object value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = value.GetType().FullName };
        }

        public static ParameterInfo FromValue<T>(T value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(T).FullName };
        }

        public static ParameterInfo FromValue<T>(String name, T value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(T).FullName };
        }

        public static ParameterInfo FromInt16(Int16 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(Int16).FullName };
        }
        public static ParameterInfo FromInt16(String name, Int16 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(Int16).FullName };
        }
        public static ParameterInfo FromInt32(Int32 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(Int32).FullName };
        }
        public static ParameterInfo FromInt32(String name, Int32 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(Int32).FullName };
        }

        public static ParameterInfo FromInt64(Int64 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(Int64).FullName };
        }
        public static ParameterInfo FromInt64(String name, Int64 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(Int64).FullName };
        }

        public static ParameterInfo FromUInt16(UInt16 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(UInt16).FullName };
        }
        public static ParameterInfo FromUInt16(String name, UInt16 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(UInt16).FullName };
        }
        public static ParameterInfo FromUInt32(UInt32 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(UInt32).FullName };
        }
        public static ParameterInfo FromUInt32(String name, UInt32 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(UInt32).FullName };
        }

        public static ParameterInfo FromUInt64(UInt64 value)
        {
            return new ParameterInfo { Value = value, TypeName = typeof(UInt64).FullName };
        }
        public static ParameterInfo FromUInt64(String name, UInt64 value)
        {
            return new ParameterInfo { Name = name, Value = value, TypeName = typeof(UInt64).FullName };
        }

        private static readonly String[] Separators = { "->" };
        public static ParameterInfo FromInterface(String name, String interfaceTypeName, IDictionary<String, Object> results)
        {

            ParameterInfo parameterInfo = new ParameterInfo { Name = name, TypeName = interfaceTypeName };

            String prefix = name + "->";

            foreach (KeyValuePair<String, Object> result in results)
            {
                if (result.Key.StartsWith(prefix))
                {
                    // TODO: handle the results dictionary => needed if a interface returns another interface or class instance
                    ParameterTrait trait = new ParameterTrait { Accessor = result.Key.Split(Separators, StringSplitOptions.None)[1], Result = ParameterInfo.FromValue(result.Key, result.Value) };
                    parameterInfo.Traits.Add(trait);
                }
            }

            return parameterInfo;
        }
    }

    public class ExplorationIterationData
    {
        public ExplorationIterationData(String pathConstraint, IDictionary<String, ParameterInfo> inputValues)
        {
            PathConstraint = pathConstraint;// ?? throw new ArgumentNullException(nameof(pathConstraint));
            InputValues = inputValues ?? throw new ArgumentNullException(nameof(inputValues));
        }

        public String PathConstraint
        {
            get;
        }

        public IDictionary<String, ParameterInfo> InputValues
        {
            get;
        }
    }
}
