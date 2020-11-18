using dnWalker.Symbolic.Expressions;
using MMC;
using MMC.Data;

namespace dnWalker.Symbolic
{
    public interface ISymbolic
    {
        IExpression Expression { get; set; }
    }

    public static class SymbolicArgs
    {
        public static SymbolicArg<T> Arg<T>(string name)
        {
            return new SymbolicArg<T>(name);
        }
    }

    public class SymbolicArg<T> : IArg
    {
        private readonly string _name;

        public SymbolicArg(string name)
        {
            _name = name;
        }

        public IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            var dataElement = DataElementFactory.CreateDataElement(typeof(T));
            if (dataElement is ISymbolic symbolic)
            {
                symbolic.Expression = new Variable<T>(_name);
            }

            return dataElement;
        }
    }
}
