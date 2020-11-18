using MMC;
using MMC.Data;

namespace dnWalker
{
    public class Args
    {
        public static Arg Arg<T>(T value)
        {
            return new Arg<T>(value);
        }
    }

    public interface IArg
    {
        IDataElement AsDataElement(DefinitionProvider definitionProvider);
    }

    public class Arg : IArg
    {
        protected object _value;

        public virtual IDataElement AsDataElement(DefinitionProvider definitionProvider)
        {
            return definitionProvider.CreateDataElement(_value);
        }
    }

    public class Arg<T> : Arg
    {
        public Arg(T value)
        {
            _value = value;
        }
    }
}
