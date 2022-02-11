using dnWalker.TestGenerator.TestClasses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.Templates
{
    public partial class TemplateBase
    {
        private void WriteExceptionAssert(ExceptionSchema schema)
        {
            // act.Should().Throw<EXCEPTIONTYPE>();
            Write("act.Should().Throw<");
            WriteTypeName(schema.ExceptionType);
            Write(">()");
            WriteLine(TemplateHelpers.Semicolon);
        }
    }
}
