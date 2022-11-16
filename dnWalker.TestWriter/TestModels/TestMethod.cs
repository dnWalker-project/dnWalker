using dnlib.DotNet;

namespace dnWalker.TestWriter.TestModels
{
    public class TestMethod
    {
        public IList<AttributeInfo> Attributes { get; } = new List<AttributeInfo>();
        public string? ReturnTypeName { get; set; }
        public string? Name { get; set; }
        public IList<ArgumentInfo> Arguments { get; } = new List<ArgumentInfo>();

        public string? Body { get; set; }
    }
}