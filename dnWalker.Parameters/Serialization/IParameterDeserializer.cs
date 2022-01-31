using System.IO;

namespace dnWalker.Parameters
{
    public interface IParameterDeserializer
    {
        IParameterSet Deserialize(Stream input);
    }
}