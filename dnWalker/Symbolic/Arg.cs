using dnlib.DotNet;

using dnWalker.DataElements;

using MMC;
using MMC.Data;

using System;
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

    public class DefaultValueArg : IArg
    {
        public string TypeName { get; set; }

        public IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            return DefinitionProvider.GetNullValue(definitionProvider.GetTypeDefinition(TypeName));
        }
    }
    public class DefaultValueArg<T> : IArg
    {

        public IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            return DefinitionProvider.GetNullValue(definitionProvider.GetTypeDefinition(typeof(T).FullName));
        }
    }

    public class InterfaceArg : IArg
    {
        private readonly string _name;
        private readonly string _interfaceName;

        public InterfaceArg(string name, string interfaceName)
        {
            _name = name;
            _interfaceName = interfaceName;
        }

        public IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            var type = definitionProvider.GetTypeDefinition(_interfaceName);
            return new InterfaceProxy(type);
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

        public override bool Equals(object obj)
        {
            return obj is SymbolicArg<T> arg &&
                   _name == arg._name &&
                   EqualityComparer<T>.Default.Equals(_value, arg._value);
        }

        public override int GetHashCode()
        {
            var hashCode = 179903332;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(_value);
            return hashCode;
        }
    }
}
