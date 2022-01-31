using dnlib.DotNet;

namespace dnWalker.TypeSystem
{
    public interface IBaseTypes : ICorLibTypes
    {
        TypeDefOrRefSig Thread { get; }
        TypeDefOrRefSig Exception { get; }
        TypeSig Delegate { get; }
    }
}
