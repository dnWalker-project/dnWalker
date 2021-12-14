using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace dnWalker.Parameters
{
    public interface IParameterStore
    {
        void AddParameter(IParameter parameter);
        void AddRootParameter(string name, IParameter parameter);
        IEnumerable<IParameter> GetAllParameters();
        IEnumerable<KeyValuePair<string, IParameter>> GetRootParameters();
        bool TryGetParameter(int id, [NotNullWhen(true)] out IParameter? parameter);
        bool TryGetRootParameter(string name, [NotNullWhen(true)] out IParameter? parameter);
    }
}