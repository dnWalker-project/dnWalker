using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace dnWalker.Parameters
{
    public class ParameterStore
    {
        private readonly IDictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();

        public Parameter AddParameter(Parameter parameter)
        {
            if (!parameter.HasName()) throw new InvalidOperationException("Cannot add parameter without a name!");

            //_parameters.Add(parameter);
            var name = parameter.Name;
            if (_parameters.ContainsKey(name))
            {
                throw new InvalidOperationException("Parameter with this name is already specified.");
            }

            _parameters.Add(name, parameter);

            return parameter;
        }

        internal void AddParameter(object p)
        {
            throw new NotImplementedException();
        }

        public bool TryGetParameter(string name, out Parameter parameter)
        {
            // try to perform walk through the parameter forest
            var rootParameterName = ParameterName.GetRootName(name);

            if (rootParameterName == name)
            {
                return _parameters.TryGetValue(name, out parameter);
            }
            else if (_parameters.TryGetValue(rootParameterName, out var rootParameter))
            {
                return rootParameter.TryGetChildParameter(name, out parameter);
            }
            else
            {
                parameter = null;
                return false;
            }
        }


        public IEnumerable<Parameter> RootParameters
        {
            get { return _parameters.Values; }
        }

        public IEnumerable<Parameter> GetAllParameters()
        {
            return _parameters.Values.Flatten(p => p.GetChildrenParameters());
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, _parameters.Values);
        }

    }

    public static class LinqExt
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> sources, Func<T, IEnumerable<T>> selector)
        {
            return sources.Concat(sources.SelectMany(s => selector(s).Flatten(selector)));
        }
    }
}
