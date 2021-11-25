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
        private readonly IDictionary<string, Parameter> _rootParameters = new Dictionary<string, Parameter>();

        public const string ThisParameterName = "#__THIS__";
        public const string ResultParameterName = "#__RESULT__";

        public Parameter AddRootParameter(Parameter parameter)
        {
            var name = parameter.LocalName;
            if (_rootParameters.ContainsKey(name))
            {
                throw new InvalidOperationException("Parameter with this name is already specified.");
            }

            _rootParameters.Add(name, parameter);

            return parameter;
        }


        public bool TryGetParameter(ParameterName name, [NotNullWhen(true)]out Parameter? parameter)
        {
            if (name.IsEmpty)
            {
                parameter = null;
                return false;
            }

            parameter = _rootParameters[name.RootName];

            if (parameter == null) return false;

            foreach(ParameterName currentName in name.TraversFromRoot().Skip(1)) // we want to skip the root 
            {
                if (parameter.TryGetChild(currentName, out parameter))
                {
                    return false;
                }
            }

            return true;
            //return TryGetNextParameter(name, parameter, out parameter);
        }

        public bool TryGetRootParameter(ParameterName name, [NotNullWhen(true)] out Parameter? parameter)
        {
            return _rootParameters.TryGetValue(name.RootName, out parameter);
        }

        public IEnumerable<Parameter> RootParameters
        {
            get { return _rootParameters.Values; }
        }

        public IEnumerable<Parameter> GetAllParameters()
        {
            return _rootParameters.Values.Flatten(p => p.GetChildren());
        }

        public void Clear()
        {
            _rootParameters.Clear();
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, _rootParameters.Values);
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
