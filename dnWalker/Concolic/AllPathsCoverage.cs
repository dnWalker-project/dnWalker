using dnWalker.Concolic.Traversal;
using dnWalker.Symbolic.Expressions;

using MMC.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Concolic
{
    public class AllPathsCoverage : IExplorationStrategy
    {
        public bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            return !constraintNode.IsExplored;
        }

        public void OnNodeExplored(ConstraintNode constraintNode)
        {
            constraintNode.MarkExplored();
        }
    }

    public class AllConditionsCoverage : IExplorationStrategy
    {
        private readonly Dictionary<CILLocation, Dictionary<Expression, bool>> _conditionCoverage = new Dictionary<CILLocation, Dictionary<Expression, bool>>();

        public bool ShouldBeExplored(ConstraintNode constraintNode)
        {
            if (constraintNode.IsExplored) return false;

            // check for the condition coverage
            CILLocation location = constraintNode.Location;
            if (location != CILLocation.None)
            {
                if (!_conditionCoverage.TryGetValue(location, out Dictionary<Expression, bool> coverage))
                {
                    coverage = new Dictionary<Expression, bool>();
                    _conditionCoverage[location] = coverage;
                }

                if (coverage.TryGetValue(constraintNode.Condition, out bool covered))
                {
                    if (covered)
                    {
                        //constraintNode.MarkExplored();
                        return false;
                    }
                }
            }
            return true;
        }

        public void OnNodeExplored(ConstraintNode constraintNode)
        {

            if (!constraintNode.IsExplored)
            {
                constraintNode.MarkExplored();

                Expression condition = constraintNode.Condition;
                if (condition != null)
                {
                    CILLocation currentLocation = constraintNode.Location;
                    Dictionary<Expression, bool> coverage = _conditionCoverage[currentLocation];

                    coverage[condition] = true;
                }
            }
        }
    }
}
