namespace dnWalker.Parameters
{
    public interface IAliasParameter : IParameter
    {
        public IParameter ReferencedParameter { get; }
    }
}