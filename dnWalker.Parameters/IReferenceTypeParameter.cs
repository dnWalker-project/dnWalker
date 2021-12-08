using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public interface IReferenceTypeParameter : IParameter
    {
        bool IsNull { get; set; }

        bool ReferenceEquals(IReferenceTypeParameter? other);

        void SetReferenceEquals(IReferenceTypeParameter other, bool value = true);
        void ClearReferenceEquals(IReferenceTypeParameter other);
    }
}
