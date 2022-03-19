using dnWalker.TypeSystem;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Parameters
{
    public class StringParameter : ReferenceTypeParameter, IStringParameter
    {
        internal StringParameter(IParameterSet set) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.String.TypeDef))
        {
        }

        internal StringParameter(IParameterSet set, ParameterRef reference) : base(set, new TypeSignature(set.Context.DefinitionProvider.BaseTypes.String.TypeDef), reference)
        {
        }

        public override IParameter CloneData(IParameterSet newSet)
        {
            return new StringParameter(newSet, Reference);
        }

        public string? Value
        {
            get;
            set;
        }
    }

    public partial class ParameterContextExtensions
    {
        public static IStringParameter CreateStringParameter(this IParameterSet set, bool? isNull = null)
        {
            return CreateStringParameter(set, set.GetParameterRef(), isNull);
        }

        public static IStringParameter CreateStringParameter(this IParameterSet set, ParameterRef reference, bool? isNull = null)
        {
            return new StringParameter(set, reference)
            {
                IsNull = isNull
            };
        }
    }
}
