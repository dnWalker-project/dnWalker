using System.IO;

namespace dnWalker.Parameters
{
    public interface IParameterSerializer
    {
        void Serialize(IParameterSet parameterSet, Stream output);
    }
}