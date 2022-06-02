
using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    public class ChangedArraySchema : TestSchema
    {
        private readonly Location _location;

        public ChangedArraySchema(Location location, ITestClassContext context) : base(context)
        {
            _location = location;
        }

        public Location Location
        {
            get
            {
                return _location;
            }
        }

        public override void Write(IWriter output, ITemplateProvider templates)
        {
            throw new System.NotImplementedException();
        }
    }
}
