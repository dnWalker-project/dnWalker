using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.Parameters
{
    public class ParameterStore
    {
        public const string ThisName = "#__THIS__";
        public const string ReturnValueName = "#__RETVAL__";


        private readonly Dictionary<string, IParameter> _rootParamters = new Dictionary<string, IParameter>();
        private readonly Dictionary<int, IParameter> _parameters = new Dictionary<int, IParameter>();

        public IEnumerable<KeyValuePair<string, IParameter>> GetRootParameters()
        {
            return _rootParamters.AsEnumerable();
        }

        public IEnumerable<IParameter> GetAllParameters()
        {
            return _parameters.Values.AsEnumerable();
        }

        public void PruneParameter(IParameter parameter)
        {
            if (!_parameters.ContainsKey(parameter.Id)) return; //already pruned

            if (parameter.Accessor is RootParameterAccessor r)
            {
                _rootParamters.Remove(r.Name);
            }

            foreach (IParameter p in parameter.GetSelfAndDescendants())
            {
                _parameters.Remove(p.Id);
            }
        }

        public void AddParameter(IParameter parameter)
        {
            _parameters[parameter.Id] = parameter;
        }

        public void AddRootParameter(string name, IParameter parameter)
        {
            _parameters[parameter.Id] = parameter;
            _rootParamters[name] = parameter;
            parameter.Accessor = new RootParameterAccessor(name);
        }

        public bool TryGetParameter(int id, [NotNullWhen(true)] out IParameter? parameter)
        {
            return _parameters.TryGetValue(id, out parameter);
        }

        public bool TryGetRootParameter(string name, [NotNullWhen(true)] out IParameter? parameter)
        {
            return _rootParamters.TryGetValue(name, out parameter);
        }
    }
}
