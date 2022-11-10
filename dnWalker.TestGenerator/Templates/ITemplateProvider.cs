namespace dnWalker.TestGenerator.Templates
{
    public interface ITemplateProvider : ITemplate
    {
        IArrangeTemplate ArrangeTemplate { get; }
        IActTemplate ActTemplate { get; }
        IAssertTemplate AssertTemplate { get; }
    }
}