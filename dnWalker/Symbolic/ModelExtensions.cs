using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public static class ModelExtensions
    {
        public static IValue GetValueOrDefault(this IModel model, IVariable variable)
        {
            if (!model.TryGetValue(variable, out var value))
            {
                value = ValueFactory.GetDefault(variable.Type);
                model.SetValue(variable, value);
            }
            return value;
        }

        public static IEnumerable<Valuation> GetValuations(this IReadOnlyModel model)
        { 
            Valuation[] valuations = new Valuation[model.Variables.Count];
            int i = 0;
            foreach(IVariable variable in model.Variables)
            {
                if (!model.TryGetValue(variable, out IValue value))
                {
                    // something is very wrong
                    Debug.Fail("At this point, each variable should have its valuation.");
                }
                valuations[i] = new Valuation(variable, value);
                ++ i;
            }
            return valuations;
        }
    }
}
