using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public abstract class ParameterName : IEquatable<ParameterName>
    {
        public static readonly string Delimiter = ":";
        public static readonly string CallNumberDelimitier = "|";

        public ParameterName? OwnerName { get; }

        protected ParameterName(ParameterName? owner)
        {
            OwnerName = owner;
        }

        public bool IsRoot
        {
            get { return OwnerName != null; }
        }

        public static ParameterName[] CreateParameterNames(string fullName)
        {
            string[] parts = fullName.Split(Delimiter);

            ParameterName[] parameterNames = new ParameterName[parts.Length];

            parameterNames[0] = new RootParameterName(parts[0]);

            for (int i = 0; i < parts.Length; ++i)
            {

            }

            return parameterNames;
        }

        public static ParameterName CreateField(ParameterName owner, string field)
        {

        }

        public static ParameterName CreateMethodResult(ParameterName owner, string methodName, int callNumber)
        {

        }

        public static ParameterName CreateArrayItem(ParameterName owner, int index)
        {

        }

        public abstract bool Equals(ParameterName? other);
    }

    public class RootParameterName : ParameterName
    {
        private readonly string _name;

        public RootParameterName(string name) : base(null)
        {
            _name = name;
        }

        public override bool Equals(ParameterName? other)
        {
            return other is RootParameterName otherRoot && otherRoot._name == _name;
        }
    }

    public class FieldParameterName : ParameterName
    {
        private readonly string _field;

        public FieldParameterName(ParameterName owner, string field) : base(owner)
        {
            _field = field;
        }

        public override bool Equals(ParameterName? other)
        {
            return other is FieldParameterName otherField && otherField._field == _field;
        }
    }

    public class MethodResultParameterName : ParameterName
    {
        private readonly string _methodName;
        private readonly int _callNumber;

        public MethodResultParameterName(ParameterName owner, string methodName, int callNumber) : base(owner)
        {
            _methodName = methodName;
            _callNumber = callNumber;
        }

        public override bool Equals(ParameterName? other)
        {
            return other is MethodResultParameterName otherMethodResult && otherMethodResult._methodName == _methodName && otherMethodResult._callNumber == _callNumber;
        }
    }

    public class ArrayItemParameterName : ParameterName
    {
        private readonly int _index;

        public ArrayItemParameterName(ParameterName owner, int index) : base(owner)
        {
            _index = index;
        }

        public override bool Equals(ParameterName? other)
        {
            return other is ArrayItemParameterName otherArrayItem && otherArrayItem._index == _index;
        }
    }
}
