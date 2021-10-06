using MMC;
using MMC.Data;

using System.Collections.Generic;
using System.Linq.Expressions;

namespace dnWalker.Symbolic
{
    public static class SymbolicArgs
    {
        public static SymbolicArg<T> Arg<T>(string name)
        {
            return new SymbolicArg<T>(name, default(T));
        }

        public static SymbolicArg<T> Arg<T>(string name, T value)
        {
            return new SymbolicArg<T>(name, value);
        }
    }

    public class SymbolicArg<T> : IArg
    {
        private readonly string _name;
        private readonly T _value;

        public SymbolicArg(string name, T value)
        {
            _name = name;
            _value = value;
        }

        public IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            return definitionProvider.CreateDataElement(_value);//, Expression.Parameter(typeof(T), _name));
        }

        public override System.Boolean Equals(System.Object obj)
        {
            return obj is SymbolicArg<T> arg &&
                   _name == arg._name &&
                   EqualityComparer<T>.Default.Equals(_value, arg._value);
        }
    }
}
