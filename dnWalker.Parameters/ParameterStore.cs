using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.Parameters
{
    public class ParameterStore : IParameterStore
    {
        public const string ThisName = "#__THIS__";
        public const string ReturnName = "#__RETVAL__";


        private readonly Dictionary<string, IParameter> _rootParameters = new Dictionary<string, IParameter>();
        private readonly Dictionary<int, IParameter> _parameters = new Dictionary<int, IParameter>();

        public IEnumerable<KeyValuePair<string, IParameter>> GetRootParameters()
        {
            return _rootParameters.AsEnumerable();
        }

        public IEnumerable<IParameter> GetAllParameters()
        {
            return _parameters.Values.AsEnumerable();
        }

        public void AddParameter(IParameter parameter)
        {
            _parameters[parameter.Id] = parameter;
        }

        public void AddRootParameter(string name, IParameter parameter)
        {
            _parameters[parameter.Id] = parameter;
            _rootParameters[name] = parameter;
            parameter.Accessor = new RootParameterAccessor(name);
        }

        public void AddThisParameter(IParameter parameter)
        {
            AddRootParameter(ThisName, parameter);
        }

        public void AddReturnParameter(IParameter parameter)
        {
            AddRootParameter(ReturnName, parameter);
        }

        public bool TryGetParameter(int id, [NotNullWhen(true)] out IParameter? parameter)
        {
            return _parameters.TryGetValue(id, out parameter);
        }

        public bool TryGetRootParameter(string name, [NotNullWhen(true)] out IParameter? parameter)
        {
            return _rootParameters.TryGetValue(name, out parameter);
        }

        public bool TryGetReturnParameter([NotNullWhen(true)]out IParameter? retVal)
        {
            return TryGetRootParameter(ReturnName, out retVal);
        }

        public bool TryGetThisParameter([NotNullWhen(true)] out IParameter? retVal)
        {
            return TryGetRootParameter(ThisName, out retVal);
        }

        public bool RemoveParameter(IParameter parameter)
        {
            bool result = true;

            if (parameter.Accessor is RootParameterAccessor root)
            {
                result &= _rootParameters.Remove(root.Name);
            }

            return result && _parameters.Remove(parameter.Id);
        }
        
    }
}
