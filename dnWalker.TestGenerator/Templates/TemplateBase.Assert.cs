using dnWalker.Parameters;
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
        protected void WriteAssert(AssertionSchema schema)
        {
            WriteAssert(schema, Context.BaseSet, Context.ExecutionSet);
        }
        protected void WriteAssert(AssertionSchema schema, IReadOnlyParameterSet baseSet, IReadOnlyParameterSet executionSet)
        {
            if (schema == null) throw new ArgumentNullException(nameof(schema));

            switch (schema)
            {
                case ExceptionSchema exception: 
                    WriteExceptionAssert(exception); 
                    break;
                
                case ReturnValueSchema retValue: 
                    WriteReturnValueAssert(retValue); 
                    break;

                case ArrayElementSchema arrayElement: 
                    WriteArrayElementAssert(arrayElement); 
                    break;

                case ObjectFieldSchema objectField: 
                    WriteObjectFieldAssert(objectField); 
                    break;

                default: 
                    throw new ArgumentException($"Unexpected schema type: {schema.GetType().Name}", nameof(schema));
            }
        }
    }
}
