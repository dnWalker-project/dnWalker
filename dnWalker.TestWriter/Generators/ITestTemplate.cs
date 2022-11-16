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
    public interface ITestTemplate
    {
        IReadOnlyList<IArrangePrimitives> ArrangeWriters { get; }
        IReadOnlyList<IActPrimitives> ActWriters { get; }
        IReadOnlyList<IAssertPrimitives> AssertWriters { get; }
    }
}
