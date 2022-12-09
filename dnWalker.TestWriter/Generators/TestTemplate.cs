using dnWalker.TestWriter.Generators.Act;
using dnWalker.TestWriter.Generators.Arrange;
using dnWalker.TestWriter.Generators.Assert;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestWriter.Generators
{
    public class TestTemplate : ITestTemplate
    {
        public TestTemplate(IReadOnlyList<IArrangePrimitives> arrangeWriters, IReadOnlyList<IActPrimitives> actWriters, IReadOnlyList<IAssertPrimitives> assertWriters)
        {
            ArrangeWriters = arrangeWriters;
            ActWriters = actWriters;
            AssertWriters = assertWriters;
        }

        public IReadOnlyList<IArrangePrimitives> ArrangeWriters
        {
            get;
        }

        public IReadOnlyList<IActPrimitives> ActWriters
        {
            get;
        }

        public IReadOnlyList<IAssertPrimitives> AssertWriters
        {
            get;
        }
    }
}
