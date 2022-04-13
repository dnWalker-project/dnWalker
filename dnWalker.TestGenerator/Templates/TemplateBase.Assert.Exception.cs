using dnWalker.TestGenerator.TestClasses;
using dnWalker.TypeSystem;

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
            if (schema.ExceptionType == TypeSignature.Empty)
            {
                Write("act.Should().NotThrow()");
                WriteLine(TemplateHelpers.Semicolon);
            }
            else
            {
                // act.Should().Throw<EXCEPTIONTYPE>();
                Write("act.Should().Throw<");
                WriteTypeName(schema.ExceptionType);
                Write(">()");
                WriteLine(TemplateHelpers.Semicolon);
            }
        }
    }
}
