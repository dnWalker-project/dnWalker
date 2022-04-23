using dnWalker.Symbolic.Expressions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Symbolic
{
    public class Precondition : IPrecondition
    {
        private readonly IModel _baseModel;
        private readonly IEnumerable<Expression> _constraints;

        public Precondition(IModel baseModel, IEnumerable<Expression> constraints)
        {
            _baseModel = baseModel ?? throw new ArgumentNullException(nameof(baseModel));
            _constraints = constraints ?? throw new ArgumentNullException(nameof(constraints));
        }

        public IModel Solve(ISolver solver)
        {
            // use the solver to solve the constraints
            // that will give valuation of primitive variables & valuation of heap node traits
            // apply the valuation & the traits on the _baseModel.Clone()
            // return the result
            SolverResult result = solver.Solve(_constraints);
            if (result.Status == Status.Unsatisfiable)
            {
                return null;
            }

            IModel model = _baseModel.Clone();

            model.Update(result.Valuations);

            return model;
        }
    }
}
