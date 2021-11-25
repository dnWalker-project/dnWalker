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

            foreach(ParameterName currentName in name.TraversFromRoot().Skip(1)) // we want to skip the root 
            {
                if (!TryGetNextParameter(currentName.LocalName, parameter, out parameter))
                {
                    return false;
                }
            }

            return TryGetNextParameter(name.LocalName, parameter, out parameter);
        }

        public bool TryGetRootParameter(ParameterName name, [NotNullWhen(true)] out Parameter? parameter)
        {
            return _rootParameters.TryGetValue(name.RootName, out parameter);
        }

        private bool TryGetNextParameter(string localName, Parameter currentParameter, [NotNullWhen(true)] out Parameter? parameter)
        {
            switch (currentParameter)
            {
                case ObjectParameter op:
                    if (localName == ReferenceTypeParameter.IsNullName)
                    {
                        parameter = op.IsNullParameter;
                        return true;
                    }
                    else
                    {
                        return op.TryGetField(localName, out parameter);
                    }

                case InterfaceParameter ip:
                    if (localName == ReferenceTypeParameter.IsNullName)
                    {
                        parameter = ip.IsNullParameter;
                        return true;
                    }
                    else
                    {
                        string[] p = localName.Split(ParameterNameUtils.CallIndexDelimiter);
                        if (p.Length == 2 && int.TryParse(p[1], out int callNumber))
                        {
                            return ip.TryGetMethodResult(p[0], callNumber, out parameter);
                        }
                        parameter = null;
                        return false;
                    }

                case ArrayParameter ap:
                    if (int.TryParse(localName, out int index))
                    {
                        return ap.TryGetItem(index, out parameter);
                    }
                    parameter = null;
                    return false;

                default:
                    parameter = null;
                    return false;
            }
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
