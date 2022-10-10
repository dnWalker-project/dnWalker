using dnlib.DotNet;

namespace dnWalker.Instructions.Extensions.NativePeers
{
    internal interface INativePeerCache<T> where T : class
    {
        bool TryGetNativePeer(MethodDef method, out T nativePeer);
    }
}