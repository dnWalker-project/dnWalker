
using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;

namespace dnWalker.TestGenerator.TestClasses.Schemas
{
    public class ChangedObjectSchema : TestSchema
    {
        private readonly Location _location;

        public ChangedObjectSchema(Location location, ITestClassContext context) : base(context)
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
