using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{

    public class ExplorationIterationData
    {
        //public ExplorationIterationData(String pathConstraint, IDictionary<String, ParameterInfo> inputValues)
        //{
        //    PathConstraint = pathConstraint;// ?? throw new ArgumentNullException(nameof(pathConstraint));
        //    InputValues = inputValues ?? throw new ArgumentNullException(nameof(inputValues));
        //}

        public String PathConstraint
        {
            get;
        }


    }
}
