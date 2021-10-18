using dnlib.DotNet;

using dnWalker.DataElements;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.Concolic.Parameters
{
    public class ParameterStore
    {
        private readonly IDictionary<String, Parameter> _parameters = new Dictionary<String, Parameter>();

        public IDictionary<String, Parameter> Parameters
        {
            get 
            {
                return _parameters; 
            }
        }

        public TParameter AddParameter<TParameter>(TParameter parameter) where TParameter : Parameter
        {
            //_parameters.Add(parameter);
            String name = parameter.Name;
            if (_parameters.ContainsKey(name))
            {
                throw new InvalidOperationException("Parameter with this name is already specified.");
            }

            _parameters.Add(name, parameter);

            return parameter;
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public override String ToString()
        {
            return String.Join(Environment.NewLine, _parameters.Values);
        }

    }
}
