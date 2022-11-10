using dnlib.DotNet;

using dnWalker.Symbolic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Input
{
    public class UserLiteral : UserData
    {
        private string _value;

        public UserLiteral()
        {

        }

        public UserLiteral(string value)
        {
            _value = value;
        }

        public string Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        public TypeSig Type
        {
            get;
            set;
        }

        public override IValue Build(IModel model, TypeSig expectedType, IDictionary<string, IValue> references)
        {
            TypeSig type = Type ?? expectedType ?? throw new Exception("The type is not provided!");

            string strValue = _value;

            IValue value = ValueFactory.ParseValue(strValue, type);

            if (value == null) throw new Exception($"Could not parse literal '{strValue}' as '{type}'");

            SetReference(value, references);
            return value;
        }
    }
}
