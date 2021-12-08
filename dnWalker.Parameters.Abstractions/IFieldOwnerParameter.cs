using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IFieldOwnerParameter : IParameter
    {
        IEnumerable<KeyValuePair<string, IParameter>> GetFields();

        bool TryGetField(string fieldName, [NotNullWhen(true)]out IParameter? parameter);
        void SetField(string fieldName, IParameter? parameter);
        void ClearField(string fieldName);
    }
}
