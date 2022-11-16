using dnlib.DotNet;

namespace dnWalker.TestWriter.TestModels
{
    public class AttributeInfo
    {

        public string? TypeName { get; set; }
        public IList<string> PositionalArguments { get; } = new List<string>();
        public IDictionary<string, string> InitializerArguments { get; } = new Dictionary<string, string>();
    }
}