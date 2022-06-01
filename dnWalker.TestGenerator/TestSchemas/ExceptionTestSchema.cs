using dnlib.DotNet;

using dnWalker.Symbolic;
using dnWalker.TestGenerator.Templates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.TestGenerator.TestSchemas
{
    public class ExceptionTestSchema : TestSchema
    {
        private readonly TypeSig? _exceptionType;
        private readonly IReadOnlyModel _inputModel;

        public TypeSig? ExceptionType => _exceptionType;

        public ExceptionTestSchema(TypeSig? exceptionType, IReadOnlyModel inputModel)
        {
            _exceptionType = exceptionType;
            _inputModel = inputModel;
        }

        public override void Write(IWriter output, TestWriter writer)
        {
            // TODO: setup the input symbols
            writer.WriteArrange(output, null);
            writer.WriteActDelegate(output, null, null);
            
            if (_exceptionType == null)
            {

            }
            else
            {

            }
        }
    }
}
